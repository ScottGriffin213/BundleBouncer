using System;

namespace BundleBouncer.Validation
{
    public static class Logging
    {
        public static Action<string> OnInfo_Str;
        public static Action<string, object[]> OnInfo_StrObjA;
        public static Action<object> OnInfo_Obj;
        public static void Info(string msg) => OnInfo_Str(msg);
        public static void Info(string msg, params object[] args) => OnInfo_StrObjA(msg, args);
        public static void Info(object o) => OnInfo_Obj(o);

        public static Action<string> OnWarning_Str;
        public static Action<string, object[]> OnWarning_StrObjA;
        public static Action<object> OnWarning_Obj;
        public static void Warning(string msg) => OnWarning_Str(msg);
        public static void Warning(string msg, params object[] args) => OnWarning_StrObjA(msg, args);
        public static void Warning(object o) => OnWarning_Obj(o);

        public static Action<string> OnError_Str;
        public static Action<string, object[]> OnError_StrObjA;
        public static Action<object> OnError_Obj;
        public static void Error(string msg) => OnError_Str(msg);
        public static void Error(string msg, params object[] args) => OnError_StrObjA(msg, args);
        public static void Error(object o) => OnError_Obj(o);
    }
}
