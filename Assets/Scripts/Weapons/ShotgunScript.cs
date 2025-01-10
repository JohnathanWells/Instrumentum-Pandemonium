using UnityEngine;

public class ShotgunScript : WeaponScriptBase
{
    public string fireInputName = "Fire1";
    //public Transform projectile;
    public ParticleSystem playerMuzzleFlash;
    public Transform muzzle;
    public ShotgunProjectileScript projectiles;
    private ParticleSystem handMuzzleFlash;
    //[SerializeField] private Transform muzzle;

    //public int cooldownLength = 1;

    public override void Init(WeaponSO weapon, WeaponInventoryScript.AmmoInventory ammo, HandWeaponScript handCounterpart, WeaponInventoryScript inventory)
    {
        base.Init(weapon, ammo, handCounterpart, inventory);

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
            //Instantiate(projectile, muzzle.position, muzzle.rotation);
            //projectiles.Play();
            var proj = Instantiate(projectiles, muzzle.position, muzzle.rotation);
            proj.dealer = inventory.owner;
            playerMuzzleFlash.Play();
            handMuzzleFlash.Play();

            ammo.SubtractAmmo(1);
        }
    }
}
