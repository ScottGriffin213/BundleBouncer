using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIExpansionKit;
using UIExpansionKit.API;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRChatUtilityKit;

namespace BundleBouncer.UI
{
    // Mostly adapted from AdvancedSafety
    public static class UIXIntegration
    {
        private static Text qmShitlistUserText;

        public static void OnApplicationStart()
        {
            Logging.Info("Initializing UIX...");
            ClassInjector.RegisterTypeInIl2Cpp<QMShitlistButtonHandler>(logSuccess: true);
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.UserQuickMenu).AddSimpleButton("Shitlist User", OnUnshitlistUser, OnConsumeUnshitlistUser);
            //ExpansionKitApi.GetExpandedMenu(ExpandedMenu.UserQuickMenu).AddSimpleButton("Blocklist Avatar", OnUnshitlistAvatar, OnConsumeUnshitlistAvatar);

            //VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += VRCUtils_OnUiManagerInit;
        }
        /*
        private static void VRCUtils_OnUiManagerInit()
        {
            VRCUiManager.prop_VRCUiManager_0.method
        }
        */

        /*
        private static void OnUnshitlistAvatar()
        {
            throw new NotImplementedException();
        }

        private static void OnConsumeUnshitlistAvatar(GameObject obj)
        {
            throw new NotImplementedException();
        }
        */

        private static void OnUnshitlistUser()
        {
            var player = VRChatUtilityKit.Utilities.VRCUtils.ActivePlayerInUserSelectMenu?.prop_Player_0;
            if(player == null) return;
            var usrID = player.field_Private_APIUser_0.id;
            if(BundleBouncer.IsOnSkiddieShitlist(usrID))
                BundleBouncer.RemoveFromSkiddieShitlist(usrID);
            else
                BundleBouncer.AddToSkiddieShitlist(usrID);
        }

        private static void OnConsumeUnshitlistUser(GameObject obj)
        {
            qmShitlistUserText = obj.GetComponentInChildren<Text>();
            obj.AddComponent<QMShitlistButtonHandler>();
        }

        internal static void TickQMUser(VRC.Player player)
        {
            var usrID = player.prop_APIUser_0.id;
            qmShitlistUserText.text = BundleBouncer.IsOnSkiddieShitlist(usrID)
                ? "Unshitlist User"
                : "Shitlist User";
        }
    }
}
