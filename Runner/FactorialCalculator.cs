using System.Numerics;

namespace Runner
{
    public static class FactorialCalculator
    {
        public static BigInteger CalculateFactorial(BigInteger number)
        {
            for (var i = number - 1; i >= 1; i--)
            {
                number = number * i;
            }
            return number;
        }
    }
}