using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    public int _health = 100;
    public int health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = value;
            OnHealthChanged?.Invoke(_health);
        }
    }
    public int maxHealth = 100;

    public UnityAction<int, IEntity> OnDamaged;
    public UnityAction<int, IEntity> OnHealed;
    public UnityAction<int> OnHealthChanged;
    public UnityAction OnSpawn;
    public UnityAction<int, IEntity, string> OnDie;

    public void Damage(int amt, WeaponScriptBase weapon, IEntity dealer = null)
    {
        health -= amt;

        if (health <= 0)
        {
            if (weapon == null)
                OnDie?.Invoke(-health, dealer, WeaponSO.defaultKillVerb);
            else
                OnDie?.Invoke(-health, dealer, weapon.weaponSO.GetKillVerb());

            health = 0;
        }
    }

    public void Heal(int amt, IEntity healer = null, bool ignoreMaxHealth = false)
    {
        int amtHealed = amt;
        health += amt;

        if (!ignoreMaxHealth && health > maxHealth)
        {
            amtHealed -= (health - maxHealth);
            health = maxHealth;
        }

        OnHealed?.Invoke(amtHealed, healer);
    }

    public void Kill()
    {
        Damage(health, null);
    }
}
