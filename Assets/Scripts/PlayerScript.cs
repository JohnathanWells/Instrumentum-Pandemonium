using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class PlayerScript : NetworkBehaviour
{
    public enum Mode { Combat, Spectator, None};
    public Mode currentMode = Mode.Spectator;

    public PlayerMode spectatorObject;
    //public MonoBehaviour spectatorCamera;

    public PlayerMode combatObject;
    public NetworkObject nO;

    [System.Serializable]
    public class PlayerMode
    {
        public GameObject obj;
        public UnityEvent OnModeSelected;
        public UnityEvent OnModeDeselected;

        public void SetSelection(bool to)
        {
            if (to)
                OnModeSelected.Invoke();
            else
                OnModeDeselected.Invoke();
        }
    }

    private void Awake()
    {
        if (nO == null)
            nO = GetComponent<NetworkObject>();

        SetMode(currentMode);
    }

    public void SetMode(Mode to)
    {
        spectatorObject.SetSelection(to == Mode.Spectator);
        combatObject.SetSelection(to == Mode.Combat);
    }
}
