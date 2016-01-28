//
//  FieldInformation.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2010-2012 Simon Mika
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
	public class FieldInformation :
		Member,
		IComparable<FieldInformation>
	{
		protected System.Reflection.FieldInfo Information { get; private set; }
		public Type Type { get { return this.Information.FieldType; } }
		internal FieldInformation(Type parentType, System.Reflection.FieldInfo information) :
			base(parentType, information)
		{
			this.Information = information;
		}
		#region IComparable<FieldInformation> Members
		Order IComparable<FieldInformation>.Compare(FieldInformation other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
	}
}

