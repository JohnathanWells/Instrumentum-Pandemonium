using UnityEngine;

public class PistolScript : WeaponScriptBase
{
    public string fireInputName = "Fire1";
    public ProjectileScript projectile;
    public ParticleSystem playerMuzzleFlash;
    private ParticleSystem handMuzzleFlash;
    [SerializeField] private Transform muzzle;
    public FirstPersonRigidbodyAnimator animator;

    //public int cooldownLength = 1;

    public override void Init(WeaponSO weapon, WeaponInventoryScript.AmmoInventory ammo, HandWeaponScript handCounterpart, WeaponInventoryScript inventory)
    {
        base.Init(weapon, ammo, handCounterpart, inventory);

        //muzzle = handCounterpart.muzzle;
        handMuzzleFlash = handCounterpart.muzzleFlash;
        animator = inventory.firstPersonAnimator;
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
            animator.ShootAttack();
            var p = Instantiate(projectile, muzzle.position, muzzle.rotation);
            p.dealer = inventory.owner;
            playerMuzzleFlash.Play();
            handMuzzleFlash.Play();

            ammo.SubtractAmmo(1);
        }
    }
}
