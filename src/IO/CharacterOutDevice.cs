// Copyright (C) 2012, 2017	Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.	If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean.Extension;

namespace Kean.IO
{
	public class CharacterOutDevice :
		ICharacterOutDevice
	{
		Func<Generic.IEnumerator<char>, Tasks.Task<bool>> write;
		event Action OnClose;
		CharacterOutDevice(Action<Generic.IEnumerator<char>> write) :
			this(buffer => { write.Call(buffer); return Tasks.Task.FromResult(write.NotNull()); })
		{ }
		CharacterOutDevice(Func<Generic.IEnumerator<char>, Tasks.Task<bool>> write)
		{
			this.write = write;
		}
		~CharacterOutDevice() {
			this.Close().Wait();
		}
		#region ICharacterOutDevice Members
		public async Tasks.Task<bool> Write(Generic.IEnumerator<char> buffer)
		{
			return this.Opened && await this.write(buffer);
		}
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return true; } }
		public bool AutoFlush
		{
			get { return true; }
			set { ; }
		}
		public Tasks.Task<bool> Flush()
		{
			return Tasks.Task.FromResult(true);
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource
		{
			get { return "string://"; }
		}
		public bool Opened
		{
			get { return this.write.NotNull(); }
		}
		public Tasks.Task<bool> Close()
		{
			var result = this.Opened;
			if (result)
			{
				this.OnClose.Call();
				this.write = null;
			}
			return Tasks.Task.FromResult(result);
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close().Wait();
		}
		#endregion
		#region Static Open
		public static ICharacterOutDevice Open(Action<char> next)
		{
			return new CharacterOutDevice(content => {
				while (content.MoveNext())
					next(content.Current);
			});
		}
		public static ICharacterOutDevice Open(Action<string> done)
		{
			Text.Builder output = null;
			var result = new CharacterOutDevice(content =>
			{
				output += content;
			});
			result.OnClose += () =>
			{
				done(output);
			};
			return result;
		}
		public static Tuple<ICharacterOutDevice, Tasks.Task<string>> Open()
		{
			Tasks.Task<string> output;
			return Tuple.Create(CharacterOutDevice.Open(out output), output);
		}
		public static ICharacterOutDevice Open(out Tasks.Task<string> output)
		{
			var outputSource = new Tasks.TaskCompletionSource<string>();
			output = outputSource.Task;
			return CharacterOutDevice.Open(content => outputSource.SetResult(content));
		}
		#endregion
	}
}
