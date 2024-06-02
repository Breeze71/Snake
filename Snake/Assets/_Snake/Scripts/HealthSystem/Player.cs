using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    [field: SerializeField] public int healthAmount {get; set; }
    [field: SerializeField]public HealthBarUI healthBarUI { get; set; }
    public HealthSystem HealthSystem { get; set; }

    private void Start() 
    {
        HealthSystem = new HealthSystem(healthAmount);

        SetupHealthSystemUI();
    }

    public void TakeDamage(int _damageAmount)
    {
        HealthSystem.TakeDamage(_damageAmount);

        if(HealthSystem.GetHealthAmount() == 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetupHealthSystemUI()
    {
        healthBarUI.SetupHealthSystemUI(HealthSystem);
    }
    public Transform GetTransform()
    {
        return transform;
    }

}
