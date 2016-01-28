// 
//  Decimal.cs
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
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Serialize.Data
{
	public class Decimal :
		Leaf<decimal>
	{
		public override string Text { get { return this.Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo); } }
		public override byte[] Raw 
		{ 
			get 
			{
				int[] bits = decimal.GetBits(this.Value);
				byte[] bytes0 = BitConverter.GetBytes(bits[0]);
				byte[] bytes1 = BitConverter.GetBytes(bits[1]);
				byte[] bytes2 = BitConverter.GetBytes(bits[2]);
				byte[] bytes3 = BitConverter.GetBytes(bits[3]);
				return new byte[] { bytes0[0], bytes0[1], bytes0[2], bytes0[3], bytes1[0], bytes1[1], bytes1[2], bytes1[3], bytes2[0], bytes2[1], bytes2[2], bytes2[3], bytes3[0], bytes3[1], bytes3[2], bytes3[3] }; 
			} 
		}
		public Decimal(decimal value) :
			base(value)
		{ }
		public Decimal(object value, Reflect.Type type) :
			base(value, type)
		{ }
	}
}
