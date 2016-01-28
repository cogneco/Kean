using System;
using Kean;
using Kean.Extension;

namespace Kean.Math.Regression.Interpolation.Splines.Geometry2D.Single
{
    public class Transform
    {
        Splines.Single[] interpolators;
        Method method = Method.Natural;
        public Method Method { get { return this.method; } set { this.method = value; } }
        Tuple<float, Kean.Math.Geometry2D.Single.Transform>[] measures;
        public Tuple<float, Kean.Math.Geometry2D.Single.Transform>[] Measures
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
                    Tuple<float, float>[][] values = new Tuple<float, float>[6][];
                    for(int i = 0; i < values.Length; i++)
                        values[i] = new Tuple<float, float>[this.measures.Length];
                    for (int i = 0; i < this.measures.Length; i++)
                    {
                        float time = this.measures[i].Item1;
                        Kean.Math.Geometry2D.Single.Transform transform = this.measures[i].Item2;
                        values[0][i] = Tuple.Create(time, transform.A);
                        values[1][i] = Tuple.Create(time, transform.B);
                        values[2][i] = Tuple.Create(time, transform.C);
                        values[3][i] = Tuple.Create(time, transform.D);
                        values[4][i] = Tuple.Create(time, transform.E);
                        values[5][i] = Tuple.Create(time, transform.F);
                    }
                    this.interpolators = new Splines.Single[values.Length];
                    for(int i = 0; i < values.Length; i++)
                        this.interpolators[i] = new Splines.Single(this.Method, values[i]);
                }
            }
        }
        public Transform(Method method) : this(method, null) { }
        public Transform(Method method, Tuple<float, Kean.Math.Geometry2D.Single.Transform>[] measures)
        {
            this.Method = method;
            this.Measures = measures;
        }
        public Kean.Math.Geometry2D.Single.Transform Interpolate(float value)
        {
            return new Kean.Math.Geometry2D.Single.Transform(
                this.interpolators[0].Interpolate(value), 
                this.interpolators[1].Interpolate(value),
                this.interpolators[2].Interpolate(value), 
                this.interpolators[3].Interpolate(value),
                this.interpolators[4].Interpolate(value), 
                this.interpolators[5].Interpolate(value));
        }
    }
}
