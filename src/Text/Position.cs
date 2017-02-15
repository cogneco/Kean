// Copyright (C) 2011, 2012, 2017  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Kean.Extension;

namespace Kean.Text
{
	public struct Position :
		IEquatable<Position>
	{
		public int Row { get; }
		public int Column { get; }
		public Position(int row, int column)
		{
			this.Row = row;
			this.Column = column;
		}
		public Position AddNewLine()
		{
			return new Position(this.Row + 1, 1);
		}
		public Position Increase(int delta = 1)
		{
			return new Position(this.Row, this.Column + delta);
		}
		public Position Increase(char data)
		{
			return data == '\n' ? this.AddNewLine() : this.Increase();
		}
		public Position Increase(string data)
		{
			return data.Length == 0 ? this : this.Increase(data.Substring(1));
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Position && this.Equals((Position)other);
		}
		public override int GetHashCode()
		{
			return this.Row.Hash(this.Column);
		}
		public override string ToString()
		{
			return string.Format("Ln{0} Col{1}", this.Row, this.Column);
		}
		#endregion
		#region IEquatable<Position> Members
		public bool Equals(Position other)
		{
			return this.Row == other.Row && this.Column == other.Column;
		}
		#endregion
		#region Operators
		public static bool operator ==(Position left, Position right)
		{
			return left.Equals(right);
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
		public static Position operator +(Position left, char right)
		{
			return left.Increase(right);
		}
		public static Position operator +(Position left, string right)
		{
			return left.Increase(right);
		}
		public static Position operator +(Position left, int right)
		{
			return left.Increase(right);
		}
		public static Position operator -(Position left, int right)
		{
			return left.Increase(-right);
		}
		#endregion
	}
}