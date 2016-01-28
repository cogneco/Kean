//
//  PropertyInformation.cs
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

using Kean;
using Kean.Extension;

namespace Kean.Reflect
{
	public class PropertyInformation : 
		Member,
		IComparable<PropertyInformation>
	{
		protected System.Reflection.PropertyInfo Information { get; private set; }
		public bool Readable { get { return this.Information.CanRead; } }
		public bool Writable { get { return this.Information.CanWrite; } }
		public Type Type { get { return this.Information.PropertyType; } }

		internal PropertyInformation(Type parentType, System.Reflection.PropertyInfo information) :
			base(parentType, information)
		{
			this.Information = information;
		}
		#region IComparable<PropertyInformation> Members
		Order IComparable<PropertyInformation>.Compare(PropertyInformation other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
	}
}

