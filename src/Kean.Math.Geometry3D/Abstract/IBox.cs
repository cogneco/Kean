﻿// 
//  IBox.cs
//  
//  Author:
//       Anders Frisk <@>
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

namespace Kean.Math.Geometry3D.Abstract
{
    public interface IBox<PointValueType, SizeValueType, V>
        where PointValueType : struct, IPoint<V>
        where SizeValueType : struct, ISize<V>
        where V : struct
    {
        PointValueType LeftTopFront { get; }
        SizeValueType Size { get; }
    }
}