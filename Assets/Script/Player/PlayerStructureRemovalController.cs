using UnityEngine;

[RequireComponent(typeof(CurrencyWallet))]
public class PlayerStructureRemovalController : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _removalRange = 2f;
    [SerializeField] private LayerMask _removalLayers;
    [SerializeField] private AudioClip _removeSound;
    private CurrencyWallet _currencyWallet;

    private Structure _targetStructure;

    private void Awake()
    {
        _currencyWallet = GetComponent<CurrencyWallet>();
    }

    private void FixedUpdate()
    {
        // PlayerControllerではローカルの左方向がプレイヤーの正面。
        Vector2 direction = -transform.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _removalRange, _removalLayers);
        Structure structure = hit.collider != null
            ? hit.collider.GetComponentInParent<Structure>()
            : null;

        SetTargetStructure(structure != null && structure.CanRemove ? structure : null);
    }

    private void SetTargetStructure(Structure structure)
    {
        if (_targetStructure == structure) return;

        if (_targetStructure != null) _targetStructure.HideRemovalIcon();

        if (structure != null) structure.ShowRemovalIcon();
        _targetStructure = structure;
    }

    private void OnDisable()
    {
        SetTargetStructure(null);
    }

    public bool TryRemoveStructure()
    {
        if (_targetStructure == null) return false;
        if (!_targetStructure.TryRemove()) return false;

        _currencyWallet.AddCurrency(CurrencyData.CurrencyType.Coin, _targetStructure.RefundCost);
        SetTargetStructure(null);
        AudioManager.Instance.PlaySoundEffect(_removeSound);
        return true;
    }
}
