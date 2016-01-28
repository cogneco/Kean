using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Math.Regression.Minimization.LevenbergMarquardt
{
    public class Double
    {
        Func<Matrix.Double, Matrix.Double> function;
        Func<Matrix.Double, Matrix.Double> jacobian;
        int iterations;
        double error1;
        double error2;
        double tao;
        public Double(Func<Matrix.Double, Matrix.Double> function, Func<Matrix.Double, Matrix.Double> jacobian) : this(function, jacobian, 200, 1e-18, 1e-18, 1e-3) { }
        public Double(Func<Matrix.Double, Matrix.Double> function, Func<Matrix.Double, Matrix.Double> jacobian, int iterations, double error1, double error2, double tao)
        {
            this.function = function;
            this.jacobian = jacobian;
            this.iterations = iterations;
            this.error1 = error1;
            this.error2 = error2;
            this.tao = tao;    
        }
        public Matrix.Double Estimate(Matrix.Double x0)
        {
            int k = 0;
            double ny = 2;
            Matrix.Double x = x0;
            Matrix.Double j = this.jacobian(x0);
            Matrix.Double f = this.function(x0);
            Matrix.Double a = j.Transpose() * j;
            Matrix.Double g = j.Transpose() * f;
            bool found = g.NormInfinity <= this.error1;
            double mu = 0;
            int order = a.Order;
            for (int i = 0; i < order; i++)
                mu = Kean.Math.Double.Maximum(mu, Kean.Math.Double.Absolute(a[i, i]));
            mu *= this.tao;
            while (!found && k < this.iterations)
            {
                k++;
                Matrix.Double hlm = (a + mu * Matrix.Double.Identity(order)).Solve(-g);
                if (hlm.Norm <= this.error2 * (x.Norm + this.error2))
                    found = true;
                else
                {
                    Matrix.Double xNew = x + hlm;
                    double rho = (Kean.Math.Double.Squared(this.function(x).Norm) - Kean.Math.Double.Squared(this.function(xNew).Norm)) / 2;
                    rho /= (hlm.Transpose() * (mu * hlm - g))[0, 0] / 2;
                    if (rho > 0)
                    {
                        x = xNew;
                        j = this.jacobian(x);
                        f = this.function(x);
                        a = j.Transpose() * j;
                        g = j.Transpose() * f;
                        found = g.NormInfinity <= this.error1;
                        mu = mu * Kean.Math.Double.Maximum(1.0 / 3.0, 1.0 - Kean.Math.Double.Power(2 * rho - 1, 3));
                        ny = 2;
                    }
                    else
                    {
                        mu = mu * ny;
                        ny = 2 * ny;
                    }
                }
            }
            //Console.WriteLine("iterations " + k);
            return x;
        }
    }
}
