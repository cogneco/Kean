// 
//  RemoteConfiguration.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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

namespace Kean.Platform.Settings
{
	public class RemoteConfiguration
	{
		private Asynchronous asynchronous;
		public Asynchronous Asynchronous 
		{
			get { return this.asynchronous; } 
			set 
			{ 
				if (this.asynchronous != value) 
					this.AsynchronousChanged.Call(this.asynchronous = value); 
			} 
		}
		public event Action<Asynchronous> AsynchronousChanged;

		public event Action<bool, string> OnDebug;

		public void Debug(bool direction, string message)
		{
			this.OnDebug.Call(direction, message);
		}
	}
}
