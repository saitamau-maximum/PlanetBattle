using UnityEngine;
using UnityEngine.UI;

public class DeathCountdownDisplay : MonoBehaviour
{
    [SerializeField] private Text _countdownText;

    private void Awake()
    {
        if (_countdownText == null)
        {
            _countdownText = GetComponent<Text>();
        }

        if (_countdownText == null)
        {
            Debug.LogError("[DeathCountdownDisplay] Text component is null!");
        }

        SetActive(false);
    }

    public void SetCountdown(float remainingTime)
    {
        if (_countdownText == null) return;

        int displaySeconds = Mathf.CeilToInt(remainingTime);
        _countdownText.text = $"Time:{displaySeconds}s";
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
