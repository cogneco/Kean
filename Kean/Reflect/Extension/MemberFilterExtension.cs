// 
//  MemberFilterExtension.cs
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

namespace Kean.Reflect.Extension
{
	public static class MemberFilterExtension
	{
		public static bool Contains(this MemberFilter me, MemberFilter value)
		{
			return (me & value) == value;
		}
		public static System.Reflection.BindingFlags AsBindingFlags(this MemberFilter me)
		{
			System.Reflection.BindingFlags result = System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.FlattenHierarchy;
			if (me.Contains(MemberFilter.Instance))
				result |= System.Reflection.BindingFlags.Instance;
			if (me.Contains(MemberFilter.Static))
				result |= System.Reflection.BindingFlags.Static;
			if (me.Contains(MemberFilter.Public))
				result |= System.Reflection.BindingFlags.Public;
			if (me.Contains(MemberFilter.NonPublic))
				result |= System.Reflection.BindingFlags.NonPublic;
			return result;
		}
	}
}
