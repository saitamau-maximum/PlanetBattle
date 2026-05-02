using Utility;
using UnityEngine;

public class BuildingBox : Structure
{
    private GameObject _structure;
    private CountdownTimer _countdownTimer;

    public void Init(StructureData data)
    {
        _structure = data.Prefab;
        _countdownTimer = new CountdownTimer(data.BuildTime);
        _countdownTimer.Start();

        transform.localScale = new Vector2(
            transform.localScale.x * data.GridSize.x,
            transform.localScale.y * data.GridSize.y
        );
    }

    private void Update()
    {
        _countdownTimer.Tick();
        if (_countdownTimer.IsFinished())
        {
            Instantiate(_structure, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
