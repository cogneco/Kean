// 
//  Map.cs
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
using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
using Kean.Draw.OpenGL.Backend.Extension;
using Collection = Kean.Collection;

namespace Kean.Draw.OpenGL
{
	public class Map :
		Draw.Map
	{
		Collection.Dictionary<string, object> data = new Collection.Dictionary<string, object>();
		protected Backend.Program Backend { get; private set; }
		bool disposeBackend = true;
		public Map(Backend.Program backend)
		{
			this.Backend = backend;
		}
		public Map(OpenGL.Backend.Programs program) :
			this(OpenGL.Backend.Context.Current.GetProgram(program))
		{
			this.disposeBackend = false;
		}
		public override void Dispose()
		{
			if (this.data.NotNull())
			{
				foreach (var item in this.data)
					if (item.Value is IDisposable)
						(item.Value as IDisposable).Dispose();
				this.data = null;
			}
			if (this.Backend.NotNull())
			{
				if (this.disposeBackend)
					this.Backend.Dispose();
				this.Backend = null;
			}
			base.Dispose();
		}
		internal void Render(Action render, params Packed[] channels)
		{
			int count = 1;
			foreach (var item in this.data)
				if (item.Value is Backend.IData)
					this.Backend.SetTexture(item.Key, count++, item.Value as Backend.IData);
				else if (item.Value is int)
					this.Backend.SetVariable(item.Key, (int)item.Value);
				else if (item.Value is float)
					this.Backend.SetVariable(item.Key, (float)item.Value);
				else if (item.Value is int[])
					this.Backend.SetVariable(item.Key, item.Value as int[]);
				else if (item.Value is float[])
					this.Backend.SetVariable(item.Key, item.Value as float[]);
				else if (item.Value is float[,])
					this.Backend.SetVariable(item.Key, item.Value as float[,]);
			for (int i = channels.Length - 1; i >= 0; i--)
				this.Backend.SetTexture("texture" + i, count++, channels[i].Backend);
			this.Backend.Use();
			render();
			this.Backend.UnUse();
			while (--count > 0)
				this.Backend.UnSetTexture(count);
		}
		public override bool Remove(string name)
		{
			object item = this.data[name];
			bool result = item.NotNull() && this.data.Remove(name);
			if (result && item is IDisposable)
				(item as IDisposable).Dispose();
			return result;
		}
		public override void Add(string name, params int[] data)
		{
			this.Remove(name);
			this.data[name] = data;
		}
		public override void Add(string name, params float[] data)
		{
			this.Remove(name);
			this.data[name] = data;
		}
		public override void Add(string name, float[,] data)
		{
			this.Remove(name);
			this.data[name] = data;
		}

		public override void Add<T>(string name, params T[] data)
		{
			object item = this.data[name];
			if (item is Backend.Data1D)
				(item as Backend.Data1D).Update(data);
			else
			{
				if (item is IDisposable)
					(item as IDisposable).Dispose();
				this.data[name] = this.Backend.CreateData1D().Update(data);
			}
		}
		public override void Add<T>(string name, T[,] data)
		{
			object item = this.data[name];
			if (item is Backend.Data2D)
				(item as Backend.Data2D).Update(data);
			else
			{
				if (item is IDisposable)
					(item as IDisposable).Dispose();
				this.data[name] = this.Backend.CreateData2D().Update(data);
			}
		}
		public override void Add(string name, Draw.Image data)
		{
			throw new NotImplementedException();
		}
		public override void Add<T>(string name, T[,,] data)
		{
			object item = this.data[name];
			if (item is Backend.Data3D)
			{
				(item as Backend.Data3D).Use();
				(item as Backend.Data3D).Update(data);
				(item as Backend.Data3D).UnUse();
			}
			else
			{
				if (item is IDisposable)
					(item as IDisposable).Dispose();
				var d = this.Backend.CreateData3D();
				d.Use();
				d.Update(data);
				d.UnUse();
				this.data[name] = d;
			}
		}
		public override void Add(string name, Geometry2D.Integer.Size data)
		{
			this.Add(name, new float[] { data.Width, data.Height, 0 });
		}
		public override void Add(string name, Geometry3D.Single.Point data)
		{
			this.Add(name, new float[] { data.X, data.Y, data.Z, 1 });
		}
		public override void Add(string name, Geometry3D.Single.Size data)
		{
			this.Add(name, new float[] { data.Width, data.Height, data.Depth, 0 });
		}
		public override void Add(string name, Geometry3D.Single.Transform data)
		{
			this.Add(name, new float[,] {
				{ data.A, data.B, data.C, 0 },
				{ data.D, data.E, data.F, 0 },
				{
					data.G,
					data.H,
					data.I,
					0
				},
				{
					data.J,
					data.K,
					data.L,
					1
				}
			});
		}
		public static Map MonochromeToBgr { get { return new Map(OpenGL.Backend.Programs.MonochromeToBgr); } }
		public static Map BgrToMonochrome { get { return new Map(OpenGL.Backend.Programs.BgrToMonochrome); } }
		public static Map BgrToU { get { return new Map(OpenGL.Backend.Programs.BgrToU); } }
		public static Map BgrToV { get { return new Map(OpenGL.Backend.Programs.BgrToV); } }
		public static Map Yuv420ToBgr { get { return new Map(OpenGL.Backend.Programs.Yuv420ToBgr); } }

		public static Map Create(string fragment)
		{
			return Map.Create(null, fragment);
		}
		public static Map Create(string vertex, string fragment)
		{
			return new Map(OpenGL.Backend.Context.Current.CreateProgram(vertex, fragment));
		}
	}
}
