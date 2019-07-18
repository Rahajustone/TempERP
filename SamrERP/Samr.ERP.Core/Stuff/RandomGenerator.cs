using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Samr.ERP.Core.Stuff
{
    public class RandomGenerator
    {
        public static string GenerateNewPassword(int length = 6)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[length];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
      
        public static int GenerateRandomNumber(int min, int max)
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                // Generate four random bytes
                byte[] four_bytes = new byte[4];
                crypto.GetBytes(four_bytes);

                // Convert the bytes to a UInt32
                UInt32 scale = BitConverter.ToUInt32(four_bytes, 0);

                // And use that to pick a random number >= min and < max
                return (int)(min + (max - min) * (scale / (uint.MaxValue + 1.0)));

            }

        }
    }
}
