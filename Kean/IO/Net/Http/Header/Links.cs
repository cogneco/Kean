//
//  Links.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
using IO = Kean.IO;
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;

namespace Kean.IO.Net.Http.Header
{
	public class Links
	{
		readonly Action<string> changed;
		readonly Collection.List<Link> backend = new Collection.List<Link>();
		internal Links(string links, Action<string> changed)
		{
			if (links.NotEmpty())
				this.Add(((Generic.IEnumerable<string>)links.SplitAt(',')).Map(link => (Link)link));
			this.changed = changed;
		}
		public Links Add(Link item)
		{
			this.backend.Add(item);
			this.Changed();
			return this;
		}
		public Links Add(params Link[] items)
		{
			this.Add((Generic.IEnumerable<Link>)items);
			return this;
		}
		public Links Add(Generic.IEnumerable<Link> items)
		{
			this.backend.Add(items);
			this.Changed();
			return this;
		}
		public Links Remove(LinkRelation relation)
		{
			this.backend.Remove(link => link.Relatation == relation);
			this.Changed();
			return this;
		}
		void Changed()
		{
			if (this.changed.NotNull())
				this.changed(this);
		}
		public static implicit operator string(Links links)
		{
			return (links.NotNull() && links.backend.Count > 0) ? ((Generic.IEnumerable<Link>)links.backend).Map(link => (string)link).Join(", ") : null;
		}
	}
}

