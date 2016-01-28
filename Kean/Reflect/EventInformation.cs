//
//  EventInformation.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2010-2012 Simon Mika
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

namespace Kean.Reflect
{
	public class EventInformation : 
		Member,
		IComparable<EventInformation>
	{
		protected System.Reflection.EventInfo Information { get; private set; }
		internal EventInformation(Type parentType, System.Reflection.EventInfo information) :
			base(parentType, information)
		{
			this.Information = information;
		}
		#region IComparable<EventInformation> Members
		Order IComparable<EventInformation>.Compare(EventInformation other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
	}
}


