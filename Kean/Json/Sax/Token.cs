//
//  Token.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2012 Simon Mika
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
using Uri = Kean.Uri;

namespace Kean.Json.Sax
{
	abstract class Token
	{
		public Uri.Region Region { get; private set; }
		protected Token (IO.Text.Mark mark)
		{
			this.Region = mark;
		}
		public class Null : Token { public Null (IO.Text.Mark mark) : base(mark) { } }
		public class Symbol : Token 
		{ 
			public char Value { get; private set; }
			protected Symbol (char value, IO.Text.Mark mark) : base(mark) { this.Value = value; } 
			public static Symbol New(char value, IO.Text.Mark mark)
			{
				Symbol result;
				switch(value)
				{
				case ':': result = new Colon(mark); break;
				case ',': result = new Comma(mark); break;
				case '{': result = new LeftBrace(mark); break;
				case '}': result = new RightBrace(mark); break;
				case '[': result = new LeftBracket(mark); break;
				case ']': result = new RightBracket(mark); break;
				default: result = null; break;
				}
				return result;
			}
		}
		public class Colon : Symbol { public Colon(IO.Text.Mark mark) : base(':', mark) { } }
		public class Comma : Symbol { public Comma(IO.Text.Mark mark) : base(',', mark) { } }
		public class LeftBrace : Symbol { public LeftBrace(IO.Text.Mark mark) : base('{', mark) { } }
		public class RightBrace : Symbol { public RightBrace(IO.Text.Mark mark) : base('}', mark) { } }
		public class LeftBracket : Symbol { public LeftBracket(IO.Text.Mark mark) : base('[', mark) { } }
		public class RightBracket : Symbol { public RightBracket(IO.Text.Mark mark) : base(']', mark) { } }
		public abstract class Primitive<T> :
			Token
		{
			public T Value { get; private set; }
			public Primitive (T value, IO.Text.Mark mark) : base(mark) { this.Value = value; }
		}
		public class Boolean : Primitive<bool> { public Boolean (bool value, IO.Text.Mark mark) : base(value, mark) { } }
		public class String : Primitive<string> { public String (string value, IO.Text.Mark mark) : base(value, mark) { } }
		public class Number : Primitive<decimal> { public Number (decimal value, IO.Text.Mark mark) : base(value, mark) { } }
	}
}

