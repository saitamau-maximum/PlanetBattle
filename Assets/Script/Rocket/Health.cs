using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    [Header("HP Bar")]
    [SerializeField] private Transform hpBarFillRoot;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateHPBar()
    {
        if (hpBarFillRoot != null)
        {
            float ratio = (float)currentHP / maxHP;
            hpBarFillRoot.localScale = new Vector3(ratio, 1f, 1f);
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} が倒れました");
        Destroy(gameObject);
    }
}
