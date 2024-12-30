using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class OLD_NetworkManager : MonoBehaviour
{
    public int MAX_SIM;

    //[HideInInspector]
    public List<Sprite> downloadedSprites = new List<Sprite>();
    public List<int> finishedDownloads = new List<int>();
    int _currentSimDL = 0;
    public int currentSimDL
    {
        get
        {
            return _currentSimDL;
        }
    }

    private void Awake()
    {
        MAX_SIM = PlayerPrefs.GetInt("MAX_SIM", 5);
    }

    public void DownloadSprite(string url)
    {
        Debug.Log("Downloading from " + url);
        StartCoroutine(downloadCoroutine(url));
    }

    IEnumerator downloadCoroutine(string url)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        _currentSimDL++;

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            _currentSimDL--;
            finishedDownloads.Add(-1);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            Sprite createdSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));

            downloadedSprites.Add(createdSprite);
            _currentSimDL--;

            finishedDownloads.Add(downloadedSprites.Count - 1);

            //foreach (var s in finishedDownloads)
            //    Debug.Log(s);

            Debug.Log("Finished downloading " + url);
        }
    }
}
