using Unity.Netcode;
using UnityEngine;

public class Gamemode_LastOneStanding : GamemodeBase
{
    public NetworkObject playerPrefab;
    public NetworkVariable<bool> matchStarted = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> readyToStart = new NetworkVariable<bool>(false);

    public override void Defeat(PlayerScript[] losers)
    {
        throw new System.NotImplementedException();
    }

    public override void OnAllPlayersSpawn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnFirstPlayerSpawn(PlayerScript player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnLastPlayerSpawn(PlayerScript lastPlayer)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerConnect(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerDisconnect(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override bool OnPlayerKilled(PlayerScript player, FragDetails details)
    {
        throw new System.NotImplementedException();
    }

    public override bool OnTickPassed()
    {
        throw new System.NotImplementedException();
    }

    public override void Victory(PlayerScript[] winners)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerLoad(Player player)
    {
        //SessionManager.Singleton.SpawnPlayerInMapRpc(player, playerPrefab);
    }

    public override void SpawnPlayer(Player player)
    {
        var instance = Instantiate(playerPrefab);
        instance.gameObject.name = "Player: " + player.playerName;
        instance.SpawnAsPlayerObject(player.OwnerClientId);
    }
}
