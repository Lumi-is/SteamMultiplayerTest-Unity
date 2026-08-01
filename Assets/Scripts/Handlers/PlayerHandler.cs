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
        List<PlayerHandler> playerHandlers = new();
        public override void OnStartLocalPlayer()
        {
            playerHandlers.Clear();
            playerHandlers = new(FindObjectsByType<PlayerHandler>());


            foreach(PlayerHandler player in playerHandlers)
            {
                if(!isLocalPlayer)
                {
                    childCamera.enabled = false;
                }
            }

            base.OnStartLocalPlayer();
        }
    }
}

