using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MachineBullet : MonoBehaviour
{
    public int damage = 2;
    private Transform target = null;
    private Vector3 direction;
    public float speed = 10f;
    private Vector3 startPosition;
    private float maxDistance;
    [SerializeField] private GameObject fxHit;

    public void SetDirection(Vector3 dir, Vector3 startPos, float radius)
    {
        direction = dir;
        startPosition = startPos;
        maxDistance = radius;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
   

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}