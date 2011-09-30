﻿// 
//  Single.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Cli.Processor.Test.Command.Geometry3D
{
	public class Single
	{
		[Property("point")]
		public Kean.Math.Geometry3D.Single.Point Point { get; private set; }
		[Property("size")]
		public Kean.Math.Geometry3D.Single.Size Size { get; private set; }
		[Property("shell")]
		public Kean.Math.Geometry3D.Single.Shell Shell { get; private set; }
		[Property("box")]
		public Kean.Math.Geometry3D.Single.Box Box { get; private set; }
		[Property("transform")]
		public Kean.Math.Geometry3D.Single.Transform Transform { get; private set; }
		public Single()
		{
		}
	}
}
