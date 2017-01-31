using System;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
using Regression = Kean.Math.Regression;
using Collection = Kean.Collection;
using Kean;
using Matrix = Kean.Math.Matrix;
using Single = Kean.Math.Single;

namespace Kean.Tests.Math.Regression.Ransac.Model
{
	class ScaleRotationTranslationRegression :
		Kean.Math.Regression.Ransac.Model<Geometry2D.Single.Point, Geometry2D.Single.Point, Kean.Math.Matrix.Single>
	{
		public ScaleRotationTranslationRegression()
		{
			this.RequiredMeasures = 5;
		}
		public override Kean.Math.Matrix.Single Estimate(Geometry2D.Single.Point[] domain, Geometry2D.Single.Point[] range)
		{
			int count = domain.Length;
			Kean.Math.Matrix.Single a = new Kean.Math.Matrix.Single(4, 2 * count);
			Kean.Math.Matrix.Single b = new Kean.Math.Matrix.Single(1, 2 * count);
			int j = 0;
			for (int i = 0; i < count; i++)
			{
				Geometry2D.Single.Point previous = domain[i];
				Geometry2D.Single.Point y = range[i];
				a[0, j] = previous.X;
				a[1, j] = -previous.Y;
				a[2, j] = 1;
				a[3, j] = 0;
				b[0, j++] = y.X;
				a[0, j] = previous.Y;
				a[1, j] = previous.X;
				a[2, j] = 0;
				a[3, j] = 1;
				b[0, j++] = y.Y;
			}
			return a.Solve(b) ?? new Kean.Math.Matrix.Single(1, 4);
		}
		public override bool Fits(Kean.Math.Matrix.Single transform, Geometry2D.Single.Point domain, Geometry2D.Single.Point range)
		{
			Func<Matrix.Single, Geometry2D.Single.Point, Geometry2D.Single.Point> map = (t, x) => new Geometry2D.Single.Point(t[0, 0] * x.X - t[0, 1] * x.Y + t[0, 2], t[0, 1] * x.X + t[0, 0] * x.Y + t[0, 3]);
			return map(transform, domain).Distance(range) < 8;
		}
	}
}
