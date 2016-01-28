//
//  Parser.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2011-2012 Simon Mika
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

namespace Kean.Json.Sax
{
	public class Parser
	{
		public event Action<string, Uri.Region, Uri.Region> OnObjectStart;
		public event Action<Uri.Region> OnObjectEnd;
		public event Action<string, Uri.Region, Uri.Region> OnArrayStart;
		public event Action<Uri.Region> OnArrayEnd;
		public event Action<string, Uri.Region, Uri.Region> OnNull;
		public event Action<string, Uri.Region, string, Uri.Region> OnString;
		public event Action<string, Uri.Region, decimal, Uri.Region> OnNumber;
		public event Action<string, Uri.Region, bool, Uri.Region> OnBoolean;
		Tokenizer tokenizer;
		public Uri.Locator Resource { get { return this.tokenizer.Resource; } }
		public Uri.Position Position { get { return this.tokenizer.Position; } }
		Parser(IO.ICharacterReader reader)
		{
			this.tokenizer = new Tokenizer(reader);
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
				catch (System.Exception e)
				{
					Error.Log.Append(Error.Level.Message, "Error Parsing JSON Document", "Failed parsing \"" + this.Resource + "\" on line: " + this.Position.Row + " and column: " + this.Position.Column + " with error: \"" + e.Message + "\"");
					result = false;
				}
			}
			else
				result = ParseHelper();
			return result;
		}
		bool ParseHelper()
		{
			Collection.Stack<Token> stack = new Collection.Stack<Token>();
			foreach (Token token in this.tokenizer)
			{
				if (token is Token.Symbol)
					switch ((token as Token.Symbol).Value)
					{
						case '{': 
							{
								Token.Colon colon;
								Token.String label;
								if (!stack.Empty && (colon = stack.Pop() as Token.Colon).NotNull() && (label = stack.Pop() as Token.String).NotNull())
									this.OnObjectStart.Call(label.Value, label.Region, label.Region + colon.Region + token.Region);
								else
									this.OnObjectStart.Call(null, null, token.Region); 
								break;
							}
						case '[':
							{
								Token.Colon colon;
								Token.String label;
								if (!stack.Empty && (colon = stack.Pop() as Token.Colon).NotNull() && (label = stack.Pop() as Token.String).NotNull())
									this.OnArrayStart.Call(label.Value, label.Region, label.Region + colon.Region + token.Region); 
								break;
							}
						case ':':
							stack.Push(token);
							break;
						case '}':
						case ']':
						case ',':
							{
								if (!stack.Empty)
								{
									Token previous = stack.Pop();
									Token.Colon colon;
									string labelValue = null;
									Uri.Region labelRegion = null;
									Token.String label;
									Uri.Region region = previous.Region + token.Region;
									if (!stack.Empty && (colon = stack.Pop() as Token.Colon).NotNull() && (label = stack.Pop() as Token.String).NotNull())
									{
										region = label.Region + colon.Region + region;
										labelValue = label.Value;
										labelRegion = label.Region;
									}
									if (previous is Token.Null)
										this.OnNull.Call(labelValue, labelRegion, region);
									else if (previous is Token.String)
										this.OnString.Call(labelValue, labelRegion, (previous as Token.String).Value, region);
									else if (previous is Token.Number)
										this.OnNumber.Call(labelValue, labelRegion, (previous as Token.Number).Value, region);
									else if (previous is Token.Boolean)
										this.OnBoolean.Call(labelValue, labelRegion, (previous as Token.Boolean).Value, region);
									if (token is Token.RightBrace)
										this.OnObjectEnd.Call(token.Region);
									else if (token is Token.RightBracket)
										this.OnArrayEnd.Call(token.Region);
								}
								break;
							}
						default:
							char symbol = (token as Token.Symbol).Value;
							Console.Write(symbol);
							break;
					}
				else
					stack.Push(token);
			}
			return stack.Empty;
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
		public static Parser Open(Uri.Locator resource)
		{
			return Parser.Open(IO.ByteDevice.Open(resource));
		}
		public static Parser Open(IO.IByteInDevice device)
		{
			return Parser.Open(IO.ByteDeviceCombiner.Open(device));
		}
		public static Parser Open(IO.IByteDevice device)
		{
			return Parser.Open(IO.CharacterDevice.Open(device));
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
