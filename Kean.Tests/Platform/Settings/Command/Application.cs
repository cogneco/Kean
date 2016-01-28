// 
//  Application.cs
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
using Kean.Extension;
namespace Kean.Platform.Settings.Test.Command
{
	public class Application
	{
        
		[Object("configuration")]
		public Configuration Configuration { get; private set; }
		[Object("media", "Control the input media.")]
		public Media Media { get; private set; }
		[Object("geometryInteger", "2D Geometry types.")]
		public Geometry2D.Integer Integer { get; private set; }
		[Object("geometrySingle", "2D Geometry types.")]
		public Geometry2D.Single Single { get; private set; }
		[Object("geometryDouble", "2D Geometry types.")]
		public Geometry2D.Double Double { get; private set; }
		[Property("mode", "Enum test")]
		public Mode Mode { get; private set; }
		[Property("time", "Datetime test")]
		public DateTime Time { get; private set; }
        [Object("car", "Test of inheritance")]
        public Car Car { get; protected set; }

		public Application()
		{
            this.Configuration = new Configuration();
            this.Configuration.Load("this.is.something.very.long", "Description", "Usage", new Car());
			this.Media = new Media();
			this.Integer = new Geometry2D.Integer();
			this.Single = new Geometry2D.Single();
			this.Double = new Geometry2D.Double();
			this.Time = DateTime.Now;
            this.Car = new Car();
		}
	}
}
