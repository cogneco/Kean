//
//  All.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2012 Simon Mika
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
namespace Kean.Xml.Serialize.Test
{
    public static class All
    {
        public static void Test()
        {
            Kean.Serialize.Test.SystemTypes<Verifier>.Test("Kean.Xml.Serialize.Test:SystemTypes");
            Kean.Serialize.Test.CoreTypes<Verifier>.Test("Kean.Xml.Serialize.Test:CoreTypes");
            Kean.Serialize.Test.BasicTypes<Verifier>.Test("Kean.Xml.Serialize.Test:BasicTypes");
            Kean.Serialize.Test.NullableTypes<Verifier>.Test("Kean.Xml.Serialize.Test:NullableTypes");
            Kean.Serialize.Test.CollectionTypes<Verifier>.Test("Kean.Xml.Serialize.Test:CollectionTypes");
            Serialize.Test.Missing.Test();
            Serialize.Test.Named.Test();
            Kean.Serialize.Test.Misfit<Verifier>.Test("Kean.Xml.Serialize.Test:Misfit");
            Serialize.Test.Preprocessor.Test();
        }
    }
}
