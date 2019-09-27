namespace Gsafety.PTMS.Media.Utility.RandomGenerators
{
    public interface IRandomGenerator
    {
        void GetBytes(byte[] buffer, int offset, int count);

        float NextFloat();
        double NextDouble();

        void Reseed();
    }

    public interface IRandomGenerator<out T> : IRandomGenerator
    {
        T Next();
    }
}
