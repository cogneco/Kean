// 
//  StringCast.cs
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

using System;

namespace Kean.Serialize.Test.Data
{
	public class StringCast :
		IData
	{
		public string String { get; set; }
		public static implicit operator string(StringCast value)
		{
			return value.String;
		}
		public static implicit operator StringCast(string value)
		{
			return new StringCast() { String = value };
		}
		#region These are only here to test that we realy pick the right cast
		public static implicit operator float(StringCast value)
		{
			return 13.37f;
		}
		public static implicit operator StringCast(float value)
		{
			return new StringCast() { String = "13.37" };
		}
		#endregion

		#region IData
		public virtual void Initialize(IFactory factory)
		{
			this.String = factory.Create<string>();
		}
		public virtual void Verify(IFactory factory, string message, params object[] arguments)
		{
			factory.Verify(this.String, message, arguments);
		}
		#endregion
	}
}
