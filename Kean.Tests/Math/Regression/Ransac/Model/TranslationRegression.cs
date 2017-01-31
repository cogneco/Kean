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
	class TranslationRegression :
		Kean.Math.Regression.Ransac.Model<Geometry2D.Single.Point, Geometry2D.Single.Point, Geometry2D.Single.Point>
	{
		public TranslationRegression()
		{
			this.RequiredMeasures = 5;
		}
		public override Geometry2D.Single.Point Estimate(Geometry2D.Single.Point[] domain, Geometry2D.Single.Point[] range)
		{
			int count = domain.Length;
			Geometry2D.Single.Point result = new Geometry2D.Single.Point();
			for (int i = 0; i < count; i++)
				result += range[i] - domain[i];
			result.X /= (float)count;
			result.Y /= (float)count;
			return result;
		}
		public override bool Fits(Geometry2D.Single.Point transform, Geometry2D.Single.Point domain, Geometry2D.Single.Point range)
		{
			return (transform + domain).Distance(range) < 10;
		}
	}
}
