using System;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
using Regression = Kean.Math.Regression;
using Collection = Kean.Collection;
using Kean;
using Matrix = Kean.Math.Matrix;
using Single = Kean.Math.Single;
using Double = Kean.Math.Double;

namespace Kean.Tests.Math.Regression.Ransac.Model
{
	class RobustPolynomialRegression :
		Kean.Math.Regression.Ransac.Model<float, float, Matrix.Single>
	{
		public RobustPolynomialRegression()
		{
			this.RequiredMeasures = 2;
			this.FitsWell = 5;
		}
		public override Matrix.Single Estimate(float[] domain, float[] range)
		{
			var degree = 2;
			Matrix.Single result = null;
			int n = domain.Length;
			Matrix.Single a = new Matrix.Single(degree, n);
			Matrix.Single b = new Matrix.Single(1, n);
			for (int i = 0; i < n; i++)
			{
				float x = domain[i];
				b[0, i] = range[i];
				for (int j = 0; j < degree; j++)
					a[j, i] = Single.Power(x, j);
			}
			result = a.Solve(b) ?? new Matrix.Single(1, degree);
			return result;
		}
		public override bool Fits(Matrix.Single transform, float domain, float range)
		{
			Func<Matrix.Single, float, float> map = (t, x) =>
			{
				float result = 0;
				for (int i = 0; i < t.Dimensions.Height; i++)
					result += t[0, i] * Single.Power(x, i);
				return result;
			};
			return Double.Absolute(map(transform, domain) - range) < 80;
		}
	}
}
