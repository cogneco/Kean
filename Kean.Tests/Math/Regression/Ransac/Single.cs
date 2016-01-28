using System;
using NUnit.Framework;

using Kean.Extension;
using Target = Kean.Math.Regression.Ransac;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Math.Regression.Test.Ransac
{
    public class Single :
        Kean.Test.Fixture<Single>
    {
        protected override void Run()
        {
            this.Run(
                this.RobustPolynomialRegression,
                this.TranslationRegression,
                this.ScaleRotationTranslationRegression
                );
        }
        string prefix = "Kean.Math.Regression.Test.Ransac.Single.";
        [Test]
        public void RobustPolynomialRegression()
        {
            int degree = 1 + 1;
            System.Func<Kean.Math.Matrix.Single, float, float> map = (t, x) =>
            {
                float result = 0;
                for (int i = 0; i < t.Dimensions.Height; i++)
                    result += t[0, i] * Kean.Math.Single.Power(x, i);
                return result;
            };
            Target.Model<float, float, Kean.Math.Matrix.Single> model = new Target.Model<float, float, Kean.Math.Matrix.Single>()
            {
                RequiredMeasures = degree,
                Estimate = (domain, range) =>
                {
                    Kean.Math.Matrix.Single result = null;
                    int n = domain.Length;
                    Kean.Math.Matrix.Single a = new Kean.Math.Matrix.Single(degree, n);
                    Kean.Math.Matrix.Single b = new Kean.Math.Matrix.Single(1, n);
                    for (int i = 0; i < n; i++)
                    {
                        float x = domain[i];
                        b[0, i] = range[i];
                        for (int j = 0; j < degree; j++)
                            a[j, i] = Kean.Math.Single.Power(x, j);
                    }
                    result = a.Solve(b) ?? new Kean.Math.Matrix.Single(1, degree);
                    return result;
                },
                FitModel = (transform, domain, range) => Kean.Math.Double.Absolute(map(transform, domain) - range) < 80,
            };
            Target.Estimator<float, float, Matrix.Single> estimate =
                new Target.Estimator<float, float, Kean.Math.Matrix.Single>(model, 2000, 0.95);
            Kean.Math.Matrix.Single coefficients = new Kean.Math.Matrix.Single(1, degree, new float[] { 20, 10 });
            Kean.Math.Random.Single.Normal generator = new Kean.Math.Random.Single.Normal(0, 30);
            Kean.Math.Random.Single.Normal large = new Kean.Math.Random.Single.Normal(0, 500);

            int count = 200;
            float[] d = new float[count];
            float[] r = new float[count];
            bool[] mask = new bool[count];
            for (int i = 0; i < count; i++)
            {
                mask[i] = i < 50 || i > 100;
                d[i] = i;
                r[i] = map(coefficients, d[i]) + generator.Generate();
                if (i % 5 == 0)
                    r[i] += large.Generate();
            }

            estimate.Load(d, r, mask);
            Target.Estimation<float, float, Matrix.Single> best = estimate.Compute();
            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string maskedPointsExport = "";
                for (int i = 0; i < count; i++)
                    if (best.Mask[i])
                        maskedPointsExport += Kean.Math.Single.ToString(d[i]) + " " + Kean.Math.Single.ToString(r[i]) + ";";
                maskedPointsExport = maskedPointsExport.TrimEnd(';');
                string nonMaskedPointsExport = "";
                for (int i = 0; i < count; i++)
                    nonMaskedPointsExport += Kean.Math.Single.ToString(d[i]) + " " + Kean.Math.Single.ToString(r[i]) + ";";
                nonMaskedPointsExport = nonMaskedPointsExport.TrimEnd(';');
                file.WriteLine("maskedpoints = [" + maskedPointsExport + "];");
                file.WriteLine("nonmaskedpoints = [" + nonMaskedPointsExport + "];");
                string consensusExport = "";
                for (int i = 0; i < best.Count; i++)
                    consensusExport += Kean.Math.Single.ToString(best.InlierDomain[i]) + " " + Kean.Math.Single.ToString(best.InlierRange[i]) + ";";
                consensusExport = consensusExport.TrimEnd(';');
                file.WriteLine("consensus = [" + consensusExport + "];");

                string bestModelExport = "";
                for (int y = 0; y < degree; y++)
                    bestModelExport += Kean.Math.Single.ToString(best.Mapping[0, y]) + ";";
                bestModelExport = bestModelExport.TrimEnd(';');
                file.WriteLine("bestModel = [" + bestModelExport + "];");
                file.WriteLine("scatter(maskedpoints(:,1), maskedpoints(:,2),'b');");
                file.WriteLine("scatter(nonmaskedpoints(:,1), nonmaskedpoints(:,2),'k');");

                string correctModelExport = "";
                for (int y = 0; y < degree; y++)
                    correctModelExport += Kean.Math.Single.ToString(coefficients[0, y]) + ";";
                correctModelExport = correctModelExport.TrimEnd(';');
                file.WriteLine("correctModel = [" + correctModelExport + "];");

                file.WriteLine("hold on;");
                file.WriteLine("scatter(consensus(:,1), consensus(:,2),'r');");
                file.WriteLine("plot(nonmaskedpoints(:,1), polyval(fliplr(correctModel'),nonmaskedpoints(:,1)), 'r');");
                file.WriteLine("plot(nonmaskedpoints(:,1), polyval(fliplr(bestModel'),nonmaskedpoints(:,1)), 'g');");
                file.Close();
                int n = best.Count;
                Matrix.Single a = new Matrix.Single(degree, n);
                Matrix.Single b = new Matrix.Single(1, n);
                for (int i = 0; i < n; i++)
                {
                    float x = best.InlierDomain[i];
                    b[0, i] = best.InlierRange[i];
                    for (int j = 0; j < degree; j++)
                        a[j, i] = Kean.Math.Single.Power(x, j);
                }
                Matrix.Single guess = new Matrix.Single(1, degree);
                for (int i = 0; i < degree; i++)
                    guess[0, i] = 1;
                guess = best.Mapping;
                Kean.Math.Matrix.Single refine = this.Estimate(a, b, guess);

            }

        }
        Kean.Math.Matrix.Single Estimate(Kean.Math.Matrix.Single a, Kean.Math.Matrix.Single b, Kean.Math.Matrix.Single guess)
        {
            Func<Kean.Math.Matrix.Single, Kean.Math.Matrix.Single> function = x => b - a * x;
            Func<Kean.Math.Matrix.Single, Kean.Math.Matrix.Single> jacobian = x => -a;
            Kean.Math.Regression.Minimization.LevenbergMarquardt.Single lm = new Kean.Math.Regression.Minimization.LevenbergMarquardt.Single(function, jacobian, 200, 1e-18f, 1e-18f, 1e-2f);
            return lm.Estimate(guess);
        }

        [Test]
        public void ScaleRotationTranslationRegression()
        {
            Func<Matrix.Single, Geometry2D.Single.Point, Geometry2D.Single.Point> map = (t, x) => new Geometry2D.Single.Point(t[0, 0] * x.X - t[0, 1] * x.Y + t[0, 2], t[0, 1] * x.X + t[0, 0] * x.Y + t[0, 3]);
            Target.Model<Geometry2D.Single.Point, Geometry2D.Single.Point, Kean.Math.Matrix.Single> model = new Target.Model<Geometry2D.Single.Point, Geometry2D.Single.Point, Kean.Math.Matrix.Single>()
            {
                RequiredMeasures = 5,
                Estimate = (domain, range) =>
                {
                    int count = domain.Length;
                    Kean.Math.Matrix.Single a = new Kean.Math.Matrix.Single(4, 2 * count);
                    Kean.Math.Matrix.Single b = new Kean.Math.Matrix.Single(1, 2 * count);
                    int j = 0;
                    for (int i = 0; i < count; i++)
                    {
                        Geometry2D.Single.Point previous = domain[i];
                        Geometry2D.Single.Point y = range[i];
                        a[0, j] = previous.X;
                        a[1, j] = -previous.Y;
                        a[2, j] = 1;
                        a[3, j] = 0;
                        b[0, j++] = y.X;
                        a[0, j] = previous.Y;
                        a[1, j] = previous.X;
                        a[2, j] = 0;
                        a[3, j] = 1;
                        b[0, j++] = y.Y;
                    }
                    return a.Solve(b) ?? new Kean.Math.Matrix.Single(1, 4);
                },
                FitModel = (tranform, domain, range) => map(tranform, domain).Distance(range) < 8,
            };
            Target.Estimator<Geometry2D.Single.Point, Geometry2D.Single.Point, Kean.Math.Matrix.Single> estimate =
                new Target.Estimator<Geometry2D.Single.Point, Geometry2D.Single.Point, Kean.Math.Matrix.Single>(model, 20, 0.999);
            Collection.IList<Tuple<Geometry2D.Single.Point, Geometry2D.Single.Point>> previousCurrentPoints =
                new Collection.List<Tuple<Geometry2D.Single.Point, Geometry2D.Single.Point>>();
            float s = 2;
            float thetaAngle = Kean.Math.Single.ToRadians(5);
            float xTranslation = 7;
            float yTranslation = 10;
            Kean.Math.Random.Single.Normal normal = new Kean.Math.Random.Single.Normal(0, 1);
            Kean.Collection.IList<Geometry2D.Single.Point> previousPoints = new Kean.Collection.List<Geometry2D.Single.Point>();
            for (int x = -100; x < 100; x += 20)
                for (int y = -100; y < 100; y += 20)
                    previousPoints.Add(new Geometry2D.Single.Point(x, y));
            for (int i = 0; i < previousPoints.Count; i++)
            {
                Geometry2D.Single.Point x = previousPoints[i];
                Geometry2D.Single.Point y = new Geometry2D.Single.Point(
                    s * Kean.Math.Single.Cosinus(thetaAngle) * x.X - s * Kean.Math.Single.Sinus(thetaAngle) * x.Y + xTranslation,
                    s * Kean.Math.Single.Sinus(thetaAngle) * x.X + s * Kean.Math.Single.Cosinus(thetaAngle) * x.Y + yTranslation);
                float[] xdd = normal.Generate(2);
                float[] ydd = normal.Generate(2);
                Geometry2D.Single.Point xd = new Kean.Math.Geometry2D.Single.Point(xdd[0], xdd[1]);
                Geometry2D.Single.Point yd = new Kean.Math.Geometry2D.Single.Point(ydd[0], ydd[1]);
                previousCurrentPoints.Add(Tuple.Create<Geometry2D.Single.Point, Geometry2D.Single.Point>(x + xd, y + yd));
            }
            previousCurrentPoints.Add(Tuple.Create<Geometry2D.Single.Point, Geometry2D.Single.Point>(new Geometry2D.Single.Point(130, 130), new Geometry2D.Single.Point(720, 70)));
            estimate.Load(previousCurrentPoints);
            Target.Estimation<Geometry2D.Single.Point, Geometry2D.Single.Point, Kean.Math.Matrix.Single> best = estimate.Compute();
            Matrix.Single correct = new Matrix.Single(1, 4, new float[] { s * Kean.Math.Single.Cosinus(thetaAngle), s * Kean.Math.Single.Sinus(thetaAngle), xTranslation, yTranslation });
            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string before = "";
                string after = "";
                foreach (Tuple<Geometry2D.Single.Point, Geometry2D.Single.Point> point in previousCurrentPoints)
                {
                    before += point.Item1.ToString() + "; ";
                    after += point.Item2.ToString() + "; ";
                }
                before = before.TrimEnd(';');
                after = after.TrimEnd(';');
                file.WriteLine("before = [" + before + "];");
                file.WriteLine("after = [" + after + "];");
                file.WriteLine("scatter(before(:,1),before(:,2),'b');");
                file.WriteLine("hold on;");
                file.WriteLine("scatter(after(:,1),after(:,2),'r');");
                string consensus = "";
                for (int i = 0; i < best.Count; i++)
                    consensus += best.InlierRange[i].ToString() + "; ";
                consensus = consensus.TrimEnd(';');
                file.WriteLine("consensus = [" + consensus + "];");
                file.WriteLine("scatter(consensus(:,1),consensus(:,2),'g');");
                file.WriteLine("bestmodel = [" + best.Mapping + "];");
                file.Close();

                int count = best.Count;
                Func<Kean.Math.Matrix.Single, Kean.Math.Matrix.Single> f = beta =>
                {
                    float scale = beta[0, 0];
                    float theta = beta[0, 1];
                    float xt = beta[0, 2];
                    float yt = beta[0, 3];
                    Kean.Math.Matrix.Single result = new Kean.Math.Matrix.Single(1, 2 * count);
                    for (int i = 0; i < count; i++)
                    {
                        Geometry2D.Single.Point previous = best.InlierDomain[i];
                        Geometry2D.Single.Point current = best.InlierRange[i];
                        result[0, 2 * i + 0] = scale * Kean.Math.Single.Cosinus(theta) * previous.X - scale * Kean.Math.Single.Sinus(theta) * previous.Y + xt - current.X;
                        result[0, 2 * i + 1] = scale * Kean.Math.Single.Sinus(theta) * previous.X + scale * Kean.Math.Single.Cosinus(theta) * previous.Y + yt - current.Y;
                    }
                    return result;
                };
                Func<Kean.Math.Matrix.Single, Kean.Math.Matrix.Single> j = beta =>
                {
                    float scale = beta[0, 0];
                    float theta = beta[0, 1];
                    float xt = beta[0, 2];
                    float yt = beta[0, 3];
                    Kean.Math.Matrix.Single result = new Kean.Math.Matrix.Single(4, 2 * count);
                    for (int i = 0; i < count; i++)
                    {
                        Geometry2D.Single.Point previous = best.InlierDomain[i];
                        result[0, 2 * i + 0] = Kean.Math.Single.Cosinus(theta) * previous.X - Kean.Math.Single.Sinus(theta) * previous.Y;
                        result[1, 2 * i + 0] = -scale * Kean.Math.Single.Sinus(theta) * previous.X - scale * Kean.Math.Single.Cosinus(theta) * previous.Y;
                        result[2, 2 * i + 0] = 1;
                        result[3, 2 * i + 0] = 0;
                        result[0, 2 * i + 1] = Kean.Math.Single.Sinus(theta) * previous.X + Kean.Math.Single.Cosinus(theta) * previous.Y;
                        result[1, 2 * i + 1] = scale * Kean.Math.Single.Cosinus(theta) * previous.X - scale * Kean.Math.Single.Sinus(theta) * previous.Y;
                        result[2, 2 * i + 1] = 0;
                        result[3, 2 * i + 1] = 1;
                    }
                    return result;
                };
                /*
                Kean.Math.Regression.Minimization.LevenbergMarquardt.Single lm =
                    new Kean.Math.Regression.Minimization.LevenbergMarquardt.Single(f, j,
                        200, 1e-8f, 1e-8f, 1e-3f);
                float scaleGuess = Kean.Math.Single.SquareRoot(Kean.Math.Single.Squared(best.Mapping[0, 0]) + Kean.Math.Single.Squared(best.Mapping[0, 1]));
                float thetaGuess = Kean.Math.Single.ArcusTangensExtended(best.Mapping[0, 1], best.Mapping[0, 0]);
                float xTranslationGuess = best.Mapping[0, 2];
                float yTranslationGuess = best.Mapping[0, 3];
                Kean.Math.Matrix.Single guess = new Kean.Math.Matrix.Single(1, 4, new float[] { 1, 1, 1, 1 }); // new Kean.Math.Matrix.Single(1, 4, new float[] { scaleGuess, thetaGuess, xTranslationGuess, yTranslationGuess }); 
                Kean.Math.Matrix.Single estimation = lm.Estimate(guess);
                */
            }


        }
        [Test]
        public void TranslationRegression()
        {
            Target.Model<Geometry2D.Single.Point, Geometry2D.Single.Point, Geometry2D.Single.Point> model = new Target.Model<Geometry2D.Single.Point, Geometry2D.Single.Point, Geometry2D.Single.Point>()
            {
                RequiredMeasures = 5,
                Estimate = (domain, range) =>
                {
                    int count = domain.Length;
                    Geometry2D.Single.Point result = new Geometry2D.Single.Point();
                    for (int i = 0; i < count; i++)
                        result += range[i] - domain[i];
                    result.X /= (float)count;
                    result.Y /= (float)count;
                    return result;
                },
                FitModel = (transform, domain, range) => (transform + domain).Distance(range) < 10,
            };
            Target.Estimator<Geometry2D.Single.Point, Geometry2D.Single.Point, Geometry2D.Single.Point> estimate =
                new Target.Estimator<Geometry2D.Single.Point, Geometry2D.Single.Point, Geometry2D.Single.Point>(model, 20);
            Collection.IList<Tuple<Geometry2D.Single.Point, Geometry2D.Single.Point>> previousCurrentPoints =
                new Collection.List<Tuple<Geometry2D.Single.Point, Geometry2D.Single.Point>>();
            Geometry2D.Single.Point translation = new Geometry2D.Single.Point(7, 10);
            Kean.Math.Random.Single.Normal normal = new Kean.Math.Random.Single.Normal(0, 2);
            Kean.Collection.IList<Geometry2D.Single.Point> previousPoints = new Kean.Collection.List<Geometry2D.Single.Point>();
            for (int x = -100; x < 100; x += 20)
                for (int y = -100; y < 100; y += 20)
                    previousPoints.Add(new Geometry2D.Single.Point(x, y));
            for (int i = 0; i < previousPoints.Count; i++)
            {
                Geometry2D.Single.Point x = previousPoints[i];
                Geometry2D.Single.Point y = translation + x;
                float[] xdd = normal.Generate(2);
                float[] ydd = normal.Generate(2);
                Geometry2D.Single.Point xd = new Kean.Math.Geometry2D.Single.Point(xdd[0], xdd[1]);
                Geometry2D.Single.Point yd = new Kean.Math.Geometry2D.Single.Point(ydd[0], ydd[1]);
                previousCurrentPoints.Add(Tuple.Create<Geometry2D.Single.Point, Geometry2D.Single.Point>(x + xd, y + yd));
            }
            previousCurrentPoints.Add(Tuple.Create<Geometry2D.Single.Point, Geometry2D.Single.Point>(new Geometry2D.Single.Point(107, 107), new Geometry2D.Single.Point(120, 130)));
            estimate.Load(previousCurrentPoints);
            Target.Estimation<Geometry2D.Single.Point, Geometry2D.Single.Point, Geometry2D.Single.Point> best = estimate.Compute();
            if (best.NotNull())
               Verify(best.Mapping.Distance(translation), Is.EqualTo(0).Within(5), this.prefix + "TranslationRegression.0");
            /*
            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string before = "";
                string after = "";
                foreach (Tuple<Geometry2D.Single.Point, Geometry2D.Single.Point> point in previousCurrentPoints)
                {
                    before += point.Item1.ToString() + "; ";
                    after += point.Item2.ToString() + "; ";
                }
                before = before.TrimEnd(';');
                after = after.TrimEnd(';');
                file.WriteLine("before = [" + before + "];");
                file.WriteLine("after = [" + after + "];");
                file.WriteLine("scatter(before(:,1),before(:,2),'b');");
                file.WriteLine("hold on;");
                file.WriteLine("scatter(after(:,1),after(:,2),'r');");
                string consensus = "";
                foreach (Tuple<Geometry2D.Single.Point, Geometry2D.Single.Point> point in best.Inliers)
                    consensus += point.Item2.ToString() + "; ";
                consensus = consensus.TrimEnd(';');
                file.WriteLine("consensus = [" + consensus + "];");
                file.WriteLine("scatter(consensus(:,1),consensus(:,2),'g');");
                file.WriteLine("bestmodel = [" + Kean.Math.Single.ToString(best.Mapping.X) + " " + Kean.Math.Single.ToString(best.Mapping.Y) + "];");
                file.WriteLine("xlabel(strcat(' x= ', num2str(bestmodel(1)), ' y= ', num2str(bestmodel(2))))");
                file.Close();
            }
            */
        }
    }
}
