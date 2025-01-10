using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class PlayerScript : NetworkBehaviour, IEntity
{
    public enum Mode { Combat, Spectator, None};
    public Mode currentMode = Mode.Spectator;

    public PlayerMode spectatorObject;
    public HealthScript playerHealth;
    //public MonoBehaviour spectatorCamera;

    public PlayerMode combatObject;
    public NetworkObject nO;

    public string identifier 
    { 
        get 
        { 
            return OwnerClientId.ToString(); 
        } 

        set
        {
            return;
        }
    }

    [field: SerializeField]
    private string _displayName = "UNKNOWN";
    public string displayName 
    { 
        get
        {
            if (SessionManager.Singleton != null && SessionManager.Singleton.TryGetRegisteredPlayer(OwnerClientId, out var player))
            {
                return player.playerName;
            }

            return _displayName;
        }

        set
        {
            return;
        }
    }

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

    private void OnEnable()
    {
        playerHealth.OnDie += Die;
    }

    private void OnDisable()
    {
        playerHealth.OnDie -= Die;
    }

    protected void Die(int overflow, IEntity dealer, string verb)
    {
        if (dealer == null)
        {
            Debug.Log(string.Format("{0} was smithen.", this.displayName));
            return;
        }

        Debug.Log(string.Format("{0} {1} {2}", dealer.displayName, verb, this.displayName));
    }

    public void SetMode(Mode to)
    {
        spectatorObject.SetSelection(to == Mode.Spectator);
        combatObject.SetSelection(to == Mode.Combat);
    }
}
