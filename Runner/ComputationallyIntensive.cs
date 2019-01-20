namespace Runner
{
    public static class ComputationallyIntensive
    {
        public static void Task()
        {
            PrimeNumberGenerator.CalculatePrimeNumbers(10000);
            FactorialCalculator.CalculateFactorial(5000);
        }
    }
}