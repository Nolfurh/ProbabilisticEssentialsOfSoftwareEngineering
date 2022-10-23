using System.IO;

class Program
{
    [STAThread]
    public static void Main()
    {
        Console.WriteLine("Enter name of file: ");
        string inputFilename = Console.ReadLine();
        if (File.Exists(inputFilename))
        {
            FrequencyTable<int> frequencyTable = CreateFrequencyTableFromFile(inputFilename);

            ShowTable(frequencyTable);
            Console.Write("\nMode: ");
            ModeFromFile(inputFilename).ForEach(i => Console.WriteLine(i));
            Console.WriteLine($"Median = {MedianFromFile(inputFilename)}");
            Console.WriteLine($"Dispercion = {Dispercion(frequencyTable)}");
            Console.WriteLine($"Standart deviation = {StandartDeviation(frequencyTable)}");

            string outputFilename = inputFilename;
            string toRemove = "input";
            if (outputFilename.ToLower().StartsWith(toRemove))
            {
                outputFilename = outputFilename.ToLower().Replace(toRemove, "");
                outputFilename = "output" + outputFilename;
            }
            WriteFrequencyTableToFile(outputFilename, frequencyTable, inputFilename);

            Lab1.HistogramWindow.RunHistogramWindow(frequencyTable);
        }
        else
            Console.WriteLine("Wrong filename");        
    }

    public static FrequencyTable<int> CreateFrequencyTableFromFile(string fileName)
    {
        FrequencyTable<int> frequencyTable = new();

        using (StreamReader reader = new StreamReader(fileName))
        {
            string? line;
            int.TryParse(reader.ReadLine(), out int quantity);
            while ((line = reader.ReadLine()) is not null)
            {
                int num;
                if (Int32.TryParse(line, out num))
                {
                    frequencyTable.AddRow(num);
                }                
            }
        }

        return frequencyTable;
    }

    public static void ShowTable<T>(FrequencyTable<T> frequencyTable) where T: notnull
    {
        string[] columns = new string[] { "Item", "Frequency", "Cumulative frequency" };

        Console.WriteLine("{0, -20}{1,-20}{2, -20}", columns[0], columns[1], columns[2]);
        foreach (var row in frequencyTable.Rows)
        {
            Console.WriteLine("{0, -20}{1, -20}{2, -20}", row.Value.Item, row.Value.Frequency, row.Value.CumulativeFrequency);
        }
        Console.WriteLine("{0, -20}{1, -20}", "Total: ", frequencyTable.TotalFrequency);
    }

    public static List<int> ModeFromFile(string fileName)
    {
        List<int> mode = new();
        using (StreamReader reader = new StreamReader(fileName))
        {
            string? line;
            int.TryParse(reader.ReadLine(), out int quantity);
            Dictionary<int, int> frequency = new();
            while ((line = reader.ReadLine()) is not null)
            {
                int num;
                if (Int32.TryParse(line, out num))
                {
                    if (frequency.ContainsKey(num))
                    {
                        frequency[num]++;
                    }
                    else
                    {
                        frequency.Add(num, 1);
                    }
                }
            }

            int maxFrequency = frequency.Max(i => i.Value);

            foreach (var item in frequency)
            {
                if (item.Value == maxFrequency)
                    mode.Add(item.Key);
            }
        }
        return mode;
    }
    public static decimal MedianFromFile(string fileName)
    {
        List<int> values = new();
        using (StreamReader reader = new StreamReader(fileName))
        {
            string? line;
            int.TryParse(reader.ReadLine(), out int quantity);
            while ((line = reader.ReadLine()) is not null)
            {
                int num;
                if (Int32.TryParse(line, out num))
                {
                    values.Add(num);
                }
            }
            values.Sort();
        }
        if(values.Count % 2 == 1)
        {
            return values[(values.Count + 1) / 2];
        }
        else
        {
            return (values[values.Count / 2] + (values[values.Count / 2] + 1)) / 2m;
        }
    }

    public static double Dispercion(FrequencyTable<int> frequencyTable)
    {
        double varX = 0d, avarage = 0d;


        foreach (var item in frequencyTable.Rows)
        {
            avarage += item.Value.Frequency * item.Key;
        }
        avarage /= frequencyTable.TotalFrequency;

        foreach (var item in frequencyTable.Rows)
        {
            varX += item.Value.Frequency * Math.Pow(item.Key - avarage, 2);
        }
        varX /= frequencyTable.TotalFrequency;


        return varX;
    }

    public static double StandartDeviation(FrequencyTable<int> frequencyTable)
    {
        return Math.Sqrt(Dispercion(frequencyTable));
    }

    public static void WriteFrequencyTableToFile(string path, FrequencyTable<int> frequencyTable, string inputFile)
    {
        string[] columns = new string[] { "Item", "Frequency", "Cumulative frequency" };

        using (StreamWriter writer = new StreamWriter(path, new FileStreamOptions() { Mode = FileMode.Create, Access = FileAccess.Write }))
        {
            writer.WriteLine("{0, -20}{1,-20}{2, -20}", columns[0], columns[1], columns[2]);
            foreach (var row in frequencyTable.Rows)
            {
                writer.WriteLine("{0, -20}{1, -20}{2, -20}", row.Value.Item, row.Value.Frequency, row.Value.CumulativeFrequency);
            }
            writer.WriteLine("{0, -20}{1, -20}", "Total: ", frequencyTable.TotalFrequency);

            writer.Write("Mode: ");
            ModeFromFile(inputFile).ForEach(i => writer.WriteLine(i));
            writer.WriteLine("Median = " + MedianFromFile(inputFile));
            writer.WriteLine("Dispersion = " + Dispercion(frequencyTable));
            writer.WriteLine("Standart deviation = " + StandartDeviation(frequencyTable));
        }
    }
}

public class FrequencyTableRow<T>
{
    public T Item { get; set; }
    public int Frequency { get; set; }
    public int CumulativeFrequency { get; set; }

    public FrequencyTableRow(T item)
    {
        Item = item;
        Frequency = 1;
    }
}

public class FrequencyTable<T> where T: notnull
{
    public SortedList<T, FrequencyTableRow<T>> Rows { get; set; }
    public int TotalFrequency
    {
        get
        {
            return Rows.Select(i => i.Value.Frequency).Sum();
        }
    }
    public int MaxFrequency
    {
        get
        {
            return Rows.Select(i => i.Value.Frequency).Max();
        }
    }

    public FrequencyTable()
    {
        Rows = new();
    }


    public FrequencyTableRow<T> this[T item]
    {
        get
        {
            return Rows[item];
        }
        set
        {
            Rows[item] = value;
        }
    }

    public void AddRow(T item)
    {
        if (Rows.ContainsKey(item))
        {
            this.Rows[item].Frequency++;
        }
        else
        {
            Rows.Add(item, new FrequencyTableRow<T>(item));
        }
        UpdateCumulativeFrequency();
    }

    protected void UpdateCumulativeFrequency()
    {
        int cumulation = 0;
        foreach (var row in Rows)
        {
            cumulation += row.Value.Frequency;
            row.Value.CumulativeFrequency = cumulation;
        }
    }
}