// 
//  List.cs
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
using System;
using Kean.Extension;

namespace Kean.Collection.Sorted
{
	public class List<T> :
		IList<T>
	{
		IList<T> data;
		Kean.Comparer<T> comparer;
		#region Constructors
		public List() :
			this(new Collection.List<T>())
		{ }
		public List(IList<T> data) :
			this((Kean.Comparer<T>) List<T>.Compare, data)
		{ }
		public List(Kean.Comparer<T> comparer) :
			this(comparer, new Collection.List<T>())
		{ }
		public List(Kean.Comparer<T> comparer, IList<T> data)
		{
			this.comparer = comparer;
			this.data = data;
		}
		public List(Comparison<T> comparison) :
			this(comparison, new Collection.List<T>())
		{ }
		public List(Comparison<T> comparison, IList<T> data) :
			this(comparison.AsComparer())
		{ }
		#endregion
		#region IList<T> Members
		public Collection.IList<T> Add(T item)
		{
			if (this.data.Count != 0)
			{
				for (int i = 0; i < this.data.Count; i++)
					switch (this.comparer(this.data[i], item))
					{
						case Kean.Order.LessThan:
							break;
						case Kean.Order.Equal:
						case Kean.Order.GreaterThan:
							this.data.Insert(i, item);
							goto Done;
					}
				this.data.Insert(this.data.Count, item);
			Done:
				;
			}
			else
				this.data.Add(item);
			return this;
		}
		public T Remove()
		{
			return this.data.Remove();
		}
		public Collection.IList<T> Insert(int index, T item)
		{
			return this.Add(item);
		}
		public T Remove(int index)
		{
			return this.data.Remove(index);
		}
		#endregion

		#region IVector<T> Members
		public int Count
		{
			get { return this.data.Count; }
		}
		public T this[int index]
		{
			get { return this.data[index]; }
			set
			{
				if (this.comparer(this.data[index], value) == Kean.Order.Equal)
					this.data[index] = value;
				else
				{
					this.Remove(index);
					this.Add(value);
				}
			}
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

		#region IEquatable<IVector<T>> Members
		public bool Equals(IVector<T> other)
		{
			return this.data.Equals(other);
		}
		#endregion
		#region Static
		static Order Compare(T left, T right)
		{
			Order result;
			if (left is IComparable<T>)
				result = List<T>.Compare(left as IComparable<T>, right);
			else if (left is IComparable<T>)
				result = List<T>.Compare(left as IComparable<T>, right);
			else if (left is IComparable)
				result = List<T>.Compare(left as IComparable, right);
			else
				throw new Exception.Unsortable(typeof(T));
			return result;
		}
		static Order Compare(IComparable<T> left, T right)
		{
			return left.Compare(right);
		}
		static Order Compare(IComparable left, T right)
		{
			return List<T>.Order(left.CompareTo(right));
		}
		static Order Order(int order)
		{
			return order > 0 ? Kean.Order.GreaterThan : order == 0 ? Kean.Order.Equal : Kean.Order.LessThan;
		}
		#endregion
	}
}
