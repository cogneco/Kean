//Sample license text.

using Kean.Collection.Extension;
using System;
using Collection = Kean.Collection;
using Error = Kean.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Kean.Extension;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Program :
		Resource
	{
		internal int Identifier { get; private set; }
		protected Program(Context context) :
			base(context)
		{
			this.Identifier = GL.CreateProgram();
		}
		protected Program(Program original) :
			base(original)
		{
			this.Identifier = original.Identifier;
			original.Identifier = 0;
		}

		protected override void Dispose(bool disposing)
		{
			if (this.Context.NotNull())
				this.Context.Delete(this.Refurbish());
		}
		#region Implementors Interface
		public abstract void Use();
		public abstract void UnUse();
		public abstract void Attach(Shader shader);
		public abstract void Detach(Shader shader);
		public abstract void Link();
		public abstract Data1D CreateData1D();
		public abstract Data2D CreateData2D();
		public abstract Data3D CreateData3D();
		public abstract void SetTexture(string name, int number, IData texture);
		public abstract void UnSetTexture(int number);
		public abstract void SetVariable(string name, float[,] values);
		public abstract void SetVariable(string name, params int[] values);
		public abstract void SetVariable(string name, params float[] values);
		protected abstract Program Refurbish();
		protected internal override void Delete()
		{
			this.Identifier = 0;
			base.Delete();
		}
		#endregion
		public override string ToString()
		{
			return this.Identifier.ToString();
		}
	}
}
