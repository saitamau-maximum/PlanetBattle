# Combat - クラス詳細

戦闘システムを構成するクラス群の詳細説明。HP管理、ダメージ判定、イベント通知の仕組みを記載しています。

---

## 目次

1. [Health](#health) — HP管理・死亡処理
2. [Hitbox](#hitbox) — 敵検出・イベント発火
3. [参考：クラス依存関係](#参考クラス依存関係)

---

## Health

### 役割
キャラクター（プレイヤー・敵）の **HP管理とダメージ受信処理** を担当します。

### 設計方針
- HP の増減と死亡判定をシンプルに実装
- ダメージ受信時にリスナーへ通知（イベント駆動）
- HP ≤ 0 時に自動削除

### クラス定義

```csharp
public class Health : MonoBehaviour
```

### メンバー

#### フィールド

| メンバー | 説明 | 型 | アクセス |
|---------|------|-----|---------|
| `_maxHealth` | 最大HP | float | private (SerializeField) |
| `_currentHealth` | 現在のHP | float | private |

#### プロパティ

| メンバー | 説明 | 型 | 計算式 |
|---------|------|-----|--------|
| `HealthRatio` | HP比率（0.0～1.0） | float | `_currentHealth / _maxHealth` |

#### イベント

```csharp
public event Action<float> OnHealthChanged;
```

**役割** — HP 変動時にリスナーに通知  
**引数** — `HealthRatio`（0.0～1.0、HPバー表示用）

### メソッド

#### private void Awake()

**役割** — HP の初期化。

```csharp
private void Awake()
{
    _currentHealth = _maxHealth;
}
```

**処理内容**
1. 現在のHP を最大HPで初期化
2. `OnHealthChanged` イベント発火（初期表示用）

#### private void OnValidate()

**役割** — Editor 上でHP値が変更された時の更新。

```csharp
private void OnValidate()
{
    if (Application.isPlaying)
    {
        NotifyHealthChanged();
    }
}
```

**用途** — Editor でHPを手動調整したときに HPバーが即座に更新される

#### public void TakeDamage(float amount)

**役割** — ダメージを受けて HP を減少させる。

```csharp
public void TakeDamage(float amount)
{
    if (amount <= 0) return;

    _currentHealth -= amount;
    _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

    NotifyHealthChanged();
    Debug.Log($"{gameObject.name} が{amount}ダメージを受けた");

    if (_currentHealth <= 0)
    {
        Die();
    }
}
```

**処理フロー**
1. `amount ≤ 0` なら何もしない
2. `_currentHealth -= amount`
3. HP を 0～_maxHealth の範囲に制限
4. `OnHealthChanged` イベント発火
5. HP ≤ 0 なら `Die()` 呼び出し

**パラメータ**
- `amount` — ダメージ量（正の値を想定）

**戻り値** — なし

**呼び出し元**
- ContactWeapon.HandleFirstHit()
- Projectile 等のダメージ処理

#### private void NotifyHealthChanged()

**役告** — HP変動をリスナーに通知するヘルパーメソッド。

```csharp
private void NotifyHealthChanged()
{
    OnHealthChanged?.Invoke(HealthRatio);
}
```

**発火タイミング**
- TakeDamage() 後
- Inspector で _maxHealth 変更時（Editor のみ）

#### private void Die()

**役割** — 死亡処理と自動削除。

```csharp
private void Die()
{
    Debug.Log($"{gameObject.name} が倒れました");
    Destroy(gameObject);
}
```

**処理内容**
1. ログ出力
2. GameObject を自動削除

**重要** — 敵が倒れたらそのGameObject自体が削除される

### ダメージフロー図

```
TakeDamage(amount)
    ├─ amount ≤ 0？
    │   └─ Yes → return（何もしない）
    │   └─ No  ↓
    ├─ _currentHealth -= amount
    ├─ Clamp(0, _maxHealth)
    │
    ├─ NotifyHealthChanged()
    │   ( OnHealthChanged.Invoke(HealthRatio) )
    │   └─ HPバー等が更新される
    │
    └─ _currentHealth ≤ 0？
        └─ Yes → Die()
                 ├─ Debug.Log()
                 └─ Destroy(gameObject)
        └─ No  → (生存継続)
```

### 使用例

#### 1. HPバーの表示更新

```csharp
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _health.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        _health.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float healthRatio)
    {
        _healthBarImage.fillAmount = healthRatio;
    }
}
```

#### 2. ダメージ受信

```csharp
Health health = enemyHealthComponent;
health.TakeDamage(25f);  // 25ダメージ
```

---

## Hitbox

### 役割
**敵検出と判定処理**。OnTriggerEnter2D、OnTriggerStay2D で敵との接触を検知し、イベント通知します。

### 設計方針
- 物理判定に専念（敵を知らない）
- Weapon が イベントをSubscribe してダメージ処理
- 単体 / 範囲攻撃に対応

### クラス定義

```csharp
public class Hitbox : MonoBehaviour
```

### メンバー

#### フィールド

| メンバー | 説明 | 型 | アクセス |
|---------|------|-----|---------|
| `_isAreaAttack` | 範囲攻撃フラグ | bool | private (SerializeField) |
| `_hitTarget` | 単体攻撃時の対象敵 | Health | private |

#### イベント

```csharp
public Action<Health> OnFirstHit;
public Action<Health> OnContinuousHit;
```

**OnFirstHit** — 最初の接触時に発火  
**OnContinuousHit** — 接触中、物理更新ごと発火

### メソッド

#### private void OnEnable()

**役割** — コンポーネント有効時に _hitTarget をリセット。

```csharp
private void OnEnable()
{
    _hitTarget = null;
}
```

**用途** — 同じ敵に複数回ダメージを与えるとき用

#### private void OnTriggerEnter2D(Collider2D collision)

**役割** — 敵との接触検出

```csharp
private void OnTriggerEnter2D(Collider2D collision)
{
    if (!collision.TryGetComponent(out Health health)) return;

    if (!_isAreaAttack && _hitTarget != null) return;

    _hitTarget = health;
    OnFirstHit?.Invoke(health);
}
```

**処理フロー**
1. `collision` から `Health` 取得できるか確認
2. 取得失敗なら return
3. 単体攻撃かつ既にヒット中 → return
4. `_hitTarget = health` で対象を記録
5. `OnFirstHit.Invoke(health)` でイベント発火

**オーバーロード条件**
| `_isAreaAttack` | `_hitTarget` | 結果 |
|--------|--------|------|
| false | null | ✅ 発火 |
| false | registered | ❌ スキップ（既にヒット中） |
| true | - | ✅ 発火 |

#### private void OnTriggerStay2D(Collider2D collision)

**役割** — 敵との接触中（毎物理更新）。

```csharp
private void OnTriggerStay2D(Collider2D collision)
{
    if (!collision.TryGetComponent(out Health health)) return;

    if (!_isAreaAttack && _hitTarget != health) return;

    OnContinuousHit?.Invoke(health);
}
```

**処理フロー**
1. `collision` から `Health` 取得
2. 取得失敗なら return
3. 単体攻撃かつ別の敵 → return
4. `OnContinuousHit.Invoke(health)` でイベント発火

**毎フレーム呼ばれるため、以下に使用予定**
- DoT（Damage over Time）ダメージ
- 状態異常の継続処理
- ノックバック適用

#### private void OnTriggerExit2D(Collider2D collision)

**役割** — 敵がHitbox から離脱時。

```csharp
private void OnTriggerExit2D(Collider2D collision)
{
    if (!collision.TryGetComponent(out Health health)) return;

    if (_hitTarget == health)
        _hitTarget = null;
}
```

**処理内容**
- 現在の対象敵なら `_hitTarget = null` でリセット

### 単体 vs 範囲攻撃

#### 単体攻撃（`_isAreaAttack = false`）

```
Sword → OnTriggerEnter2D → _hitTarget = Enemy1
                           ↓
                    OnFirstHit 発火
        
        複数敵が接触しても、_hitTarget == null 時のみ更新
        （一体にのみダメージ）
```

#### 範囲攻撃（`_isAreaAttack = true`）

```
Explosion → OnTriggerEnter2D → 複数敵全てに OnFirstHit 発火
                               （_isAreaAttack なのでスキップ判定なし）
```

### セットアップ例

```
Sword (GameObject)
├── ContactWeapon (Component)
├── SpriteRenderer
├── Animator
└── HitboxCollider (子オブジェクト)
    ├── Hitbox (Component)
    ├── BoxCollider2D (Trigger)
    └── _isAreaAttack = false
```

### 使用例

```csharp
Hitbox hitbox = GetComponent<Hitbox>();

// Weapon が購読
private void Start()
{
    hitbox.OnFirstHit += HandleFirstHit;
    hitbox.OnContinuousHit += HandleContinuousHit;
}

private void HandleFirstHit(Health targetHealth)
{
    targetHealth.TakeDamage(_weaponData.DamageAmount);
}

private void HandleContinuousHit(Health targetHealth)
{
    // DoT ダメージ等を処理
}
```

---

## 参考：クラス依存関係

```
Hitbox (判定コンポーネント)
  ├─ OnFirstHit イベント
  ├─ OnContinuousHit イベント
  └─ Health を受け取る
      │
      ↓ イベント購読
      │
ContactWeapon (武器)
  ├─ HandleFirstHit()
  ├─ HandleContinuousHit()
  └─ Health.TakeDamage()
      │
      ↓
      Health (HP管理)
      ├─ _currentHealth を減少
      ├─ OnHealthChanged イベント発火
      └─ HP ≤ 0 → Die() → Destroy
```

---

