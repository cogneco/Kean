// 
//  Control.cs
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
using Kean.Extension;
using Parallel = Kean.Parallel;
using Forms = System.Windows.Forms;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL
{
	public class Control :
		Forms.UserControl
	{
		public event Action Initialized;
		public event Action<Surface> Draw;

		public Parallel.ThreadPool ThreadPool { get { return this.backend.NotNull() ? this.backend.ThreadPool : null; } }

		bool designMode;
		protected new bool DesignMode
		{
			get { return this.designMode || base.DesignMode || System.Diagnostics.Process.GetCurrentProcess().ProcessName.StartsWith("devenv") || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime; }
			set { this.designMode = value; }
		}
		#region AutoStart
		[System.ComponentModel.DefaultValue(true)]
		public bool AutoStart
		{
			get { return this.backend.NotNull() && this.backend.AutoStart; }
			set
			{
				if (this.backend.NotNull())
					this.backend.AutoStart = value;
			}
		}
		#endregion
		#region PointerEvents
		Geometry2D.Single.Point lastPointerPosition;
		public event Action<Geometry2D.Single.Size, Geometry2D.Single.Point, bool, bool, bool> PointerMoved;
		public event Action<Geometry2D.Single.Point> PointerLeftClicked;
		public event Action<Geometry2D.Single.Point> PointerMiddleClicked;
		public event Action<Geometry2D.Single.Point> PointerRightClicked;
		public event Action<float, Geometry2D.Single.Point> PointerWheelChanged;
		#endregion
		Backend.Control backend;
		public Control()
		{
			this.DesignMode = this.DesignMode;
			this.SuspendLayout();
			// 
			// backend
			// 
			if (!this.DesignMode)
			{
				this.backend = Backend.Control.Create();
				this.backend.Initialized += () => this.Initialized.Call();
				#region Pointer Events Hookup
				this.backend.MouseClick += (sender, arguments) =>
				{
					Error.Log.Call((Action<System.Windows.Forms.MouseEventArgs>)(e =>
					{
						Geometry2D.Single.Point position = new Geometry2D.Single.Point(e.X, e.Y);
						switch (e.Button)
						{
							case System.Windows.Forms.MouseButtons.Left:
								this.PointerLeftClicked(position);
								break;
							case System.Windows.Forms.MouseButtons.Middle:
								this.PointerMiddleClicked(position);
								break;
							case System.Windows.Forms.MouseButtons.Right:
								this.PointerRightClicked(position);
								break;
						}
					}), arguments);
				};
				this.backend.MouseMove += (sender, arguments) =>
				{
					Error.Log.Call((Action<System.Windows.Forms.MouseEventArgs>)(e =>
					{
						Geometry2D.Single.Point currentPosition = new Geometry2D.Single.Point(e.X, e.Y);
						this.PointerMoved((Geometry2D.Single.Size)(currentPosition - this.lastPointerPosition), currentPosition, e.Button == System.Windows.Forms.MouseButtons.Left, e.Button == System.Windows.Forms.MouseButtons.Middle, e.Button == System.Windows.Forms.MouseButtons.Right);
						this.lastPointerPosition = currentPosition;
					}), arguments);
				};
				this.backend.MouseWheel += (sender, arguments) => Error.Log.Call((Action<System.Windows.Forms.MouseEventArgs>)(e => this.PointerWheelChanged(e.Delta / 120f, new Geometry2D.Single.Point(e.X, e.Y))), arguments);
				#endregion
				this.backend.Draw += renderer =>
				{
					using (Surface surface = new Surface(renderer))
					{
						surface.Use();
						surface.Transform = Geometry2D.Single.Transform.CreateTranslation(surface.Size / 2);
						this.Draw(surface);
						surface.Unuse();
					}
				};
				this.backend.AutoSize = true;
				this.backend.BackColor = System.Drawing.Color.Transparent;
				this.backend.Dock = System.Windows.Forms.DockStyle.Fill;
				this.backend.Location = new System.Drawing.Point(0, 0);
				this.backend.Name = "backend";
				this.backend.Size = new System.Drawing.Size(150, 150);
				this.backend.TabIndex = 0;
				this.backend.VSync = false;
				this.Controls.Add(this.backend);
			}
			// 
			// Control
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "Viewer";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		public Draw.Image Read()
		{
			return this.backend.Read();
		}
		public void Initialize()
		{
			if (!this.DesignMode)
				this.backend.Initialize();
		}
		public void Redraw()
		{
			this.backend.Invalidate();
		}
		protected override void Dispose(bool disposing)
		{
			if (this.backend.NotNull())
			{
				this.backend.Dispose();
				this.backend = null;
			}
		}
	}
}
