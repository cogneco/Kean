// 
//  CharacterReader.cs
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
using Uri = Kean.Uri;
using Error = Kean.Error;

namespace Kean.IO.Null
{
	public class CharacterReader :
		ICharacterReader
	{
		public Uri.Locator Resource { get { return "null://"; } }
		public int Row { get; private set; }
		public int Column { get; private set; }
		public bool Opened { get { return true; } }
		public bool Empty { get { return true; } }
		public bool Readable { get { return true; } }
		public char Last { get; private set; }
		public CharacterReader()
		{
			this.Row = 1;
		}
		public bool Next()
		{
			return false;
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
	}
}
