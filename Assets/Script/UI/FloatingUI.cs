using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    [SerializeField] private float _amplitude = 10f;   // 振幅（ローカルY単位）
    [SerializeField] private float _frequency = 1f;    // 周波数（往復速度）

    private Vector3 _baseLocalPos;
    private Transform _parent;

    private void Awake()
    {
        _baseLocalPos = transform.localPosition;
        _parent = transform.parent;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        // --- 上下にゆっくり移動（ローカル基準） ---
        float offsetY = Mathf.Sin(Time.time * _frequency) * _amplitude;
        transform.localPosition = _baseLocalPos + new Vector3(0f, offsetY, 0f);
    }
}