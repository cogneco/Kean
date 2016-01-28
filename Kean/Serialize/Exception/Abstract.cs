// 
//  Exception.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2011 Simon Mika
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

namespace Kean.Serialize.Exception
{
	public class Abstract :
		Error.Exception
	{
		internal Abstract(Error.Level level, string title, string message, params object[] arguments) : 
			this(null, level, title, message, arguments) { }
		internal Abstract(System.Exception innerException, Error.Level level, string title, string message, params object[] arguments) : 
			base(innerException, level, title, message, arguments) { }

		protected static string Location(string prefix, Uri.Region region)
		{
			return Abstract.Location(prefix, region, "");
		}
		protected static string Location(Uri.Region region, string postfix)
		{
			return Abstract.Location("", region, postfix);
		}
		protected static string Location(string prefix, Uri.Region region, string postfix)
		{
			string result = Abstract.Location(region);
			if (result.NotEmpty())
				result = prefix + result + postfix;
			return result;
		}
		protected static string Location(Uri.Region region)
		{
			string result;
			if (region.IsNull())
				result = "";
			else if (region.Start.Row == region.End.Row)
				result = string.Format("on line {0} between column {1} and {2} in file \"{3}\"", region.Start.Row, region.Start.Column, region.End.Column, region.Resource);
			else
				result = string.Format("between line {0} column {1} and line {2} column {3} in file \"{4}\"", region.Start.Row, region.Start.Column, region.End.Row, region.End.Column, region.Resource);
			return result;
		}
	}
}
