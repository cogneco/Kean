using System;
using NUnit.Framework;

using Kean.Extension;
using Target = Kean.Math.Regression.Minimization.LevenbergMarquardt;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Collection;
using Matrix = Kean.Math.Matrix;

namespace Kean.Math.Regression.Test.Minimization
{
    public class Single :
        Kean.Test.Fixture<Single>
    {
        protected override void Run()
        {
            this.Run(
                this.LevenbergMarquardt1,
                this.LevenbergMarquardt2,
                this.LevenbergMarquardt3
                );
        }
        string prefix = "Kean.Math.Regression.Test.Minimization.Single.";
        [Test]
        public void LevenbergMarquardt1()
        {
            Matrix.Single a = new Matrix.Single(3, 3, new float[] { 3, 2, -1, 2, -2, 0.5f, -1, 4, -1 });
            Matrix.Single b = new Matrix.Single(1, 3, new float[] { 1, -2, 0 });
            Matrix.Single guess = new Matrix.Single(1, 3, new float[] { 1, 1, 1 });
            Matrix.Single result = this.Estimate(a, b, guess);
            Matrix.Single correct = new Matrix.Single(1, 3, new float[] { 1, -2, -2 });
            Verify(result.Distance(correct), Is.EqualTo(0).Within(0.5f), this.prefix + "LevenbergMarquardt1.0");
        }
        Matrix.Single Estimate(Matrix.Single a, Matrix.Single b, Matrix.Single guess)
        {
            Func<Matrix.Single, Matrix.Single> function = x => b - a * x;
            Func<Matrix.Single, Matrix.Single> jacobian = x => -a;
            Target.Single lm = new Target.Single(function, jacobian, 200, 1e-8f, 1e-8f, 1e-3f);
            return lm.Estimate(guess);
        }
        [Test]
        public void LevenbergMarquardt2()
        {
            Matrix.Single a = new Matrix.Single(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            int n = 15;
            Matrix.Single aa = new Matrix.Single(5, n * 5);
            for (int i = 0; i < n; i++)
                aa = aa.Paste(0, 5 * i, a);
            Matrix.Single y = new Matrix.Single(1, 5, new float[] { -1, 2, -3, 4, 5 });
            Matrix.Single yy = new Matrix.Single(1, n * 5);
            for (int i = 0; i < n; i++)
                yy = yy.Paste(0, 5 * i, y);
            Matrix.Single correct = new Matrix.Single(1, 5, new float[] { -70, 231, -296, 172, -38 });
            Matrix.Single luApproximation = aa.Solve(yy);
            Matrix.Single iterative = this.Estimate(aa, yy, new Kean.Math.Matrix.Single(1, 5, new float[] { 1, 1, 1, 1, 1 }));
            Verify(luApproximation.Distance(correct), Is.EqualTo(0).Within(7f), this.prefix + "LevenbergMarquardt2.0");
            Verify(iterative.Distance(correct), Is.EqualTo(0).Within(0.5f), this.prefix + "LevenbergMarquardt2.1");
        }
        [Test]
        public void LevenbergMarquardt3()
        {
            Func<float, float, float, float> map = (x,a,b) => a * Kean.Math.Single.Exponential(b * x);
            int n = 100;
            float[] xx = new float[n];
            float[] yy = new float[n];
            float aa = 1;
            float bb = 2;
            Random.Single.Normal generator = new Random.Single.Normal(0, 1);
            float[] noise = generator.Generate(n);
            for(int i = 0; i < n; i++)
            {
                xx[i] = -i + 10;
                yy[i] = map(xx[i], aa, bb) + noise[i];
            }

            Func<Matrix.Single, Matrix.Single> f = ab =>
            {
                Matrix.Single result = new Matrix.Single(1, n);
                for (int i = 0; i < n; i++)
                {
                    result[0, i] = yy[i] - map(xx[i], ab[0, 0], ab[0, 1]);
                }
                return result;
            };
            Func<Matrix.Single, Matrix.Single> j = ab =>
            {
                Matrix.Single result = new Matrix.Single(2, n);
                for (int i = 0; i < n; i++)
                {
                    result[0, i] = -Kean.Math.Single.Exponential(ab[0, 1] * xx[i]);
                    result[1, i] = -ab[0,0] * xx[i] * Kean.Math.Single.Exponential(ab[0, 1] * xx[i]);
                }
                return result;
            };
            Target.Single lm = new Target.Single(f, j);
            Matrix.Single estimate = lm.Estimate(new Matrix.Single(1,2, new float[] {3,3}));
        }
    }
}
