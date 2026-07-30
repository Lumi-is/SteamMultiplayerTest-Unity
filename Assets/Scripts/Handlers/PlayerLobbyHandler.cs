using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

namespace Steam.Multiplayer
{
    public class PlayerLobbyHandler : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnReadyStatusChanged))]

    public bool isReady; // Whether Player is ready to start game or not
    public Button readyButton;
    
    public TMP_Text nameText; //Player Name

    void Start()
    {
        readyButton.interactable = isLocalPlayer;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        readyButton.interactable = true;
        isReady = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        LobbyUIManager.instance.RegisterPlayer(this);
    }

    [Command]
    void CmdSetReady()
    {
        isReady = !isReady;
        OnReadyStatusChanged(!isReady, isReady);
    }

    public void OnReadyButtonClicked()
    {
        CmdSetReady();
    }

    void SetSelectedButtonColor(Color color)
    {
        ColorBlock cb = readyButton.colors;
        cb.normalColor = color;
        cb.selectedColor = color;
        cb.disabledColor = color;
        readyButton.colors = cb; 
    }
    
    void OnReadyStatusChanged(bool oldValue, bool newValue)
    {
        if(NetworkServer.active)
        {
            LobbyUIManager.instance.CheckAllPlayersReady();
        }

        if(isReady)
        {
            SetSelectedButtonColor(Color.green);
        } else
        {
            SetSelectedButtonColor(Color.white);
        }
    }
}
}

