using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Steam.Multiplayer
{
    public class PlayerMovementHandler : NetworkBehaviour
    {
        void Update()
        {
            if(isLocalPlayer)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                transform.position += new Vector3(h, 0, v);
            }
        }
    }
}

