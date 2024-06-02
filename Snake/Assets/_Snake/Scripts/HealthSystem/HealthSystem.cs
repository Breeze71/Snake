using System;

public class HealthSystem
{
    public event Action HealthChangedEvent;

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

        if(health <= 0) health = 0;
        if(HealthChangedEvent != null) HealthChangedEvent?.Invoke();
    }

    public void Heal(int _healthAmount)
    {
        health += _healthAmount;

        if(health >= healthMax) health = healthMax;
        if(HealthChangedEvent != null) HealthChangedEvent?.Invoke();
    }
}
