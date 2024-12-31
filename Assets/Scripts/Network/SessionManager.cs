using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System.Collections.Generic;

public class SessionManager : NetworkBehaviour
{
    private static NetworkManager m_NetworkManager;

    public static SessionManager Singleton;

    public UnityAction<ulong> OnPlayerConnected;
    public UnityAction<ulong> OnPlayerDisconnected;

    private List<ulong> players = new List<ulong>();
    //private Dictionary<ulong, PlayerScript> players = new Dictionary<ulong, PlayerScript>();

    private void Awake()
    {
        if (m_NetworkManager == null)
            m_NetworkManager = GetComponent<NetworkManager>();

        Singleton = this;
    }

    void OnEnable()
    {
        m_NetworkManager.OnConnectionEvent += UpdatePlayerList;
    }

    private void OnDisable()
    {
        m_NetworkManager.OnConnectionEvent -= UpdatePlayerList;
    }

    private void UpdatePlayerList(NetworkManager nm, ConnectionEventData connectionData)
    {
        if (!IsOwner)
            return;

        switch (connectionData.EventType)
        {
            case ConnectionEvent.ClientConnected:
            case ConnectionEvent.PeerConnected:
                players.Add(connectionData.ClientId);
                OnPlayerConnected.Invoke(connectionData.ClientId);
                break;
            case ConnectionEvent.ClientDisconnected:
            case ConnectionEvent.PeerDisconnected:
                players.Remove(connectionData.ClientId);
                OnPlayerDisconnected.Invoke(connectionData.ClientId);
                break;
            default:
                break;
        }


    }

}
