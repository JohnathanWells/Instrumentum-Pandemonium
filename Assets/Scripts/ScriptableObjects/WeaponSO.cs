using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public string weaponID;
    public uint weaponInventoryIndex;
    public HandWeaponScript holdWeaponModel;
    public WeaponScriptBase cameraWeaponModel;
    public Transform pickupModel;

    public uint startingAmmo;
    public AmmoSO ammoType;
}
