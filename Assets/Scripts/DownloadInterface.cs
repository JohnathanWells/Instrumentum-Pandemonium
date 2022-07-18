using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DownloadInterface : MonoBehaviour
{
    public TMP_InputField input;
    
    public void ReadInput()
    {
        string[] strs = input.text.Split(null);

        StartCoroutine(LoadWaifus(strs));
    }

    IEnumerator LoadWaifus(string[] strs)
    {
        System.DateTime lastTimeRecorded = System.DateTime.UtcNow;

        foreach (string s in strs)
        {

            WaifuManager.Instance.AddQuery(s);

            if ((System.DateTime.UtcNow - lastTimeRecorded).TotalSeconds > Loading.maxFrameTime)
            {
                yield return null;
                lastTimeRecorded = System.DateTime.UtcNow;
            }
        }

        WaifuManager.Instance.DownloadWaifus();
    }
}
