using System.Numerics;

namespace Lab4;

class Program
{
    public static void Main()
    {
        Task1();
        Task2();
        Task3();
        Task4();
        Task5();
        Task6();
        Task7();
        Task8();
        Task9();
        Task10();
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

    public static double BayesTheorem(List<(double p1, double p2)> pAs, int sIndex)
    {
        return (pAs[sIndex].p1 * pAs[sIndex].p2) / TotalProbability(pAs);
    }

    public static double TotalProbability(List<(double p1, double p2)> p)
    {
        double totalProbability = 0.0;
        foreach (var item in p)
        {
            totalProbability += item.p1 * item.p2;
        }
        return totalProbability;
    }
    #endregion

    #region Tasks
    public static void Task1()
    {
        Console.WriteLine("Task1");

        int blackShoes = 40, brownShoes = 26, redShoes = 22, blueShoes = 12;
        double totalShoes = blackShoes + brownShoes + redShoes + blueShoes;
        double P = (redShoes + blueShoes) / totalShoes;
        Console.WriteLine($"P = {P}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }
    public static void Task2()
    {
        Console.WriteLine("Task2");

        int employees = 10, consultants = 8;
        int selectedEmployees = 2;
        int notConsultants = employees - consultants;
        double revP = 1.0;
        for (int i = 0; i < selectedEmployees; i++)
        {
            revP *= notConsultants / (double)employees;
            notConsultants--;
            employees--;
        }
        double P = 1 - revP;
        Console.WriteLine($"P = {P}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }
    public static void Task3()
    {
        Console.WriteLine("Task3");

        int managers = 10, reletives = 2;
        int selectedManagers = 3;
        int notReletives = managers - reletives;
        double revP = 1.0;
        for (int i = 0; i < selectedManagers; i++)
        {
            revP *= notReletives / (double)managers;
            notReletives--;
            managers--;
        }
        double P = 1 - revP;
        Console.WriteLine($"P = {P}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }
    public static void Task4()
    {
        Console.WriteLine("Task4");

        decimal p1 = 0.15m, p2 = 0.25m, p3 = 0.2m, p4 = 0.1m, p5;
        p5 = 1 - (p1 + p2 + p3 + p4);
        Console.WriteLine($"p5 = {p5}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }

    public static void Task5()
    {
        Console.WriteLine("Task5");

        int railways = 120, trains = 80, selectedTrains = 2;
        double P = C(selectedTrains, trains) / (double)C(selectedTrains, railways);
        Console.WriteLine($"P = {P}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }

    public static void Task6()
    {
        Console.WriteLine("Task6");

        List<(double probabilityToProduce, double probabilityToproduceFirstGrade)> producing = new()
        {
            (0.9, 0.8),
        };
        Console.WriteLine($"Probability of producing first grade product by this machine: {BayesTheorem(producing, 0)}");
        Console.WriteLine($"Probability of producing first grade product: {TotalProbability(producing)}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }

    public static void Task7()
    {
        Console.WriteLine("Task7");

        int students = 10, tests = 20;
        List<(int sudents, int knowTests)> studentsTypes = new()
        {
            (3, 20),
            (4, 16),
            (2, 10),
            (1, 5)
        };

        List<(double pa, double pba)> pAs = new List<(double pa, double pba)>();
        foreach (var item in studentsTypes)
        {
            pAs.Add((item.sudents / (double)students, (item.knowTests / (double)tests) *
                                                      ((item.knowTests - 1.0) / ((double)tests - 1.0)) *
                                                      ((item.knowTests - 2.0) / ((double)tests - 2.0))));
        }
        double P1 = BayesTheorem(pAs, 0);
        double P2 = BayesTheorem(pAs, 3);
        Console.WriteLine($"Probability that student prepared perfectly: {P1}");
        Console.WriteLine($"Probability that student prepared bad: {P2}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }

    public static void Task8()
    {
        Console.WriteLine("Task8");

        List<(double detailProbability, double standartDetailProbability)> details = new()
        {
            (0.4, 0.9),
            (0.3, 0.95),
            (0.3, 0.95),
        };
        Console.WriteLine($"P = {TotalProbability(details)}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }

    public static void Task9()
    {
        Console.WriteLine("Task9");

        List<(double symptomProbability, double healProbability)> patients = new()
        {
            (0.4, 0.8),
            (0.3, 0.7),
            (0.3, 0.85),
        };
        Console.WriteLine($"P = {BayesTheorem(patients, 1)}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }
    public static void Task10()
    {
        Console.WriteLine("Task10");

        List<(double symptomProbability, double healProbability)> specialists = new()
        {
            (0.3, 0.9),
            (0.7, 0.8),
        };
        Console.WriteLine($"P = {BayesTheorem(specialists, 0)}");

        Console.WriteLine(new String('-', 100));
        Console.WriteLine("");
    }
    #endregion
}