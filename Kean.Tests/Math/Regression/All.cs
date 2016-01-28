using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Math.Regression.Test
{
    public static class All
    {
        public static void Test()
        {
            Filter.Kalman.Test();
            Ransac.Single.Test();
            Minimization.Single.Test();
            Minimization.Double.Test();
            Interpolation.Splines.Geometry2D.Test();
        }
    }
}
