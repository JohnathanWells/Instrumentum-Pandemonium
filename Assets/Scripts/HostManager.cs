using UnityEngine;
using System.Collections.Generic;

public static class HostManager
{
    private static GamemodeSO[] availableGamemodeList = new GamemodeSO[0];
    private static Dictionary<string, GamemodeSO> availableGamemodes = new Dictionary<string, GamemodeSO>();
    private static string _selectedGamemodeID = null;
    public static GamemodeSO selectedGamemode
    {
        get
        {
            if (_selectedGamemodeID != null && availableGamemodes.TryGetValue(_selectedGamemodeID, out var val))
                return val;
            
            return null;
        }
    }

    private static MapSO[] availableMapList = new MapSO[0];
    private static Dictionary<string, MapSO> availableMaps = new Dictionary<string, MapSO>();
    private static string _selectedMapID = null;
    public static MapSO selectedMap
    {
        get
        {
            if (_selectedMapID != null && availableMaps.TryGetValue(_selectedMapID, out var val))
                return val;

            return null;
        }
    }

    public static void LoadAllGamemodes()
    {
        availableGamemodes.Clear();

        //Debug.Log("LOADING");
        Debug.Log(availableGamemodeList.Length);
        availableGamemodeList = Resources.LoadAll<GamemodeSO>("gamemodes/");

        foreach (var g in availableGamemodeList)
        {
            if (g.hidden)
                continue;

            if (availableGamemodes.ContainsKey(g.gamemodeID))
            {
                Debug.LogError("Failed to add " + g.gamemodeID + " to the gamemode list. Another gamemode with the same ID already loaded.");
                continue;
            }

            availableGamemodes.Add(g.gamemodeID, g);
            //Debug.Log("Added game mode with id \"" + g.gamemodeID + "\".");
        }
        //Debug.Log(availableGamemodeList.Length);
    }
    
    public static void LoadAllMaps()
    {
        availableMaps.Clear();
        availableMapList = Resources.LoadAll<MapSO>("maps/");

        foreach (var m in availableMapList)
        {
            if (m.hidden)
                continue;

            if (availableMaps.ContainsKey(m.mapID))
            {
                Debug.LogError("Failed to add " + m.mapID + " to the map list. Another map with the same ID already loaded.");
                continue;
            }

            availableMaps.Add(m.mapID, m);
            //Debug.Log("Added map with id \"" + m.mapID + "\".");
        }
        //Debug.Log(availableMapList.Length);
    }

    public static GamemodeSO[] GetGamemodeList()
    {
        return availableGamemodeList;
    }

    public static MapSO[] GetMapList()
    {
        return availableMapList;
    }

    public static void DeselectGamemode()
    {
        _selectedGamemodeID = null;
    }

    public static void DeselectMap()
    {
        _selectedMapID = null;
    }

    public static void SetSelectedGamemode(string to)
    {
        if (!availableGamemodes.ContainsKey(to))
        {
            Debug.LogError("Couldn't selected gamemode with ID \"" + to + "\", it has not been loaded.");
            return;
        }

        _selectedGamemodeID = to;
        Debug.Log("Set hostmanager's selectedgamemodeID to " + selectedGamemode);
    }


    public static void SetSelectedMap(string to)
    {
        //Debug.Log(availableMaps.Count);
        if (!availableMaps.ContainsKey(to))
        {
            Debug.LogError("Couldn't selected map with ID " + to + ", it has not been loaded.");
            return;
        }

        _selectedMapID = to;
        Debug.Log("Set hostmanager's selectedmapID to " + selectedMap);
    }

    public static Sprite GetMapSprite()
    {
        if (_selectedMapID == null)
        {
            return null;
        }
        else
        {
            return selectedMap.thumbnail;
        }
    }
}
