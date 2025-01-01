using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Collections;

public class PlayerListItem : MonoBehaviour
{
    public FixedString64Bytes _playerName;
    public FixedString64Bytes playerName
    {
        set
        {
            _playerName = value;
            playerNameDisplay.text = _playerName.ToString();
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
