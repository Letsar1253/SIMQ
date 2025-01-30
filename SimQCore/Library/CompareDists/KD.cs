using System;

namespace SimQCore.Library.CompareDists {
    //Kolmogorov Distance
    public class KD : ICompareDists
    {
        public bool CompareDists(double[] dist1, double[] dist2, out double result)
        {
            return KolmogorovDistance( dist1, dist2, out result );
        }

        public static bool KolmogorovDistance(double[] dist1, double[] dist2, out double kolmogorovDistance)
        {
            int N = dist1.Length;

            double tempMax = double.MinValue;
            double tempDif;

            for (int i = 0; i < N; i++)
            {
                tempDif = Math.Abs(dist1[i] - dist2.Length < i ? 0 : dist2[i] );
                if (tempDif > tempMax)
                {
                    tempMax = tempDif;
                }
            }

            kolmogorovDistance = tempMax; //По формуле у меня Dn

            return true;
        }
    }
}
