using SceneSkope.HashingFunctions;
using System.Security.Cryptography;
using Xunit;

namespace TestHashingFunctions
{
    public class TestMurmur128
    {
        [Fact]
        public void VerifySimpleX64HashWorksOk()
        {
            const int bits = 128;
            const uint expected = 0x6384BA69U;

            var computed = HashVerifier.ComputeVerificationHash(bits, seed => new SceneSkope.HashingFunctions.Murmur128X64(seed));
            Assert.Equal(expected, computed);
        }

        [Fact]
        public void VerifySimpleX86HashWorksOk()
        {
            const int bits = 128;
            const uint expected = 0xB3ECE62A;

            var computed = HashVerifier.ComputeVerificationHash(bits, seed => new SceneSkope.HashingFunctions.Murmur128X86(seed));
            Assert.Equal(expected, computed);
        }

        [Fact]
        public void VerifyHashesMatchOriginalCode()
        {
            const int bits = 128;

            var computedNew = HashVerifier.ComputeVerificationHash(bits, seed => new SceneSkope.HashingFunctions.Murmur128X64(seed));
            var computedOld = HashVerifier.ComputeVerificationHash(bits, seed => Murmur.MurmurHash.Create128(seed, true, Murmur.AlgorithmPreference.X64));
            Assert.Equal(computedOld, computedNew);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(1024, true)]
        [InlineData(1023, true)]
        [InlineData(1021, true)]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(1024, false)]
        [InlineData(1023, false)]
        [InlineData(1021, false)]
        public void VerifyDifferentLengthHashesAreTheSameNewAndOld(int length, bool useX64)
        {
            var input = new byte[length];
            using (var crypto = RNGCryptoServiceProvider.Create())
            {
                crypto.GetNonZeroBytes(input);
            }

            using (var newHash = useX64 ? (Murmur128)new Murmur128X64() : new Murmur128X86())
            using (var oldHash = Murmur.MurmurHash.Create128(0, true, useX64 ? Murmur.AlgorithmPreference.X64 : Murmur.AlgorithmPreference.X86))
            {
                var newHashOutput = new byte[16];
                newHash.TryComputeHash(new System.ReadOnlySpan<byte>(input), new System.Span<byte>(newHashOutput), out int newBytesWritten);
                var oldHashOutput = oldHash.ComputeHash(input);

                var newHashOutputOldStyle = newHash.ComputeHash(input);

                Assert.Equal(oldHashOutput, newHashOutput);
                Assert.Equal(oldHashOutput, newHashOutputOldStyle);
            }
        }
    }
}
