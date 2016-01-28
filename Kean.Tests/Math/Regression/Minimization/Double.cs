using System;
using NUnit.Framework;

using Kean.Extension;
using Target = Kean.Math.Regression.Minimization.LevenbergMarquardt;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Collection;
using Matrix = Kean.Math.Matrix;

namespace Kean.Math.Regression.Test.Minimization
{
    public class Double :
        Kean.Test.Fixture<Double>
    {
        protected override void Run()
        {
            this.Run(
                this.LevenbergMarquardt1,
                this.LevenbergMarquardt2
                );
        }
        string prefix = "Kean.Math.Regression.Test.Minimization.Double.";
        [Test]
        public void LevenbergMarquardt1()
        {
            Matrix.Double a = new Matrix.Double(3, 3, new double[] { 3, 2, -1, 2, -2, 0.5, -1, 4, -1 });
            Matrix.Double b = new Matrix.Double(1, 3, new double[] { 1, -2, 0 });
            Matrix.Double guess = new Matrix.Double(1, 3, new double[] { 1, 1, 1 });
            Matrix.Double result = this.Estimate(a, b, guess);
            Matrix.Double correct = new Matrix.Double(1, 3, new double[] { 1, -2, -2 });
            Verify(result.Distance(correct), Is.EqualTo(0).Within(0.5f), this.prefix + "LevenbergMarquardt1.0");
        }
        Matrix.Double Estimate(Matrix.Double a, Matrix.Double b, Matrix.Double guess)
        {
            Func<Matrix.Double, Matrix.Double> function = x => b - a * x;
            Func<Matrix.Double, Matrix.Double> jacobian = x => -a;
            Target.Double lm = new Target.Double(function, jacobian, 200, 1e-8, 1e-8, 1e-3);
            return lm.Estimate(guess);
        }
        [Test]
        public void LevenbergMarquardt2()
        {
            Matrix.Double a = new Matrix.Double(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            int n = 15;
            Matrix.Double aa = new Matrix.Double(5, n * 5);
            for (int i = 0; i < n; i++)
                aa = aa.Paste(0, 5 * i, a);
            Matrix.Double y = new Matrix.Double(1, 5, new double[] { -1, 2, -3, 4, 5 });
            Matrix.Double yy = new Matrix.Double(1, n * 5);
            for (int i = 0; i < n; i++)
                yy = yy.Paste(0, 5 * i, y);
            Matrix.Double correct = new Matrix.Double(1, 5, new double[] { -70, 231, -296, 172, -38 });
            Matrix.Double luApproximation = aa.Solve(yy);
            Verify(luApproximation.Distance(correct), Is.EqualTo(0).Within(0.5f), this.prefix + "LevenbergMarquardt2.0");
            Matrix.Double iterative = this.Estimate(aa, yy, new Matrix.Double(1, 5, new double[] { 1, 1, 1, 1, 1 }));
            Verify(iterative.Distance(correct), Is.EqualTo(0).Within(0.5f), this.prefix + "LevenbergMarquardt2.1");

        }
    }
}
