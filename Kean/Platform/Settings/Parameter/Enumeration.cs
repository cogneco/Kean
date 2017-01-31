// 
//  Enumeration.cs
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

using System;
using Kean;
using Kean.Extension;
using Reflect = Kean.Reflect;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Platform.Settings.Parameter
{
	class Enumeration :
		Abstract
	{
		string[] values;
		public string[] Values
		{
			get
			{
				if (this.values.IsNull())
					this.values = Enum.GetNames(this.Type);
				return this.values;
			}
		}
		internal Enumeration(Reflect.Type type) :
			base(type)
		{ }
		public override string AsString(object value)
		{
			return value.AsString();
		}
		public override object FromString(string value)
		{
			return value.Parse(this.Type);
		}
		public override string Complete(string incomplete)
		{
			Collection.List<string> alternatives = new Collection.List<string>();
			foreach (string value in this.Values)
				if (value.StartsWith(incomplete))
					alternatives.Add(value);
			string result = "";
			if (alternatives.Count > 0)
				for (int i = 0; i < alternatives[0].Length && alternatives.All(s => s[i] == alternatives[0][i]); i++)
					result += alternatives[0][i];
			return result;
		}
		public override string Help(string incomplete)
		{
			return this.Values.Fold((value, a) => a + value + "\n", "");
		}
	}
}
