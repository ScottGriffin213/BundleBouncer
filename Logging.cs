using MelonLoader;

namespace BundleBouncer
{

    internal static class Logging
    {
        internal static MelonLogger.Instance LI;

        public static void Info(string msg) => LI.Msg(msg);
        public static void Info(string msg, params object[] args) => LI.Msg(msg, args);
        public static void Info(object o) => LI.Msg(o);

        public static void Warning(string msg) => LI.Warning(msg);
        public static void Warning(string msg, params object[] args) => LI.Warning(msg, args);
        public static void Warning(object o) => LI.Warning(o);

        public static void Error(string msg) => LI.Error(msg);
        public static void Error(string msg, params object[] args) => LI.Error(msg, args);
        public static void Error(object o) => LI.Error(o);
    }
}