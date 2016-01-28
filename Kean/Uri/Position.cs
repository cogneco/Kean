// 
//  Position.cs
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
using Kean;
using Kean.Extension;

namespace Kean.Uri
{
	public class Position :
		IEquatable<Position>
	{
		public int Row { get; private set; }
		public int Column { get; private set; }
		public Position(int row, int column)
		{
			this.Row = row;
			this.Column = column;
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Position);
		}
		public override int GetHashCode()
		{
			return this.Row.GetHashCode() ^ this.Column.GetHashCode();
		}
		public override string ToString()
		{
			return string.Format("Ln{0} Col{1}", this.Row, this.Column);
		}
		#endregion
		#region IEquatable<Position> Members
		public bool Equals(Position other)
		{
			return other.NotNull() && this.Row == other.Row && this.Column == other.Column;
		}
		#endregion
		#region Operators
		public static bool operator ==(Position left, Position right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Position left, Position right)
		{
			return !(left == right);
		}
		public static bool operator <(Position left, Position right)
		{
			return left.Row < right.Row || left.Row == right.Row && left.Column < right.Column;
		}
		public static bool operator >(Position left, Position right)
		{
			return left.Row > right.Row || left.Row == right.Row && left.Column > right.Column;
		}
		#endregion
	}
}
