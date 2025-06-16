using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public int damage = 2;
    public float speed = 40f;
    public float lifetime = 20f;
    private Vector3 direction;
    private Vector3 startPosition;
    private float maxDistance;
    public ParticleSystem myParticleSystem;

    void Start()
    {
        if (myParticleSystem != null)
        {
            myParticleSystem.Play();
        }

        Destroy(gameObject, lifetime);
    }
    public void SetDirection(Vector3 dir, Vector3 startPos, float radius)
    {
        direction = dir.normalized;
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
    private void OnTriggerEnter(Collider other)
    {

    }
}