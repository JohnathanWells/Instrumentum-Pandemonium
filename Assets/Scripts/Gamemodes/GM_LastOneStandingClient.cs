using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM_LastOneStandingClient : GM_LastOneStandingBase
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        clientInstance = this;

        enabled = IsClient;
    }

    public override void GameEnds()
    {
        //throw new System.NotImplementedException();
    }

    public override void Init()
    {
        //TODO
    }

    public override void OnAllPlayersLoad(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnPlayerConnect(ulong clientId)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnPlayerDisconnect(ulong clientId)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnPlayerKilled(PlayerScript player, FragDetails details)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnPlayerLoad(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        //TODO
    }

    public override void OnTickPassed()
    {
        //throw new System.NotImplementedException();
    }

    public override void SpawnPlayer(ulong clientId)
    {
        //throw new System.NotImplementedException();
    }

    public override bool IsActiveForRole()
    {
        return IsClient;
    }

    //public override void OnPlayerJoin(ulong player)
    //{
    //    if (!IsServer)
    //        return;

    //    Player targetPlayer;

    //    if (SessionManager.Singleton.TryGetRegisteredPlayer(player, out targetPlayer))
    //    {
    //        SpawnPlayer(targetPlayer);
    //    }

    //}

    //public override void SpawnPlayer(Player player)
    //{
    //    if (!IsServer)
    //        return;

    //    //NetworkObject.InstantiateAndSpawn(playerPrefab.gameObject, NetworkManager.Singleton, player.OwnerClientId, true, true);
    //    var instance = Instantiate(playerPrefab);
    //    instance.gameObject.name = "Player: " + player.playerName;
    //    Debug.Log(player.name + " , " + player.OwnerClientId);
    //    SessionManager.Singleton.SpawnNetworkObject(instance);
    //    //instance.SpawnWithOwnership(player.OwnerClientId, true);
    //    //instance.Spawn(true);
    //}

    //[ServerRpc(RequireOwnership =false)]
    //public void SpawnPlayerServerRpc(ulong withID)
    //{
    //    Debug.Log(withID + " " + IsServer);
    //    if (!IsServer)
    //        return;

    //    Player targetPlayer;

    //    if (SessionManager.Singleton.TryGetRegisteredPlayer(withID, out targetPlayer))
    //    {
    //        SpawnPlayer(targetPlayer);
    //    }
    //}
}
