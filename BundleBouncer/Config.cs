/**
 * BundleBouncer Configuration
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

namespace BundleBouncer
{
    public class Config
    {
        private bool noSyncSet;
        private bool devModeSet;
        private MelonPreferences_Category bbCat;
        private MelonPreferences_Entry<bool> _syncDefinitions;
        private MelonPreferences_Entry<bool> _devMode;

        public Config()
        {
            noSyncSet = HasCommandLineOption("--bb.no-sync");
            devModeSet = HasCommandLineOption("--bb.dev-mode");

            bbCat = MelonPreferences.CreateCategory("BundleBouncer");
            _syncDefinitions = bbCat.CreateEntry("SyncRemoteDefinitions", true, "Synchronize Definitions", "Do you wish to automatically download new shitlists from github?");
            _devMode = bbCat.CreateEntry("DevMode", false, is_hidden:true);
        }

        public bool SyncDefinitions
        {
            get
            {
                if (DevMode || noSyncSet)
                    return false;
                return _syncDefinitions.Value;
            }
        }

        public bool DevMode
        {
            get
            {
                return devModeSet || _devMode.Value;
            }
        }

        private bool HasCommandLineOption(string v)
        {
            return Environment.CommandLine.Contains(v);
        }
    }
}
