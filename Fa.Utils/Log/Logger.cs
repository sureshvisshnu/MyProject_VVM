using System;

namespace fa.api.Log
{
    public static class Logger
    {
        public static void LogError(Exception e)
        {            
            Console.WriteLine(e.Message);
        }
    }
}
