using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MapDetails", menuName = "ScriptableObjects/MapDetails")]
public class MapScriptableObject : ScriptableObject
{
    public string mapName;
    public string sceneName;
    public Sprite thumbnail;
}
