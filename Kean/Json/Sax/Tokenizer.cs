//
//  Tokenizer.cs
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
using Text = System.Text;

namespace Kean.Json.Sax
{
	class Tokenizer :
		System.Collections.Generic.IEnumerable<Token>
	{
		IO.ICharacterReader reader;
		public Uri.Locator Resource { get { return this.reader.Resource; } }
		public Uri.Position Position { get { return new IO.Text.Position (this.reader); } }
		public Tokenizer (IO.ICharacterReader reader)
		{
			this.reader = reader;
		}
		IO.Text.Mark Mark ()
		{
			return new IO.Text.Mark (this.reader);
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}
		public System.Collections.Generic.IEnumerator<Token> GetEnumerator ()
		{
			this.reader.Next ();
			bool empty = false;
			while (!empty)
			{
				empty = this.reader.Empty;
				IO.Text.Mark mark = this.Mark ();
				switch (this.reader.Last)
				{
				case '{':
				case '}':
				case '[':
				case ']':
				case ':':
				case ',':
					yield return Token.Symbol.New (this.reader.Last, mark);
					this.reader.Next ();
					break;
				case 'n':
					if (this.Verify ("null"))
						yield return new Token.Null (mark);
					break;
				case 't':
					if (this.Verify ("true"))
						yield return new Token.Boolean (true, mark);
					break;
				case 'f':
					if (this.Verify ("false"))
						yield return new Token.Boolean (false, mark);
					break;
				case '"':
					{
						Text.StringBuilder result = new Text.StringBuilder ();
						while (this.reader.Next() && this.reader.Last != '"')
							if (this.reader.Last == '\\' && this.reader.Next ())
								switch (this.reader.Last)
								{
								case '"':
								case '\\':
								case '/':
								case 'b':
									result.Append ('\b');
									break;
								case 'f':
									result.Append ('\f');
									break;
								case 'n':
									result.Append ('\n');
									break;
								case 'r':
									result.Append ('\r');
									break;
								case 't':
									result.Append ('\t');
									break;
								case 'u': 
									char[] accumulated = new char[4];
									for (int i = 0; i < 4; i++)
										if (this.reader.Next ())
											accumulated [i] = this.reader.Last;
									int r;
									if (int.TryParse (new string (accumulated), System.Globalization.NumberStyles.HexNumber, null, out r))
										result.Append ((char)r);
									break;
								}
							else
								result.Append (this.reader.Last);
						this.reader.Next ();
						yield return new Token.String (result.ToString (), mark);
					}
					break;
				default:
					if ((char.IsDigit (this.reader.Last) || this.reader.Last == '.' || this.reader.Last == '-' || char.ToLower (this.reader.Last) == 'e'))
					{
						Text.StringBuilder result = new Text.StringBuilder ();
						do
						{
							result.Append (this.reader.Last); 
						}
						while (this.reader.Next() && (char.IsDigit(this.reader.Last) || this.reader.Last == '.' || this.reader.Last == '-' || char.ToLower(this.reader.Last) == 'e'));
						decimal value;
						if (decimal.TryParse (result.ToString (), System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent | System.Globalization.NumberStyles.AllowLeadingSign, null, out value))
							yield return new Token.Number (value, mark);
					}
					else
						this.reader.Next ();
					break;
				}
			}
			yield return Token.Symbol.New (',', this.Mark ()); // marks end of stream
		}
		string AccumulateUntilNonLetter ()
		{
			Text.StringBuilder result = new Text.StringBuilder ();
			do
			{
				result.Append (this.reader.Last); 
			}
			while (this.reader.Next() && char.IsLetter(this.reader.Last));
			return result.ToString ();
		}
		bool Verify (string value)
		{
			bool result = true;
			for (int i = 0; i < value.Length; i++)
				if (!(result = this.reader.Last == value [i] && (result = this.reader.Next ())))
					break;
			return result;
		}
	}
}

