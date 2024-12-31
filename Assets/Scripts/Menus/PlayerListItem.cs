using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviour
{
    public string _playerName;
    public string playerName
    {
        set
        {
            _playerName = value;
            playerNameDisplay.text = _playerName;
        }
        get
        {
            return _playerName;
        }
    }
    private int _playerLives;
    public int playerLives
    {
        set
        {
            _playerLives = value;
            playerLivesDisplay.text = _playerLives.ToString();
        }
        get
        {
            return _playerLives;
        }
    }
    public TextMeshProUGUI playerNameDisplay;
    public TextMeshProUGUI playerLivesDisplay;
    public int userID;
}
