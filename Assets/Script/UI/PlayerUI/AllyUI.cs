using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;

public class AllyUI : MonoBehaviour
{
    [SerializeField] private PlayerAllyManager _playerAllyManager;
    [SerializeField] private Image _experieneBarFill;
    [SerializeField] private Image _nextAllyWeaponImage;
    [SerializeField] private TMP_Text _entryNumberText;
    [SerializeField] private float _speed = 5f; // 大きいほど速い


    private float _targetExperienceRatio;

    private void Start()
    {
        _playerAllyManager.OnSpawnCapacityChanged += UpdateSpawnCapacity;
        _playerAllyManager.OnNextAllyEntryChanged += UpdateWeaponImage;

        _experieneBarFill.fillAmount = _playerAllyManager.ExperienceForNextRatio;
        _entryNumberText.text = _playerAllyManager.EntryAllyCount.ToString();
        _nextAllyWeaponImage.sprite = _playerAllyManager.NextAllyEntry.WeaponImage;
    }

    private void OnDestroy()
    {
        _playerAllyManager.OnSpawnCapacityChanged -= UpdateSpawnCapacity;
        _playerAllyManager.OnNextAllyEntryChanged -= UpdateWeaponImage;
    }

    private void Update()
    {
        _experieneBarFill.fillAmount = Mathf.Lerp(
            _experieneBarFill.fillAmount,
            _targetExperienceRatio,
            Time.deltaTime * _speed
        );
    }

    private void UpdateSpawnCapacity(float experienceForNextRatio, int entryCount)
    {
        _targetExperienceRatio = experienceForNextRatio;
        _entryNumberText.text = entryCount.ToString();
    }

    private void UpdateWeaponImage(AllyData data)
    {
        _nextAllyWeaponImage.sprite = data.WeaponImage;
    }
}