using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System.Linq;

namespace Player
{
    public class PlayerHandler : NetworkBehaviour
    {
        public Camera childCamera;
        public override void OnStartClient()
        {
            if(isLocalPlayer)
            {
                childCamera.enabled = true;
            } else
            {
                childCamera.enabled = false;
            }
            
            base.OnStartClient();
        }
    }
}

