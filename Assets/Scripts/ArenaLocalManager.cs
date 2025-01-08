using UnityEngine;

public class ArenaLocalManager : MonoBehaviour
{
    //public GameModePair defaultGamemode;
    [SerializeField] private GameModePair[] gamemodesInMap;

    [System.Serializable]
    public class GameModePair
    {
        public GamemodeSO gamemodeSO;
        public GamemodeBase host;
        public GamemodeBase client;

        public GamemodeBase GetRelevantGameModeHandler()
        {
            if (host.IsActiveForRole())
                return host;

            if (client.IsActiveForRole())
                return client;
            
            return null;
        }

        public void Disable()
        {
            host.enabled = false;
            client.enabled = false;
        }

        public void Enable()
        {
            host.enabled = host.IsActiveForRole();
            client.enabled = client.IsActiveForRole();
        }
    }

    private void Start()
    {
        SessionManager.Singleton.OnMapLoad();
    }
    
    private void SetUpGameModesInScene()
    {
        Debug.Log("HELLO");
        foreach (var c in gamemodesInMap)
        {
            c.host.gamemodeSO = c.gamemodeSO;
            c.client.gamemodeSO = c.gamemodeSO;
        }
    }

    public bool AssignActiveGameMode(string withId)
    {
        GamemodeBase.activeGamemode = null;

        SetUpGameModesInScene();

        foreach (var c in gamemodesInMap)
        {
            var relevantHandler = c.GetRelevantGameModeHandler();
            if (relevantHandler.gamemodeSO.gamemodeID == withId)
            {
                relevantHandler.SetAsActiveGameMode();
            }
            else
            {
                c.Disable();
            }
        }

        if (GamemodeBase.activeGamemode == null)
        {
            return false;
        }

        return true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    gamemodesInMap = FindObjectsByType<GamemodeBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

    //    StartSelectedGamemode();
    //}

    //public void StartSelectedGamemode()
    //{
    //    if (HostManager.selectedGamemode == null)
    //    {
    //        if (defaultGamemode == null)
    //        {
    //            Debug.LogError("COULDNT START GAMEMODE: NO GAMEMODE OR DEFAULT GAMEMODE SET");
    //            return;
    //        }
    //        HostManager.LoadAllGamemodes();
    //        HostManager.SetSelectedGamemode(defaultGamemode.gamemodeSO.gamemodeID);
    //    }


    //    foreach (var g in gamemodesInMap)
    //    {
    //        if (g.gamemodeSO.gamemodeID == HostManager.selectedGamemode.gamemodeID)
    //        {
    //            g.enabled = true;
    //            _activeGamemode = g;
    //        }
    //        else
    //        {
    //            g.enabled = false;
    //        }
    //    }

    //    if (_activeGamemode == null)
    //    {
    //        Debug.LogError("No valid gamemode was found in this map.");
    //        return;
    //    }

    //    Debug.Log("Gamemode set to " + activeGamemode.gamemodeSO.gamemodeID);

    //    activeGamemode.Init();
    //    //activeGamemode.NotifyPlayerArrivalToGamemodeRpc(Player.localPlayer.OwnerClientId);
    //}
}
