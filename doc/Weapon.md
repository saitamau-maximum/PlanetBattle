# 武器システム
## クラス詳細
- [Weapon関連](../Assets/Script/Weapon/README.md)
- [Hitbox/Health関連](../Assets/Script/Combat/README.md)

## 全体フロー

### Playerの攻撃実行フロー（入力から攻撃までの流れ）
```
┌──────────────────────────────────────────┐
│   PlayerController                       │
│  ・入力を検出                              │
└────────┬─────────────────────────────────┘
         │
         │ 攻撃入力（例：左クリック）
         ▼
┌──────────────────────────────────────────┐
│   PlayerWeaponManager                    │
│  • 装備中の武器を保持                       │
│  • 入力を受けて現在の武器を呼び出し 　　 　   　│
│  • 複数武器の切り替え管理                    │
└────────┬─────────────────────────────────┘
         │
         │ TryUsePrimaryWeapon(),TryUseSecondaryWeapon()で武器を呼び出し
         ▼
┌──────────────────────────────────────────┐
│   WeaponBase (基底クラス)                  │
│  • 攻撃フロー管理                         　│
│    (Idle → Attacking → CoolingDown)      │
│  • 状態チェック（攻撃可能か判定）             │
│  • クールタイム管理                         │
└──────────────────────────────────────────┘

```

---

### ダメージ発生フロー（敵への接触からダメージまでの流れ）

```
┌────────────────────────────────────────┐
│   敵がHitboxに接触                     │
│  (OnTrigger)                          │
└────────┬───────────────────────────────┘
         │
         ▼
┌────────────────────────────────────────┐
│   Hitbox (子オブジェクト)               │
│  【責務】敵検出とイベント発火            │
│  • OnTriggerEnter2D() で敵を検出       │
│  • Health コンポーネント取得            │
│  • 単体/範囲攻撃の判定                  │
│  → OnFirstHit.Invoke(Health)          │
│  → OnContinuousHit.Invoke(Health)          │
└────────┬────────────────────────────────┘
         │
         │ イベント発火
         ▼
┌────────────────────────────────────────┐
│   派生クラス（例: ContactWeapon）      │
│  ダメージ量決定と適用                   │
│  • OnFirstHit イベントをSubscribe     │
│  • HandleFirstHit() でダメージ処理        │
│  → Health.TakeDamage(damage) 呼び出し │
└────────┬────────────────────────────────┘
         │
         ▼
┌────────────────────────────────────────┐
│   Health (敵のコンポーネント)           │
│  【責務】HP管理と死亡処理               │
│  • 現在のHPを管理                       │
│  • ダメージを受けてHP減少               │
│  • OnHealthChanged イベント発火         │
│  • HP ≤ 0 で死亡処理実行              │
└────────────────────────────────────────┘
```

---

## 各クラスの役割

| 役割 | クラス | 具体的な役割 |
|------|--------|-----------|
| **入力** | PlayerController | プレイヤー入力の検出。武器管理は関知しない |
| **武器管理** | PlayerWeaponManager | 装備中の武器を管理。入力と武器の仲介役 |
| **攻撃フロー** | WeaponBase(基底クラス) | 状態管理（Idle・Attacking・CoolingDown）・クールタイム |
| **攻撃実行** | ContactWeapon(派生クラス) | Hitbox購読・ダメージ処理・アニメーション実行 |
| **判定検出** | Hitbox | 物理判定のみ。**誰** に当たったか通知する |
| **体力管理** | Health | HPを管理。ダメージを受けて減少、死亡判定 |

---

## 武器の追加方法(Script)

### 武器タイプ別の実装例(簡略)

#### 1. ContactWeapon（既存）
剣、斧など、武器自体が敵を攻撃するもの。(**Hitbox を使用してダメージ判定**)

