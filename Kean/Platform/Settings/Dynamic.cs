// 
//  Dynamic.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;

namespace Kean.Platform.Settings
{
	public class Dynamic :
		Dynamic<object>
	{
		public Dynamic()
		{ }
	}
	public class Dynamic<T> :
		Collection.Abstract.ReadOnlyDictionary<string, T>,
		IDynamic
		where T : class
	{
		Collection.Dictionary<string, Tuple<string, string, T>> data = new Collection.Dictionary<string, Tuple<string, string, T>>();
		Action<string, object> loaded;

		public override T this[string name] 
		{ 
			get 
			{
				Tuple<string, string, T> item = this.data[name];
				return item.NotNull() ? item.Item3 : default(T); 
			} 
		}

		object IDynamic.this[string name]
		{
			get
			{
				object result;
				if (name.NotEmpty())
				{
					string[] splitted = (name ?? "").Split(new char[] { '.' }, 2);
					Tuple<string, string, T> member = this.data[splitted[0]];
					result = member.NotNull() ? (object)member.Item3 : null;
					if (result is IDynamic && splitted.Length > 1)
						result = (result as IDynamic)[splitted[1]];
				}
				else
					result = this;
				return result;
			}
			set 
			{
				if (value.NotNull())
					this.Load(name, value);
				else
					this.Unload(name);
			}
		}

		public Dynamic()
		{ }

		public void Load(string name, object value)
		{
			this.Load(name, null, value);
		}
		public void Load(string name, string description, object value)
		{
			this.Load(name, description, null, value);
		}
		public void Load(string name, string description, string usage, object value)
		{
			string[] path = name.Split(new char[] { '.' }, 2);
			if (path.Length > 1)
			{
				IDynamic next = (this as IDynamic)[path[0]] as IDynamic;
				if (next.IsNull())
					this.Load(path[0], next = new Dynamic());
				next.Load(name.Substring(path[0].Length + 1), description, usage, value);
			}
			else if (value is T)
			{
				this.data[name] = Tuple.Create(description, usage, value as T);
				this.loaded.Call(name, value);
				this.Reload();
			}
		}
		public void Unload(string name)
		{
			this.data.Remove(name);
			this.Reload();
		}
		public void WhenLoaded<S>(string name, Action<S> loaded)
		{
			object member = (this as IDynamic)[name];
			if (member is S)
				loaded.Call((S)member);
			this.loaded += (n, m) =>
			{
				if (n == name && m is S)
					loaded.Call((S) m);
			};
		}

		public override bool Contains(string key)
		{
			return this.data.Contains(key);
		}

		public override System.Collections.Generic.IEnumerator<KeyValue<string, T>> GetEnumerator()
		{
			foreach (KeyValue<string, Tuple<string, string, T>> member in this.data)
				yield return KeyValue.Create(member.Key, member.Value.Item3);
		}

		void Reload()
		{
			this.reload.Call();
		}

		System.Collections.Generic.IEnumerable<Tuple<string, string, string, object>> IDynamic.GetDynamic()
		{
			foreach (KeyValue<string, Tuple<string, string, T>> member in this.data)
				yield return Tuple.Create(member.Key, member.Value.Item1, member.Value.Item2, (object)member.Value.Item3);
		}
		#region IReload Members
		event Action reload;
		event Action IReload.Reload
		{
			add { this.reload += value; }
			remove { this.reload -= value; }
		}
		#endregion

		public virtual void Dispose()
		{
			if (this.data.NotNull())
			{
				foreach (KeyValue<string, Tuple<string, string, T>> item in this.data)
					if (item.Value.Item3 is IDisposable)
						(item.Value.Item3 as IDisposable).Dispose();
				this.data = null;
				this.loaded = null;
			}
		}
	}
}
