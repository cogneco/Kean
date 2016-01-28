// 
//  CharacterWriter.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

namespace Kean.IO.Null
{
	public class CharacterWriter :
		ICharacterWriter
	{
		public char[] NewLine { get; set; }
		public Uri.Locator Resource { get { return "null://"; } }
		public bool Opened { get { return true; } }
		public bool Writable { get { return true; } }
		public CharacterWriter()
		{
			this.NewLine = new char[] { '\n' };
		}
		public bool Close()
		{
			return true;
		}
		#region IDisposable Members
		void IDisposable.Dispose()
		{
		}
		#endregion
		#region ICharacterWriter Members
		public bool Write(params char[] buffer)
		{
			return true;
		}
		public bool Write(string value)
		{
			return true;
		}
		public bool Write<T>(T value) where T : IConvertible
		{
			return true;
		}
		public bool Write(string format, params object[] arguments)
		{
			return true;
		}
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return true;
		}
		public bool WriteLine()
		{
			return true;
		}
		public bool WriteLine(params char[] buffer)
		{
			return true;
		}
		public bool WriteLine(string value)
		{
			return true;
		}
		public bool WriteLine<T>(T value) where T : IConvertible
		{
			return true;
		}
		public bool WriteLine(string format, params object[] arguments)
		{
			return true;
		}
		public bool WriteLine(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return true;
		}
		#endregion
		#region IOutDevice Members
		public bool AutoFlush { get; set; }
		public bool Flush()
		{
			return true;
		}
		#endregion
	}
}
