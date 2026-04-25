## クラス
### PlayerController
- 入力を受け取り、PlayerBuildingManagerに建築モード開始、終了の指示を出します。
- 建造物設置の入力があったことをPlayerBuildingManagerに伝えます。

### PlayerBuildingManager
- 建造物のスロット(_structures)を管理します。 
- 建築モードの切り替えを行います。
- PlayerControllerから建造物設置の入力があったという指示が来ると、現在選択中の建造物の建築に必要なコインを所持しているのかを判断、建築可能な場合はデータ(StructureData)をStructurePlacementControllerに渡し、建築指示を出します。

### StructurePlacementController
- 建造物設置の全体フローを管理します。
- PlayerBuildingManagerから設置する建造物のデータ(StructureData)を受け取ります。
- プレビューをグリッドにスナップして表示します。
- 設置可能かをStructurePlacementValidatorで判定し、StructurePreviewで見た目を更新します。
- 設置指示を受けたら、判定結果がOKな場合のみBuildingBoxをインスタンス化して配置します。

### StructurePlacementValidator
- 建造物の配置可能性を判定するバリデーターです。
- StructurePlacementControllerから建造物のサイズを受け取ります。
- 実際にその場所に建造物の設置が可能かどうかを判定します。
- 設置可能の判定条件
  - 下に地面があるか（Raycastで複数ポイントから検査）
  - 他の建造物と重なっていないか（OnTriggerEnter/Exit2Dで判定）

### StructurePreview
- 建造物設置時のプレビュー表示を管理します。
- StructurePlacementControllerから建造物のデータ(StructureData)を受け取ります。
- StructureDataに合わせ見た目の大きさなどを変更します。
- UpdateState()メソッドで判定結果に応じたビジュアルフィードバック（色変更など）を行います。

### StructureData
- 建造物のデータです。
  - GameObject Prefab : 建造物のPrefab
  - int Cost : 建築にかかるコインの枚数
  - float BuildTime : 建築に係る時間
  - Vector2 GridSize : 建造物をグリッドで表示するときのサイズ
- `Assets/ScriptableObject/Structure`にこのクラスのScriptableObjectがあります。そこで設定を行ってください。
