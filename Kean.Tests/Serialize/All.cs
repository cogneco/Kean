// 
//  All.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012-2013 Simon Mika
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

namespace Kean.Json.Serialize.Test
{
	public static class All
	{
		public static void Test()
		{
			Core.Serialize.Test.SystemTypes<Verifier>.Test("Kean.Json.Serialize.Test:SystemTypes");
			Core.Serialize.Test.CoreTypes<Verifier>.Test("Kean.Json.Serialize.Test:CoreTypes");
			Core.Serialize.Test.BasicTypes<Verifier>.Test("Kean.Json.Serialize.Test:BasicTypes");
			Core.Serialize.Test.NullableTypes<Verifier>.Test("Kean.Json.Serialize.Test:NullableTypes");
			Core.Serialize.Test.CollectionTypes<Verifier>.Test("Kean.Json.Serialize.Test:CollectionTypes");
			Core.Serialize.Test.Misfit<Verifier>.Test("Kean.Json.Serialize.Test:Misfit");
		}
	}
}
