using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyControl : MonoBehaviour
{
    [SerializeField] Transform target;
    Rigidbody2D rb;
    float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        rb.linearVelocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("kk");
            target.name = "test";
        }
    }

}
