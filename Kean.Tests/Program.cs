//
//  Main.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2009-2013 Simon Mika
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
//  You should have received data copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace Kean.Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			Error.Log.CatchErrors = true;
            Kean.Math.Test.All.Test();
            //new Kean.Math.Algebra.Test.MatrixTest().Multiply();
			new Kean.Math.Geometry2D.Test.Single.Point().Project8DofMulti();
            //new Kean.Math.Geometry2D.Test.Single.Point().AngleFormulas();
			//Kean.Math.Algebra.Test.All.Test();
			//Kean.Test.StringExtension.Test();
			//Kean.Uri.Test.All.Test();
			//Cli.Test.Program.Run(args);
			//IO.Net.Test.Program.Run(args);
			//DB.Sql.Test.Program.Run(args);
			//Platform.Settings.Test.Program.Run(args);
			#region Draw
			//Draw.OpenGL.Test.All.Test();
			//Draw.Test.All.Test();
			//Draw.Raster.Test.All.Test();
			//Draw.Cairo.Test.All.Test();
			//Draw.Vector.Test.All.Test();
			//Draw.Net.Test.All.Test();
			//Draw.Jpeg.Test.All.Test();
			#endregion
			#region Core
			//Kean.Test.All.Test();
			//Kean.Collection.Test.All.Test();
			//Kean.Error.Test.All.Test();
			//Kean.Reflect.Test.All.Test();
			//Kean.Uri.Test.All.Test();
			#endregion
			#region Json
			//Json.Dom.Test.All.Test();
			//Json.Serialize.Test.All.Test();
			#endregion
			#region Xml
			//Xml.Serialize.Test.All.Test();
			//Xml.Dom.Test.All.Test();
			#endregion
			#region Html
			//Html.Dom.Test.All.Test();
			#endregion
			#region Math
			//Kean.Math.Test.All.Test();
			//Kean.Math.Complex.Test.All.Test();
			//Kean.Math.Geometry2D.Test.All.Test();
			//Kean.Math.Geometry3D.Test.All.Test();
			//Kean.Math.Matrix.Test.All.Test();
			//Kean.Math.Random.Test.All.Test();
			//Kean.Math.Regression.Test.All.Test();
			#endregion
			Console.WriteLine();
			Console.WriteLine("Press any key to continue");
			Console.ReadKey(true);
		}
	}
}