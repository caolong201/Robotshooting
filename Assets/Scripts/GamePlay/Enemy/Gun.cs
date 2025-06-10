using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab đạn
    public Transform firePoint; // Vị trí bắn
    public int bulletDamage = 2; // Sát thương mỗi viên đạn
    public float bulletRange = 100f; // Tầm bay tối đa của đạn   
    public int bulletsPerBurst = 10;
    public float timeBetweenBullets = 0.05f;
    public float timeBetweenBursts = 5f;
    public Transform target;
    private void Start()
    {
        StartCoroutine(AutoFire());
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
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("ll");
            return;
        }
      

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.damage = bulletDamage;
            bullet.SetDirection(firePoint.forward, firePoint.position, bulletRange);
        }
        Debug.DrawRay(firePoint.position, firePoint.forward * bulletRange, Color.red, 1f);
    }

}
