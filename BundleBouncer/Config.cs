using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _syncDefinitions = bbCat.CreateEntry("SyncDefinitions", false, "Synchronize Definitions", "Do you wish to automatically download new shitlists from github?");
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
