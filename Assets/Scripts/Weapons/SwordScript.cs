using UnityEngine;

public class SwordScript : WeaponScriptBase
{
    public string fireInputName = "Fire1";
    public FirstPersonRigidbodyAnimator animator;

    public override void Init(WeaponSO weapon, WeaponInventoryScript.AmmoInventory ammo, HandWeaponScript handCounterpart, WeaponInventoryScript inventory)
    {
        base.Init(weapon, ammo, handCounterpart, inventory);

        animator = inventory.firstPersonAnimator;
    }

    public override void TickAlways()
    {

    }

    public override void TickEquipped()
    {
        if (Input.GetButtonDown(fireInputName))
        {
            Swipe();
        }
    }

    public void Swipe()
    {
        animator.MeleeAttack();
    }
}
