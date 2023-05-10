using System.Numerics;

namespace Tickets
{
    internal class TicketsTask
    {
        public static BigInteger Solve(int halfLen, int totalSum)
        {
            if (totalSum % 2 != 0)
                return 0;
            var opt = FillOptTable(halfLen, totalSum / 2);
            var halfResult = CountHappyTickets(opt, halfLen, totalSum / 2);
            return halfResult * halfResult;
        }
 
        private static BigInteger[,] FillOptTable(int halfLen, int halfSum)
        {
            var opt = new BigInteger[halfLen + 1, halfSum + 1];
            for (var i = 0; i <= halfLen; i++)
            for (var j = 0; j <= halfSum; j++)
                opt[i, j] = -1;
            return opt;
        }
 
        private static BigInteger CountHappyTickets(BigInteger[,] opt, int len, int halfSum)
        {
            if (opt[len, halfSum] >= 0) 
                return opt[len, halfSum];
            if (halfSum == 0) 
                return 1;
            if (len == 0) 
                return 0;
            opt[len, halfSum] = 0;
            for (var i = 0; i < 10; i++)
                if (halfSum - i >= 0)
                    opt[len, halfSum] += CountHappyTickets(opt, len - 1, halfSum - i);
            return opt[len, halfSum];
        }
    }
}