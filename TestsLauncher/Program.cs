using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string nunitWhere = "";
        string nunitDLL = "";
        string arguments;

        Console.WriteLine("Hello,");

        if (args.Length == 0)
        {
            Console.WriteLine("Brak parametrów uruchomieniowych.");
            return;
        }
        else
        {
            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--testName":
                    case "-t":
                        if (i + 1 < args.Length)
                        {
                            Console.WriteLine($"Uruchamiam test: {args[i + 1]}");
                            nunitWhere = $"FullyQualifiedName={args[i + 1]}";
                        }
                        else
                        {
                            Console.WriteLine("Brak nazwy testu po --testName.");
                        }
                        break;
                    case "--testCategory":
                    case "-c":
                        if (i + 1 < args.Length)
                        {
                            Console.WriteLine($"Uruchamiam kategorię: {args[i + 1]}");
                            nunitWhere = $"TestCategory={args[i + 1]}";
                        }
                        else
                        {
                            Console.WriteLine("Brak nazwy klasy testowej po --testClassName.");
                        }
                        break;
                    case "--testDLL":
                    case "-d":
                        if (i + 1 < args.Length)
                        {
                            Console.WriteLine($"Uruchamiam testy z DLL: {args[i + 1]}");
                            nunitDLL = $"{args[i + 1]}.dll";
                        }
                        else
                        {
                            Console.WriteLine("Brak nazwy DLL po --testDLL.");
                        }
                        break;
                    case "--help":
                    case "-h":
                        Console.WriteLine("Dostępne parametry:");
                        Console.WriteLine("--testDLL [nazwa_DLL] lub -d [nazwa_DLL] - uruchamia testy z konkretnej DLL.");
                        Console.WriteLine("--testCategory [nazwa_kategorii] lub -c [nazwa_kategorii] - uruchamia wszystkie testy z daną kategorią.");
                        Console.WriteLine("--testName [nazwa_testu] lub -t [nazwa_testu] - uruchamia konkretny test.");
                        Console.WriteLine("--help lub -h - wyświetla tę pomoc.");
                        return;
                    default:
                        Console.WriteLine($"Nieznany parametr: {args[i]}");
                        break;
                }
            }
        }

        if(nunitDLL == string.Empty)
        {
            Console.WriteLine("Nie podano DLL z testami. Użyj --testDLL [nazwa_DLL] lub -d [nazwa_DLL] " +
                "aby uruchomić testy z konkretnej DLL.");
            return;
        }

        arguments = $"test {nunitDLL}";

        if ( nunitWhere == string.Empty)
        {
            Console.WriteLine("Uruchamiam wszystkie testy.");
        }
        else
        {
            arguments += $" --filter \"{nunitWhere}\"";
        }

        Process nunitProcess = new();
        nunitProcess.StartInfo.FileName = "dotnet";
        nunitProcess.StartInfo.Arguments = arguments;
        nunitProcess.StartInfo.WorkingDirectory = ".";

        nunitProcess.StartInfo.UseShellExecute = false;
        nunitProcess.StartInfo.RedirectStandardOutput = true;
        nunitProcess.StartInfo.RedirectStandardError = true;

        nunitProcess.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine(e.Data);
            }
        };
        nunitProcess.ErrorDataReceived += (sender, e) => 
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.Error.WriteLine(e.Data);
            }
        };

        nunitProcess.Start();
        nunitProcess.BeginOutputReadLine();
        nunitProcess.BeginErrorReadLine();
        nunitProcess.WaitForExit();

        Console.WriteLine($"Proces NUnit zakończył się z kodem wyjścia: {nunitProcess.ExitCode}");
    }
}