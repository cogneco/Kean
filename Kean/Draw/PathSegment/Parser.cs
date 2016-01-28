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
using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Collection;

namespace Kean.Draw.PathSegment
{
	public class Parser 
	{
		public Parser()
		{
		}
		public Abstract Result { get; set; }
		public Abstract Parse(string path)
		{
			Collection.IQueue<object> arguments = new Tokenizer().Tokenize(path);
			while(arguments.Count > 0)
			{
				char segmentType = (char)arguments.Dequeue();
				switch (segmentType)
				{
					case 'Z' :
						this.Append(new Close());
						break;
					case 'C':
						this.Append(new CurveTo(
						    new Geometry2D.Single.Point((float)arguments.Dequeue(), (float)arguments.Dequeue()), // first
						    new Geometry2D.Single.Point((float)arguments.Dequeue(), (float)arguments.Dequeue()), // second
						    new Geometry2D.Single.Point((float)arguments.Dequeue(), (float)arguments.Dequeue()))); // end
						break;
					case 'L':
					    this.Append(new LineTo(
							new Geometry2D.Single.Point((float)arguments.Dequeue(), (float)arguments.Dequeue()))); //end
					    break;
					case 'M':
					    this.Append(new MoveTo(
							new Geometry2D.Single.Point((float)arguments.Dequeue(), (float)arguments.Dequeue()))); //end
						break;
					case 'A':
						this.Append(new EllipticalArcTo(
							new Geometry2D.Single.Size((float)arguments.Dequeue(), (float)arguments.Dequeue()),
							(float)arguments.Dequeue(), (float)arguments.Dequeue() == 1, (float)arguments.Dequeue() == 1,
							new Geometry2D.Single.Point((float)arguments.Dequeue(), (float)arguments.Dequeue())));
						break;
				}
			}

			return this.Result;
		}
		private void Append(Abstract segment)
		{
			if (this.Result.IsNull())
				this.Result = segment;
			else
				this.Result.Append(segment);
		}
		class Tokenizer
		{
			Collection.IQueue<object> arguments;
			System.Text.StringBuilder token;
			public Collection.IQueue<object> Tokenize(string path)
			{
				this.arguments = new Collection.Queue<object>();
				this.token = new System.Text.StringBuilder();
				foreach (char c in path)
				{
					if (char.IsDigit(c) || c == '.' || c == '-')
						this.token.Append(c);
					else if (c == 'A' || c == 'C' || c == 'L' || c == 'M' || c == 'Z')
					{
						this.AddArgument();
						this.arguments.Enqueue(c);
					}
					else
						this.AddArgument();

				}
				this.AddArgument();
				return this.arguments;
			}
			void AddArgument()
			{
				if (this.token.Length > 0)
				{
					this.arguments.Enqueue(Kean.Math.Single.Parse(this.token.ToString()));
					this.token = new System.Text.StringBuilder();
				}
			}
		}
	}
}
