/**
 * BundleBouncer Logging
 * 
 * Copyright (c) 2022 BundleBouncer Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using MelonLoader;
using System;
using System.IO;

namespace BundleBouncer
{

    public static class Logging
    {
        internal static MelonLogger.Instance LI;

        const string ANSI_RESET = "\u001b[0m";
        const string ANSI_RED = "\u001b[31m";

        static readonly string BLOCKLOG = Path.Combine("UserData", "BundleBouncer", "Blocks.log");

        internal static void Startup()
        {
            var bbdir = Path.GetDirectoryName(BLOCKLOG);
            if (!Directory.Exists(bbdir))
                Directory.CreateDirectory(bbdir);
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
            File.AppendAllText(BLOCKLOG, $"[{GetTimeStamp()}] {msg}\n");
        }
    }
}