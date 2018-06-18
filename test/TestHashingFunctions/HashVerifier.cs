﻿using System;
using System.Security.Cryptography;

namespace TestHashingFunctions
{
    public static class HashVerifier
    {
        public static uint ComputeVerificationHash(int bits, Func<uint, HashAlgorithm> algorithmFactory)
        {
            int bytes = bits / 8;
            byte[] key = new byte[256];
            byte[] hashes = new byte[bytes * 256];
            for (int i = 0; i < 256; i++)
            {
                key[i] = (byte)i;
                using (var algorithm = algorithmFactory((uint)(256 - i)))
                    Array.Copy(algorithm.ComputeHash(key, 0, i), 0, hashes, i * bytes, bytes);
            }

            using (var algorithm = algorithmFactory(0))
                return BitConverter.ToUInt32(algorithm.ComputeHash(hashes), 0);
        }
    }
}
