using UnityEngine;

public class BuildingIconUI : MonoBehaviour
{
    [SerializeField] private GameObject _coinIcon;
    [SerializeField] private GameObject _groundIcon;

    public void SetCoinIconActive(bool active)
    {
        _coinIcon.SetActive(active);
    }

    public void SetGroundIconActive(bool active)
    {
        _groundIcon.SetActive(active);
    }
}
