// 
//  Class.cs
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

namespace Kean.Serialize.Test.Data
{
	public class Class :
		IData
	{
		[Kean.Serialize.Parameter]
		public bool Boolean { get; set; }
		[Kean.Serialize.Parameter]
		public int Integer { get; set; }
		[Kean.Serialize.Parameter]
		public float Float { get; set; }
		[Kean.Serialize.Parameter]
		public Enumerator Enumerator { get; set; }
		[Kean.Serialize.Parameter]
		public string String { get; set; }

		#region IData
		public virtual void Initialize(IFactory factory)
		{
			this.Boolean = factory.Create<bool>();
			this.Integer = factory.Create<int>();
			this.Float = factory.Create<float>();
			this.Enumerator = factory.Create<Enumerator>();
			this.String = factory.Create<string>();
		}
		public virtual void Verify(IFactory factory, string message, params object[] arguments)
		{
			factory.Verify(this.Boolean, message, arguments);
			factory.Verify(this.Integer, message, arguments);
			factory.Verify(this.Float, message, arguments);
			factory.Verify(this.Enumerator, message, arguments);
			factory.Verify(this.String, message, arguments);
		}
		#endregion
	}
}
