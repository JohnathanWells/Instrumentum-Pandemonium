using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.Collections;

public class SessionManager : NetworkBehaviour
{
    [SerializeField] private NetworkManager m_NetworkManager;

    public static SessionManager Singleton;

    public UnityAction<ulong> OnPlayerConnected;
    public UnityAction<ulong> OnPlayerDisconnected;

    public UnityAction<NetworkBehaviour> OnPlayerObjectSpawned;
    public UnityAction<NetworkBehaviour> OnPlayerObjectDespawned;

    /*private NetworkVariable<FixedString64Bytes> _selectedMapID;

    public FixedString64Bytes selectedMapID
    {
        get
        {
            return _selectedMapID.Value;
        }

        set
        {
            _selectedMapID.Value = value;
            Debug.Log("_selectedMapID.Value = " + value);
            //HostManager.SetSelectedMap(value.ToString());
            //OnMapChanged.Invoke(value.ToString());
        }
    }

    private NetworkVariable<FixedString64Bytes> _selectedGamemodeID;

    public FixedString64Bytes selectedGamemodeID
    {
        get
        {
            return _selectedGamemodeID.Value;
        }

        set
        {
            _selectedGamemodeID.Value = value;
            Debug.Log("_selectedGamemodeID.Value = " + value);
            //HostManager.SetSelectedGamemode(value.ToString());
            //OnGamemodeChanged.Invoke(value.ToString());
        }
    }*/


    public UnityAction<string> OnMapChanged;
    public UnityAction<string> OnGamemodeChanged;


    public UnityAction OnPlayerSettingsChanged;


    [SerializeField] private NetworkObject playerManagerPrefab;

    private List<ulong> players = new List<ulong>();
    private Dictionary<ulong, Player> connectedPlayers = new Dictionary<ulong, Player>();

    public SessionSettings sessionSettings = new SessionSettings();

    private void Awake()
    {
        Debug.Log("Setting singletone.");

        if (m_NetworkManager == null)
            m_NetworkManager = NetworkManager.Singleton;


        if (Singleton != null)
            Destroy(Singleton.gameObject);

        Singleton = this;

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        m_NetworkManager.OnConnectionEvent += UpdatePlayerList;
        m_NetworkManager.OnConnectionEvent += UpdateNewClient;
        m_NetworkManager.OnConnectionEvent += UpdateOwnershipDependentStates;
        m_NetworkManager.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        //m_NetworkManager.OnConnectionEvent += SpawnPlayerManager;
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        SpawnAllPlayersInMapRpc();
    }

    private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
    {
        OnMapLoad();
    }

    private void OnDisable()
    {
        Debug.Log("Disabling.");
        m_NetworkManager.OnConnectionEvent -= UpdatePlayerList;
        m_NetworkManager.OnConnectionEvent -= UpdateNewClient;
        m_NetworkManager.OnConnectionEvent -= UpdateOwnershipDependentStates;

        //m_NetworkManager.OnConnectionEvent -= SpawnPlayerManager;
    }

    public void LoadSelectedMap()
    {
        if (HostManager.selectedMap == null)
            Debug.LogError("ERROR: No map selected.");

        var status = m_NetworkManager.SceneManager.LoadScene(HostManager.selectedMap.mapName, UnityEngine.SceneManagement.LoadSceneMode.Single);

        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load {HostManager.selectedMap.mapName} " +
                  $"with a {nameof(SceneEventProgressStatus)}: {status}");
        }
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
        instance.SpawnAsPlayerObject(connectionData.ClientId, false);
    }

    private void UpdateOwnershipDependentStates(NetworkManager nm, ConnectionEventData connectionData)
    {
        DynamicInteractivityNetworkScript.UpdateAll();
    }

    void UpdateNewClient(NetworkManager nm, ConnectionEventData connectionData)
    {
        if (!IsServer || connectionData.EventType != ConnectionEvent.ClientConnected)
            return;

        UpdateClientMapRpc(HostManager.selectedMap.mapID);
        UpdateClientGamemodeRpc(HostManager.selectedGamemode.gamemodeID);
    }

    public void OnMapLoad()
    {
        Debug.Log("Getting local gamemode handler...");
        var candidates = FindObjectsByType<GamemodeBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (var c in candidates)
        {
            if (c.gamemodeSO == HostManager.selectedGamemode)
            {
                c.SetAsSingleton();
            }
            else
            {
                Destroy(c.gameObject);
            }
        }

        if (GamemodeBase.Singleton == null)
        {
            Debug.LogError("The scene does not have the required gamemode!");
            return;
        }


    }

    public void RegisterPlayer(Player p)
    {
        if (!connectedPlayers.ContainsKey(p.OwnerClientId))
        {
            connectedPlayers.Add(p.OwnerClientId, p);
        }
    }

    public void UnregisterPlayer(ulong clientId)
    {
        if (!connectedPlayers.ContainsKey(clientId))
            return;

        connectedPlayers.Remove(clientId);
    }

    [Rpc(SendTo.Server)]
    public void SpawnAllPlayersInMapRpc()
    {
        foreach (var p in NetworkManager.ConnectedClientsIds)
        {
            GamemodeBase.Singleton.SpawnPlayer(connectedPlayers[p]);
        }
    }

    [Rpc(SendTo.Server)]
    public void SetMapRpc(string to)
    {
        //selectedMapID = to;
        //HostManager.SetSelectedMap(to);
        Debug.Log("Server Map set to " + to);

        UpdateClientMapRpc(to);
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateClientMapRpc(string to)
    {
        HostManager.SetSelectedMap(to);
        OnMapChanged.Invoke(to);
    }

    [Rpc(SendTo.NotServer)]
    public void UpdateOnlyClientMapRpc(string to)
    {
        HostManager.SetSelectedMap(to);
        OnMapChanged.Invoke(to);
    }

    [Rpc(SendTo.Server)]
    public void SetGamemodeRpc(string to)
    {
        //selectedGamemodeID = to;
        Debug.Log("Server gamemode set to " + to);

        UpdateClientGamemodeRpc(to);
    }

    [Rpc(SendTo.NotServer)]
    public void UpdateOnlyClientGamemodeRpc(string to)
    {
        HostManager.SetSelectedGamemode(to);
        OnGamemodeChanged.Invoke(to);
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateClientGamemodeRpc(string to)
    {
        HostManager.SetSelectedGamemode(to);
        OnGamemodeChanged.Invoke(to);
    }

}
