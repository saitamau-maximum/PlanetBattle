using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    [Header("HP Bar")]
    [SerializeField] private Transform hpBarFillRoot;

    // インスペクターで値が変更されたときに呼ばれる
    private void OnValidate()
    {
        // ゲーム実行中のみ、HPバーの表示を更新する
        if (Application.isPlaying && hpBarFillRoot != null)
        {
            // 範囲外の数値にならないよう制限
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            UpdateHPBar();
        }
    }

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
            // 0除算を防ぐため maxHP が 0 より大きいか確認
            float ratio = maxHP > 0 ? (float)currentHP / maxHP : 0;
            hpBarFillRoot.localScale = new Vector3(ratio, 1f, 1f);
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} が倒れました");
        Destroy(gameObject);
    }
}