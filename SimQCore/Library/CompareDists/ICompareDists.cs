namespace SimQCore.Library.CompareDists {
    public interface ICompareDists
    {
        public bool CompareDists(double[] dist1, double[] dist2, int N, out double result);
    }
}
