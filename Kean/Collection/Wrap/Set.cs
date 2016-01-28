// 
//  Set.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;

namespace Kean.Collection.Wrap
{
    public class Set<T> :
        ISet<T>
    {
        ISet<T> data;
        public Set(ISet<T> data)
        {
            this.data = data;
        }

        #region ISet<T> Members
        public int Count { get { return this.data.Count; } }
        public bool Add(T item)
        {
            return this.data.Add(item);
        }
        public bool Remove(T item)
        {
            return this.data.Remove(item);
        }
        public bool Contains(T item)
        {
            return this.data.Contains(item);
        }
        #endregion

        #region IEnumerable<T> Members
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }
        #endregion
        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region IEquatable<ISet<T>> Members
        public bool Equals(ISet<T> other)
        {
            return this.data.Equals(other);
        }
        #endregion
        #region Object overrides
        public override bool Equals(object other)
        {
            return this.data.Equals(other);
        }
        public override int GetHashCode()
        {
            return this.data.GetHashCode();
        }
        public override string ToString()
        {
            return this.data.ToString();
        }
        #endregion
    }
}
