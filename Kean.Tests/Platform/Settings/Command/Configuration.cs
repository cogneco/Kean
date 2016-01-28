// 
//  Configuration.cs
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

namespace Kean.Platform.Settings.Test.Command
{
	public class Configuration :
        Dynamic
	{
		[Property("name", "Name of configuration.", "The name of the current configuration.")]
		public string Name { get; set; }
		[Property("type", "Type of configuration.", "The type of the current configuration.")]
		public string Type { get; set; }
		[Property("comment", "Comment describing the configuration.", "Comment that describes the current configuration.")]
		public string Comment { get; set; }
		public Configuration()
		{
		}
		//[Method("save", "Saves configuration.", "Saves the current configuration to the default path.")]
		//public void Save()
		//{
		//}
		[Method("save", "Saves configuration.", "Saves the current configuration to the default path.")]
		public void Save(bool overwrite)
		{
		}
		//[Method("save", "Saves configuration.", "Saves the current configuration to the default path.")]
		//public void Save([Parameter("Filename of file to save to.")] string filename)
		//{
		//}
	}
}
