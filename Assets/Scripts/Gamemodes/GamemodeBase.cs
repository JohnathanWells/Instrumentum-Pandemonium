using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public abstract class GamemodeBase : NetworkBehaviour
{
    public GamemodeSO gamemodeSO;

    public NetworkObject playerPrefab;

    public static GamemodeBase activeGamemode;

    protected void OnEnable()
    {
        NetworkManager.SceneManager.OnLoadComplete += OnPlayerLoad;
        NetworkManager.SceneManager.OnLoadEventCompleted += OnAllPlayersLoad;
        NetworkManager.OnConnectionEvent += OnPlayerChange;
    }


    protected void OnDisable()
    {
        NetworkManager.SceneManager.OnLoadComplete -= OnPlayerLoad;
        NetworkManager.SceneManager.OnLoadEventCompleted -= OnAllPlayersLoad;
        NetworkManager.OnConnectionEvent -= OnPlayerChange;
    }

    public abstract void Init();

    public abstract void OnPlayerLoad(ulong clientId, string sceneName, LoadSceneMode loadSceneMode);

    public abstract void OnAllPlayersLoad(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut);

    protected void OnPlayerChange(NetworkManager nm, ConnectionEventData connectionData)
    {
        switch (connectionData.EventType)
        {
            case ConnectionEvent.PeerConnected:
            case ConnectionEvent.ClientConnected:
                OnPlayerConnect(connectionData.ClientId);
                break;
            case ConnectionEvent.ClientDisconnected:
            case ConnectionEvent.PeerDisconnected:
                OnPlayerDisconnect(connectionData.ClientId);
                break;
            default:
                break;
        }
    }

    public abstract void OnPlayerDisconnect(ulong clientId);

    public abstract void OnPlayerConnect(ulong clientId);

    public abstract void GameEnds();

    public abstract void OnPlayerKilled(PlayerScript player, FragDetails details);

    public abstract void OnTickPassed();

    public abstract void SpawnPlayer(ulong clientId);

    public abstract bool IsActiveForRole();

    public void SetAsActiveGameMode()
    {
        activeGamemode = this;
    }
}
