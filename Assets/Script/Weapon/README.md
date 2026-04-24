# Weapon - クラス詳細

武器クラス群の詳細説明。各クラスの責務、実装方法、使用例を記載しています。

---

## 目次

1. [WeaponData](#weapondata) — 武器パラメータ管理
2. [WeaponBase](#weaponbase抽象基底クラス) — 攻撃フロー管理・状態制御
3. [ContactWeapon](#contactweapon) — 接触判定型武器の実装
4. [ProjectileWeapon](#projectileweapon) — 投射物型武器の実装
5. [BurstProjectileWeapon](#burstprojectileweapon) — 連射投射物型武器の実装
6. [参考：クラス依存関係](#参考クラス依存関係)

---

## WeaponData

### 役割
武器パラメータを保持する **シリアライズ可能なデータクラス**。

### 設計方針
- Serializable で提供
- 各武器インスタンスに紐付ける
- Inspector で編集可能

### クラス定義

```csharp
[Serializable]
public class WeaponData
```

### プロパティ

| メンバー | 説明 | 型 | デフォルト | 用途 |
|---------|------|-----|-----------|------|
| `WeaponName` | 武器の名前 | string | - | アニメーション・デバッグ用 |
| `DamageAmount` | 1回の攻撃ダメージ量 | float | - | Health.TakeDamage() の引数 |
| `CoolTime` | 攻撃後のクールタイム（秒） | float | - | WeaponBase の状態遷移 |

### Inspector での設定

```
WeaponBase
├── Weapon Data (WeaponData)
│   ├── Weapon Name:     "Iron Sword"
│   ├── Damage Amount:   25.0
│   └── Cool Time:       0.8
```

---

## WeaponBase（抽象基底クラス）

### 役割
全ての武器の基底となるクラス。**攻撃フロー管理とクールタイム制御** を担当します。

### 設計方針
- 状態管理のみに責務を限定
- 派生クラスで具体的な攻撃処理（演出・ダメージ）を実装

### クラス定義

```csharp
public abstract class WeaponBase : MonoBehaviour
```

### メンバー

#### 定数（Enum）
```csharp
public enum WeaponState
{
    Idle,        // 待機状態。攻撃可能
    Attacking,   // 攻撃中。演出実行中
    CoolingDown  // クールタイム中。再攻撃不可
}
```

#### プロパティ

| メンバー | 説明 | 型 | アクセス |
|---------|------|-----|---------|
| `_weaponData` | 武器パラメータ（ダメージ量等）| WeaponData | protected |
| `_renderer` | スプライト描画用 | SpriteRenderer | private |
| `CurrentState` | 現在の武器状態 | WeaponState | public {get;} |
| `WeaponName` | 武器名（WeaponData から） | string | public |

### メソッド

#### public bool TryUseWeapon()

**役割** — 武器を使用できるか判定し、使用チェック。

**処理フロー**
1. 現在の状態が Idle か確認
2. Idle 以外なら **false を返す**（攻撃不可）
3. Idle なら `AttackAndCooldown()` コルーチンを開始
4. **true を返す**（攻撃開始）

**戻り値**
- `true` — 攻撃を開始した
- `false` — 攻撃不可（Attacking/CoolingDown 状態）

**使用例**
```csharp
if (weaponBase.TryUseWeapon())
{
    Debug.Log("攻撃開始");
}
else
{
    Debug.Log("攻撃中またはクールタイム中");
}
```

#### private IEnumerator AttackAndCooldown()

**役割** — 攻撃と クールタイム の状態遷移を管理。

**処理フロー**
```
1. CurrentState = Attacking
2. AttackCoroutine() 実行（派生クラスで実装）
3. 演出終了まで待機
4. CurrentState = CoolingDown
5. _weaponData.CoolTime 秒待機
6. CurrentState = Idle
```

#### protected abstract IEnumerator AttackCoroutine()

**役割** — **派生クラスで実装する抽象メソッド**。具体的な攻撃演出を定義。

**実装例**
```csharp
// ContactWeapon から
protected override IEnumerator AttackCoroutine()
{
    _animator.SetTrigger(_hashAttack);
    float duration = _animator.GetCurrentAnimatorStateInfo(0).length;
    yield return new WaitForSeconds(duration);
}
```

**実装時の注意**
- 攻撃完了まで `yield return` で待機すること
- ダメージ処理はここでなく、Hitbox イベントで処理

#### public virtual void Equip()

**役割** — 武器を装備した時の処理（描画表示）。

**デフォルト実装**
```csharp
public virtual void Equip()
{
    _renderer.enabled = true;
}
```

#### public virtual void Unequip()

**役割** — 武器を外した時の処理（描画非表示）。

**デフォルト実装**
```csharp
public virtual void Unequip()
{
    _renderer.enabled = false;
}
```

### 派生クラスの実装ルール

派生クラスで **必ず実装すべきメソッド**

| メソッド | 必須 | 説明 |
|---------|------|------|
| `AttackCoroutine()` | ✅ | 攻撃演出の定義 |
| `Equip()` | △ | 装備時処理（必要に応じて） |
| `Unequip()` | △ | 外す時処理（必要に応じて） |

---

## ContactWeapon

### 役割
**接触判定型の武器**。Hitbox コンポーネントと連携してダメージを処理します。  
例：剣、ハンマー等のメレー武器。スパイクなどの接触武器

### 設計方針
- Hitbox イベントを購読してダメージ処理
- アニメーションで攻撃表現
- 敵に直接接触してダメージ

### プロパティ

| メンバー | 説明 | 型 | 役割 |
|---------|------|-----|------|
| `_animator` | アニメーション制御 | Animator | 攻撃演出実行 |
| `_hitboxes[]` | ダメージ判定群 | Hitbox[] | 敵との接触検出 |
| `_hashAttack` | "Attack" パラメータのハッシュ | int | アニメーション制御 |

### メソッド

#### protected override void Awake()

**役割** — コンポーネント及びHitbox の初期化。

```csharp
protected override void Awake()
{
    base.Awake();  // WeaponBase の初期化（_renderer取得）
    _animator = GetComponent<Animator>();
    _hitboxes = GetComponentsInChildren<Hitbox>(true);
}
```

**処理内容**
1. `base.Awake()` で _renderer を取得
2. Animator コンポーネント取得
3. **子オブジェクト** の Hitbox 全て取得（true = 非アクティブも含む）

#### private void Start()

**役割** — Hitbox イベント購読。

```csharp
private void Start()
{
    foreach (var hitbox in _hitboxes)
    {
        hitbox.OnFirstHit += HandleFirstHit;
        hitbox.OnContinuousHit += HandleContinuousHit;
    }
}
```

#### private void OnDestroy()

**役割** — Hitbox イベント購読解除。

```csharp
private void OnDestroy()
{
    foreach (var hitbox in _hitboxes)
    {
        hitbox.OnFirstHit -= HandleFirstHit;
        hitbox.OnContinuousHit -= HandleContinuousHit;
    }
}
```

**重要** — メモリリーク防止のため必須

#### private void HandleFirstHit(Health targetHealth)

**役割** — 初回ヒット時のダメージ処理。

```csharp
private void HandleFirstHit(Health targetHealth)
{
    targetHealth.TakeDamage(_weaponData.DamageAmount);
}
```

**処理**
- Health に _weaponData.DamageAmount 分 のダメージを与える

**呼び出し元**
- Hitbox.OnFirstHit イベント（敵との初回接触時）

#### private void HandleContinuousHit(Health targetHealth)

**役割** — 継続ヒット時の処理（拡張用TODO）。

```csharp
private void HandleContinuousHit(Health targetHealth)
{
    //TODO:特殊効果付与
}
```

**現在の実装** — 空（TODO）
**想定される用途** — DoT ダメージ、ノックバック、状態異常付与 等

#### protected override IEnumerator AttackCoroutine()

**役割** — アニメーション武器の攻撃演出。

```csharp
protected override IEnumerator AttackCoroutine()
{
    _animator.SetTrigger(_hashAttack);
    
    int layerIndex = 0;
    float attackDuration = _animator.GetCurrentAnimatorStateInfo(layerIndex).length;
    yield return new WaitForSeconds(attackDuration);
}
```

**処理フロー**
1. "Attack" アニメーションをトリガー
2. Layer 0 のアニメーション長を取得
3. その時間だけ待機
4. 完了したら WeaponBase が状態遷移

**注意点**
- アニメーション長に「トランジション時間」は含まれない
- ステート遷移に1フレーム必要な場合がある（検討のポイント）

### セットアップ例

```
Sword (GameObject)
├── ContactWeapon (Component)
├── Animator
├── SpriteRenderer
└── HitboxCollider (子オブジェクト)
    ├── Hitbox (Component)
    │　　　└── _isAreaAttack = false
    └── BoxCollider2D (Trigger)
```

---

## ProjectileWeapon

### 役割
**投射物型の武器**。弾丸やミサイルなどのプロジェクタイルを発射します。  
例：銃、弓、魔法などの遠距離武器

### 設計方針
- Projectile プレハブをインスタンス化して発射
- Muzzle (銃口) から発射方向を決定
- アニメーションで発射演出を実行

### プロパティ

| メンバー | 説明 | 型 | 役割 |
|---------|------|-----|------|
| `_projectilePrefab` | 発射する投射物のプレハブ | Projectile | インスタンス化して発射 |
| `_muzzle` | 銃口の Transform（発射位置・方向） | Transform | 投射物の生成位置・回転 |
| `_animator` | アニメーション制御 | Animator | 発射演出実行 |
| `_hashAttack` | "Attack" パラメータのハッシュ | int | アニメーション制御 |

### メソッド

#### protected override void Awake()

**役割** — コンポーネント初期化。

```csharp
protected override void Awake()
{
    base.Awake();  // WeaponBase の初期化（_renderer取得）
    _animator = GetComponent<Animator>();
}
```

#### protected override IEnumerator AttackCoroutine()

**役割** — 投射物発射の演出。

```csharp
protected override IEnumerator AttackCoroutine()
{
    Projectile projectile = Instantiate(_projectilePrefab, _muzzle.position, _muzzle.rotation);
    projectile.Init(_weaponData.DamageAmount);

    _animator.SetTrigger(_hashAttack);
    int layerIndex = 0;
    float attackDuration = _animator.GetCurrentAnimatorStateInfo(layerIndex).length;
    yield return new WaitForSeconds(attackDuration);
}
```

**処理フロー**
1. Projectile をインスタンス化（muzzle 位置・回転）
2. Projectile に ダメージ量を初期化
3. "Attack" アニメーションをトリガー
4. アニメーション長だけ待機
5. 完了したら WeaponBase が状態遷移

### セットアップ例

```
Gun (GameObject)
├── ProjectileWeapon (Component)
│   ├── Projectile Prefab: [Bullet.prefab]
│   └── Muzzle: [Muzzle Transform]
├── Animator
├── SpriteRenderer
└── Muzzle (子オブジェクト)
    └── Transform（発射位置・方向の基準点）
```

---

## BurstProjectileWeapon

### 役割
**連射投射物型の武器**。ProjectileWeapon を継承し、複数回の連射を実現します。  
例：マシンガン、連射弓、連続魔法など

### 設計方針
- ProjectileWeapon の AttackCoroutine を複数回ループ
- 各射撃間に指定時間のインターバルを設定
- 親クラスの状態管理を活用

### プロパティ

| メンバー | 説明 | 型 | 役割 |
|---------|------|-----|------|
| `_burstShotCount` | 連射する回数 | int | 発射回数を制御 |
| `_burstShotInterval` | 各射撃間のインターバル（秒） | float | 射撃間の時間待機 |

### メソッド

#### protected override IEnumerator AttackCoroutine()

**役割** — 連射の実行。

```csharp
protected override IEnumerator AttackCoroutine()
{
    for (int i = 0; i < _burstShotCount; i++)
    {
        yield return base.AttackCoroutine();
        yield return new WaitForSeconds(_burstShotInterval);
    }
}
```

**処理フロー**
1. `_burstShotCount` 回ループ
2. 各ループで `base.AttackCoroutine()` 実行（親クラスの発射処理）
3. インターバル待機
4. 次の射撃へ
5. すべて完了したら WeaponBase が状態遷移

### 使用例

```csharp
// Inspector での設定
_burstShotCount = 3        // 3回連射
_burstShotInterval = 0.2f  // 各射撃間 0.2 秒
```

結果: 0秒目に1発 → 0.2秒後に2発 → 0.2秒後に3発

### セットアップ例

```
MachineGun (GameObject)
├── BurstProjectileWeapon (Component)
│   ├── Burst Shot Count: 3
│   ├── Burst Shot Interval: 0.2
│   ├── Projectile Prefab: [Bullet.prefab]
│   └── Muzzle: [Muzzle Transform]
├── Animator
├── SpriteRenderer
└── Muzzle (子オブジェクト)
```

---

## 参考：クラス依存関係

```
WeaponData
    ↑
    │ 保持
    │
WeaponBase
    ↑
    │ 継承
    ├─→ ContactWeapon
    │       │
    │       └─ Hitbox[] を管理
    │           └─ Health イベント受信
    │
    └─→ ProjectileWeapon
            │
            └─→ BurstProjectileWeapon
                    │
                    └─ Projectile[] を発射
```

---
