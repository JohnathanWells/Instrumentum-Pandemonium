using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    private NetworkVariable<FixedString64Bytes> _playerName = new NetworkVariable<FixedString64Bytes>();
    public string playerName
    {
        set
        {
            _playerName.Value = value;
        }
        get
        {
            return _playerName.Value.ToString();
        }
    }
    public UnityAction OnPlayerNameChanged;

    private void Awake()
    {
        //Init();
        //LobbyManager.Singleton.UpdatePlayers();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            if (!IsHost)
            {
                Debug.Log("NOT HOST");
                SetNameRpc(PlayerSettings.playerName);
                return;
            }

            playerName = PlayerSettings.playerName;
        }
        else
        {
            Debug.Log("Player " + NetworkManager.Singleton.LocalClientId + " is not the owner of " + gameObject.name + ", " + OwnerClientId);
            Debug.Log("IsHost: " + IsHost.ToString());
        }

        //LobbyManager.Singleton.UpdatePlayers();
    }

    void NetworkPlayerName_OnValueChanged(FixedString64Bytes prevVal, FixedString64Bytes newVal)
    {
        //Nothing
    }

    //[Rpc(SendTo.Everyone)]
    //public void SetNameRpc(string to)
    //{
    //    Debug.Log();
    //    this.playerName = to;
    //}

    [Rpc(SendTo.Server)]
    public void SetNameRpc(string to)
    {
        this.playerName = to;

        Debug.Log("Name of " + gameObject.name + " set to " + this.playerName);

        OnPlayerNameChanged?.Invoke();
    }

    //public void Init()
    //{
    //    if (IsOwner)
    //        SetNameRpc(PlayerSettings.playerName);

    //    return;

    //    if (IsOwner)
    //    {
    //        this.playerName = PlayerSettings.playerName;
    //        Debug.Log("Player is called " + this.playerName);
    //    }
    //    else
    //    {
    //        Debug.Log("Player " + NetworkManager.Singleton.LocalClientId + " is not the owner of " + gameObject.name + ", " + OwnerClientId);
    //        Debug.Log("IsHost: " + IsHost.ToString());

    //        SetNameRpc(PlayerSettings.playerName);
    //    }

    //}


}
