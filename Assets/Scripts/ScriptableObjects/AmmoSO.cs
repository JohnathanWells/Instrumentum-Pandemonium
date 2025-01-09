using UnityEngine;

[CreateAssetMenu(fileName = "AmmoSO", menuName = "ScriptableObjects/AmmoSO")]
public class AmmoSO : ScriptableObject
{
    public enum AmmoType { Pistol, Shotgun, Rifle, Sniper };
    public AmmoType type;
    public uint maxAmmo;
}
