using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public Rigidbody2D rb;

    public float startBulletLive;
    private float timeBulletLive;

    void Start()
    {
        rb.velocity = transform.right * speed;
        timeBulletLive = startBulletLive;
    }
    void Update()
    {
        if (timeBulletLive <= 0)
        {
            timeBulletLive = startBulletLive;
            Destroy(gameObject);
        }
        else
        {
            timeBulletLive -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherGameObject = other.gameObject;
        var tags = new[] { "Item", "Bullet", "Player"};
        if (!tags.Any(tagToMatch => otherGameObject.CompareTag(tagToMatch)))
        {
            Debug.Log("wdw");
            Destroy(gameObject);
        }
    }
}
