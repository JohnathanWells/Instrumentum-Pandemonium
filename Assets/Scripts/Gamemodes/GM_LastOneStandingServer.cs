using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM_LastOneStandingServer : GM_LastOneStandingBase
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        serverInstance = this;

        enabled = IsServer;
    }

    public void DetermineWinners()
    {
        //TODO
    }

    public override void Init()
    {
        //TODO
    }

    public override void OnPlayerLoad(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        SpawnPlayer(clientId);
    }

    public override void OnAllPlayersLoad(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        //TODO
    }

    public override void OnPlayerDisconnect(ulong clientId)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnPlayerConnect(ulong clientId)
    {
        //throw new System.NotImplementedException();
    }

    public override void GameEnds()
    {
        DetermineWinners();
    }

    public override void OnPlayerKilled(PlayerScript player, FragDetails details)
    {
        //TODO
    }

    public override void OnTickPassed()
    {
        //throw new System.NotImplementedException();
    }

    public override void SpawnPlayer(ulong clientId)
    {
        var instance = Instantiate(playerPrefab);
        instance.gameObject.name = "Player: " + Player.localPlayer.playerName;
        Debug.Log("Bump");
        instance.SpawnWithOwnership(clientId);
    }

    public override bool IsActiveForRole()
    {
        return IsServer;
    }
}