```csharp
public class ContactWeapon : WeaponBase
{
    private Animator _animator;
    private SpriteRenderer _renderer;
    private Hitbox[] _hitboxes;
    readonly int _hashAttack = Animator.StringToHash("Attack");

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _hitboxes = GetComponentsInChildren<Hitbox>(true);
    }

    private void Start()
    {
        // Hitbox イベント購読
        foreach (var hitbox in _hitboxes)
        {
            hitbox.OnFirstHit += HandleFirstHit;
            hitbox.OnContinuousHit += HandleContinuousHit;
        }
    }

    private void OnDestroy()
    {
        // Hitbox イベント購読解除
        foreach (var hitbox in _hitboxes)
        {
            hitbox.OnFirstHit -= HandleFirstHit;
            hitbox.OnContinuousHit -= HandleContinuousHit;
        }
    }

    private void HandleFirstHit(Health targetHealth)
    {
        targetHealth.TakeDamage(_weaponData.DamageAmount);
    }

    private void HandleContinuousHit(Health targetHealth)
    {
        // TODO: 特殊効果付与
    }

    protected override IEnumerator AttackCoroutine()
    {
        //アニメーションの再生
        _animator.SetTrigger(_hashAttack);
        float attackDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(attackDuration);
    }
}
```

#### 2. ProjectileWeapon
銃、弓など、プロジェクタイルを発射する武器。(**Hitbox を使用しない**,ダメージは弾側で処理)

```csharp
public class ProjectileWeapon : WeaponBase
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _muzzle;
    private SpriteRenderer _renderer;

    protected override IEnumerator AttackCoroutine()
    {
        // 弾を発射
        // (Projectile が敵にぶつかってダメージを処理)
        Instantiate(_projectilePrefab, _muzzle.position, Quaternion.identity);
        
        // 発射アニメーション等
        yield return new WaitForSeconds(0.1f);
    }
}
```

**重要：** ProjectileWeapon には Hitbox がないため、ダメージ処理は**弾のコンポーネント**で行う

---

### 実装チェックリスト

#### 📋 共通（全ての武器）
- [ ] `WeaponBase` を継承
- [ ] `AttackCoroutine()` を実装（攻撃演出を定義）
- [ ] Inspector で **WeaponData をアサイン**
  - [ ] WeaponName（武器名）
  - [ ] DamageAmount（ダメージ量）
  - [ ] CoolTime（クールタイム）

#### 🗡️ Hitboxを使う場合
- [ ] **Hitbox コンポーネント** を子オブジェクトに配置
  - [ ] BoxCollider2D または CircleCollider2D を設定
  - [ ] **Is Trigger = true** に設定
  - [ ] _isAreaAttack フラグを設定（単体 or 範囲）
- [ ] Start() で Hitbox イベントを購読
- [ ] OnDestroy() で購読解除
- [ ] HandleFirstHit(),HandleContinuousHit() でダメージ処理

---

## ゲーム内セットアップ方法

### 武器の追加(Prefab)
#### 🗡️ ContactWeapon(Swordを参考に)

1. **武器用GameObjectを作成**
   - 親: 新しく作成
   - 子: Hitbox（BoxCollider2D / CircleCollider2D　**Is Trigger = true**）

2. **コンポーネント設定**
   - 親に `ContactWeapon` をアタッチ
   - `WeaponData` の値を設定（Inspector）

3. **当たり判定の設定**
   - 子オブジェクトに `Hitbox.cs` をアタッチ
   - コライダーを Trigger モードに設定
   - _isAreaAttack フラグを設定

4. **Animationの設定**
   - 武器にAnimatorをアタッチ
   - AnimationClip内でHitboxの有効化、無効化を制御 

#### 🔫 ProjectileWeapon(これから作る予定)

1. **武器用GameObjectを作成**
   - Hitbox 不要（弾が敵にぶつかってダメージ処理）

2. **コンポーネント設定**
   - `ProjectileWeapon` をアタッチ
   - `WeaponData` の値を設定（Inspector）
   - _projectilePrefab を割り当てる
   - _muzzle （発射位置）を設定

3. **Projectile Prefab を用意**
   - ダメージ処理ロジックを実装
   - Health.TakeDamage() を呼び出し

### 敵などのキャラクターに武器を持たせる(Flogを参考に)
- 武器を子オブジェクトにする
- `CharacterAIController`のWeaponに設定(Inspector)
