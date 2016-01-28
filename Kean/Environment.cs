// 
//  Environment.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
namespace Kean
{
    public static class Environment
    {
        static OperatingSystem? operatingSystem;
        public static OperatingSystem OperatingSystem
        { 
            get
            {
                if (!Environment.operatingSystem.HasValue)
                {
                    switch (System.Environment.OSVersion.Platform)
                    {
                        case PlatformID.Unix:
                            if (System.IO.Directory.Exists("/Applications") && System.IO.Directory.Exists("/System") && System.IO.Directory.Exists("/Users") && System.IO.Directory.Exists("/Volumes"))
                                Environment.operatingSystem = OperatingSystem.MacOSX;
                            else
                                Environment.operatingSystem = OperatingSystem.Linux;
                            break;
                        case PlatformID.MacOSX:
                            Environment.operatingSystem = OperatingSystem.MacOSX;
                            break;
                        default:
                            Environment.operatingSystem = OperatingSystem.Windows;
                            break;
                    }
                }
                return Environment.operatingSystem.Value;
            }
        }
        public static bool IsWindows { get { return Environment.OperatingSystem == OperatingSystem.Windows; } }
        public static bool IsMono { get { return Environment.OperatingSystem != OperatingSystem.Windows; } }
    }
}

