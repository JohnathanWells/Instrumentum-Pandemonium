using UnityEngine;

public class PistolScript : WeaponScriptBase
{
    public string fireInputName = "Fire1";
    public Transform projectile;
    public ParticleSystem playerMuzzleFlash;
    private ParticleSystem handMuzzleFlash;
    [SerializeField] private Transform muzzle;

    //public int cooldownLength = 1;

    public override void Init(WeaponSO weapon, WeaponInventoryScript.AmmoInventory ammo, HandWeaponScript handCounterpart)
    {
        base.Init(weapon, ammo, handCounterpart);

        //muzzle = handCounterpart.muzzle;
        handMuzzleFlash = handCounterpart.muzzleFlash;
    }

    public override void TickAlways()
    {
        
    }

    public override void TickEquipped()
    {
        if (Input.GetButtonDown(fireInputName))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (ammo.amount > 0)
        {
            Instantiate(projectile, muzzle.position, muzzle.rotation);
            playerMuzzleFlash.Play();
            handMuzzleFlash.Play();

            ammo.SubtractAmmo(1);
        }
    }
}
