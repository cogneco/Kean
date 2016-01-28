using System;
using Kean;
using Kean.Extension;
using Matrix = Kean.Math.Matrix;

namespace Kean.Math.Regression.Filter
{
    public class Kalman
    {
        Matrix.Single statePredicted;              // Predicted state (x'(k)).
        Matrix.Single statePostCorrected;          // Corrected state (x(k)).
        Matrix.Single transition;                  // State transition matrix (A).
        Matrix.Single control;                     // Control matrix (B) (not used if there is no control).
        Matrix.Single measurement;                 // Measurement matrix (H).
        Matrix.Single processNoiseCovariance;      // Process noise covariance matrix (Q).
        Matrix.Single measurementNoiseCovariance;  // Measurement noise covariance matrix (R).
        Matrix.Single errorCovariancePriori;       // Priori error estimate covariance matrix (P'(k)).
        Matrix.Single gain;                        // Kalman gain matrix (K(k)).
        Matrix.Single errorCovariancePosteriori;   // Posteriori error estimate covariance matrix (P(k)).

        public Kalman(
            Matrix.Single transition, 
            Matrix.Single measurement, 
            Matrix.Single measurementNoiseCovariance, 
            Matrix.Single processNoiseCovariance,
            Matrix.Single initialState,
            Matrix.Single initialErrorCovariance,
            Matrix.Single control)
        {
            int dynamicParameters = transition.Order;
            int measureParameters = measurementNoiseCovariance.Order;
            this.transition = transition;
            this.processNoiseCovariance = processNoiseCovariance;
            this.measurement = measurement;
            this.measurementNoiseCovariance = measurementNoiseCovariance;

            this.statePostCorrected = initialState;
            this.errorCovariancePosteriori = initialErrorCovariance;
            
            this.statePredicted = new Kean.Math.Matrix.Single(1, dynamicParameters);
            this.errorCovariancePriori = new Kean.Math.Matrix.Single(dynamicParameters);
            this.gain = new Kean.Math.Matrix.Single(measureParameters, dynamicParameters);
            
            this.control = control;
        }
        public Kalman(int dynamicParameters, int measureParameters, int controlParameters)
        {
            this.statePredicted = new Kean.Math.Matrix.Single(1, dynamicParameters);
            this.statePostCorrected = new Kean.Math.Matrix.Single(1, dynamicParameters);
            this.transition = Matrix.Single.Identity(dynamicParameters);
            this.processNoiseCovariance = Matrix.Single.Identity(dynamicParameters);
            this.measurement = new Kean.Math.Matrix.Single(dynamicParameters, measureParameters);
            this.measurementNoiseCovariance = Matrix.Single.Identity(measureParameters);
            this.errorCovariancePriori = new Kean.Math.Matrix.Single(dynamicParameters);
            this.errorCovariancePosteriori = new Kean.Math.Matrix.Single(dynamicParameters);
            this.gain = new Kean.Math.Matrix.Single(measureParameters, dynamicParameters);
            if (controlParameters > 0)
                this.control = new Kean.Math.Matrix.Single(controlParameters, dynamicParameters);
        }
        public Matrix.Single Predict()
        {
            return this.Predict(null);
        }
        public Matrix.Single Predict(Matrix.Single control)
        {
            Matrix.Single result;    
            // Update state
            // x'(k) = A*x(k)
            this.statePredicted = this.transition * this.statePostCorrected;
            // x'(k) = x'(k) + B*u(k)
            if (this.control.NotNull() && control.NotNull())
                this.statePredicted += this.control * control;
            // update error covariance
            // P'(k) = A*P(k)*At + Q
            this.errorCovariancePriori = (this.transition * this.errorCovariancePosteriori) * this.transition.Transpose() + this.processNoiseCovariance;
            result = this.statePredicted;
            return result;
        }
        public Matrix.Single Correct(Matrix.Single measurement)
        {
            Matrix.Single result = this.statePostCorrected;
            if (measurement.NotNull())
            {
                // a = H*P'(k)
                Matrix.Single b = this.measurement * this.errorCovariancePriori;
                // b = temp2*Ht + R
                Matrix.Single a = b * this.measurement.Transpose() + this.measurementNoiseCovariance;
                // c = inv(a) * b
                Matrix.Single c = a.Solve(b);
                // K(k) = xt
                this.gain = c.Transpose();
                // x(k) = x'(k) + K(k)*(z(k) - H*x'(k))
                result = this.statePostCorrected = this.statePredicted + this.gain * (measurement - this.measurement * this.statePredicted);
                // P(k) = P'(k) - K(k)*temp2
                this.errorCovariancePosteriori = this.errorCovariancePriori - this.gain * b;
            }
            return result;
        }
    }
}
