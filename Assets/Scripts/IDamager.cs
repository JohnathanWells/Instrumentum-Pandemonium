using UnityEngine;

public interface IDamager
{
    public IEntity dealer { get; set; }
    public int minDamage { get; set; }
    public int maxDamage { get; set; }

}
