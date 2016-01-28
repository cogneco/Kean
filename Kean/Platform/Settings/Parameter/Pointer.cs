// 
//  Pointer.cs
//  
//  Author:
//       Anders Frisk <anderfrisk77@gmail.com>
//		 Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Anders Frisk, 2013 Simon Mika
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

namespace Kean.Platform.Settings.Parameter
{
	class Pointer :
		Abstract
	{
		internal Pointer(Reflect.Type type) :
			base(type)
		{
		}
		public override string AsString(object value)
		{
			return ((int)(IntPtr)value).ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
		public override object FromString(string value)
		{
			int result = 0;
			int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out result);
			return (IntPtr)result;
		}
		public override string Complete(string incomplete)
		{
			return incomplete;
		}
		public override string Help(string incomplete)
		{
			return "";
		}
	}
}
