using System;
using Kean;
using Kean.Extension;
using Matrix = Kean.Math.Matrix;

namespace Kean.Math.Regression.Interpolation.Splines
{
    public class Double
    {
        struct Element
        {
            public double a, b, c, d, left, right;
            public Element(double a, double b, double c, double d, double left, double right)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
                this.left = left;
                this.right = right;
            }
            public double Interpolate(double value)
            {
                double x = value;
                x = (x - this.left) / (this.right - this.left); // Normalize to [0,1]
                return this.a + this.b * x + this.c * x * x + this.d * x * x * x;
            }
        }
        Method method = Method.Natural;
        public Method Method { get { return this.method; } set { this.method = value; } }
        Element[] elements = null;
        Tuple<double, double>[] measures;
        public Tuple<double, double>[] Measures
        {
            get
            {
                return this.measures;
            }
            set
            {
                this.measures = value;
                if (this.measures.NotNull())
                {
                    if (this.measures.Length >= 2)
                        switch (this.Method)
                        {
                            default:
                            case Method.Natural:
                                this.elements = this.Natural(this.measures);
                                break;
                            case Method.Periodic:
                                this.elements = this.Periodic(this.measures);
                                break;
                        }
                    else if (this.measures.Length == 1)
                    {
                        this.elements = new Element[] { new Element(this.measures[0].Item2, 0, 0, 0, 0, 1)};
                    }
                    else
                        this.elements = new Element[] { new Element(0, 0, 0, 0, 0, 1) };
                }
            }
        }
        public Double(Method method) : this(method, null) { }
        public Double(Method method, Tuple<double, double>[] measures)
        {
            this.Method = method;
            this.Measures = measures;
        }
        public double Interpolate(double value)
        {
            double result = 0;
            if (this.elements.NotNull())
            {
                if (value <= this.elements[0].left)
                    result = this.elements[0].Interpolate(this.elements[0].left);
                else if (value >= this.elements[this.elements.Length - 1].right)
                    result = this.elements[this.elements.Length - 1].Interpolate(this.elements[this.elements.Length - 1].right);
                else
                {
                    int left = 0;
                    for (int x = 0; x < this.elements.Length; x++)
                        if (this.elements[x].left <= value && value <= this.elements[x].right)
                        {
                            left = x;
                            break;
                        }
                    result = this.elements[left].Interpolate(value);
                }
            }
            return result;
        }
        Element[] Natural(Tuple<double, double>[] measures)
        {
            // Compute natural piecewise cubic splines.
            Element[] result = new Element[measures.Length - 1];
            Matrix.Double left = new Matrix.Double(measures.Length, measures.Length);
            Matrix.Double right = new Matrix.Double(1, measures.Length);
            left[0, 0] = 2;
            left[1, 0] = 1;
            left[measures.Length - 1, measures.Length - 1] = 2;
            left[measures.Length - 2, measures.Length - 1] = 1;
            for (int y = 1; y < measures.Length - 1; y++)
            {
                int x = y - 1;
                left[x, y] = 1;
                left[x + 1, y] = 4;
                left[x + 2, y] = 1;
            }
            right[0, 0] = 3 * (measures[1].Item2 - measures[0].Item2);
            right[0, measures.Length - 1] = 3 * (measures[measures.Length - 1].Item2 - measures[measures.Length - 2].Item2);
            for (int y = 1; y < measures.Length - 1; y++)
                right[0, y] = 3 * (measures[y + 1].Item2 - measures[y - 1].Item2);
            Matrix.Double solution = left.Solve(right);

            for (int x = 0; x < measures.Length - 1; x++)
            {
                double a = measures[x].Item2;
                double b = solution[0, x];
                double c = 3 * (measures[x + 1].Item2 - measures[x].Item2) - 2 * solution[0, x] - solution[0, x + 1];
                double d = 2 * (measures[x].Item2 - measures[x + 1].Item2) + solution[0, x] + solution[0, x + 1];
                result[x] = new Element(a, b, c, d, measures[x].Item1, measures[x + 1].Item1);
            }
            return result;
        }
        Element[] Periodic(Tuple<double, double>[] measures)
        {
            // Compute natural piecewise cubic splines.
            Element[] result = new Element[measures.Length - 1];
            Matrix.Double left = new Matrix.Double(measures.Length, measures.Length);
            Matrix.Double right = new Matrix.Double(1, measures.Length);
            left[0, 0] = 2;
            left[1, 0] = 1;
            left[measures.Length - 1, 0] = 2;
            left[measures.Length - 2, 0] += 1;
            left[0, measures.Length - 1] = 1;
            left[measures.Length - 1, measures.Length - 1] = -1;
            for (int y = 1; y < measures.Length - 1; y++)
            {
                int x = y - 1;
                left[x, y] = 1;
                left[x + 1, y] = 4;
                left[x + 2, y] = 1;
            }
            right[0, 0] = 3 * (measures[1].Item2 - measures[0].Item2) + 3 * (measures[measures.Length - 1].Item2 - measures[measures.Length - 2].Item2);
            for (int y = 1; y < measures.Length - 1; y++)
                right[0, y] = 3 * (measures[y + 1].Item2 - measures[y - 1].Item2);
            Matrix.Double solution = left.Solve(right);

            for (int x = 0; x < measures.Length - 1; x++)
            {
                double a = measures[x].Item2;
                double b = solution[0, x];
                double c = 3 * (measures[x + 1].Item2 - measures[x].Item2) - 2 * solution[0, x] - solution[0, x + 1];
                double d = 2 * (measures[x].Item2 - measures[x + 1].Item2) + solution[0, x] + solution[0, x + 1];
                result[x] = new Element(a, b, c, d, measures[x].Item1, measures[x + 1].Item1);
            }
            return result;
        }
    }
}
