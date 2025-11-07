using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [SerializeField] private LoadingUI _loadingUI;

    private const float LOADED_SCENE_RATE = 0.9f;

    public enum SceneType
    {
        Title,
        GetStarted_Scene
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(SceneType scene)
    {
        StartCoroutine(LoadSceneAsyncWithLoadingUI(scene));
    }

    private IEnumerator LoadSceneAsyncWithLoadingUI(SceneType scene)
    {
        _loadingUI.gameObject.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress;
            _loadingUI.UpdateProgress(progress);

            if (progress >= LOADED_SCENE_RATE)
            {
                _loadingUI.UpdateProgress(1);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        _loadingUI.gameObject.SetActive(false);
    }

    public T GetData<T>() { return default; } //TODO:シーン間でのデータのやり取り用
}
