// 
//  StartTagMissing.cs
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

using Error = Kean.Error;
using Uri = Kean.Uri;

namespace Kean.Xml.Dom.Exception
{
	public class StartTagMissing : 
		Abstract
	{
		public string Name { get; private set; }
		public Uri.Region Region { get; private set; }
		internal StartTagMissing(string name, Uri.Region region) : 
			this(null, name, region) 
		{ }
		internal StartTagMissing(System.Exception innerException, string name, Uri.Region region) :
			base(innerException, Error.Level.Recoverable, "XML Tree error", "XML end element \"{0}\" between row {1} column {2} and row {3} column {4} does not have a corresponding start tag in \"{5}\".", name, region.Start.Row.ToString(), region.Start.Column.ToString(), region.End.Row.ToString(), region.End.Column.ToString(), region.Resource)
		{
			this.Name = name;
			this.Region = region;
		}
	}
}