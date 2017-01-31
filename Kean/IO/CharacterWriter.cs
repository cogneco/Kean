// 
//  CharacterWriter.cs
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
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Error = Kean.Error;

namespace Kean.IO
{
	public class CharacterWriter :
		Abstract.CharacterWriter
	{
		ICharacterOutDevice backend;
		public override Uri.Locator Resource { get { return this.backend.Resource; } }
		public override bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public override bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		protected CharacterWriter(ICharacterOutDevice backend)
		{
			this.backend = backend;
			this.NewLine = new char[] { '\n' };
		}
		public override bool Close()
		{
			bool result = this.backend.NotNull() && this.backend.Close();
			if (result)
				this.backend = null;
			return result;
		}
		#region implemented abstract and virtual members of Kean.IO.Abstract.CharacterWriter
		public override bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.backend.Write(new Collection.Enumerable<char>(() => new NewLineConverter(buffer.GetEnumerator(), this.NewLine)));
		}
		public override bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		public override bool Flush()
		{
			return this.backend.NotNull() && this.backend.Flush();
		}
		#endregion
		#region NewLineConverter Class
		class NewLineConverter :
			System.Collections.Generic.IEnumerator<char>
		{
			System.Collections.Generic.IEnumerator<char> backend;
			char[] newLine;
			int index;
			#region Constructors
			public NewLineConverter(System.Collections.Generic.IEnumerator<char> backend, params char[] newLine)
			{
				this.backend = backend;
				this.newLine = newLine;
			}
			#endregion
			#region IEnumerator<char> Members
			public char Current { get; private set; }
			#endregion
			#region IDisposable Members
			public void Dispose()
			{
				this.backend.Dispose();
			}
			#endregion
			#region IEnumerator Members
			object System.Collections.IEnumerator.Current { get { return this.Current; } }
			public bool MoveNext()
			{
				bool result = true;
				if (this.index > 0)
				{
					this.Current = this.newLine[this.index++];
					if (this.index >= this.newLine.Length)
						this.index = 0;
				}
				else if (this.backend.MoveNext())
				{
					if (this.backend.Current == '\n')
					{
						this.Current = this.newLine[0];
						if (this.newLine.Length > 1)
							this.index = 1;
					}
					else
						this.Current = this.backend.Current;
				}
				else
					result = false;
				return result;
			}
			public void Reset()
			{
				this.backend.Reset();
				this.index = 0;
			}
			#endregion
		}
		#endregion
		#region Static Open & Create
		public static ICharacterWriter Open(Uri.Locator resource)
		{
			return CharacterWriter.Open(CharacterDevice.Open(resource));
		}
		public static ICharacterWriter Create(Uri.Locator resource)
		{
			return CharacterWriter.Open(CharacterDevice.Create(resource));
		}
		public static ICharacterWriter Open(ICharacterOutDevice backend)
		{
			return backend.NotNull() ? new CharacterWriter(backend) : null;
		}
		#endregion
	}
}
