// 
//  Program.cs
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
using Error = Kean.Error;

namespace Kean.Platform.Settings.Test
{
	public class Object :
		Dynamic
	{
		[Settings.Property("name", "Name of configuration.", "The name of the current configuration.")]
		public string Name { get; set; }
		[Settings.Property("type", "Type of configuration.", "The type of the current configuration.")]
		public string Type { get; set; }
		[Settings.Property("comment", "Comment describing the configuration.", "Comment that describes the current configuration.")]
		public string Comment { get; set; }

		[Settings.Method("load")]
		public void Load(string name)
		{
			this.Load(name, new Object());
		}
		[Settings.Method("unload")]
		public new void Unload(string name)
		{
			base.Unload(name);
		}
		[Settings.Method("throw")]
		public void Throw()
		{
			Error.Log.Append(Error.Level.Recoverable, "Test Error", new System.ArgumentException("Test error"));
		}
	}
}
