//
//  Range.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2015 Simon Mika
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
using Long = Kean.Math.Long;
using Kean.Extension;

namespace Kean.IO.Net.Http.Header
{
	public class Range
	{
		public string Type { get; set; }
		public long? First { get; set; }
		public long? Last { get; set; }
		public long? Size { get { return this.Last - this.First + 1; } }
		public long? Total { get; set; }
		public Range() 
		{
		}
		public Range(string type, long? first, long? last, long? total = null)
		{
			this.Type = type;
			this.First = first;
			this.Last = last;
			this.Total = total;
		}
		public static implicit operator string(Range range) 
		{
			return range.NotNull() ? "{0} {1}-{2}{3}".Format((object)range.Type, range.First.HasValue ? range.First.Value.AsString() : "", range.Last.HasValue ? range.Last.Value.AsString() : "", range.Total.HasValue ? "/" + range.Total.Value.AsString() : "") : null;
		}
		public static implicit operator Range(string range) 
		{
			Range result;
			if (range.IsEmpty())
				result = null;
			else 
			{
				var splitted = range.Split(new char[] { '=' }, 2);
				result = new Range();
				result.Type = splitted[0].Trim();
				splitted = splitted[1].Split(new char[] { '-' }, 2);
				result.First = splitted[0].IsEmpty() ? (long?)null : Long.Parse(splitted[0]);
				splitted = splitted[1].Split(new char[] { '/' }, 2);
				result.Last = splitted[0].IsEmpty() ? (long?)null : Long.Parse(splitted[0]);
				result.Total = splitted.Length < 2 || splitted[1].IsEmpty() ? (long?)null : Long.Parse(splitted[1]);
			}
			return result;
		}
	}
}