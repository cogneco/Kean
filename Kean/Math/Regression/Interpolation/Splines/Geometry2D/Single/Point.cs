using System;
using Kean;
using Kean.Extension;

namespace Kean.Math.Regression.Interpolation.Splines.Geometry2D.Single
{
    public class Point
    {
        Splines.Single[] interpolators;
        Method method = Method.Natural;
        public Method Method { get { return this.method; } set { this.method = value; } }
        Tuple<float, Kean.Math.Geometry2D.Single.Point>[] measures;
        public Tuple<float, Kean.Math.Geometry2D.Single.Point>[] Measures
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
                    Tuple<float, float>[][] values = new Tuple<float, float>[2][];
                    for (int i = 0; i < values.Length; i++)
                        values[i] = new Tuple<float, float>[this.measures.Length];
                    for (int i = 0; i < this.measures.Length; i++)
                    {
                        float time = this.measures[i].Item1;
                        Kean.Math.Geometry2D.Single.Point point = this.measures[i].Item2;
                        values[0][i] = Tuple.Create(time, point.X);
                        values[1][i] = Tuple.Create(time, point.Y);
                    }
                    this.interpolators = new Splines.Single[values.Length];
                    for (int i = 0; i < values.Length; i++)
                        this.interpolators[i] = new Splines.Single(this.Method, values[i]);
                }
            }
        }
        public Point(Method method) : this(method, null) { }
        public Point(Method method, Tuple<float, Kean.Math.Geometry2D.Single.Point>[] measures)
        {
            this.Method = method;
            this.Measures = measures;
        }
        public Kean.Math.Geometry2D.Single.Point Interpolate(float value)
        {
            return new Kean.Math.Geometry2D.Single.Point(this.interpolators[0].Interpolate(value), this.interpolators[1].Interpolate(value));
        }
    }
}
