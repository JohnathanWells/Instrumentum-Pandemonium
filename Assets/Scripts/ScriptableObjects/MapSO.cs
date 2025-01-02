using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MapDetails", menuName = "ScriptableObjects/MapDetails")]
public class MapSO : ScriptableObject
{
    [SerializeField] public bool hidden = false;
    [SerializeField] public string mapID;
    [SerializeField] public string mapName;
    [SerializeField] public string sceneName;
    [SerializeField] public Sprite thumbnail;
    [TextArea, SerializeField] public string mapDescription;
    [SerializeField] public GamemodeSO[] validGamemodes; 
}
