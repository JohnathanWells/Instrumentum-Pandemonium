using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System.Collections.Generic;

public class SessionManager : NetworkBehaviour
{
    [SerializeField] private NetworkManager m_NetworkManager;

    public static SessionManager Singleton;

    public UnityAction<ulong> OnPlayerConnected;
    public UnityAction<ulong> OnPlayerDisconnected;

    public NetworkVariable<int> mapID;
    public NetworkVariable<int> gamemodeID;


    public UnityAction<int> OnMapChanged;
    public UnityAction<int> OnGamemodeChanged;


    [SerializeField] private NetworkObject playerManagerPrefab;

    private List<ulong> players = new List<ulong>();
    //private Dictionary<ulong, PlayerScript> players = new Dictionary<ulong, PlayerScript>();

    private void Awake()
    {
        if (m_NetworkManager == null)
            m_NetworkManager = NetworkManager.Singleton;

        Singleton = this;
    }

    void OnEnable()
    {
        m_NetworkManager.OnConnectionEvent += UpdatePlayerList;
        //m_NetworkManager.OnConnectionEvent += SpawnPlayerManager;
    }

    private void OnDisable()
    {
        m_NetworkManager.OnConnectionEvent -= UpdatePlayerList;
        //m_NetworkManager.OnConnectionEvent -= SpawnPlayerManager;
    }

    private void UpdatePlayerList(NetworkManager nm, ConnectionEventData connectionData)
    {
        if (!IsOwner)
            return;

        switch (connectionData.EventType)
        {
            case ConnectionEvent.PeerConnected:
            case ConnectionEvent.ClientConnected:
                SpawnPlayerManager(nm, connectionData);
                players.Add(connectionData.ClientId);
                OnPlayerConnected?.Invoke(connectionData.ClientId);
                break;
            case ConnectionEvent.ClientDisconnected:
            case ConnectionEvent.PeerDisconnected:
                players.Remove(connectionData.ClientId);
                OnPlayerDisconnected?.Invoke(connectionData.ClientId);
                break;
            default:
                break;
        }


    }

    private void SpawnPlayerManager(NetworkManager nm, ConnectionEventData connectionData)
    {
        if (!IsServer || connectionData.EventType != ConnectionEvent.ClientConnected)
            return;

        var instance = Instantiate(playerManagerPrefab);
        instance.gameObject.name = "PlayerManager#" + connectionData.ClientId.ToString();
        instance.SpawnAsPlayerObject(connectionData.ClientId);
    }

    [Rpc(SendTo.Server)]
    public void SetMapRpc(int to)
    {
        mapID.Value = to;
    }

    [Rpc(SendTo.Server)]
    public void SetGamemodeRpc(int to)
    {
        gamemodeID.Value = to;
    }

}
