using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BundleBouncer
{
    internal class HarmonyUtils
    {
        internal static bool MatchParameters(MethodBase method, Type[] types) => method.GetParameters().Select(x => x.ParameterType).SequenceEqual(types);

        internal static void ShowDStack()
        {
            Logging.Info((new Il2CppSystem.Diagnostics.StackTrace(0, false)).ToString());
        }

        internal static bool HasProp(dynamic thing, string key)
        {
            if (thing == null)
            {
                return false;
            }
            else if (thing is IDictionary<string, object> dict)
            {
                return dict.ContainsKey(key);
            }
            return thing.GetType().GetProperty(key) != null;
        }
    }
}