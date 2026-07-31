using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
using Steamworks;


namespace Steam.Multiplayer
{
    public class LobbyUIManager : NetworkBehaviour
    {
        public static LobbyUIManager instance;

        public Transform playerListParent;

        public List<TextMeshProUGUI> playerNameTexts = new();

        public List<PlayerLobbyHandler> playerLobbyHandlers = new();

        public Button playGameButton;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            } else if(instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            playGameButton.interactable = false; 
        }

        public void UpdateClientsLobbyUI()
        {
            playerNameTexts.Clear();
            playerLobbyHandlers.Clear();

            var lobby = new CSteamID(SteamLobby.instance.lobbyID);
            int memberCount = SteamMatchmaking.GetNumLobbyMembers(lobby);

            CSteamID hostID = new CSteamID(ulong.Parse(SteamMatchmaking.GetLobbyData(lobby, "HostAddress")));
            List<CSteamID> orderedMembers = new List<CSteamID>();

            if(memberCount == 0)
            {
                Debug.LogWarning("Lobby has no members... retrying...");
                StartCoroutine(RetryUpdate());
                return;
            }

            orderedMembers.Add(hostID);

            for(int i = 0; i < memberCount; i++)
            {
                CSteamID memberID = SteamMatchmaking.GetLobbyMemberByIndex(lobby, i);
                if(memberID != hostID)
                {
                    orderedMembers.Add(memberID);
                }
            }

            int j = 0; 
            foreach (var member in orderedMembers)
            {
                TextMeshProUGUI txtMesh = playerListParent.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>();
                PlayerLobbyHandler playerLobbyHandler = playerListParent.GetChild(j).GetComponent<PlayerLobbyHandler>();

                playerLobbyHandlers.Add(playerLobbyHandler);
                playerNameTexts.Add(txtMesh);

                string playerName = SteamFriends.GetFriendPersonaName(member);
                playerNameTexts[j].text = playerName;
                j++;
            }
        }

        public void OnPlayButtonClicked()
        {
            if(NetworkServer.active)
            {
                CustomNetworkManager.singleton.ServerChangeScene("GameplayScene");
            }
        }

        public void RegisterPlayer(PlayerLobbyHandler player)
        {
            player.transform.SetParent(playerListParent, false); // TODO
            UpdateClientsLobbyUI();
        }

        [Server]
        public void CheckAllPlayersReady()
        {
            foreach (var player in playerLobbyHandlers)
            {
                if(!player.isReady)
                {
                    RpcSetPlayButtonInteractable(false);
                    return;
                }
            }

            RpcSetPlayButtonInteractable(true);
        }

        [ClientRpc]
        void RpcSetPlayButtonInteractable(bool truthStatus)
        {
            playGameButton.interactable = truthStatus;
        }

        private IEnumerator RetryUpdate()
        {
            yield return new WaitForSeconds(1f);
            UpdateClientsLobbyUI(); 
        }
    }
}

