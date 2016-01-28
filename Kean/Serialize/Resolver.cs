// 
//  Storage.cs
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
using System;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Kean.Reflect.Extension;

namespace Kean.Serialize
{
	public class Resolver
	{
		Collection.IDictionary<Uri.Locator, object> targets;
		Collection.IDictionary<object, Uri.Locator> reverse;
		Collection.IDictionary<Uri.Locator, Action<object>> looseEnds;
		public object this [Uri.Locator locator]
		{
			get { return this.targets[locator]; }
			set
			{				
				if (locator.NotNull() && value.NotNull())
				{
					if (this.targets[locator].IsNull())
					{
						this.targets[locator] = value;
						if (this.reverse[value].IsNull())
							this.reverse[value] = locator;
						Action<object> looseEnd = this.looseEnds[locator];
						if (looseEnd.NotNull())
							looseEnd.Call(value);
					}
					else
						new Exception.DuplicateIdentifier(locator).Throw();
				}
			}
		}
		public Uri.Locator this [object data]
		{
			get { return this.reverse[data]; }
			set { this[value] = data; }
		}
		public Resolver()
		{
			this.targets = new Collection.Dictionary<Uri.Locator, object>();
			this.reverse = new Collection.Dictionary<object, Uri.Locator>();
			this.looseEnds = new Collection.Dictionary<Uri.Locator, Action<object>>();
		}
		public bool Resolve(Uri.Locator locator, Action<object> set)
		{
			bool result;
			object target = this.targets[locator];
			if (result = target.NotNull())
				set.Call(target);
			else
			{
				Action<object> looseEnd = this.looseEnds[locator];
				if (looseEnd.NotNull())
					looseEnd += set;
				else
					this.looseEnds[locator] = set;
			}
			return result;
		}
		internal Uri.Locator Update(object data, Uri.Locator locator)
		{
			Uri.Locator result = null;
			if (!data.Type().Category.IsValue())
			{
				result = this[data];
				if (result.IsNull())
					this[data] = locator;
			}
			return result;
		}
	}
}
