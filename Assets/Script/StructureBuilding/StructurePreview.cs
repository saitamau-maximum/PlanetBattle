using UnityEngine;

public class StructurePreview : MonoBehaviour
{
    [SerializeField] private Color _previewValidColor;
    [SerializeField] private Color _previewInvalidColor;

    private SpriteRenderer _previewRenderer;
    private StructureData _structureData;
    private Vector2 _initialScale;

    private void Awake()
    {
        _previewRenderer = GetComponent<SpriteRenderer>();
        _initialScale = transform.localScale;
    }

    public void SetStructure(StructureData data)
    {
        _structureData = data;
        transform.localScale = new Vector2(
           _initialScale.x * _structureData.GridSize.x,
           _initialScale.y * _structureData.GridSize.y
       );
    }

    public void UpdateState(bool canPlace)
    {
        if (canPlace)
        {
            SetPlacementValid();
        }
        else
        {
            SetPlacementInvalid();
        }
    }

    private void SetPlacementValid()
    {
        _previewRenderer.color = _previewValidColor;
    }

    private void SetPlacementInvalid()
    {
        _previewRenderer.color = _previewInvalidColor;
    }

}
