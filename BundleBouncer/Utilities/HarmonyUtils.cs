using System;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace BundleBouncer
{
    internal class HarmonyUtils
    {
        internal static bool MatchParameters(MethodInfo method, Type[] types)
        {
            var p = method.GetParameters();
            if (types.Length != p.Length)
                return false;
            return p.Select(x => x.ParameterType).ToArray() == types;
        }

        internal static void ShowDStack()
        {
            Logging.Info((new Il2CppSystem.Diagnostics.StackTrace(1, false)).ToString());
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