// Copyright (C) 2011, 2017	Simon Mika <simon@mika.se>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.	If not, see <http://www.gnu.org/licenses/>.
//

using Tasks = System.Threading.Tasks;
using Kean.Extension;

namespace Kean.IO
{
	public class TextMark
	{
		Text.Builder content;
		ITextReader reader;
		Tasks.Task<Text.Position> start;
		Tasks.Task<Text.Position> end;
		public TextMark(ITextReader reader)
		{
			this.reader = reader;
			this.start = reader.Position;
			reader.OnRead += this.Read;
		}
		void Read(char character)
		{
			this.content += character;
		}
		public void End()
		{
			this.reader.OnRead -= this.Read; // Stop receiving new Read events, already started once will styll arrive.
			this.end = reader.Position;
		}
		async Tasks.Task<Text.Fragment> ToFragment()
		{
			this.End();
			var start = await this.start;
			var end = await (this.end	?? this.reader.Position);
			return new Text.Fragment(this.content, start, end, this.reader.Resource);
		}
		public static implicit operator Tasks.Task<Text.Fragment>(TextMark mark)
		{
			return mark.IsNull() ? null : mark.ToFragment();
		}
	}
}