using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponScriptBase : MonoBehaviour
{
    public bool Equipped = false;

    public WeaponSO weaponSO;

    public WeaponInventoryScript.AmmoInventory ammo;

    public WeaponInventoryScript inventory;

    public HandWeaponScript handCounterpart;

    public UnityAction OnWeaponEquipped;
    public UnityAction OnWeaponUnequipped;
    public UnityAction OnWeaponAdquired;

    public GameObject thisWeaponParent;

    public virtual void Init(WeaponSO weapon, WeaponInventoryScript.AmmoInventory ammoReserve, HandWeaponScript handScript, WeaponInventoryScript inventory)
    {
        weaponSO = weapon;
        ammo = ammoReserve;
        handCounterpart = handScript;
        this.inventory = inventory;
    }

    public void UnEquipWeapon()
    {
        thisWeaponParent.SetActive(false);
        Equipped = false;
    }

    public void EquipWeapon()
    {
        thisWeaponParent.SetActive(true);
        Equipped = true;
    }


    //public abstract void AddAmmo(uint amount);
    //public abstract void SubtractAmmo(uint amount);
    public abstract void TickEquipped();
    public abstract void TickAlways();

    public void Update()
    {
        if (Equipped)
        {
            TickEquipped();
        }

        TickAlways();
    }
}
