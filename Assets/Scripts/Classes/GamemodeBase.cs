using UnityEngine;

public abstract class GamemodeBase : MonoBehaviour
{
    public abstract bool OnPlayerKilled(PlayerScript player);

    public abstract bool OnTickPassed();
}
