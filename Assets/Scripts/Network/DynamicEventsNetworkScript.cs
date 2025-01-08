using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class DynamicEventsNetworkScript : NetworkBehaviour
{
    public UnityEvent OnClient;
    public UnityEvent OnServer;

    public UnityEvent OnOwner;
    public UnityEvent OnNotOwner;

    public UnityEvent OnNotConnected;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        UpdateEvents();
    }

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();

        UpdateEvents();
    }


    public override void OnLostOwnership()
    {
        base.OnLostOwnership();

        UpdateEvents();
    }

    void UpdateEvents()
    {
        if (!IsClient && !IsServer)
        {
            OnNotConnected.Invoke();
            return;
        }

        if (IsClient)
        {
            OnClient.Invoke();
        }

        if (IsServer)
        {
            OnServer.Invoke();
        }

        if (IsOwner)
            OnOwner.Invoke();
        else
            OnNotOwner.Invoke();
    }
}
