using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class WeaponInventoryScript : MonoBehaviour
{
    public Transform handPivot;
    public Transform cameraPivot;
    public List<WeaponEquipped> weaponInventory = new List<WeaponEquipped>();
    public List<AmmoInventory> ammoInventory = new List<AmmoInventory>();
    public WeaponKey[] inventoryKeys;
    private int _selectedWeapon;



    public WeaponEquipped selectedWeapon
    {
        get
        {
            if (_selectedWeapon < weaponInventory.Count && _selectedWeapon >= 0)
                return weaponInventory[_selectedWeapon];

            return null;
        }
    }

    [System.Serializable]
    public class WeaponKey
    {
        public int index;
        public string inputManagerButtonName;
    }

    [System.Serializable]
    public class AmmoInventory
    {
        public AmmoSO ammoSO;
        public uint _amount = 0;
        public uint amount
        {
            get
            {
                return _amount;
            }
        }
        public UnityAction OnAmmoDepleted;
        public UnityAction<uint> OnAmmoGained;
        public UnityAction<uint> OnAmmoUsed;
        public UnityAction OnAmmoFull;

        public AmmoInventory(uint amount, AmmoSO ammoSO)
        {
            this._amount = amount;
            this.ammoSO = ammoSO;
            Debug.Log(amount);
        }

        public void AddAmmo(uint amt)
        {
            _amount += amt;

            if (_amount > ammoSO.maxAmmo)
                _amount = ammoSO.maxAmmo;

            Debug.Log(amount);
            OnAmmoGained?.Invoke(amt);
        }

        public void SubtractAmmo(uint amt)
        {
            _amount -= amt;
            OnAmmoUsed?.Invoke(amt);

            if (_amount <= 0)
            {
                _amount = 0;
                OnAmmoDepleted?.Invoke();
            }
            Debug.Log(amount);
        }
    }

    public class WeaponEquipped
    {
        public bool owned = true;
        public bool equipped = false;
        public bool empty = false;
        public WeaponSO associatedWeapon;
        public WeaponScriptBase cameraEquippedWeapon;
        public HandWeaponScript handWeapon;

        public UnityAction<WeaponEquipped> OnRemoved;


        public WeaponEquipped(WeaponSO SO, Transform handPivot, Transform cameraPivot, AmmoInventory ammo)
        {
            associatedWeapon = SO;

            cameraEquippedWeapon = Instantiate(SO.cameraWeaponModel, cameraPivot);
            cameraEquippedWeapon.transform.localPosition = Vector3.zero;
            cameraEquippedWeapon.transform.localRotation = Quaternion.identity;

            handWeapon = Instantiate(SO.holdWeaponModel, handPivot);
            handWeapon.transform.localPosition = Vector3.zero;
            handWeapon.transform.localRotation = Quaternion.identity;

            cameraEquippedWeapon.Init(SO, ammo, handWeapon);
        }

        public void UnSelectWeapon()
        {
            equipped = false;
            cameraEquippedWeapon.UnEquipWeapon();
            cameraEquippedWeapon.OnWeaponUnequipped?.Invoke();
        }
        public bool SelectWeapon()
        {
            equipped = true;
            cameraEquippedWeapon.EquipWeapon();
            cameraEquippedWeapon.OnWeaponEquipped?.Invoke();

            return true;
        }

        public void RemoveWeapon()
        {
            OnRemoved.Invoke(this);
            Destroy(cameraEquippedWeapon.gameObject);
            Destroy(handWeapon.gameObject);
        }

        public void MarkAsEmpty()
        {
            empty = true;
        }

        public void UnmarkAsEmpty()
        {
            empty = false;
        }

        //public void AddWeaponAmmo(uint amt)
        //{
        //    cameraEquippedWeapon.AddAmmo(amt);
        //}
        //public void SubtractWeaponAmmo(uint amt)
        //{
        //    cameraEquippedWeapon.SubtractAmmo(amt);
        //}

    }

    public void Update()
    {
        CheckWeaponSelectionInput();
    }

    void CheckWeaponSelectionInput()
    {
        foreach (var k in inventoryKeys)
        {
            if (Input.GetButtonDown(k.inputManagerButtonName))
            {
                Debug.Log("Switching weapons");
                SelectWeapon(k.index);
                return;
            }
        }
    }

    public void AddWeapon(WeaponSO weapon)
    {

        if (weaponInventory.Count <= weapon.weaponInventoryIndex)
        {
            for (int n = weaponInventory.Count; n < weapon.weaponInventoryIndex; n++)
            {
                weaponInventory.Add(null);
            }
        }
        else if (weaponInventory[(int)weapon.weaponInventoryIndex] != null)
        {
            return;
        }

        var newWeapon = new WeaponEquipped(weapon, handPivot, cameraPivot, GetAmmoInventory(weapon.ammoType.type));

        newWeapon.OnRemoved += UnregisterWeaponWithAmmo; 

        weaponInventory.Add(newWeapon);

        RegisterWeaponWithAmmo(newWeapon);

        if (selectedWeapon == null || _selectedWeapon < weapon.weaponInventoryIndex)
        {
            SelectWeapon((int)weapon.weaponInventoryIndex);
        }

            //newWeapon.SelectWeapon();
    }

    public void SelectWeapon(int index)
    {
        if (index == _selectedWeapon)
            return;

        if (index >= weaponInventory.Count || index < 0)
            return;

        if (weaponInventory[index] == null)
            return;

        if (selectedWeapon != null)
        {
            selectedWeapon.UnSelectWeapon();
        }

        _selectedWeapon = index;

        selectedWeapon.SelectWeapon();
        
    }

    public void RemoveWeapon(int index)
    {
        if (index == _selectedWeapon)
        {

        }

        if (index >= weaponInventory.Count || index < 0)
            return;

        var weaponToRemove = weaponInventory[_selectedWeapon];

        if (weaponToRemove == null)
            return;

        selectedWeapon.RemoveWeapon();
        weaponInventory.RemoveAt(index);
    }

    public void SelectNextWeapon(bool withAmmo = false)
    {
        if (weaponInventory.Count == 0)
            return;

        int weaponInd = _selectedWeapon;
        int startingInd = _selectedWeapon;

        do
        {
            weaponInd = (weaponInd + 1) % weaponInventory.Count;

            SelectWeapon(weaponInd);
        } while (startingInd != weaponInd && (weaponInventory[_selectedWeapon] == null || (withAmmo && selectedWeapon.empty)));
    }

    public void SelectPrevWeapon(bool withAmmo = false)
    {

        if (weaponInventory.Count == 0)
            return;

        int weaponInd = _selectedWeapon;
        int startingInd = _selectedWeapon;

        do
        {
            weaponInd = weaponInd - 1;

            if (weaponInd < 0)
                weaponInd = weaponInventory.Count - 1;

            SelectWeapon(weaponInd);
        } while (startingInd != weaponInd && (weaponInventory[_selectedWeapon] == null || (withAmmo && selectedWeapon.empty)));
    }

    public void PickUpWeapon(WeaponSO weapon)
    {
        //var weaponInIndex = AlreadyHasWeapon(weapon);

        PickUpAmmo(weapon.startingAmmo, weapon.ammoType);

        AddWeapon(weapon);

        //else
        //{
        //    weaponInIndex.cameraEquippedWeapon.AddAmmo(weapon.startingAmmo);
        //}
    }

    public void PickUpAmmo(uint amount, AmmoSO ammoSO)
    {

        int ind = ammoInventory.FindIndex(x => x.ammoSO.type == ammoSO.type);

        if (ind < 0)
        {
            ammoInventory.Add(new AmmoInventory(amount, ammoSO));
            return;
        }

        ammoInventory[ind].AddAmmo(amount);

    }

    void RegisterWeaponWithAmmo(WeaponEquipped weapon)
    {

        int ind = ammoInventory.FindIndex(x => x.ammoSO.type == weapon.associatedWeapon.ammoType.type);

        if (ind < 0)
        {
            return;
        }

        ammoInventory[ind].OnAmmoDepleted += weapon.MarkAsEmpty;
        ammoInventory[ind].OnAmmoGained += (x) => weapon.UnmarkAsEmpty();
        //ammoInventory[ind].OnAmmoGained += weapon.AddWeaponAmmo;
        //ammoInventory[ind].OnAmmoUsed += weapon.SubtractWeaponAmmo;
    }

    void UnregisterWeaponWithAmmo(WeaponEquipped weapon)
    {
        int ind = ammoInventory.FindIndex(x => x.ammoSO.type == weapon.associatedWeapon.ammoType.type);

        if (ind < 0)
        {
            return;
        }

        ammoInventory[ind].OnAmmoDepleted -= weapon.MarkAsEmpty;
        ammoInventory[ind].OnAmmoGained -= (x) => weapon.UnmarkAsEmpty();
        //ammoInventory[ind].OnAmmoGained -= weapon.AddWeaponAmmo;
        //ammoInventory[ind].OnAmmoUsed -= weapon.SubtractWeaponAmmo;
    }

    public WeaponEquipped AlreadyHasWeapon(WeaponSO weapon)
    {
        int index = (int)weapon.weaponInventoryIndex;

        if (index == _selectedWeapon)
            return null;

        if (index >= weaponInventory.Count || index < 0)
            return null;

        if (weaponInventory[index] != null && weaponInventory[index].associatedWeapon.weaponID == weapon.weaponID)
            return weaponInventory[index];

        return null;
    }

    public AmmoInventory GetAmmoInventory(AmmoSO.AmmoType type)
    {
        int ind = ammoInventory.FindIndex(x => x.ammoSO.type == type);

        if (ind < 0)
        {
            return null;
        }

        return ammoInventory[ind];
    }
}
