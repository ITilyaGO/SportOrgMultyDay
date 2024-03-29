﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Processing
{
    public static class Logger
    {
        public static async void Log(string log)
        {
            string toWrite = $"[{DateTime.Now:HH:mm:ss}] >> {log}";
            Console.WriteLine(toWrite);
            await File.AppendAllTextAsync("Log.txt", toWrite + "\n");
        }

        public static void LogError(string id, Exception ex)
        {
            Log($"Error MessageID: {id} \n {ex.Message}\n,{ex.Source}");
        }
        public static void LogError(string id, string message)
        {
            Log($"Error MessageID: {id} \n {message}");
        }
        public static Exception LogError(string id, string message, bool throwException = false)
        {
            LogError(id, message);
            var e = new Exception(message);
            if (throwException)
                throw e;
            else 
                return e;
        }
    }
}
