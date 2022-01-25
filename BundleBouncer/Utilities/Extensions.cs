using System;

namespace BundleBouncer.Utilities
{
    static class Extensions
    {
        // Shamelessly stolen from lou
        // https://github.com/loukylor/VRC-Mods/blob/e4cd8aaeea877595782f8cd0f0ddf50ff75557ca/PlayerList/Utilities/Extensions.cs#L43
        public static void SafeInvoke(this Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                Logging.Error("Error while invoking delegate:\n" + ex.ToString());
            }
        }
        public static void SafeInvoke<T>(this Action<T> action, T arg1)
        {
            try
            {
                action.Invoke(arg1);
            }
            catch (Exception ex)
            {
                Logging.Error("Error while invoking delegate:\n" + ex.ToString());
            }
        }
        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            try
            {
                action.Invoke(arg1, arg2);
            }
            catch (Exception ex)
            {
                Logging.Error("Error while invoking delegate:\n" + ex.ToString());
            }
        }
    }
}
