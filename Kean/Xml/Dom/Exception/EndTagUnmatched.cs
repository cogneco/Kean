// 
//  EndTagUnmatched.cs
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

using Error = Kean.Error;
using Uri = Kean.Uri;

namespace Kean.Xml.Dom.Exception
{
	public class EndTagUnmatched : 
		Abstract
	{
		public string StartName { get; private set; }
		public Uri.Region StartRegion { get; private set; }
		public string EndName { get; private set; }
		public Uri.Region EndRegion { get; private set; }
		internal EndTagUnmatched(string startName, Uri.Region startRegion, string endName, Uri.Region endRegion) : 
			this(null, startName, startRegion, endName, endRegion) 
		{ }
		internal EndTagUnmatched(System.Exception innerException, string startName, Uri.Region startRegion, string endName, Uri.Region endRegion) :
			base(innerException, Error.Level.Recoverable, "XML Tree error", "XML end element \"{5}\" between row {6} column {7} and row {8} column {9} does not match start element \"{0}\" between row {1} column {2} and row {3} column {4} in \"{10}\".", startName, startRegion.Start.Row.ToString(), startRegion.Start.Column.ToString(), startRegion.End.Row.ToString(), startRegion.End.Column.ToString(), endName, endRegion.Start.Row.ToString(), endRegion.Start.Column.ToString(), endRegion.End.Row.ToString(), endRegion.End.Column.ToString(), endRegion.Resource)
		{
			this.StartName = startName;
			this.StartRegion = startRegion;
			this.EndName = endName;
			this.EndRegion = endRegion;
		}
	}
}