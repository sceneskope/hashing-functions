namespace SceneSkope.HashingFunctions
{
    internal static class MurmurExtensions
    {
        internal static uint RotateLeft(this uint x, byte r) => (x << r) | (x >> (32 - r));

        internal static ulong RotateLeft(this ulong x, byte r) => (x << r) | (x >> (64 - r));

        internal static uint FMix(this uint h)
        {
            h = (h ^ (h >> 16)) * 0x85ebca6b;
            h = (h ^ (h >> 13)) * 0xc2b2ae35;
            return h ^ (h >> 16);
        }

        internal static ulong FMix(this ulong h)
        {
            h = (h ^ (h >> 33)) * 0xff51afd7ed558ccd;
            h = (h ^ (h >> 33)) * 0xc4ceb9fe1a85ec53;
            return h ^ (h >> 33);
        }
    }
}
