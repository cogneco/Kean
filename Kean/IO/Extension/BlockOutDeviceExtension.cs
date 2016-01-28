//
//  BlockOutDeviceExtension.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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

namespace Kean.IO.Extension
{
	public static class BlockOutDeviceExtension
	{
		public static bool Write(this IBlockOutDevice me, params byte[] buffer)
		{
			return me.Write(new Collection.Vector<byte>(buffer));
		}
		public static bool Write(this IBlockOutDevice me, IBlockInDevice source)
		{
			bool result = source.NotNull() && me.NotNull();
			Collection.IVector<byte> data;
			while (result && !source.Empty && (data = source.Read()).NotNull() && data.Count > 0)
				result &= me.Write(data);
			return result;
		}
	}
}
