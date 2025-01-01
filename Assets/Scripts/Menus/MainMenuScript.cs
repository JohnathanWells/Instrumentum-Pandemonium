using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MainMenuScript : MonoBehaviour
{
    public static MainMenuScript Singleton;

    [SerializeField] private TMP_InputField nameField;


    public UnityEvent<string> OnPlayerNameValid;
    public UnityEvent<string> OnPlayerNameNotValid;

    [System.Serializable]
    public class Window
    {
        public string id;
        public bool open = false;
        public GameObject obj;
        public UnityEvent OnOpen;
        public UnityEvent OnClose;

        public void Open()
        {
            OnOpen.Invoke();
            obj.SetActive(true);
        }
        public void Close()
        {
            OnClose.Invoke();
            obj.SetActive(false);
        }
    }

    [SerializeField] private Window[] windows; 

    private void Awake()
    {
        if (Singleton && Singleton != this)
        {
            Destroy(Singleton);
        }

        Singleton = this;
    }

    public void ValidatePlayerName(string nm)
    {
        if (string.IsNullOrWhiteSpace(nm))
        {
            OnPlayerNameNotValid.Invoke(nm);

            return;
        }

        OnPlayerNameValid.Invoke(nm);
    }

    public void EnterName(string nm)
    {
        //Debug.Log("Setting player name as " + nm);
        PlayerSettings.playerName = nm;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenWindow(string withID)
    {
        foreach (var w in windows)
        {
            if (w.id.CompareTo(withID) == 0)
            {
                if (!w.open)
                    w.Open();
                return;
            }
        }
    }

    public void CloseWindow(string withID)
    {
        foreach (var w in windows)
        {
            if (w.id.CompareTo(withID) == 0)
            {
                if (w.open)
                    w.Close();
                return;
            }
        }
    }

    public void OpenWindowOnly(string withID)
    {
        foreach (var w in windows)
        {
            if (w.id.CompareTo(withID) == 0)
            {
                if (!w.open)
                    w.Open();
            }
            else
            {
                if (w.open)
                    w.Close();
            }
        }
    }

    public void CloseAllWindows()
    {
        foreach (var w in windows)
        {
            if (w.open)
                w.Close();
        }
    }
}
