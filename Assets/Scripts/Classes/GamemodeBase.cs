using UnityEngine;
using Unity.Netcode;

public abstract class GamemodeBase : NetworkBehaviour
{
    public GamemodeSO gamemodeSO;

    public static GamemodeBase Singleton;
    //protected void Awake()
    //{
    //    Singleton = this;
    //}

    public abstract void OnFirstPlayerSpawn(PlayerScript player);

    public abstract void OnLastPlayerSpawn(PlayerScript lastPlayer);

    public abstract void OnPlayerLoad(Player player);

    public abstract void OnAllPlayersSpawn();

    public abstract void OnPlayerDisconnect(Player player);

    public abstract void OnPlayerConnect(Player player);

    public abstract void Victory(PlayerScript[] winners);

    public abstract void Defeat(PlayerScript[] losers);

    public abstract bool OnPlayerKilled(PlayerScript player, FragDetails details);

    public abstract bool OnTickPassed();

    public abstract void SpawnPlayer(Player player);

    public void SetAsSingleton()
    {
        Singleton = this;
    }
}
