using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public string weaponID;
    public Transform weapon;
    public Transform pickupModel;

    public int maxAmmo;
    public float rateOfFire;
}
