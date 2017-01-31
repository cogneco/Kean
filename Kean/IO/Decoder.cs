// 
//  Decoder.cs
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
using Kean.IO.Extension;
using Generic = System.Collections.Generic;

namespace Kean.IO
{
	class Decoder :
		ICharacterInDevice
	{
		IByteInDevice backend;
		System.Text.Encoding encoding;
		Generic.IEnumerator<char?> queue;
		char? peeked;
		#region Constructors
		public Decoder(IByteInDevice backend, System.Text.Encoding encoding)
		{
			this.backend = backend;
			this.encoding = encoding;
			this.queue = this.backend.AsEnumerable().Decode(this.encoding).Cast(c => (char?)c).GetEnumerator();
		}
		~Decoder()
		{
			Error.Log.Wrap((Func<bool>)this.Close)();
		}
		#endregion
		#region ICharacterInDevice Members
		char? RawRead()
		{
			return this.queue.NotNull() ? this.queue.Next() : null;
		}
		public char? Peek()
		{
			return this.peeked ?? (this.peeked = this.RawRead());
		}
		public char? Read()
		{
			char? result;
			if (this.peeked.HasValue)
			{
				result = this.peeked;
				this.peeked = null;
			}
			else
				result = this.RawRead();
			return result;
		}
		#endregion
		#region IInDevice Members
		public bool Empty
		{
			get { return !this.peeked.HasValue && (this.backend.IsNull() || this.backend.Empty); }
		}
		public bool Readable
		{
			get { return this.peeked.HasValue || this.backend.NotNull() && this.backend.Readable; }
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public bool Opened
		{
			get { return this.peeked.HasValue || this.backend.NotNull() && this.backend.Opened; }
		}
		public bool Close()
		{
			bool result = this.backend.NotNull() && this.backend.Close();
			if (result)
			{
				this.backend = null;
				this.queue = null;
			}
			return result;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
	}
}
