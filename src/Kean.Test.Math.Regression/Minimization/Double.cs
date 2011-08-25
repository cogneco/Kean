﻿using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Regression.Minimization.LevenbergMarquardt;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;
using Matrix = Kean.Math.Matrix;

namespace Kean.Test.Math.Regression.Minimization
{
    public class Double :
        AssertionHelper
    {
        [Test]
        public void LevenbergMarquardt()
        {
            Matrix.Double a = new Matrix.Double(3, 3, new double[] {3, 2, -1, 2, -2, 0.5, -1,4,-1});
            Matrix.Double b = new Matrix.Double(1, 3, new double[] {1 , -2, 0});
            Matrix.Double guess = new Matrix.Double(1,3, new double[] {1,1,1});
            Matrix.Double result = this.Estimate(a, b, guess);
            Matrix.Double correct = new Matrix.Double(1, 3, new double[] { 1, -2, -2 });
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
            Matrix.Single x = aa.Solve(yy);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(0.5f));
      
        }
        public void Run()
        {
            this.Run(
                this.LevenbergMarquardt,
                this.LevenbergMarquardt2
                );
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }
        public static void Test()
        {
            Double fixture = new Double();
            fixture.Run();
        }

    }
}