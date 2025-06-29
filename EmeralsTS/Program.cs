using System;
using System.IO;

class Program
{
    static bool doesFixAlreadyExist(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            if (line.Contains("127.0.0.1 lh.v10.network"))
            {
                return true;
            }
        }
        return false;
    }

    static void TerminateAllTeamspeakProcesses()
    {
        var processes = System.Diagnostics.Process.GetProcessesByName("ts3client_win64");
        foreach (var process in processes)
        {
            try
            {
                process.Kill();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error terminating process {process.ProcessName}: {ex.Message}");
            }
        }
    }

    static void AddToHostsFile()
    {
        bool error = false;
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "drivers", "etc", "hosts");

        if (File.Exists(path))
        {
            if (doesFixAlreadyExist(path))
            {
                Console.WriteLine("Fix already exists, nothing to do.");
                return;
            }

            string lineToAdd = "127.0.0.1 lh.v10.network";

            File.AppendAllText(path, Environment.NewLine + lineToAdd);
            Console.WriteLine("Completed DNS Fix.");
        }
        else
        {
            error = true;
        }

        if (error)
        {
            Console.WriteLine("An error occurred while trying to fix the DNS issue.");
        }
    }

    static bool IsSaltyChatPluginInstalled()
    {
        string pluginPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TS3Client", "plugins", "SaltyChat");
        return Directory.Exists(pluginPath);
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Terminating all TeamSpeak processes...");
        TerminateAllTeamspeakProcesses();
        Console.WriteLine("All TeamSpeak processes terminated.");

        Console.WriteLine("Running TeamSpeak DNS Fix...");
        AddToHostsFile();

        Console.WriteLine("Checking for saltychat plugin...");
        if (IsSaltyChatPluginInstalled())
        {
            Console.WriteLine("SaltyChat plugin is installed.");
        }
        else
        {
            Console.WriteLine("SaltyChat plugin is not installed, please install it.");
        }

        Console.ReadLine();
    }
}