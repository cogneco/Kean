﻿using System;
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Math.Regression.Interpolation.Splines.Geometry2D.Double
{
    public class TransformValue
    {
        Splines.Double[] interpolators;
        Method method = Method.Natural;
        public Method Method { get { return this.method; } set { this.method = value; } }
        Tuple<double, Kean.Math.Geometry2D.Double.TransformValue>[] measures;
        public Tuple<double, Kean.Math.Geometry2D.Double.TransformValue>[] Measures
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
                    Tuple<double, double>[][] values = new Tuple<double, double>[6][];
                    for(int i = 0; i < values.Length; i++)
                        values[i] = new Tuple<double, double>[this.measures.Length];
                    for (int i = 0; i < this.measures.Length; i++)
                    {
                        double time = this.measures[i].Item1;
                        Kean.Math.Geometry2D.Double.TransformValue transform = this.measures[i].Item2;
                        values[0][i] = Tuple.Create(time, transform.A);
                        values[1][i] = Tuple.Create(time, transform.B);
                        values[2][i] = Tuple.Create(time, transform.C);
                        values[3][i] = Tuple.Create(time, transform.D);
                        values[4][i] = Tuple.Create(time, transform.E);
                        values[5][i] = Tuple.Create(time, transform.F);
                    }
                    this.interpolators = new Splines.Double[values.Length];
                    for(int i = 0; i < values.Length; i++)
                        this.interpolators[i] = new Splines.Double(this.Method, values[i]);
                }
            }
        }
        public TransformValue(Method method) : this(method, null) { }
        public TransformValue(Method method, Tuple<double, Kean.Math.Geometry2D.Double.TransformValue>[] measures)
        {
            this.Method = method;
            this.Measures = measures;
        }
        public Kean.Math.Geometry2D.Double.TransformValue Interpolate(double value)
        {
            return new Kean.Math.Geometry2D.Double.TransformValue(
                this.interpolators[0].Interpolate(value), 
                this.interpolators[1].Interpolate(value),
                this.interpolators[2].Interpolate(value), 
                this.interpolators[3].Interpolate(value),
                this.interpolators[4].Interpolate(value), 
                this.interpolators[5].Interpolate(value));
        }
    }
}
