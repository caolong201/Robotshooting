using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab đạn
    public Transform firePoint; // Vị trí bắn
    public int bulletDamage = 2; // Sát thương mỗi viên đạn
    public float bulletRange = 100f; // Tầm bay tối đa của đạn   
    public int bulletsPerBurst = 10;
    public float timeBetweenBullets = 0.05f;
    public float timeBetweenBursts = 5f;
    private Transform target;
    public float hitAccuracy = 0.5f; // Tỷ lệ bắn trúng (50%)
    public float missAngleRange = 20f; // Độ lệch khi bắn tự do
    public float rotationSpeed = 0.5f; // Tốc độ xoay súng (xoay mượt)
    private Vector3 currentShootDirection;

    public void Init(Transform target)
    {
        this.target = target;
    }

    private void Start()
    {
        StartCoroutine(AutoFire());
    }

    private void Update()
    {
        if (currentShootDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentShootDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator AutoFire()
    {
        while (true)
        {
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                Shoot();
                yield return new WaitForSeconds(timeBetweenBullets);
            }

            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || target == null)
        {
            return;
        }

        if (Vector3.Distance(target.position, firePoint.position) > bulletRange)
            return;

        Vector3 directionToTarget = (target.position - firePoint.position).normalized;
        Vector3 shootDirection;

        //firePoint.rotation = Quaternion.LookRotation(directionToTarget);
        if (Random.value <= hitAccuracy)
        {
            // Bắn trúng
            shootDirection = directionToTarget;
        }
        else
        {
            // Bắn tự do
            shootDirection = GetRandomDirection(directionToTarget, missAngleRange);
        }

        currentShootDirection = shootDirection;
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("Spawned bullet: " + bulletObj.name);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.damage = bulletDamage;
            bullet.SetDirection(firePoint.forward, firePoint.position, bulletRange);
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * bulletRange, Color.red, 1f);
    }

    private Vector3 GetRandomDirection(Vector3 baseDirection, float maxAngle)
    {
        float randomAngleX = Random.Range(-maxAngle, maxAngle);
        float randomAngleY = Random.Range(-maxAngle, maxAngle);

        Quaternion randomRotation = Quaternion.Euler(randomAngleX, randomAngleY, 0);
        return randomRotation * baseDirection;
    }
}