using System;
using UnityEngine;

namespace BundleBouncer.UI
{
    public class QMShitlistButtonHandler: MonoBehaviour
    {
        private float myTimeAccumulator;

        public QMShitlistButtonHandler(IntPtr ptr):base(ptr){}
        

        private void Update()
        {
            myTimeAccumulator += Time.deltaTime;
            if (myTimeAccumulator < .5f) return;
            myTimeAccumulator = 0;
            var player = VRChatUtilityKit.Utilities.VRCUtils.ActivePlayerInUserSelectMenu?.prop_Player_0;
            if(player == null) return;
            UIXIntegration.TickQMUser(player);
        }
    }
}