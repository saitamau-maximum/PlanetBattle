using UnityEngine;

public class Abyss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.GetComponentInParent<Item>();
        if (item != null)
        {
            Destroy(item.gameObject);
            return;
        }

        Health health = other.GetComponentInParent<Health>();
        if (health != null)
        {
            health.Kill();
        }
    }
}
