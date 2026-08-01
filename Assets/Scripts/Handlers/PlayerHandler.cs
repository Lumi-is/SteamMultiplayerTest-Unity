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
            List<Camera> cameras = new(FindObjectsByType<Camera>());
            
            foreach(Camera camera in cameras)
            {
                if(camera != childCamera)
                {
                    camera.enabled = false;
                }
            }
            
            base.OnStartClient();
        }
    }
}

