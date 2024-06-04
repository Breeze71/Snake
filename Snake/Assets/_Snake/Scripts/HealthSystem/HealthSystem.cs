using System;

public class HealthSystem
{
    public event Action<int> HealthChangedEvent;

    private int health;
    private int healthMax;

    public HealthSystem(int _healthMax)
    {
        healthMax = _healthMax;
        health = healthMax;
    }

    public int GetHealthAmount()
    {
        return health;
    }
    
    public float GetHealthPercent()
    {
        return (float)health / healthMax;
    }

    public void TakeDamage(int _damageAmount)
    {
        health -= _damageAmount;

        if(health <= 0)
        {
            health = 0;
        }

        HealthChangedEvent?.Invoke(health);
    }

    public void Heal(int _healthAmount)
    {
        health += _healthAmount;

        if(health >= healthMax) health = healthMax;

        HealthChangedEvent?.Invoke(health);
    }

    public void HealNoLimit(int _healthAmount)
    {
        health += _healthAmount;

        HealthChangedEvent?.Invoke(health);        
    }
}
