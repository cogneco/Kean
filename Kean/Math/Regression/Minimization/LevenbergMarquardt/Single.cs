using System;


namespace Kean.Math.Regression.Minimization.LevenbergMarquardt
{
    public class Single
    {
        Func<Matrix.Single, Matrix.Single> function;
        Func<Matrix.Single, Matrix.Single> jacobian;
        int iterations;
        float error1;
        float error2;
        float tao;
        public Single(Func<Matrix.Single, Matrix.Single> function, Func<Matrix.Single, Matrix.Single> jacobian) : this(function, jacobian, 200, 1e-18f, 1e-18f, 1e-3f) { }
        public Single(Func<Matrix.Single, Matrix.Single> function, Func<Matrix.Single, Matrix.Single> jacobian, int iterations, float error1, float error2, float tao)
        {
            this.function = function;
            this.jacobian = jacobian;
            this.iterations = iterations;
            this.error1 = error1;
            this.error2 = error2;
            this.tao = tao;
        }
        public Matrix.Single Estimate(Matrix.Single x0)
        {
            int k = 0;
            float ny = 2;
            Matrix.Single x = x0;
            Matrix.Single j = this.jacobian(x0);
            Matrix.Single f = this.function(x0);
            Matrix.Single a = j.Transpose() * j;
            Matrix.Single g = j.Transpose() * f;
            bool found = g.NormInfinity <= this.error1;
            float mu = 0;
            int order = a.Order;
            for (int i = 0; i < order; i++)
                mu = Kean.Math.Single.Maximum(mu, Kean.Math.Single.Absolute(a[i, i]));
            mu *= this.tao;
            while (!found && k < this.iterations)
            {
                k++;
                Matrix.Single hlm = (a + mu * Matrix.Single.Identity(order)).Solve(-g);
                if (hlm.Norm <= this.error2 * (x.Norm + this.error2))
                    found = true;
                else
                {
                    Matrix.Single xNew = x + hlm;
                    float rho = (Kean.Math.Single.Squared(this.function(x).Norm) - Kean.Math.Single.Squared(this.function(xNew).Norm)) / 2;
                    rho /= (hlm.Transpose() * (mu * hlm - g))[0, 0] / 2;
                    if (rho > 0)
                    {
                        x = xNew;
                        j = this.jacobian(x);
                        f = this.function(x);
                        a = j.Transpose() * j;
                        g = j.Transpose() * f;
                        found = g.NormInfinity <= this.error1;
                        mu = mu * Kean.Math.Single.Maximum(1.0f / 3.0f, 1.0f - Kean.Math.Single.Power(2 * rho - 1, 3));
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
