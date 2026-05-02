using UnityEngine;
using Utility;

public class BuildingBox : MonoBehaviour
{
    private GameObject _structurePrefab;
    private CountdownTimer _buildTimer;

    public void Init(StructureData data)
    {
        _structurePrefab = data.Prefab;
        _buildTimer = new CountdownTimer(data.BuildTime);
        _buildTimer.Start();
        transform.localScale = new Vector2(
            transform.localScale.x * data.GridSize.x,
            transform.localScale.y * data.GridSize.y
        );
    }

    private void Update()
    {
        _buildTimer.Tick();
        if (_buildTimer.IsFinished())
        {
            Instantiate(_structurePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}