//
//  SeekableBlockInDeviceExtension.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2015 Simon Mika
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

using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;

namespace Kean.IO.Extension
{
	public static class SeekableBlockInDeviceExtension
	{
		public static ISeekableBlockInDevice Slice(this ISeekableBlockInDevice me, long start, long end = 0)
		{
			return Wrap.SlicedBlockInDevice.Slice(me, start, end);
		}
	}
}

