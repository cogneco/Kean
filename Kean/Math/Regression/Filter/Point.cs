using System;
using Kean;
using Kean.Extension;
using Matrix = Kean.Math.Matrix;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Math.Regression.Filter
{
    public class Point
    {
        Kalman kalman;
        public Point(float measurementCovariance, float processCovariance, Geometry2D.Single.Point start, float startCovariance, Geometry2D.Single.Size velocity, float velocityCovariance)
        {
            Matrix.Single transition = new Kean.Math.Matrix.Single(4, 4, new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1 });
            Matrix.Single measurement = new Kean.Math.Matrix.Single(4, 2, new float[] { 1, 0, 0, 1, 0, 0, 0, 0 });
            Matrix.Single measurementNoiseCovariance = measurementCovariance * Matrix.Single.Identity(2);
            Matrix.Single processNoiseCovariance = processCovariance * Matrix.Single.Identity(4);
            Matrix.Single initialState = new Kean.Math.Matrix.Single(1, 4, new float[] {start.X, start.Y, velocity.Width, velocity.Height});
            Matrix.Single initialErrorCovariance = new Kean.Math.Matrix.Single(4, 4, new float[] { startCovariance, 0, 0, 0, 0, startCovariance, 0, 0, 0, 0, velocityCovariance, 0, 0, 0, 0, velocityCovariance });
            this.kalman = new Kalman(transition, measurement, measurementNoiseCovariance, processNoiseCovariance,initialState, initialErrorCovariance, null);
        }
        public Tuple<Geometry2D.Single.Point, Geometry2D.Single.Size> Predict()
        {
            Tuple<Geometry2D.Single.Point, Geometry2D.Single.Size> result;
            Matrix.Single prediction = this.kalman.Predict();
            result = Tuple.Create(new Geometry2D.Single.Point(prediction[0, 0], prediction[0, 1]), new Geometry2D.Single.Size(prediction[0, 2], prediction[0, 3]));
            return result;
        }
        public Tuple<Geometry2D.Single.Point, Geometry2D.Single.Size> Correct(Geometry2D.Single.Point measurement)
        {
            Tuple<Geometry2D.Single.Point, Geometry2D.Single.Size> result;
            Matrix.Single correct = this.kalman.Correct(new Matrix.Single(1,2,new float[] {measurement.X, measurement.Y}));
            result = Tuple.Create(new Geometry2D.Single.Point(correct[0, 0], correct[0, 1]), new Geometry2D.Single.Size(correct[0, 2], correct[0, 3]));
            return result;
        }
      
    }
}
