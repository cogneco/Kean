// 
//  DateTime.cs
//  
//  Author:
//       Anders Frisk <anderfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using System.Text.RegularExpressions;

namespace Kean.Platform.Settings.Parameter
{
	class DateTime :
		Abstract
	{
		internal DateTime(Reflect.Type type) :
			base(type)
		{
		}
		public override string AsString(object value)
		{
			return ((System.DateTime)value).ToString("o");
		}
		public override object FromString(string value)
		{
			System.DateTime result;
			return 
				System.DateTime.TryParseExact(value, new string[] {"ss", "ss.fff", "mm:ss", "mm:ss.fff", "HH:mm:ss", "HH:mm:ss.fff"} , System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out result) ||
				System.DateTime.TryParse(value, out result) ?
				(object)result : null;
		}
		public override string Complete(string incomplete)
		{
			return incomplete.NotEmpty() && char.IsDigit(incomplete[0]) ? incomplete : "";
		}
		public override string Help(string incomplete)
		{
			return "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
		}
	}
}
