using UIExpansionKit.API;
using UIExpansionKit.API.Controls;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;

namespace BundleBouncer.UI
{
    // Mostly adapted from AdvancedSafety
    public static class UIXIntegration
    {
        //private static IMenuButton shitlistAvatarButton;
        private static IMenuButton shitlistUserButton;

        public static void OnApplicationStart()
        {
            Logging.Info("Initializing UIX...");
            ClassInjector.RegisterTypeInIl2Cpp<QMShitlistButtonHandler>(logSuccess: true);

            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += VRCUtils_OnUiManagerInit;
        }

        private static void VRCUtils_OnUiManagerInit()
        {
            shitlistUserButton = ExpansionKitApi.GetExpandedMenu(ExpandedMenu.UserQuickMenu).AddSimpleButton("Shitlist User", OnUnshitlistUser);
            shitlistUserButton.OnInstanceCreated += updateUserShitlistButton;

            //shitlistAvatarButton = ExpansionKitApi.GetExpandedMenu(ExpandedMenu.UserQuickMenu).AddSimpleButton("Blocklist Avatar", OnUnshitlistAvatar);
        }


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
            if (player == null) return;
            var usrID = player.field_Private_APIUser_0.id;
            if (BundleBouncer.IsOnSkiddieShitlist(usrID))
                BundleBouncer.RemoveFromSkiddieShitlist(usrID);
            else
                BundleBouncer.AddToSkiddieShitlist(usrID);
            updateUserShitlistButton(shitlistUserButton.CurrentInstance);
        }

        private static void updateUserShitlistButton(GameObject button)
        {
            var player = VRChatUtilityKit.Utilities.VRCUtils.ActivePlayerInUserSelectMenu?.prop_Player_0;
            if (player == null) return;
            var usrID = player.field_Private_APIUser_0.id;
            button.GetComponentInChildren<Text>().text = BundleBouncer.IsOnSkiddieShitlist(usrID) ? "Unshitlist User" : "Shitlist User";
        }
    }
}
