using UnityEngine;

public class Missile2D : MonoBehaviour
{
    public float speed = 8f;
    public float rotateSpeed = 200f;
    public float homingDelay = 0.5f;

    private Transform target;
    private float timer;

    private enum State { Launch, Homing }
    private State state = State.Launch;

    void Start()
    {
        // Enemyタグからランダムターゲット
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            target = enemies[Random.Range(0, enemies.Length)].transform;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 一定時間後に追尾開始
        if (timer > homingDelay)
        {
            state = State.Homing;
        }

        // 追尾処理
        if (state == State.Homing && target != null)
        {
            Vector2 dir = (target.position - transform.position).normalized;

            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float angle = Mathf.MoveTowardsAngle(
                transform.eulerAngles.z,
                targetAngle,
                rotateSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // 常に前進（2Dは right が前）
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}