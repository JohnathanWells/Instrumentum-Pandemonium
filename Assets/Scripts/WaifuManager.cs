using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaifuManager : MonoBehaviour
{
    public Pawn pawnPrefab;
    public List<Pawn> spawnedPawns = new List<Pawn>();

    public NetworkManager downloadManager;
    Queue<string> queryList = new Queue<string>();

    public static WaifuManager Instance;
    int lastCheckedOut = 0;

    public void Awake()
    {
        Instance = this;
    }

    public void AddQuery(string url)
    {
        queryList.Enqueue(url);
        Debug.Log(url + " added to queue.");
    }

    public void DownloadWaifus()
    {
        StartCoroutine(DownloadQueue());
    }

    IEnumerator DownloadQueue()
    {
        Pawn temp;
        System.DateTime lastTimeRecorded = System.DateTime.UtcNow;
        int queryAttemptsLeft = queryList.Count;

        while (queryList.Count > 0)
        {
            while (downloadManager.currentSimDL >= downloadManager.MAX_SIM || (System.DateTime.UtcNow - lastTimeRecorded).TotalSeconds > Loading.maxFrameTime)
            {
                yield return null;
                lastTimeRecorded = System.DateTime.UtcNow;
            }

            downloadManager.DownloadSprite(queryList.Dequeue());
        }

        while (queryAttemptsLeft > 0)
        {
            if (lastCheckedOut == downloadManager.finishedDownloads.Count)
            {
                if ((System.DateTime.UtcNow - lastTimeRecorded).TotalSeconds > 10)
                {
                    Debug.Log("Download forced to quit (taking too long).");
                    break;
                }
                else
                {
                    yield return null;
                }
            }
            else if ((System.DateTime.UtcNow - lastTimeRecorded).TotalSeconds > Loading.maxFrameTime)
            {
                yield return null;
                lastTimeRecorded = System.DateTime.UtcNow;
            }

            //Debug.Log(string.Format("pawnsLeft: {1}\ncurrentFinished: {2}\ncurrentFinishedCount: {3}",
            //    0, queryAttemptsLeft, lastCheckedOut, downloadManager.finishedDownloads.Count));

            for (; lastCheckedOut < downloadManager.finishedDownloads.Count; lastCheckedOut++)
            {
                if ((System.DateTime.UtcNow - lastTimeRecorded).TotalSeconds > Loading.maxFrameTime)
                {
                    yield return null;
                    lastTimeRecorded = System.DateTime.UtcNow;
                }

                int dequeuedElement = downloadManager.finishedDownloads[lastCheckedOut]; 
                //Debug.Log("Dequeued " + dequeuedElement);

                if (dequeuedElement >= 0)
                {                 

                    temp = Instantiate(pawnPrefab.transform).GetComponent<Pawn>();
                    spawnedPawns.Add(temp);

                    temp.Initialize(downloadManager.downloadedSprites[dequeuedElement]);

                    //For demonstration purposes only
                    temp.transform.position = Vector3.right * (spawnedPawns.Count - 1) * 1.5f; 
                }

                queryAttemptsLeft--;
            }
        }
    }
}
