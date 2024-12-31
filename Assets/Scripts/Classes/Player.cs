using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public string playerName;

    private void Awake()
    {
        Init();
        LobbyManager.Singleton.UpdatePlayers();
    }

    public void Init()
    {
        if (IsLocalPlayer)
            this.playerName = PlayerSettings.playerName;
        Debug.Log("Player is called " + this.playerName);
    }

}
