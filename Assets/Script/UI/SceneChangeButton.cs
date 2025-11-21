using UnityEngine;

public class SceneSwitchButton : MonoBehaviour
{
    [SerializeField] private ScreenManager.SceneType _sceneType;

    public void OnClickRequestSceneChange()
    {
        ScreenManager.Instance.ChangeScene(_sceneType);
    }
}
