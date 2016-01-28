// 
//  Parser.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2011 Simon Mika
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

namespace Kean.Cli.Argument
{
	/// <summary>
	/// Contains handlers for command line arguments and parses the argument list.
	/// </summary>
	public class Parser
	{
		enum TokenType
		{
			Short,
			Long,
			Parameter,
		}
		struct Token
		{
			public TokenType Type;
			public string Value;
			public Token(TokenType type, string value)
				: this()
			{
				this.Type = type;
				this.Value = value;
			}
		}

		Collection.List<Argument> arguments;

		Argument this[char identifier]
		{
			get { return this.arguments.Find(argument => argument.Short == identifier); }
		}
		Argument this[string identifier]
		{
			get { return this.arguments.Find(argument => argument.Long == identifier); }
		}
		/// <summary>
		/// Handler to handle parameters not associated with an argument.
		/// </summary>
		public Action<string> UnassociatedParameterHandler { get; set; }
		
		/// <summary>
		/// Creates a new parser.
		/// </summary>
		public Parser()
		{
			this.arguments = new Collection.List<Argument>();
		}

		/// <summary>
		/// Adds a new argument handler to the parser.
		/// Used for arguments with 0 parameters.
		/// </summary>
		/// <param name="shortIdentifier">Argument short form character (for example v for -v).</param>
		/// <param name="longIdentifier">Argument long form string (for example version for --version).</param>
		/// <param name="handler">Function to call when argument is found during parsing.</param>
		public void Add(char shortIdentifier, string longIdentifier, Action handler)
		{
			this.Add(shortIdentifier, longIdentifier, 0, s => handler.Call());
		}
		public void Add(char shortIdentifier, string longIdentifier, Action<string> handler)
		{
			this.Add(shortIdentifier, longIdentifier, 1, s => handler.Call(s[0]));
		}
		public void Add(char shortIdentifier, string longIdentifier, Action<string, string> handler)
		{
			this.Add(shortIdentifier, longIdentifier, 2, s => handler.Call(s[0], s[1]));
		}
		public void Add(char shortIdentifier, string longIdentifier, Action<string, string, string> handler)
		{
			this.Add(shortIdentifier, longIdentifier, 3, s => handler.Call(s[0], s[1], s[2]));
		}
		public void Add(char shortIdentifier, string longIdentifier, Action<string, string, string, string> handler)
		{
			this.Add(shortIdentifier, longIdentifier, 4, s => handler.Call(s[0], s[1], s[2], s[3]));
		}
		public void Add(char shortIdentifier, string longIdentifier, Action<string, string, string, string, string> handler)
		{
			this.Add(shortIdentifier, longIdentifier, 5, s => handler.Call(s[0], s[1], s[2], s[3], s[4]));
		}
		public void Add(char shortIdentifier, string longIdentifier, Action<string, string, string, string, string, string> handler)
		{
			this.Add(shortIdentifier, longIdentifier, 6, s => handler.Call(s[0], s[1], s[2], s[3], s[4], s[5]));
		}
		public void Add(char shortIdentifier, string longIdentifier, Action<string, string, string, string, string, string, string> handler)
		{
			this.Add(shortIdentifier, longIdentifier, 7, s => handler.Call(s[0], s[1], s[2], s[3], s[4], s[5], s[6]));
		}
		/// <summary>
		/// Adds a new argument handler to the parser.
		/// </summary>
		/// <param name="shortIdentifier">Argument short form character (for example v for -v).</param>
		/// <param name="longIdentifier">Argument long form string (for example version for --version).</param>
		/// <param name="parameters">Number of required parameters.</param>
		/// <param name="handler">Function to call when argument is found during parsing.</param>
		public void Add(char shortIdentifier, string longIdentifier, int parameters, Action<string[]> handler)
		{
			this.arguments.Add(new Argument() { Short = shortIdentifier, Long = longIdentifier, Parameters = parameters, Handler = handler });
		}
		
		/// <summary>
		/// Parses the argument list and activates the respective callbacks.
		/// </summary>
		/// <param name="arguments">Argument list to parse.</param>
		/// <returns></returns>
		public bool Parse(string[] arguments)
		{
			bool result = true;
			Collection.Queue<Token> tokens = new Collection.Queue<Token>();
			foreach(string argument in arguments)
			{
				if (argument.StartsWith("--"))
					tokens.Enqueue(new Token(TokenType.Long, argument.Substring(2)));
				else if (argument.StartsWith("-") && argument.Length > 1 && char.IsLetter(argument[1]))
				{
					foreach (char c in argument.Substring(1))
						tokens.Enqueue(new Token(TokenType.Short, c.ToString()));
				}
				else
					tokens.Enqueue(new Token(TokenType.Parameter, argument));
			}
			while (tokens.Count > 0 && result)
			{
				Argument argument = null;
				switch (tokens.Peek().Type)
				{
					case TokenType.Long:
						argument = this[tokens.Dequeue().Value];
						break;
					case TokenType.Short:
						argument = this[tokens.Dequeue().Value[0]];
						break;
					case TokenType.Parameter:
						this.UnassociatedParameterHandler.Call(tokens.Dequeue().Value);
						break;
				}
				if (argument.NotNull() && result)
				{
					Collection.List<string> parameters = new Collection.List<string>();
					for (int i = 0; i < argument.Parameters && result; i++)
					{
						if (!tokens.Empty && tokens.Peek().Type == TokenType.Parameter)
							parameters.Add(tokens.Dequeue().Value);
						else
							result = false;
					}
					if (result)
						argument.Handler(parameters.ToArray());
				}
			}
			return result;
		}
	}
}
