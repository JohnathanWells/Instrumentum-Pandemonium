using UnityEngine;

public class WeaponPickupScript : PickupScriptBase
{
    public WeaponSO weaponSO;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        var wM = Instantiate(weaponSO.pickupModel, pickableParent);
        wM.localPosition = Vector3.zero;
        wM.localRotation = Quaternion.identity;
        wM.localScale = Vector3.one;
    }

    public override void OnPickup(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Picking up " + weaponSO.weaponName);
            var playerInventory = other.gameObject.GetComponentInChildren<WeaponInventoryScript>();

            if (playerInventory != null)
            {
                playerInventory.PickUpWeapon(weaponSO);
                PickUp();
            }
        }
    }

    public override void ResetPickup()
    {
        pickableParent.gameObject.SetActive(true);
        isPickedUp = false;
    }

    public override void PickUp()
    {
        pickableParent.gameObject.SetActive(true);
        isPickedUp = false;

        StartCoroutine(StartResetTimer());
    }
}
