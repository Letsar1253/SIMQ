namespace SimQCore.Library.CompareDists {
    internal interface ICompareDists
    {
        bool CompareDists(double[] Dist1, double[] Dist2, int N, out double Result);
    }
}
