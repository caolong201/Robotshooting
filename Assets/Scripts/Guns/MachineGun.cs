using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MachineGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float attackCooldown = 2f;
    private float nextAttackTime = 0;
    public float range = 500f;
    public int attackPower = 2;

    public LayerMask enemyLayer;

    private Collider target = null;
    [SerializeField] private int fireTime = 5;
    private int currentFireTime = 5;

    [SerializeField] private float startDelayFire = 0;
    [SerializeField] bool isPlayer = false;
    private float posZ = 0;
    [SerializeField] private Transform crossHair;
    [SerializeField] private Image crossHairDot;

    private void Start()
    {
        posZ = transform.localPosition.z;
        currentFireTime = fireTime;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameStatus != EGameStatus.Live) return;

        if (startDelayFire > 0)
        {
            startDelayFire -= Time.deltaTime;
            return;
        }

        RayEnemy();
        nextAttackTime -= Time.deltaTime;
        if (nextAttackTime <= 0)
        {
            nextAttackTime = attackCooldown;
            Shoot();
            if (currentFireTime <= 0)
            {
                nextAttackTime += 3;
                currentFireTime = fireTime;
            }
        }
    }

    private void RayEnemy()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, enemyLayer))
        {
            target = hit.collider;
            if (crossHairDot != null)
            {
                crossHairDot.color = Color.green;
            }
        }
        else
        {
            target = null;
            if (crossHairDot != null)
            {
                crossHairDot.color = Color.white;
            }
        }


        // Optional: visualize ray
        Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red, 0.1f);
    }

    void Shoot()
    {
        if (target == null) return;

        GameObject obj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        MachineBullet bullet = obj.GetComponent<MachineBullet>();
        bullet.damage = attackPower;
        bullet.SetDirection(firePoint.forward, firePoint.position, range);

        currentFireTime -= 1;

        if (isPlayer)
        {
            transform.DOKill();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, posZ);
            transform.DOLocalMoveZ(posZ - 0.02f, 0.1f).SetLoops(1, LoopType.Yoyo);

            crossHair.DOKill();
            crossHair.localScale = Vector3.one * 2;
            crossHair.DOScale(Vector3.one * 2 * 0.85f, 0.1f).SetLoops(1, LoopType.Yoyo);
            ;
        }
    }
}