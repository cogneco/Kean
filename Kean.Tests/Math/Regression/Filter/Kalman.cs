using System;
using NUnit.Framework;

using Kean.Extension;
using Target = Kean.Math.Regression.Filter;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Collection;


namespace Kean.Math.Regression.Test.Filter
{
    public class Kalman :
    Kean.Test.Fixture<Kalman>
    {
        protected override void Run()
        {
            this.Run(
                this.Point
                );
        }
        [Test]
        public void Point()
        {
            Random.Single.Normal generator = new Kean.Math.Random.Single.Normal(0,2);
            float radius = 100;
            Collection.List<Geometry2D.Single.Point> measures = new Collection.List<Kean.Math.Geometry2D.Single.Point>();
            for (int i = 0; i < 360; i++)
                measures.Add(new Kean.Math.Geometry2D.Single.Point(Kean.Math.Single.Cosine(Kean.Math.Single.ToRadians(i)) * radius, Kean.Math.Single.Sine(Kean.Math.Single.ToRadians(i)) * radius) + new Geometry2D.Single.Point(generator.Generate(), generator.Generate()));
            Target.Point kalman = new Target.Point(2f, 0.001f, measures[0], 2f, new Geometry2D.Single.Size(0, 0), 1f);
            Collection.List<Geometry2D.Single.Point> predictions = new Collection.List<Kean.Math.Geometry2D.Single.Point>();
            Collection.List<Geometry2D.Single.Point> estimates = new Collection.List<Kean.Math.Geometry2D.Single.Point>();
            foreach (Geometry2D.Single.Point p in measures)
            {
                Tuple<Geometry2D.Single.Point, Geometry2D.Single.Size> predict = kalman.Predict();
                predictions.Add(predict.Item1);
                Tuple<Geometry2D.Single.Point, Geometry2D.Single.Size> estimate = kalman.Correct(p);
                estimates.Add(estimate.Item1);
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
            file.WriteLine("clear all;");
            file.WriteLine("close all;");
            file.WriteLine("figure; hold on;");
            file.WriteLine("axis equal;");

            string measuresExport = "";
            for (int i = 0; i < measures.Count; i++)
                measuresExport += Kean.Math.Single.ToString(measures[i].X) + " " + Kean.Math.Single.ToString(measures[i].Y) + ";";
            measuresExport = measuresExport.TrimEnd(';');
            string estimatesExport = "";
            for (int i = 0; i < estimates.Count; i++)
                estimatesExport += Kean.Math.Single.ToString(estimates[i].X) + " " + Kean.Math.Single.ToString(estimates[i].Y) + ";";
            estimatesExport = estimatesExport.TrimEnd(';');
            string predictionsExport = "";
            for (int i = 0; i < estimates.Count; i++)
                predictionsExport += Kean.Math.Single.ToString(predictions[i].X) + " " + Kean.Math.Single.ToString(predictions[i].Y) + ";";
            predictionsExport = predictionsExport.TrimEnd(';');
            file.WriteLine("measuresExport = [" + measuresExport + "];");
            file.WriteLine("estimatesExport = [" + estimatesExport + "];");
            file.WriteLine("predictionsExport = [" + predictionsExport + "];");
            file.WriteLine("plot(measuresExport(:,1), measuresExport(:,2),'b');");
            file.WriteLine("plot(estimatesExport(:,1), estimatesExport(:,2),'r');");
            file.WriteLine("plot(predictionsExport(:,1), predictionsExport(:,2),'g');");
            file.Close();    
        }
    }
}
