﻿using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Matrix.Single;

namespace Kean.Test.Math.Matrix.Algorithms
{
    public class Single :
        AssertionHelper
    {
        string prefix = "Kean.Test.Math.Matrix.Algorithms.Single.";
        [Test]
        public void Cholesky()
        {
            Target a = new Kean.Math.Matrix.Single(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target c = a.Cholesky();
            Expect(c * c.Transpose(), Is.EqualTo(a), this.prefix + "Cholesky.0");
            Target d = c.Transpose();
            Expect(d.Transpose() * d, Is.EqualTo(a), this.prefix + "Cholesky.1");
        }
        [Test]
        public void LeastSquare1()
        {
            Target a = new Kean.Math.Matrix.Single(3, 3, new float[] { 1, 2, 3, 4, 5, 6, 7, 8, -1});
            Target y = new Kean.Math.Matrix.Single(1, 3, new float[] { -1, 2, -3});
            Target correct = new Kean.Math.Matrix.Single(1, 3, new float[] { 5.1333f, -2.9333f, 0.8000f });
            Target x = a.LeastSquare(y);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-4f), this.prefix + "LeastSquare1.0");
        }
        [Test]
        public void LeastSquare2()
        {
            Target a = new Kean.Math.Matrix.Single(3, 3, new float[] { 1, 2, 3, 4, 5, 6, 7, 8, -1 });
            Target aa = new Kean.Math.Matrix.Single(3, 6);
            aa.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 0), a);
            aa.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 3), a);
            Target y = new Kean.Math.Matrix.Single(1, 3, new float[] { -1, 2, -3 });
            Target yy = new Kean.Math.Matrix.Single(1, 6);
            yy.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 0), y);
            yy.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 3), y);
            Target correct = new Kean.Math.Matrix.Single(1, 3, new float[] { 5.1333f, -2.9333f, 0.8000f });
            Target x = aa.LeastSquare(yy);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-4f), this.prefix + "LeastSquare2.0");
        }

        public void Run()
        {
            this.Run(
                this.Cholesky,
                this.LeastSquare1,
                this.LeastSquare2
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
            Single fixture = new Single();
            fixture.Run();
        }
    }
}
