using System;
using Kean;
using Kean.Extension;

namespace Kean.Math.Regression.Interpolation.Splines.Geometry2D.Double
{
    public class Point
    {
        Splines.Double[] interpolators;
        Method method = Method.Natural;
        public Method Method { get { return this.method; } set { this.method = value; } }
        Tuple<double, Kean.Math.Geometry2D.Double.Point>[] measures;
        public Tuple<double, Kean.Math.Geometry2D.Double.Point>[] Measures
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
                    Tuple<double, double>[][] values = new Tuple<double, double>[2][];
                    for (int i = 0; i < values.Length; i++)
                        values[i] = new Tuple<double, double>[this.measures.Length];
                    for (int i = 0; i < this.measures.Length; i++)
                    {
                        double time = this.measures[i].Item1;
                        Kean.Math.Geometry2D.Double.Point point = this.measures[i].Item2;
                        values[0][i] = Tuple.Create(time, point.X);
                        values[1][i] = Tuple.Create(time, point.Y);
                    }
                    this.interpolators = new Splines.Double[values.Length];
                    for (int i = 0; i < values.Length; i++)
                        this.interpolators[i] = new Splines.Double(this.Method, values[i]);
                }
            }
        }
        public Point(Method method) : this(method, null) { }
        public Point(Method method, Tuple<double, Kean.Math.Geometry2D.Double.Point>[] measures)
        {
            this.Method = method;
            this.Measures = measures;
        }
        public Kean.Math.Geometry2D.Double.Point Interpolate(double value)
        {
            return new Kean.Math.Geometry2D.Double.Point(this.interpolators[0].Interpolate(value), this.interpolators[1].Interpolate(value));
        }
    }
}
