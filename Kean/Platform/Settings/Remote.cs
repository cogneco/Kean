// 
//  Remote.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Uri = Kean.Uri;
using IO = Kean.IO;
using Kean.Extension;
using Parallel = Kean.Parallel;
using Collection = Kean.Collection;
using Error = Kean.Error;
using Kean;

namespace Kean.Platform.Settings
{
	public class Remote :
		Synchronized,
		IDisposable
	{
		RemoteConfiguration configuration = new RemoteConfiguration();
		public RemoteConfiguration Configuration
		{
			get { return this.configuration; }
			set
			{ 
				this.configuration = value;
				if (this.configuration.NotNull())
				{
					Action<Asynchronous> asynchronousChanged = v => this.Set("settings.asynchronous", v);
					this.Configuration.AsynchronousChanged += asynchronousChanged;
					asynchronousChanged(this.Configuration.Asynchronous);
				}
			}
		}
		Collection.IDictionary<string, Action<string>> values = new Collection.Synchronized.Dictionary<string, Action<string>>();
		Collection.IDictionary<string, Action<string>> notifications = new Collection.Synchronized.Dictionary<string, Action<string>>();
		Collection.IDictionary<string, Action<string>> types = new Collection.Synchronized.Dictionary<string, Action<string>>();
		object onResponseLock = new object();
		Action<bool> onResponse;
		event Action<bool> OnResponse
		{
			add
			{
				lock (this.onResponseLock)
					this.onResponse += value;
			}
			remove
			{
				lock (this.onResponseLock)
					this.onResponse -= value;
			}
		}
		IO.ICharacterReader reader;
		IO.ICharacterWriter writer;
		Parallel.RepeatThread thread;
		Remote(IO.ICharacterReader reader, IO.ICharacterWriter writer)
		{
			this.reader = reader;
			this.writer = writer;
			this.writer.AutoFlush = true;

			IO.Text.Builder line = null;
			while (this.reader.Next() && this.reader.Last != '\n')
			{
				if (this.reader.Last == '>' && line == "# ")
					break;
				else if (!char.IsControl(this.reader.Last))
					line += this.reader.Last;
			}
			this.thread = Parallel.RepeatThread.Start("RemoteClient", this.Receive);
			this.Configuration = this.Configuration;

		}
		public bool Exists(string @object)
		{
			string result = this.GetType(@object);
			return result.NotNull() && result.StartsWith("object");
		}
		public bool Call(string method, params object[] arguments)
		{
			return this.Call(method, this.Configuration.Asynchronous.HasFlag(Settings.Asynchronous.Call), arguments);
		}
		public bool Call(string method, bool asynchronous, params object[] arguments)
		{
			bool result = false;
			if (asynchronous)
				this.Send(method, arguments);
			else
			{
				object @lock = new object();
				bool done = false;
				Action<string> previous = this.values[method];
				Action<bool> onResponse = success =>
				{
					lock (@lock)
					{
						result = success;
						done = true;
						this.values[method] = previous;
						System.Threading.Monitor.PulseAll(@lock);
					}
				};
				this.values[method] = value =>
				{
					lock (@lock)
					{
						if (value == "failed")
						{
							result = false;
							this.OnResponse -= onResponse;
							done = true;
							System.Threading.Monitor.PulseAll(@lock);
						}
					}
					this.values.Remove(method);
					previous.Call(value);
				};
				this.OnResponse += onResponse;
				string sent = this.Send(method, arguments);
				if (this.thread.IsCurrent)
					while (!done)
						this.Receive();
				else
					lock (@lock)
						System.Threading.Monitor.Wait(@lock, 5000);
				if (!done)
					Error.Log.Append(Error.Level.Recoverable, "Remote Method Call Timed Out.", "Timed out while waiting for response when calling \"{0}\" over \"{1}\".", sent, this.writer.Resource);
			}
			return result;
		}
		public T Call<T>(string method, params object[] arguments)
		{
			return this.Receive<T>(method, () => this.Send(method, arguments));
		}
		public T Get<T>(string property)
		{
			return this.Receive<T>(property, () => this.Send(property));
		}
		public T Set<T>(string property, T value)
		{
			return this.Set(property, value, this.Configuration.Asynchronous.HasFlag(Settings.Asynchronous.Set));
		}
		public T Set<T>(string property, T value, bool asynchronous)
		{
			T result;
			if (asynchronous)
			{
				this.Send(property, value);
				result = value;
			}
			else
				result = this.Receive<T>(property, () => this.Send(property, value));
			return result;
		}
		public void Listen<T>(string property, Action<T> callback)
		{
			this.Send(property + " notify");
			this.notifications[property] = value => callback(value.Parse<T>());
		}
		T Receive<T>(string property, Func<string> send)
		{
			object @lock = new object();
			bool done = false;
			T result = default(T);
			Action<string> previous = this.values[property];
			this.values[property] = value =>
			{
				lock (@lock)
				{
					try
					{
						result = value.Parse<T>();
					}
					catch
					{
						Error.Log.Append(Error.Level.Recoverable, "Remote failed parsing.", "Failed parsing result of {0} with value {1} of type {2}.", property, value, typeof(T));
					}
					finally
					{
						done = true;
						System.Threading.Monitor.PulseAll(@lock);
					}
				}
				this.values.Remove(property);
				previous.Call(value);
			};
			string sent = send();
			if (this.thread.IsCurrent)
				while (!done)
					this.Receive();
			else
				lock (@lock)
					System.Threading.Monitor.Wait(@lock, 5000);
			if (!done)
				Error.Log.Append(Error.Level.Recoverable, "Remote Property Read Timed Out.", "Timed out while waiting for response when requesting property \"{0}\" over \"{1}\".", sent, this.writer.Resource);
			return result;
		}
		public string GetType(string name)
		{
			object @lock = new object();
			bool done = false;
			string result = null;
			Action<string> previous = this.types[name];
			this.types[name] = value =>
			{
				lock (@lock)
				{
					result = value;
					done = true;
					System.Threading.Monitor.PulseAll(@lock);
				}
				this.values.Remove(name);
				previous.Call(value);
			};
			string sent = this.Send("? " + name);
			if (this.thread.IsCurrent)
				while (!done)
					this.Receive();
			else
				lock (@lock)
					System.Threading.Monitor.Wait(@lock, 5000);
			if (!done)
				Error.Log.Append(Error.Level.Recoverable, "Remote Type Request Read Timed Out.", "Timed out while waiting for response when requesting type of \"{0}\" over \"{1}\".", sent, this.writer.Resource);
			return result;
		}
		void Receive()
		{
			lock (this.Lock)
			{
				IO.Text.Builder line = null;
				while (this.reader.Next() && this.reader.Last != '\n')
				{
					if (line.NotNull() || !char.IsWhiteSpace(this.reader.Last))
					if (this.reader.Last == '>' && line == "# ")
					{
						line = null;
						this.OnResponseCall(true);
					}
					else if (!char.IsControl(this.reader.Last))
						line += this.reader.Last;
				}
				this.Configuration.Debug(false, (string)line);
				string[] splitted = ((string)line ?? "").Split(new char[] { ' ' }, 3);
				if (splitted.Length > 1)
				{
					switch (splitted[0])
					{
						case "$": // value
							if (splitted.Length > 2)
							{
								this.values[splitted[1]].Call(splitted[2]);
								this.OnResponseCall(true);
							}
							break;
						case "%": // notification
							if (splitted.Length > 2)
								this.notifications[splitted[1]].Call(splitted[2]);
							break;
						default:
						case "?": // type
							this.types[splitted[1]].Call(splitted.Length > 2 ? splitted[2] : "");
							break;
						case "!": // error
							this.OnResponseCall(false);
							System.Diagnostics.Debug.WriteLine(line);
							break;
					}
				}
			}
		}
		void OnResponseCall(bool success)
		{
			Action<bool> onResponse;
			lock (this.onResponseLock)
			{
				onResponse = this.onResponse;
				this.onResponse = null;
			}
			onResponse.Call(success);
		}
		string Send(string message, params object[] arguments)
		{
			IO.Text.Builder builder = new IO.Text.Builder(message);
			foreach (object argument in arguments)
				builder += " \"" + argument.AsString() + "\"";
			string result = builder;
			this.Configuration.Debug(true, result);
			this.writer.WriteLine(result);
			return result;
		}
		public void Dispose()
		{
			if (this.thread.NotNull())
			{
				this.thread.Dispose();
				this.thread = null;
			}
			if (this.writer.NotNull())
			{
				this.writer.Dispose();
				this.writer = null;
			}
			if (this.reader.NotNull())
			{
				this.reader.Dispose();
				this.reader = null;
			}
		}
		public static Remote Open(Uri.Locator resource)
		{
			IO.ICharacterDevice result = null;
			switch (resource.Scheme)
			{
				case "tcp":
					result = IO.CharacterDevice.Open(IO.Net.Tcp.Connection.Connect(resource.Authority == "" ? (Uri.Endpoint)"127.0.0.1" : resource.Authority.Endpoint).ByteDevice);
					break;
			}
			return Remote.Open(result);
		}
		public static Remote Open(IO.ICharacterDevice characterDevice)
		{
			return Remote.Open(IO.CharacterReader.Open(characterDevice), IO.CharacterWriter.Open(characterDevice));
		}
		public static Remote Open(IO.ICharacterInDevice characterInDevice, IO.ICharacterOutDevice characterOutDevice)
		{
			return new Remote(IO.CharacterReader.Open(characterInDevice), IO.CharacterWriter.Open(characterOutDevice));
		}
		public static Remote Open(IO.ICharacterReader reader, IO.ICharacterWriter writer)
		{
			return reader.NotNull() && writer.NotNull() ? new Remote(reader, writer) : null;
		}
		public static Remote Open(System.IO.Stream input, System.IO.Stream output)
		{
			return Remote.Open(IO.ByteDevice.Open(input), IO.ByteDevice.Open(output));
		}
		public static Remote Open(IO.IByteInDevice input, IO.IByteOutDevice output)
		{
			return Remote.Open(IO.ByteDeviceCombiner.Open(input, output));
		}
		public static Remote Open(IO.IByteDevice device)
		{
			//device = IO.Tap.ByteDevice.Open(device, b => System.Diagnostics.Debug.Write(b == 10 ? "\n" : "s{0:X2}".Format(b)), b => System.Diagnostics.Debug.Write(b == 10 ? "\n" : "c{0:X2}".Format(b)));
			return Remote.Open(IO.CharacterDevice.Open(IO.BufferingByteDevice.Open(device)));
		}
	}
}
