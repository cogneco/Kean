// Copyright (C) 2013  Simon Mika <simon@mika.se>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using Tasks = System.Threading.Tasks;
using Generic = System.Collections.Generic;

namespace Kean.IO.Extension
{
	public static class ByteInDeviceExtension
	{
		#region GetEnumerator
		public static Generic.IEnumerator<Tasks.Task<byte?>> GetEnumerator(this IByteInDevice device)
		{
			yield return device.Read();
		}
		public static Generic.IEnumerator<Tasks.Task<Collection.IBlock<byte>>> GetEnumerator(this IBlockInDevice device)
		{
			yield return device.Read();
		}
		#endregion
		#region Skip
		public static async Tasks.Task<T> Skip<T>(this T me, int count)
			where T : IByteInDevice
		{
			while (count > 0 && (await me.Read()).HasValue)
				count--;
			return me;
		}
		public static async Tasks.Task<T> Skip<T>(this T me, params byte[] separator)
			where T : IByteInDevice
		{
			byte? next;
			int position = 0;
			while ((next = (await me.Read())).HasValue)
			{
				if (next != separator[position++])
					position = 0;
				else if (separator.Length == position)
					break;
			}
			return me;
		}
		#endregion
	}
}
