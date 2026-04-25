## クラス
### PlayerController
- 入力を受け取り、PlayerBuildingManagerに建築モード開始、終了の指示を出します。
- 建造物設置の入力があったことをPlayerBuildingManagerに伝えます。

### PlayerBuildingManager
- 建造物のスロット(_structures)を管理します。 
- 建築モードの切り替えを行います。
- PlayerControllerから建造物設置の入力があったという指示が来ると、現在選択中の建造物の建築に必要なコインを所持しているのかを判断、建築可能な場合はデータ(StructureData)をStructurePlacementControllerに渡し、建築指示を出します。

### StructurePlacementController
- 建造物設置フローの管理
- プレビューオブジェクトから各コンポーネントを取得し、SetStructure()とPlaceStructure()でフローを制御
- 建造物が設置可能な場合、BuildingBoxを生成

### StructurePlacementValidator
- 配置可能判定（地面接触、オブジェクト重複判定）を行う

### StructurePreview
- プレビュー表示の管理
- サイズ変更と色変更

### GridSnapper
- プレビューオブジェクトの位置をグリッドにスナップ
- 建造物サイズに応じたオフセット計算

### BuildingBox
- 実際に配置された建造物のコンテナ
- 建造時間のカウントダウン後、実際の建造物Prefabをインスタンス化

### StructureData
- 建造物のデータです。
  - GameObject Prefab : 建造物のPrefab
  - int Cost : 建築にかかるコインの枚数
  - float BuildTime : 建築に係る時間
  - Vector2 GridSize : 建造物をグリッドで表示するときのサイズ
- `Assets/ScriptableObject/Structure`にこのクラスのScriptableObjectがあります。そこで設定を行ってください。
