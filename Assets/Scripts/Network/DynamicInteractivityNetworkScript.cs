using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

[RequireComponent(typeof(Selectable))]
public class DynamicInteractivityNetworkScript : MonoBehaviour
{
    public Selectable selectable;

    public enum visibilityKind { OnlyServer, OnlyClient, Client, NotServer, AlwaysOn, AlwaysOff};
    public visibilityKind selectableCondition;
    public bool activeWhenDisconnected = true;



    public void UpdateInteractability()
    {
        //Debug.Log(NetworkManager.Singleton.IsClient + " " + NetworkManager.Singleton.IsServer);

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            selectable.interactable = activeWhenDisconnected;
            return;
        }
        //Debug.Log(string.Format("{0},{1},{2}", selectable != null, SessionManager.Singleton != null, transform.name));
        switch (selectableCondition)
        {
            case visibilityKind.Client:
                selectable.interactable = SessionManager.Singleton.IsClient;
                break;
            case visibilityKind.OnlyServer:
                selectable.interactable = SessionManager.Singleton.IsServer;
                break;
            case visibilityKind.OnlyClient:
                selectable.interactable = SessionManager.Singleton.IsClient && !SessionManager.Singleton.IsServer;
                break;
            case visibilityKind.NotServer:
                selectable.interactable = !SessionManager.Singleton.IsServer;
                break;
            case visibilityKind.AlwaysOn:
                selectable.interactable = true;
                break;
            case visibilityKind.AlwaysOff:
                selectable.interactable = false;
                break;
        }
    }

    private void OnEnable()
    {
        if (selectable == null)
            selectable = GetComponent<Selectable>();

        UpdateInteractability();
    }

    public static void UpdateAll()
    {
        var all = FindObjectsByType<DynamicInteractivityNetworkScript>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);

        foreach (var o in all)
            o.UpdateInteractability();
    }
}
