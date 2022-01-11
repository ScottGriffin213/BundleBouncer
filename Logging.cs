using MelonLoader;
using System;
using System.IO;

namespace BundleBouncer
{

    internal static class Logging
    {
        internal static MelonLogger.Instance LI;

        const string ANSI_RESET = "\u001b[0m";
        const string ANSI_RED = "\u001b[31m";

        static readonly string BLOCKLOG = Path.Combine("UserData", "BundleBouncer", "Blocks.log");

        internal static void Startup() {
            File.WriteAllText(BLOCKLOG, $"BundleBouncer BlockLog started @ {GetTimeStamp()}\n");
        }

        private static string GetTimeStamp()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public static void Info(string msg) => LI.Msg(msg);
        public static void Info(string msg, params object[] args) => LI.Msg(msg, args);
        public static void Info(object o) => LI.Msg(o);

        public static void Warning(string msg) => LI.Warning(msg);
        public static void Warning(string msg, params object[] args) => LI.Warning(msg, args);
        public static void Warning(object o) => LI.Warning(o);

        public static void Error(string msg) => LI.Error(msg);
        public static void Error(string msg, params object[] args) => LI.Error(msg, args);
        public static void Error(object o) => LI.Error(o);

        public static void Gottem(string msg)
        {
            Info($"{ANSI_RED}{msg}{ANSI_RESET}");
            File.AppendAllText(BLOCKLOG, $"[{GetTimeStamp()}] {msg}");
        }
    }
}