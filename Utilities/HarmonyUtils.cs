using System;
using System.Reflection;
using System.Linq;

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
    }
}