using System;

namespace SimQCore {

    public enum LogStatus {
        ERROR,
        INFO,
        WARNING,
        SUCCESS
    }

    public static class Misc {
        public static void Log( string message, LogStatus status ) {
            switch( status ) {
                case LogStatus.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogStatus.INFO:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogStatus.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogStatus.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            Console.WriteLine( message );
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
