//
//  Factory.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2011-2012 Simon Mika
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

namespace Kean.Json.Dom.Test
{
	public abstract class Factory<T> :
		Kean.Test.Fixture<T>
		where T : Factory<T>, new()
	{
		protected abstract void Verify(string name);
		protected Item Open(string name)
		{
			return Item.OpenResource("Json/Dom/Data/" + name + ".json");
		}
		protected Object Create(string name)
		{
			Object result = new Object();
			switch (name)
			{
				case "Empty":
					break;
				case "Null": 
					result.Add("null", new Null());
					break;
				case "BooleanFalse": 
					result.Add("booleanFalse", false);
					break;
				case "BooleanTrue": 
					result.Add("booleanTrue", true);
					break;
				case "Number":
					result.Add("number", 1337);
					break;
				case "String":
					result.Add("string", "Text");
					break;
				case "Array":
					result.Add("array", 0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
					break;
				case "ObjectEmpty":
					result.Add("empty", this.Create("Empty"));
					break;
				case "ObjectSingle":
					result.Add("object", this.Create("String"));
					break;
				case "Object":
					result.Add("string", "Text").Add("number", "1337").Add("boolean", true);
					break;
				case "ObjectNested":
					result.Add("nested", this.Create("Object")).Add("string", "Text");
					break;
			}
			return result;
		}
	}
}
