//
//  Parser.cs
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
using Collection = Kean.Collection;
using Kean;
using Kean.Extension;
using Text = System.Text;
using Uri = Kean.Uri;
using IO = Kean.IO;
using Error = Kean.Error;
namespace Kean.Xml.Sax
{
	public class Parser :
		IDisposable
	{
		public event Action<float, string, bool?, Uri.Region> OnDeclaration;
		public event Action<string, Collection.IList<Tuple<string, string, Uri.Region>>, Uri.Region> OnElementStart;
		public event Action<string, Uri.Region> OnElementEnd;
		public event Action<string, Uri.Region> OnText;
		public event Action<string, Uri.Region> OnData;
		public event Action<string, string, Uri.Region> OnProccessingInstruction;
		public event Action<string, Uri.Region> OnComment;
		IO.ICharacterReader reader;
		public Uri.Locator Resource { get { return this.reader.Resource; } }
		public Uri.Position Position { get { return new IO.Text.Position(this.reader); } }
		Parser(IO.ICharacterReader reader)
		{
			this.reader = reader;
		}
		~Parser()
		{
			this.Dispose();
		}
		public void Dispose()
		{
			if (this.reader.NotNull())
			{
				this.reader.Dispose();
				this.reader = null;
			}
		}
		IO.Text.Mark Mark()
		{
			return new IO.Text.Mark(this.reader);
		}
		public bool Parse()
		{
			bool result;
			if (Error.Log.CatchErrors)
			{
				try
				{
					result = this.ParseHelper();
				}
				catch (System.Exception)
				{
					Error.Log.Append(Error.Level.Message, "Error Parsing XML Document", "Failed parsing \"" + this.Resource + "\" on line: " + this.Position.Row + " and column: " + this.Position.Column); 
					result = false;
				}
			}
			else
				result = ParseHelper();
			return result;
		}
		bool ParseHelper()
		{
			this.reader.Next();
			while (!this.reader.Empty)
			{
				IO.Text.Mark mark = this.Mark();
				switch (this.reader.Last)
				{
					case '<':
						if (!this.reader.Next())
							return false;
						switch (this.reader.Last)
						{
							case '/': // end element
								this.OnElementEnd(this.AccumulateUntil('>').Trim(), mark);
								this.reader.Next();
								break;
							case '?': // proccessing instruction
								string target = this.AccumulateUntilWhiteSpace().Trim();
								if (target == "xml")
								{
									if (this.AccumulateUntil('=').Trim() != "version")
										return false;
									this.AccumulateUntil('"');
									float version = float.Parse(this.AccumulateUntil('"'), System.Globalization.CultureInfo.InvariantCulture);
									if (this.AccumulateUntil('=').Trim() != "encoding")
										return false;
									this.AccumulateUntil('"');
									string encoding = this.AccumulateUntil('"');
									this.AccumulateUntil("?>").Trim();
									this.OnDeclaration.Call(version, encoding, null, mark);
								}
								else
									this.OnProccessingInstruction.Call(target, this.AccumulateUntil("?>").Trim(), mark);
								this.reader.Next();
								break;
							case '!': // CDATA section or comment
								if (!this.reader.Next())
									return false;
								switch (this.reader.Last)
								{
									case '[': // CDATA section
										if (!this.Verify("CDATA["))
											return false;
										this.OnData.Call(this.AccumulateUntil("]]>"), mark);
										this.reader.Next();
										break;
									case '-': // comment
										if (!this.Verify("-"))
											return false;
										this.OnComment.Call(this.AccumulateUntil("-->"), mark);
										this.reader.Next();
										break;
								}
								break;
							default: // element name
								string name = this.reader.Last + this.AccumulateUntilWhiteSpaceOr('/', '>').Trim();
								Collection.IList<Tuple<string, string, Uri.Region>> attributes = new Collection.List<Tuple<string, string, Uri.Region>>();
								if (this.reader.Last != '/' && this.reader.Last != '>')
									do
									{
										IO.Text.Mark attributeMark = this.Mark();
										string key = (this.reader.Last + this.AccumulateUntil('=')).Trim();
										this.AccumulateUntil('"');
										attributes.Add(Tuple.Create(key, this.AccumulateUntil('"'), (Uri.Region)attributeMark));
										while (this.reader.Next() && this.reader.Last == ' ')
											;
									} while (this.reader.Last != '/' && this.reader.Last != '>' && !this.reader.Empty);
								this.OnElementStart.Call(name, attributes, mark);
								if (this.reader.Last == '/' && this.Verify(">"))
								{
									this.OnElementEnd(name, mark);
									this.reader.Next(); // Might have been the last element in fragment, eol acceptable.
								}
								else if (this.reader.Last != '>' || !this.reader.Next())
									return false;
								break;
						}
						break;
					default:
						string text = (this.reader.Last + this.AccumulateUntil('<')).Trim();
						if (text.NotEmpty())
							this.OnText.Call(text, mark);
						break;
				}
			}
			return true; // returns false on errors within the function body
		}
		bool Verify(string value)
		{
			bool result = true;
			for (int i = 0; i < value.Length; i++)
				if (!(result = this.reader.Next() && this.reader.Last == value[i]))
					break;
			return result;
		}
		string AccumulateUntil(params char[] needles)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !needles.Contains(this.reader.Last))
				result.Append(this.reader.Last);
			return result.ToString();
		}
		string AccumulateUntilWhiteSpace()
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !char.IsWhiteSpace(this.reader.Last))
				result.Append(this.reader.Last);
			return result.ToString();
		}
		string AccumulateUntilWhiteSpaceOr(params char[] needles)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			while (this.reader.Next() && !char.IsWhiteSpace(this.reader.Last) && !needles.Contains(this.reader.Last))
				result.Append(this.reader.Last);
			return result.ToString();
		}
		string AccumulateUntil(string needle)
		{
			Text.StringBuilder result = new Text.StringBuilder();
			int position = 0;
			while (this.reader.Next())
				if (this.reader.Last == needle[position])
				{
					if (++position == needle.Length)
						break;
				}
				else if (position > 0)
				{
					result.Append(needle.Substring(0, position));
					position = 0;
				}
				else
					result.Append(this.reader.Last);
			return result.ToString();
		}

		#region Static Open

		public static Parser Open(System.Reflection.Assembly assembly, Uri.Path path)
		{
			return Parser.Open(IO.ByteDevice.Open(assembly, path));
		}
		public static Parser Open(System.IO.Stream stream)
		{
			return Parser.Open(IO.ByteDevice.Open(stream));
		}
		public static Parser Open(IO.IByteInDevice device)
		{
			return Parser.Open(IO.CharacterDevice.Open(IO.ByteDeviceCombiner.Open(device)));
		}
		public static Parser Open(Uri.Locator resource)
		{
			return Parser.Open(IO.ByteDevice.Open(resource));
		}
		public static Parser Open(IO.ICharacterInDevice device)
		{
			return Parser.Open(IO.CharacterReader.Open(device));
		}
		public static Parser Open(IO.ICharacterReader reader)
		{
			return reader.NotNull() ? new Parser(reader) : null;
		}

		#endregion

	}
}
