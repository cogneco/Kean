//
//  Variable.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2014 Simon Mika
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

using Kean.Extension;

namespace Kean.Math.Algebra
{
	public class Variable :
	Expression
	{
		public override int Precedence { get { return 0; } }
		public string Name { get; private set; }
		Variable(string name)
		{
			this.Name = name;
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return variables.Find(variable => variable.Key == this.Name).Value;
		}
		public override Expression Derive(string variable)
		{
			return variable == this.Name ? 1 : 0;
		}
		public override Expression Simplify()
		{
			return this;
		}
		public override bool Equals(Expression other)
		{
			return other is Variable && this.Name == ((Variable)other).Name;
		}
		#region Object Overrides
		public override string ToString()
		{
			return this.Name;
		}
		public override int GetHashCode()
		{
			return this.Name.Hash();
		}
		#endregion
		#region Static Create
		public static Expression Create(string name)
		{
			switch (name)
			{
				case "pi":
					return Constant.Pi;
				case "e":
					return Constant.E;
				default:
					return new Variable(name);
			}
		}
		#endregion
	}
}

