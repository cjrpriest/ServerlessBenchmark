using System.Collections.Generic;
using System.Numerics;

namespace Runner
{
    public static class PrimeNumberGenerator
    {
        public static IReadOnlyCollection<BigInteger> CalculatePrimeNumbers(BigInteger upTo)
        {
            var primeNosFound = new List<BigInteger>();
            for (int i = 0; i < upTo; i++)
            {
                if (IsPrime(i))
                {
                    primeNosFound.Add(i);
                }
            }

            return primeNosFound;
        }

        private static bool IsPrime(BigInteger candidate)
        {
            if ((candidate & 1) == 0)
            {
                return candidate == 2;
            }
            
            for (var i = 3; i * i <= candidate; i += 2)
            {
                if (candidate % i == 0)
                {
                    return false;
                }
            }
            return candidate != 1;
        }
    }
}