using System;
using NUnit.Framework;

using Kean.Extension;
using Kean;

namespace Kean.Math.Regression.Test.Interpolation.Splines
{
    public class Geometry2D :
        Kean.Test.Fixture<Geometry2D>
    {
        protected override void Run()
        {
            this.Run(
                this.Point
                );
        }
        string prefix = "Kean.Math.Regression.Test.Interpolation.Splines.Geometry2D.";
        [Test]
        public void Point()
        {
			Kean.Math.Regression.Interpolation.Splines.Geometry2D.Single.Point interpolate = new Kean.Math.Regression.Interpolation.Splines.Geometry2D.Single.Point(Kean.Math.Regression.Interpolation.Splines.Method.Periodic);
            interpolate.Measures = new Tuple<float, Kean.Math.Geometry2D.Single.Point>[] 
                { 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.Point>(0,new Kean.Math.Geometry2D.Single.Point(10, 0)), 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.Point>(1,new Kean.Math.Geometry2D.Single.Point(0, 10)), 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.Point>(2, new Kean.Math.Geometry2D.Single.Point(-10, 0)), 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.Point>(3, new Kean.Math.Geometry2D.Single.Point(0, -10)), 
                   Tuple.Create<float, Kean.Math.Geometry2D.Single.Point>(4, new Kean.Math.Geometry2D.Single.Point(10, 0)) 
                };
            int count = 100;
            for (int i = 0; i < count; i++)
            {
                float time = i / (float)count;
                Kean.Math.Geometry2D.Single.Point value = interpolate.Interpolate(time * 4);
                Kean.Math.Geometry2D.Single.Point onCircle = new Kean.Math.Geometry2D.Single.Point( 10 * Kean.Math.Single.Cosinus(Kean.Math.Single.Pi * 2 * time), 10 * Kean.Math.Single.Sinus(Kean.Math.Single.Pi * 2 * time));
                float distance = onCircle.Distance(value);
               Verify(distance, Is.LessThan(0.5f), this.prefix + "Point.0");
            }
            /*
            Kean.Collection.IList<Kean.Math.Geometry2D.Double.Point> points = new Kean.Collection.List<Kean.Math.Geometry2D.Double.Point>();
            int count = 100;
            for (int i = 0; i <= count; i++)
            {
                float time = i / (float)count * 4;
                points.Add(interpolate.Interpolate(time));
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
            file.WriteLine("clear all;");
            file.WriteLine("close all;");
            string pointsExport = "";
            foreach (Kean.Math.Geometry2D.Single.Point point in points)
                pointsExport += (string)point + ";";
            pointsExport = pointsExport.TrimEnd(';');
            file.WriteLine("points = [" + pointsExport + "];");
            file.WriteLine("plot(points(:,1), points(:,2),'r');");
            file.WriteLine("axis equal;");
            file.Close();
            */
        }
    }
}
