using System.Security.Cryptography;

namespace SceneSkope.HashingFunctions
{
    public abstract class Murmur128 : HashAlgorithm
    {
        protected readonly uint _seed;

        protected Murmur128(uint seed)
        {
            _seed = seed;
            HashSizeValue = 128;
        }
    }
}
