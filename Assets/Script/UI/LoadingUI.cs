using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _progressText;

    public void UpdateProgress(float progressRate)
    {
        _progressText.text = (int)(progressRate * 100) + "%";
    }
}
