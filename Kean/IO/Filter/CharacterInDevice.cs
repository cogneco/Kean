// 
//  CharacterInDevice.cs
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
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Error = Kean.Error;

namespace Kean.IO.Filter
{
	public class CharacterInDevice :
		ICharacterInDevice
	{
		ICharacterInDevice backend;
		Collection.IQueue<char> queue = new Collection.Queue<char>();
		Func<Func<char?>, char[]> filter;
		public Func<Func<char?>, char[]> Filter
		{
			get { return this.filter ?? this.NullFilter; }
			set { this.filter = value; }
		}
		#region Constructors
		protected CharacterInDevice(ICharacterInDevice backend)
		{
			this.backend = backend;
		}
		~CharacterInDevice()
		{
			Error.Log.Wrap((Func<bool>)this.Close)();
		}
		#endregion
		char[] NullFilter(Func<char?> read)
		{
			char? next = read();
			return next.HasValue ? new char[] { next.Value } : new char[0];
		}
		#region ICharacterInDevice Members
		public char? Peek()
		{
			if (this.queue.Empty)
				this.queue.Enqueue(this.Filter(this.backend.Read)); 
			return this.queue.Empty ? (char?)null : this.queue.Peek();
		}
		public char? Read()
		{
			if (this.queue.Empty)
				this.queue.Enqueue(this.Filter(this.backend.Read)); 
			char? result = this.queue.Empty ? (char?)null : this.queue.Dequeue();
			return result;
		}
		#endregion
		#region IInDevice Members
		public bool Readable { get { return !this.queue.Empty || (this.backend.NotNull() && this.backend.Readable); } }
		public bool Empty { get { return this.queue.Empty && (this.backend.IsNull() || this.backend.Empty); } }
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public bool Opened
		{
			get { return !this.queue.Empty || this.backend.NotNull() && this.backend.Opened; }
		}
		public bool Close()
		{
			bool result = this.backend.NotNull() && this.backend.Close();
			if (result)
				this.backend = null;
			return result;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#region Static Open
		public static ICharacterInDevice Open(ICharacterInDevice backend)
		{ 
			return backend.NotNull() ? new CharacterInDevice(backend) : null; 
		}
		public static ICharacterInDevice Open(ICharacterInDevice backend, Func<Func<char?>, char[]> filter)
		{ 
			return backend.NotNull() ? new CharacterInDevice(backend) { Filter = filter } : null; 
		}
		#endregion
	}
}
