using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public GameObject Rocket { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterPlayer(GameObject player)
    {
        Player = player;
    }

    public void RegisterRocket(GameObject rocket)
    {
        Rocket = rocket;
        if (rocket.TryGetComponent(out Health rocketHealth))
        {
            rocketHealth.OnDied += GameOver;
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
    }

    private void GameClear()
    {

    }
}