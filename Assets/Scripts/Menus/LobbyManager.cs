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

    public TMP_InputField preLobbyServerNameInputField;
    public TMP_Dropdown preLobbyGamemodeDropdown;
    public TMP_Dropdown preLobbyMapDropdown;

    //public TextMeshProUGUI lobbyServerNameDisplay;
    public TMP_Dropdown lobbyGamemodeDropdown;
    public TMP_Dropdown lobbyMapDropdown;

    public Image mapThumbnail;

    public UnityEvent<string> OnServerNameValid;
    public UnityEvent<string> OnServerNameNotValid;

    public RectTransform playerListArea;
    public PlayerListItem playerListItem;
    //private Dictionary<ulong, PlayerListItem> playersInList = new Dictionary<ulong, PlayerListItem>();
    private List<PlayerListItem> playersInList = new List<PlayerListItem>();

    public static LobbyManager Singleton;

    private void Start()
    {
        Singleton = this;
    }

    public void PopulateGamemodeDropdown()
    {

        List<string> gamemodeNames = new List<string>();

        foreach (var gm in HostManager.Singleton.GetGamemodes())
        {
            gamemodeNames.Add(gm.gamemodeName);
        }

        preLobbyGamemodeDropdown.ClearOptions();
        preLobbyGamemodeDropdown.AddOptions(gamemodeNames);

        lobbyGamemodeDropdown.ClearOptions();
        lobbyGamemodeDropdown.AddOptions(gamemodeNames);

        if (HostManager.Singleton.selectedGamemode == null && preLobbyGamemodeDropdown.options.Count > 0)
        {
            SetGamemode(0);
        }
    }

    public void PopulateMapDropdown()
    {

        List<string> mapNames = new List<string>();

        foreach (var m in HostManager.Singleton.GetMaps())
        {
            foreach (var gm in m.validGamemodes)
            {
                if (gm == HostManager.Singleton.selectedGamemode)
                {
                    mapNames.Add(m.mapName);
                }
            }
        }

        preLobbyMapDropdown.ClearOptions();
        preLobbyMapDropdown.AddOptions(mapNames);

        lobbyMapDropdown.ClearOptions();
        lobbyMapDropdown.AddOptions(mapNames);

        if (HostManager.Singleton.selectedMap == null && preLobbyMapDropdown.options.Count > 0)
        {
            SetMap(0);
        }
    }

    public void SetGamemode(int to)
    {
        HostManager.Singleton.SelectGamemode(to);

        //if (mapsAvailable.options.Count > 0)
        //    mapsAvailable.onValueChanged.Invoke(0);
    }

    public void SetMap(int to)
    {
        HostManager.Singleton.SelectMap(to);

        mapThumbnail.sprite = HostManager.Singleton.selectedMap.thumbnail;

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
    }

    private void OnDisable()
    {
        //SessionManager.Singleton.OnPlayerConnected -= AddPlayerToList;
        //SessionManager.Singleton.OnPlayerDisconnected -= RemovePlayerFromList;
        SessionManager.Singleton.OnPlayerConnected -= UpdatePlayers;
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

    //public void AddPlayerToList(ulong withId)
    //{
    //    playersInList.Add(withId, Instantiate(playerListItem, playerListArea));
    //}

    //public void RemovePlayerFromList(ulong withID)
    //{
    //    Destroy(playersInList[withID].gameObject);
    //    playersInList.Remove(withID);
    //}

    //public void ClearPlayersFromList()
    //{
    //    //for (int n = playersInList.Count - 1; n >= 0; n--)
    //    //{
    //    //    Destroy(playersInList[n].gameObject);
    //    //}

    //    Transform[] playerIcons = playerListArea.GetComponentsInChildren<Transform>();

    //    for (int n = playerIcons.Length; n > 0; n--)
    //    {
    //        Destroy(playerIcons[n].gameObject);
    //    }

    //    playersInList.Clear();
    //}

    public void UpdatePlayers()
    {
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
            Debug.Log(p.playerName);
            playersInList.Add(listItem);
        }
    }

    public void UpdatePlayers(ulong var)
    {
        UpdatePlayers();
    }
}
