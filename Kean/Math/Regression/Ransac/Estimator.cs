// 
//  Estimate.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

// RANSAC Method implemented according to described method given at http://en.wikipedia.org/wiki/RANSAC
using System;
using Collection = Kean.Collection;
using Kean.Extension;
using Kean.Collection.Extension;
namespace Kean.Math.Regression.Ransac
{
    public class Estimator<Domain, Range, Transform>
    {
        Model<Domain, Range, Transform> model;
        int maximumIterations;
        Domain[] domain;
        Range[] range;
        Random.IInterval<int> random;
        bool[] mask;
        double confidence;
        public Estimator(Model<Domain, Range, Transform> model, int maximumIterations) : this(model, maximumIterations, 0.99) { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model">The model used for movement approximation.</param>
        /// <param name="maximumIterations">Maximum number of iterations.</param>
        /// <param name="confidence">Result confidence.</param>
        public Estimator(Model<Domain, Range, Transform> model, int maximumIterations, double confidence)
        {
            this.model = model;
            this.maximumIterations = maximumIterations;
            this.confidence = confidence;
            this.random = new Random.Integer.Interval();
        }
        public void Load(Collection.IList<Tuple<Domain, Range>> data)
        {
            int count = data.Count;
            this.domain = new Domain[count];
            this.range = new Range[count];
            for (int i = 0; i < count; i++)
            {
                Tuple<Domain, Range> pair = data[i];
                this.domain[i] = pair.Item1;
                this.range[i] = pair.Item2;
            }
        }
        public void Load(Domain[] domain, Range[] range)
        {
            this.Load(domain, range, null);
        }
        /// <summary>
        /// Load the point ranges used when computing the estimation.
        /// </summary>
        /// <param name="domain">The domain values.</param>
        /// <param name="range">The range values.</param>
        /// <param name="mask">The mask values. Points will not be considered if their corresponding mask value is false. </param>
        public void Load(Domain[] domain, Range[] range, bool[] mask)
        {
            this.mask = mask;
            if (this.mask.IsNull())
            {
                this.domain = domain;
                this.range = range;
            }
            else
            {
                int count = 0;
                mask.Apply(b => count += b ? 1 : 0);
                this.domain = new Domain[count];
                this.range = new Range[count];
                int i = 0, j = 0;
                mask.Apply(b =>
                {
                    if (b)
                    {
                        this.domain[i] = domain[j];
                        this.range[i++] = range[j];
                    }
                    j++;
                });
            }
        }
        public void Reset()
        {
            this.domain = null;
            this.range = null;
            this.mask = null;
        }

        /// <summary>
        /// Compute the estimation. Points must first be loaded using load().  
        /// </summary>
        /// <returns>Computed estimation. Returns null if no estimation is found.</returns>
        public Estimation<Domain, Range, Transform> Compute()
        {
            Estimation<Domain, Range, Transform> result = null;
            int neededMeasures = this.model.FitsWell == 0 ? this.model.RequiredMeasures : this.model.FitsWell;
            int count = this.domain.Length;
            if (count >= neededMeasures)
            {
                int iterations = this.maximumIterations;
                bool[] bestMask = null;
                int maximumInliers = 0;
                //Console.WriteLine();
                for (int d = 0; d < iterations; d++)
                {
                    this.random.Floor = 0;
                    this.random.Ceiling = count - 1;
                    int[] indices = this.random.GenerateUnique(neededMeasures);
                    if (indices.NotNull())
                    {
                        Domain[] subDomain = new Domain[indices.Length];
                        Range[] subRange = new Range[indices.Length];
                        for (int i = 0; i < indices.Length; i++)
                        {
                            subDomain[i] = this.domain[indices[i]];
                            subRange[i] = this.range[indices[i]];
                        }
                        Transform maybeModel = this.model.Estimate(subDomain, subRange);
                        bool[] maybeMask = new bool[this.domain.Length];
                        int currentInliers = 0;
                        for (int i = 0; i < this.domain.Length; i++)
                            currentInliers += (maybeMask[i] = this.model.Fits(maybeModel, this.domain[i], this.range[i])) ? 1 : 0;
                        if (currentInliers > Kean.Math.Integer.Maximum(neededMeasures - 1, maximumInliers))
                        {
                            maximumInliers = currentInliers;
                            bestMask = maybeMask;
                            iterations = this.UpdateNumberOfIterations((double)(maximumInliers) / count, neededMeasures, iterations);
                            //Console.WriteLine(" iterations : " + d + " of total " + iterations + " consensus " + currentInliers + " of total " + count);
                        }
                    }
                }
                if (bestMask.NotNull())
                {
                    Domain[] subDomain = new Domain[maximumInliers];
                    Range[] subRange = new Range[maximumInliers];
                    for (int i = 0, k = 0; i < count; i++)
                        if (bestMask[i])
                        {
                            subDomain[k] = this.domain[i];
                            subRange[k++] = this.range[i];
                        }
                    bool[] totalMask = null;
                    if (this.mask.NotNull())
                    {
                        totalMask = new bool[this.mask.Length];
                        int i = 0, j = 0; 
                        this.mask.Apply(b =>
                        {
                            if (b && bestMask[i++])
                                totalMask[j] = true;
                            j++;
                        });
                    }
                    result = new Estimation<Domain, Range, Transform>(subDomain, subRange, this.model.Estimate(subDomain, subRange), totalMask);
                }
            }
            return result;
        }
        // See  parameter section under http://en.wikipedia.org/wiki/RANSAC
        int UpdateNumberOfIterations(double inlierProbability, int neededMeasures, int iterations)
        {
            int result = 0;
            double denominator = 1 - Kean.Math.Double.Power(inlierProbability, neededMeasures);
            denominator = Kean.Math.Double.Clamp(denominator, double.Epsilon, 1 - double.Epsilon);
            double nominator = Kean.Math.Double.Maximum(1 - this.confidence, double.Epsilon);
            nominator = Kean.Math.Double.Logarithm(nominator);
            denominator = Kean.Math.Double.Logarithm(denominator);
            result = -nominator >= -denominator * iterations ? iterations : Kean.Math.Integer.Round(nominator / denominator);
            return result;
        }
        Collection.IList<Tuple<Domain, Range>> Select(Collection.IList<Tuple<Domain, Range>> data, int needed)
        {
            Collection.IList<Tuple<Domain, Range>> result = new Collection.List<Tuple<Domain, Range>>(data.Count);
            this.random.Ceiling = data.Count - 1;
            int[] indices = this.random.GenerateUnique(needed);
            for (int i = 0; i < indices.Length; i++)
                result.Add(data[indices[i]]);
            return result;
        }
    }
}
