using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _cost = 1;
    [SerializeField] private float _lifetime = 10f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CoinWallet wallet))
        {
            wallet.AddCoins(_cost);
            Destroy(gameObject);
        }
    }
}
