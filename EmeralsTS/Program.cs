using System;
using System.IO;

class Program
{
    static Boolean doesFixAlreadyExist(string path)
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

    static void Main(string[] args)
    {
        Console.WriteLine("Running TeamSpeak DNS Fix...");
        string path = Path.GetPathRoot(Environment.SystemDirectory);
        path += "Windows\\System32\\drivers\\etc\\hosts";

        bool error = false;
        if (File.Exists(path))
        {
            if (doesFixAlreadyExist(path))
            {
                Console.WriteLine("Fix already exists, nothing to do.");
                Console.ReadLine();
                return;
            }

            string lineToAdd = "127.0.0.1 lh.v10.network";

            File.AppendAllText(path, Environment.NewLine + lineToAdd);
        }
        else
        {
            error = true;
        }

        if (error)
        {
            Console.WriteLine("An error occurred while trying to fix the DNS issue.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Fix done!");

        Console.ReadLine();
    }
}