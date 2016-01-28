// 
//  ListSet.cs
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
using System;
using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.Collection.Abstract
{
    public abstract class ListSet<T> :
        ISet<T>
    {
        IList<T> data;
        protected ListSet()
        {
            this.data = this.CreateList();
        }
        protected abstract IList<T> CreateList();

        #region ISet<T> Members
        public int Count { get { return this.data.Count; } }
        public bool Add(T item)
        {
            bool result = -1 == this.data.Index(t => t.SameOrEquals(item));
            if (result)
                this.data.Add(item);
            return result;
        }
        public bool Remove(T item)
        {
            return this.data.Remove(t => t.SameOrEquals(item));
        }
        public bool Contains(T item)
        {
            return this.data.Exists(t => t.SameOrEquals(item));
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
            return this.Contains(other) && other.Contains(this);
        }
        #endregion
        #region Object overrides
        public override bool Equals(object other)
        {
            return this.Equals(other as ISet<T>);
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
