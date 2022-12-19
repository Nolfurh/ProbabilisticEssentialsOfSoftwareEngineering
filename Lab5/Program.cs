using System.Numerics;

namespace Lab5;

class Program
{
    public static void Main()
    {
        Task1(3, 5, 0.2);
        Task2(4, 5, 0.8);
        Task3(80, 400, 0.2);
        Task4(5, 100000, 0.0001);
        Task5(228, 252, 600, 0.4);
        Task6(100, 0.4);
        Task7(0, 170, 4000, 0.04);
        Task8(5000, 10000);
        Task9(5, 1000, 0.002);
        Task10(150, 0.03m);
    }


    #region Calculation
    public static int C(int m, int n)
    {
        BigInteger num = Factiorial(n) / (Factiorial(m) * Factiorial(n - m));
        return (int)num;
    }

    public static BigInteger Factiorial(int a)
    {
        BigInteger num = 1;
        for (int i = 2; i <= a; i++)
        {
            num *= i;
        }
        return num;
    }

    public static double BernoulliTrial(int m, int n, double p)
    {
        return C(m, n) * Math.Pow(p, m) * Math.Pow(ReverseP(p), n - m);
    }

    public static double ReverseP(double p)
    {
        return 1 - p;
    }

    public static double LocalDeMoivreLaplaceTheorem(int m, int n, double p)
    {
        var x = (m - n * p) / (Math.Sqrt(n * p * ReverseP(p)));
        var a = 1 / Math.Sqrt(n * p * ReverseP(p));
        var b = (1 / Math.Sqrt(2 * Math.PI));
        var c = Math.Pow(Math.E, -(Math.Pow(x, 2) / 2));

        return a * b * c;
    }
    public static double IntegralDeMoivreLaplaceTheorem(int m1, int m2, int n, double p)
    {
        var x1 = (m1 - n * p) / Math.Sqrt(n * p * ReverseP(p));
        var x2 = (m2 - n * p) / Math.Sqrt(n * p * ReverseP(p));

        var P = Phi(x2) - Phi(x1);
        return P;
    }
    public static double Phi(double x)
    {
        return (1 / Math.Sqrt(2 * Math.PI) * Math.Sqrt(Math.PI / 2) * Erf(x / Math.Sqrt(2)));
    }
    public static double Erf(double x)
    {
        const double a1 = 0.254829592;
        const double a2 = -0.284496736;
        const double a3 = 1.421413741;
        const double a4 = -1.453152027;
        const double a5 = 1.061405429;
        const double p = 0.3275911;

        int sign = 1;
        if (x < 0)
            sign = -1;
        x = Math.Abs(x);

        // A&S formula 7.1.26
        double t = 1.0 / (1.0 + p * x);
        double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

        return sign * y;
    }

    public static double PoissonsEquation(int m, int n, double p)
    {
        var l = n * p;
        return (Math.Pow(l, m) / (double)Factiorial(m)) * Math.Pow(Math.E, -l);
    }

    public static void MostLikelyNumber(int n, decimal p, out decimal m1, out decimal m2)
    {
        decimal q = (decimal)ReverseP((double)p);
        m1 = n * p - q;
        m2 = n * p + p;
    }

    #endregion

    #region Tasks
    public static void Task1(int m, int n, double p)
    {
        Console.WriteLine("Task 1");
        Console.WriteLine($"P = {BernoulliTrial(m, n, p)}");
        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task2(int m, int n, double p)
    {
        Console.WriteLine("Task 2");
        Console.WriteLine($"Event A will be executed exactly {m} times: {BernoulliTrial(m, n, p)}");

        double pNotLessThenM = 0.0;
        for (int i = n; i >= m; i--)
        {
            pNotLessThenM += BernoulliTrial(i, n, p);
        }

        Console.WriteLine($"Evant A will be executed not less than {m} times: {pNotLessThenM}");
        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task3(int m, int n, double p)
    {
        Console.WriteLine("Task 3");
        Console.WriteLine($"Probability that among {n} candle will be exactly {m} lollipops: {LocalDeMoivreLaplaceTheorem(m, n, p)}");
        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task4(int m, int n, double p)
    {
        Console.WriteLine("Task 4");
        Console.WriteLine($"P = {PoissonsEquation(m, n, p)}");
        //Console.WriteLine($"Poisson's version: {PoissonsEquation(m, n, p)}");
        //Console.WriteLine(new String('.', 50));
        //Console.WriteLine($"Laplace's version: {LocalDeMoivreLaplaceTheorem(m, n, p)}");
        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task5(int m1, int m2, int n, double p)
    {
        Console.WriteLine("Task 5");
        var P = IntegralDeMoivreLaplaceTheorem(m1, m2, n, p);
        Console.WriteLine($"P = {P}");
        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task6(int n, double p)
    {
        Console.WriteLine("Task 6");
        MostLikelyNumber(n, (decimal)p, out decimal m1, out decimal m2);
        if (m1 == Math.Ceiling(m1) && m2 == Math.Ceiling(m2))
        {
            double P = LocalDeMoivreLaplaceTheorem((int)Math.Ceiling(m1), n, p);
            Console.WriteLine($"Probability of most likely number of clints every day ({m1}, {m2}) is: {P}");
        }
        else
        {
            double P = LocalDeMoivreLaplaceTheorem((int)Math.Ceiling(m1), n, p); ;
            Console.WriteLine($"Probability of most likely number of clints every day ({Math.Ceiling(m1)}) is: {P}");
        }

        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task7(int m1, int m2, int n, double p)
    {
        Console.WriteLine("Task 7");
        var P = IntegralDeMoivreLaplaceTheorem(m1, m2, n, p);
        Console.WriteLine($"P = {P}");
        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task8(int m, int n)
    {
        Console.WriteLine("Task 8");
        double p = 0.5;
        Console.WriteLine($"P = {LocalDeMoivreLaplaceTheorem(m, n, p)}");
        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task9(int m, int n, double p)
    {
        Console.WriteLine("Task 9");
        Console.WriteLine($"P = {PoissonsEquation(m, n, p)}");
        Console.WriteLine(new String('-', 100));
        Console.WriteLine();
    }

    public static void Task10(int n, decimal q)
    {
        Console.WriteLine("Task 10");
        decimal p = (decimal)ReverseP((double)q);
        MostLikelyNumber(n, p, out decimal m1, out decimal m2);
        if (m1 == Math.Ceiling(m1) && m2 == Math.Ceiling(m2))
        {
            Console.WriteLine($"m1 = {m1}");
            Console.WriteLine($"m2 = {m2}");
        }
        else
            Console.WriteLine($"m = {Math.Ceiling(m1)}");

        Console.WriteLine(new String('-', 100));
    }

    #endregion
}