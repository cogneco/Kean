
// Copyright (C) 2009  Simon Mika <simon@mika.se>
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

namespace Kean.Error
{
	public abstract class Exception :
		System.Exception
	{
		static Level threshold = Level.Warning;
		public static Level Threshold
		{
			get { return Exception.threshold; }
			set
			{
				if (value < Level.Recoverable)
					Exception.threshold = value;
			}
		}
		public DateTime Time { get; }
		public Level Level { get; }
		public string Title { get; }
		protected Exception(Level level, string title, string message, params object[] arguments) : this(null, level, title, message, arguments)
		{
		}
		protected Exception(System.Exception exception, Level level, string title, string message, params object[] arguments) :
			base(System.String.Format(message, arguments), exception)
		{
			this.Time = DateTime.Now;
			this.Level = level;
			this.Title = title;
		}
		public void Throw()
		{
			if (this.Level >= Exception.Threshold)
				throw this;
		}
	}
}
