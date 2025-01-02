using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    //private NetworkVariable<FixedString64Bytes> _playerNameNet = new NetworkVariable<FixedString64Bytes>();
    private string _playerName = "UNKNOWN";
    public string playerName
    {
        set
        {
            //_playerName.Value = value;
            _playerName = value;
            //_playerNameNet.Value = _playerName;
        }
        get
        {
            //return _playerName.Value.ToString();
            return _playerName;
        }
    }
    public UnityAction OnPlayerNameChanged;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            SetNameRpc(PlayerSettings.playerName);
            SessionManager.Singleton.RegisterPlayer(this);
        }
        else
        {
            Debug.Log("Player " + NetworkManager.Singleton.LocalClientId + " is not the owner of " + gameObject.name + ", " + OwnerClientId);
            Debug.Log("IsHost: " + IsHost.ToString());
        }

        SessionManager.Singleton.OnPlayerObjectSpawned?.Invoke(this);

        //LobbyManager.Singleton.UpdatePlayers();
    }

    private void OnEnable()
    {
        NetworkManager.OnConnectionEvent += UpdateNewClientOnName;
    }

    private void OnDisable()
    {
        NetworkManager.OnConnectionEvent -= UpdateNewClientOnName;
    }

    private void UpdateNewClientOnName(NetworkManager nm, ConnectionEventData connectionData)
    {
        if (connectionData.EventType == ConnectionEvent.PeerConnected)
            SetNameRpc(playerName, RpcTarget.Single(connectionData.ClientId, RpcTargetUse.Temp));
    }

    [Rpc(SendTo.SpecifiedInParams)]
    public void SetNameRpc(string to, RpcParams rpcParams = default)
    {
        this.playerName = to;

        Debug.Log("Name of " + OwnerClientId + " set to " + this.playerName);

        OnPlayerNameChanged?.Invoke();
    }

    [Rpc(SendTo.Owner)]
    public void PromptNameRpc()
    {
        Debug.Log(playerName);
        SetNameRpc(playerName);
    }

    [Rpc(SendTo.Server)]
    public void SetNameRpc(string to)
    {
        UpdateNameRpc(to);
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateNameRpc(string to)
    {
        this.playerName = to;

        Debug.Log("Name of " + OwnerClientId + " set to " + this.playerName);

        SessionManager.Singleton.OnPlayerSettingsChanged?.Invoke();
        OnPlayerNameChanged?.Invoke();

    }

}
