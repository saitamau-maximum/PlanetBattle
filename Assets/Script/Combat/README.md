# Hitbox
- 当たり判定を検出し、対象（Health）を通知するコンポーネント。  

## 概要
Hitboxは接触した対象に対して以下のイベントを発行する
- OnFirstHit：最初の接触時(OnTriggerEnter2D)
- OnContinuousHit：接触中（OnTriggerStay2D）

## モード
### 単体攻撃（_isAreaAttack = false）
- 最初に接触した1体のみを対象とする
- 対象はコンポーネントが有効化されるごとにリセットする

### 範囲攻撃（_isAreaAttack = true）
- 接触したすべてを対象とする

## イベント
### OnFirstHit
最初に接触したとき呼ばれる。

### OnContinuousHit
接触している間、呼ばれる。

## 使用例
```csharp
using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _damageAmount;
    protected Hitbox _hitbox;

    private void Awake()
    {
        _hitbox = GetComponent<Hitbox>();
    }

    private void Start()
    {
        _hitbox.OnFirstHit += DealDamage;
    }

    private void OnDestroy()
    {
        _hitbox.OnFirstHit -= DealDamage;
    }

    private void DealDamage(Health targetHealth)
    {
        targetHealth.TakeDamage(_damageAmount);
    }
}
```

---
