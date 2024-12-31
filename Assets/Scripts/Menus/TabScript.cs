using UnityEngine;
using UnityEngine.Events;

public class TabScript : MonoBehaviour
{
    [System.Serializable]
    public class Tab
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
            open = true;
        }
        public void Close()
        {
            OnClose.Invoke();
            obj.SetActive(false);
            open = false;
        }
    }

    [SerializeField] private Tab[] tabs;

    private void Start()
    {
        OpenTab(0, true);
    }

    public void OpenTab(string withID)
    {
        OpenTab(withID, false);
    }

    public void OpenTab(string withID, bool ignoreOpenStatus)
    {
        foreach (var w in tabs)
        {
            if (w.id.CompareTo(withID) == 0)
            {
                if (!w.open || ignoreOpenStatus)
                    w.Open();
            }
            else
            {
                if (w.open || ignoreOpenStatus)
                    w.Close();
            }
        }
    }

    public void OpenTab(int withID)
    {
        OpenTab(withID, false);
    }

    public void OpenTab(int withID, bool ignoreOpenStatus)
    {
        if (tabs.Length <= withID)
            return;

        for (int i = 0; i < tabs.Length; i++)
        {
            if (i == withID)
            {
                if (!tabs[i].open || ignoreOpenStatus)
                    tabs[i].Open();
            }
            else
            {
                if (tabs[i].open || ignoreOpenStatus)
                    tabs[i].Close();
            }
        }
    }
}
