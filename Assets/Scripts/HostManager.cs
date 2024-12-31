using UnityEngine;

public class HostManager : MonoBehaviour
{
    private GamemodeSO[] availableGamemodes;
    private GamemodeSO _selectedGamemode;
    public GamemodeSO selectedGamemode
    {
        get
        {
            return _selectedGamemode;
        }
    }
    private MapSO[] availableMaps;
    private MapSO _selectedMap;
    public MapSO selectedMap
    {
        get
        {
            return _selectedMap;
        }
    }

    public static HostManager Singleton;

    private void Awake()
    {
        if (Singleton != this)
        {
            Destroy(Singleton);
        }

        Singleton = this;

        LoadAllGamemodes();
        LoadAllMaps();
    }

    public void LoadAllGamemodes()
    {
        availableGamemodes = Resources.LoadAll<GamemodeSO>("gamemodes/");
        Debug.Log(availableGamemodes.Length);
    }
    
    public void LoadAllMaps()
    {
        availableMaps = Resources.LoadAll<MapSO>("maps/");
    }

    public GamemodeSO[] GetGamemodes()
    {
        return availableGamemodes;
    }

    public MapSO[] GetMaps()
    {
        return availableMaps;
    }

    public void SelectGamemode(int ind)
    {
        _selectedGamemode = availableGamemodes[ind];
    }

    public void SelectMap(int ind)
    {
        _selectedMap = availableMaps[ind];
    }
}
