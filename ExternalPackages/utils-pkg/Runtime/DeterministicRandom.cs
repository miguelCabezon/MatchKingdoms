namespace JackSParrot.Utils
{
    public class DeterministicRandom
    {
        long _prev = 0;
        const long m = 4294967296; // aka 2^32
        const long a = 1664525;
        const long c = 1013904223;

        public DeterministicRandom(long seed = 0)
        {
            Init(seed);
        }

        public void Init(long seed)
        {
            _prev = seed;
        }

        public void Init(string seed)
        {
            long nseed = 0;
            for(int i = 0; i < seed.Length; ++i)
            {
                nseed += (int)seed[i];
            }
            _prev = nseed;
        }

        public long Next()
        {
            _prev = ((a * _prev) + c) % m;
            return _prev;
        }

        public long Next(long min, long max)
        {
            Next();
            return (_prev % max + min) % max;
        }
    }
}