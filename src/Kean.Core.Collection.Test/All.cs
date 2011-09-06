﻿//  
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

namespace Kean.Core.Collection.Test
{
    public static class All
    {
        public static void Test()
        {
            Vector.Test();
            List.Test();
            Queue.Test();
            Stack.Test();
            Dictionary.Test();
            Linked.List.Test();
            Linked.Queue.Test();
            Linked.Stack.Test();
            Array.Vector.Test();
            Array.List.Test();
            Array.Queue.Test();
            Array.Stack.Test();
            Sorted.List.Test();
        }
    }
}