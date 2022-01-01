﻿using System;
using System.Linq;
using System.Reflection;

namespace BundleBouncer.Utilities
{
    class Xref
    {
        // This method is practically stolen from https://github.com/BenjaminZehowlt/DynamicBonesSafety/blob/master/DynamicBonesSafetyMod.cs
        // And stolen again https://github.com/loukylor/VRC-Mods/blob/e4cd8aaeea877595782f8cd0f0ddf50ff75557ca/PlayerList/Utilities/Xref.cs#L9
        public static bool CheckMethod(MethodInfo method, string match)
        {
            try
            {
                return UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(method)
                    .Where(instance => instance.Type == UnhollowerRuntimeLib.XrefScans.XrefType.Global && instance.ReadAsObject().ToString().Contains(match)).Any();
            }
            catch { }
            return false;
        }
        public static bool CheckUsed(MethodInfo method, string methodName)
        {
            try
            {
                return UnhollowerRuntimeLib.XrefScans.XrefScanner.UsedBy(method)
                    .Where(instance => instance.TryResolve() != null && instance.TryResolve().Name.Contains(methodName)).Any();
            }
            catch { }
            return false;
        }
        public static bool CheckUsing(MethodInfo method, string match, Type type)
        {
            foreach (UnhollowerRuntimeLib.XrefScans.XrefInstance instance in UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(method))
                if (instance.Type == UnhollowerRuntimeLib.XrefScans.XrefType.Method)
                    try
                    {
                        if (instance.TryResolve().DeclaringType == type && instance.TryResolve().Name.Contains(match))
                            return true;
                    }
                    catch
                    {

                    }
            return false;
        }
    }
}
