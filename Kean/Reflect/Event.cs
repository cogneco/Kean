// 
//  Property.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
	public class Event : 
		EventInformation,
		IComparable<Event>
	{
		protected object Parent { get; private set; }
		Event(object parent, Type parentType, System.Reflection.EventInfo information) :
			base(parentType, information)
		{
			this.Parent = parent;
		}
		public void Add(System.Delegate handler)
		{
			this.Information.AddEventHandler(this.Parent, handler);
		}
		public void Remove(System.Delegate handler)
		{
			this.Information.RemoveEventHandler(this.Parent, handler);
		}
		#region IComparable<Event> Members
		Order IComparable<Event>.Compare(Event other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
		internal static Event Create(object parent, Type parentType, System.Reflection.EventInfo information)
		{
			return information.NotNull() ? new Event(parent, parentType, information) : null;
		}
	}
}

