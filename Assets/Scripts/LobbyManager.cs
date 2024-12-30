using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button HostBtn;
    public Button JoinBtn;

    private void Update()
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            HostBtn.interactable = false;
            JoinBtn.interactable = false;
        }
    }

    public void HostServer()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void JoinServer()
    {
        NetworkManager.Singleton.StartClient();
    }

}
