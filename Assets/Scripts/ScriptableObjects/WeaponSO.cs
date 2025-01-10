using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public string weaponID;
    public uint weaponInventoryIndex;
    public int priority = 0;
    public HandWeaponScript holdWeaponModel;
    public WeaponScriptBase cameraWeaponModel;
    public Transform pickupModel;
    public string[] killVerbs;
    public static string defaultKillVerb = "killed";

    public uint startingAmmo;
    public AmmoSO ammoType;

    public string GetKillVerb()
    {
        if (killVerbs.Length == 0)
            return defaultKillVerb;

        return killVerbs[Random.Range(0, killVerbs.Length)];
    }
}
