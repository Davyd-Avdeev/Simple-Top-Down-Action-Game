using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletV2 : MonoBehaviour
{
    public float Damage;
    public int speed;
    public float MaxTravelDistance;
    private Vector2 moveVelocity;
    public Rigidbody2D rb;

    private Vector3 _startPosition;

    public void StartMove()
    {
        _startPosition = transform.position;
        transform.position = _startPosition + transform.right;
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(1, 1);
        moveVelocity = moveInput.normalized * speed;
    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        var travelledDistance = Vector3.Distance(transform.position, _startPosition);
        if (travelledDistance > MaxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherGameObject = other.gameObject;
        var tags = new[] { "Npc", "Wall"};
        if (tags.Any(tagToMatch => otherGameObject.CompareTag(tagToMatch)))
        {
            Destroy(gameObject);
        }
    }
}
