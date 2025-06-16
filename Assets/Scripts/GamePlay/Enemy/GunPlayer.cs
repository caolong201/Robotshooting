using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPlayerr : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab đạn
    public Transform firePoint; // Vị trí bắn
    public int bulletDamage = 2; // Sát thương mỗi viên đạn
    public float bulletRange = 100f; // Tầm bay tối đa của đạn   
    public int bulletsPerBurst =10;
    public float timeBetweenBullets = 0.05f;
    public float timeBetweenBursts = 5f;
    //public LayerMask enemyLayerMask;
  
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    public RectTransform crosshairUI;
    public Camera mainCamera;

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            if (IsCrosshairOnEnemy(out Vector3 shootDirection))
            {
                Shoot(shootDirection);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private bool IsCrosshairOnEnemy(out Vector3 shootDirection)
    {
        shootDirection = Vector3.zero;

        // Lấy vị trí của crosshair trên màn hình
        Vector3 screenPosition = crosshairUI.position;

        // Bắn ray từ camera qua crosshair
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, bulletRange))
        {
            Debug.Log("Raycast trúng: " + hit.transform.name);

            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Ngắm trúng enemy: " + hit.transform.name);
                shootDirection = (hit.point - firePoint.position).normalized;
                return true;
            }
            else
            {
                Debug.Log("Raycast trúng nhưng KHÔNG phải enemy, tag: " + hit.transform.tag);
            }
        }
        else
        {
            Debug.Log("Không raycast trúng gì");
        }

        return false;
    }

    private void Shoot(Vector3 direction)
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Thiếu prefab hoặc firePoint");
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Debug.Log("Spawned bullet: " + bulletObj.name);

        Bullet1 bullet = bulletObj.GetComponent<Bullet1>();
        if (bullet != null)
        {
            bullet.damage = bulletDamage;
            bullet.SetDirection(direction, firePoint.position, bulletRange);
        }

        Debug.DrawRay(firePoint.position, direction * bulletRange, Color.red, 1f);
    }
}
