using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class LobbyManager : MonoBehaviour
{
    public NetworkManager m_networkManager;

    public Button HostBtn;
    public Button JoinBtn;

    public Button startMatchBtn;

    public TMP_InputField preLobbyServerNameInputField;
    public TMP_Dropdown[] gamemodeDropdownList;
    public TMP_Dropdown[] mapDropdownList;

    private List<string> gamemodesInDropdown = new List<string>();
    private List<string> mapsInDropdown = new List<string>();


    public Image[] mapThumbnails;
    public Sprite emptyThumbnail;

    public UnityEvent<string> OnServerNameValid;
    public UnityEvent<string> OnServerNameNotValid;

    public UnityEvent OnServerOptionsValid;
    public UnityEvent OnServerOptionsNotValid;

    public RectTransform playerListArea;
    public PlayerListItem playerListItem;
    //private Dictionary<ulong, PlayerListItem> playersInList = new Dictionary<ulong, PlayerListItem>();
    private List<PlayerListItem> playersInList = new List<PlayerListItem>();

    public static LobbyManager Singleton;

    private void Start()
    {
        Singleton = this;

        HostManager.LoadAllGamemodes();
        HostManager.LoadAllMaps();
        PopulateGamemodeDropdown();
        PopulateMapDropdown();

        ValidateServerName(string.Empty);
    }

    public void PopulateGamemodeDropdown()
    {
        List<string> gamemodeNames = new List<string>();
        gamemodesInDropdown = new List<string>();

        foreach (var gm in HostManager.GetGamemodeList())
        {
            gamemodeNames.Add(gm.gamemodeName);
            gamemodesInDropdown.Add(gm.gamemodeID);
        }

        foreach (var d in gamemodeDropdownList)
        {
            d.ClearOptions();
            d.AddOptions(gamemodeNames);
        }

        if (gamemodesInDropdown.Count > 0)
        {
            if (HostManager.selectedGamemode == null)
            {
                SetGamemode(gamemodesInDropdown[0]);
            }
            else if (gamemodesInDropdown.Contains(HostManager.selectedGamemode.gamemodeID))
            {
                SetGamemode(HostManager.selectedGamemode.gamemodeID);
            }

        }
        else
        {
            HostManager.DeselectGamemode();
            HostManager.DeselectMap();
        }
    }

    public void PopulateMapDropdown()
    {
        mapsInDropdown = new List<string>();
        List<string> mapNames = new List<string>();

        if (HostManager.selectedGamemode == null)
            return;

        foreach (var m in HostManager.GetMapList())
        {
            foreach (var gm in m.validGamemodes)
            {
                if (gm.gamemodeID == HostManager.selectedGamemode.gamemodeID)
                {
                    mapNames.Add(m.mapName);
                    mapsInDropdown.Add(m.mapID);
                }
            }
        }

        foreach (var d in mapDropdownList)
        {
            d.ClearOptions();
            d.AddOptions(mapNames);
        }

        if (mapsInDropdown.Count > 0)
        {
            if (HostManager.selectedMap == null)
            {
                SetMap(mapsInDropdown[0]);
                return;
            }
            else if (mapsInDropdown.Contains(HostManager.selectedMap.mapID))
            {
                SetMap(HostManager.selectedMap.mapID);
            }
        }
        else
        {
            HostManager.DeselectMap();
        }
    }

    public void SetGamemode(int to)
    {
        if (to >= gamemodesInDropdown.Count || to < 0)
        {
            return;
        }

        SetGamemode(gamemodesInDropdown[to]);
    }

    public void SetGamemode(string to)
    {

        if (NetworkManager.Singleton.IsServer)
            SessionManager.Singleton.SetGamemodeRpc(to);
        else if (!NetworkManager.Singleton.IsClient)
        {
            HostManager.SetSelectedGamemode(to);
            UpdateGameModeDropdowns();
        }

        ValidateServerOptions();
    }

    public void SetMap(int to)
    {
        if (to >= mapsInDropdown.Count || to < 0)
        {
            return;
        }

        SetMap(mapsInDropdown[to]);
    }

    public void SetMap(string to)
    {

        if (NetworkManager.Singleton.IsServer)
            SessionManager.Singleton.SetMapRpc(to);
        else if (!NetworkManager.Singleton.IsClient)
        {
            HostManager.SetSelectedMap(to);
            UpdateMapDropdowns();
        }

        ValidateServerOptions();
    }

    public void UpdateDropdowns(string dummy)
    {
        UpdateMapDropdowns();
        UpdateGameModeDropdowns();
    }

    public void UpdateMapDropdowns()
    {
        int mapInd = -1;
        for (int i = 0; i < mapsInDropdown.Count; i++)
        {
            if (mapsInDropdown[i] == HostManager.selectedMap.mapID)
            {
                mapInd = i;
                break;
            }
        }

        //Debug.Log("Updating map dropdowns to match " + HostManager.selectedMap.mapID);
        if (mapInd >= 0 && mapInd < mapDropdownList.Length)
        {
            foreach (var mDp in mapDropdownList)
            {
                mDp.value = mapInd;
            }
        }

        foreach (var t in mapThumbnails)
        {
            if (HostManager.selectedMap == null)
            {
                t.sprite = emptyThumbnail;
                continue;
            }
            
            t.sprite = HostManager.GetMapSprite();
        }
    }    
    
    public void UpdateGameModeDropdowns()
    {
        int gmInd = -1;
        for (int i = 0; i < gamemodesInDropdown.Count; i++)
        {
            if (gamemodesInDropdown[i] == HostManager.selectedGamemode.gamemodeID)
            {
                gmInd = i;
                break;
            }
        }

        //Debug.Log("Updating gamemode dropdowns to match " + HostManager.selectedGamemode.gamemodeID);
        if (gmInd >= 0 && gmInd < gamemodeDropdownList.Length)
        {
            foreach (var gmDp in gamemodeDropdownList)
            {
                gmDp.value = gmInd;
            }
        }
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            HostBtn.interactable = false;
            JoinBtn.interactable = false;
        }
    }

    private void OnEnable()
    {
        //SessionManager.Singleton.OnPlayerConnected += AddPlayerToList;
        //SessionManager.Singleton.OnPlayerDisconnected += RemovePlayerFromList;
        SessionManager.Singleton.OnPlayerConnected += UpdatePlayers;
        SessionManager.Singleton.OnPlayerDisconnected += UpdatePlayers;
        SessionManager.Singleton.OnGamemodeChanged += UpdateDropdowns;
        SessionManager.Singleton.OnMapChanged += UpdateDropdowns;
        //SessionManager.Singleton.OnPlayerObjectSpawned += IntroduceToPlayer;
        SessionManager.Singleton.OnPlayerSettingsChanged += UpdatePlayers;
        startMatchBtn.onClick.AddListener(() => StartGame());

        foreach (var p in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            //p.OnPlayerNameChanged += UpdatePlayers;
            p.PromptNameRpc();
        }
    }

    private void OnDisable()
    {
        //SessionManager.Singleton.OnPlayerConnected -= AddPlayerToList;
        //SessionManager.Singleton.OnPlayerDisconnected -= RemovePlayerFromList;
        SessionManager.Singleton.OnPlayerConnected -= UpdatePlayers;
        SessionManager.Singleton.OnPlayerDisconnected -= UpdatePlayers;
        SessionManager.Singleton.OnGamemodeChanged -= UpdateDropdowns;
        SessionManager.Singleton.OnMapChanged -= UpdateDropdowns;
        //SessionManager.Singleton.OnPlayerObjectSpawned -= IntroduceToPlayer;
        SessionManager.Singleton.OnPlayerSettingsChanged -= UpdatePlayers;
        startMatchBtn.onClick.RemoveListener(() => StartGame());

        //foreach (var p in FindObjectsByType<Player>(FindObjectsSortMode.None))
        //{
        //    p.OnPlayerNameChanged -= UpdatePlayers;
        //}
    }

    public void HostServer()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void JoinServer()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void ValidateServerName(string nm)
    {
        if (string.IsNullOrWhiteSpace(nm))
        {
            OnServerNameNotValid.Invoke(nm);

            return;
        }

        OnServerNameValid.Invoke(nm);
    }

    public void ValidateServerOptions()
    {
        if (HostManager.selectedGamemode == null || HostManager.selectedMap == null)
        {
            OnServerOptionsNotValid?.Invoke();
            ValidateIfGameIsReady();
            return;
        }
                
        OnServerOptionsValid?.Invoke();
        //ValidateIfGameIsReady();
    }

    public void ValidateIfGameIsReady()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            startMatchBtn.interactable = false;
            return;
        }

        if (SessionManager.Singleton == null)
        {
            startMatchBtn.interactable = false;
            return;
        }
        
        startMatchBtn.interactable = (playersInList.Count <= SessionManager.Singleton.sessionSettings.maxPlayerCount);
    }

    public void IntroduceToPlayer(NetworkBehaviour newPlayer)
    {
        if (newPlayer is Player)
        {
            UpdatePlayers();
        }
    }

    public void UpdatePlayers()
    {
        Debug.Log("updating player list...");
        for (int n = playersInList.Count - 1; n >= 0; n--)
        {
            Destroy(playersInList[n].gameObject);
        }

        playersInList.Clear();

        foreach (var p in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            PlayerListItem listItem = Instantiate(playerListItem, playerListArea);
            //p.Init();
            listItem.playerName = p.playerName;
            playersInList.Add(listItem);
        }
    }

    public void UpdatePlayers(ulong var)
    {
        UpdatePlayers();
    }

    public void StartGame()
    {

        SessionManager.Singleton.LoadSelectedMap();
    }
}
