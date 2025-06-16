using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float attackCooldown = 2f;
    private float nextAttackTime;
    public float range = 500f;
    public int attackPower = 2;

    public LayerMask enemyLayer;

    private Collider target = null;

    private void Update()
    {
        RayEnemy();
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Shoot();
        }

    }

    private void RayEnemy()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, enemyLayer))
        {
            target = hit.collider;
        }
        else
        {
            target = null;
        }
        

        // Optional: visualize ray
        Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red, 1f);
    }

    void Shoot()
    {
        if(target == null) return;
        
        GameObject obj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        MachineBullet bullet = obj.GetComponent<MachineBullet>();
        bullet.damage = attackPower;
        bullet.SetDirection(firePoint.forward, firePoint.position, range);
    }

}