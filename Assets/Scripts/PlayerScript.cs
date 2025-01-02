using UnityEngine;
using Unity.Netcode;

public class PlayerScript : NetworkBehaviour
{
    public enum Mode { Combat, Spectator, None};
    public Mode currentMode = Mode.Spectator;

    public GameObject spectatorObject;
    //public MonoBehaviour spectatorCamera;

    public GameObject combatObject;

    //[Rpc(SendTo.NotOwner)]

    //public void OnDisable()
    //{
        
    //}
}
