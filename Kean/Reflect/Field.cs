// 
//  Field.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
	public class Field :
		FieldInformation,
		IComparable<Field>
	{
		protected object Parent { get; private set; }
		public object Data
		{
			get { return this.Information.GetValue(this.Parent); }
			set { this.Information.SetValue(this.Parent, value); }
		}
		internal Field(object parent, Type parentType, System.Reflection.FieldInfo information) :
			base( parentType, information)
		{
			this.Parent = parent;
		}
		public Field<T> Convert<T>()
		{
			return new Field<T>(this.Parent, this.ParentType, this.Information);
		}
		#region IComparable<Field> Members
		Order IComparable<Field>.Compare(Field other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
		internal static Field Create(object parent, Type parentType, System.Reflection.FieldInfo fieldInformation)
		{
			Field result;
			if (fieldInformation.FieldType == typeof(int))
				result = new Field<int>(parent, parentType, fieldInformation);
			else
				result = new Field(parent, parentType, fieldInformation);
			return result;
		}
	}
	public class Field<T> :
		Field
	{
		public T Value
		{
			get { return (T)this.Information.GetValue(this.Parent); }
			set { this.Information.SetValue(this.Parent, value); }
		}
		internal Field(object parent, Type parentType, System.Reflection.FieldInfo fieldInformation) :
			base(parent, parentType, fieldInformation)
		{ }
	}
}

