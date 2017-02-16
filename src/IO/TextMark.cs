// Copyright (C) 2011, 2017  Simon Mika <simon@mika.se>
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

using Kean.Extension;

namespace Kean.IO
{
	public class TextMark
	{
		string content;
		ITextReader reader;
		Text.Position start;
		Text.Position? end;
		public TextMark(ITextReader reader)
		{
			this.reader = reader;
			this.start = reader.Position;
			reader.OnNext += character => this.content += character;
		}
		public void End()
		{
			this.end = reader.Position;
		}
		public static implicit operator Text.Fragment(TextMark mark)
		{
			return mark.IsNull() ? null : new Text.Fragment(mark.content, mark.start, mark.end ?? mark.reader.Position, mark.reader.Resource);
		}
	}
}