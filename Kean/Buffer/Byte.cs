// 
//  Byte.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2009-2012 Anders Frisk
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

namespace Kean.Buffer
{
	public class Byte : 
		Vector<byte>
	{
		public Byte(int size) : this(size, null) { }
		public Byte(int size, Action<IntPtr> free) : base(size, free) { }
		public Byte(byte[] data) : base(data, null) { }
		public Byte(byte[] data, Action<IntPtr> free) : base(data, free) { }
	}
}
