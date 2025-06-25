using System;
using DG.Tweening;
using TMPro;
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
    [SerializeField] private Transform cameraSlowMotionTransform;
    [SerializeField] private TextMeshProUGUI txtRemainAmmo, txtTotalAmmo;
    private Ray ray;
    RaycastHit hit;
    private bool isDead = false;

    private void Start()
    {
        posZ = transform.localPosition.z;
        currentFireTime = fireTime;
        if (isPlayer)
        {
            txtRemainAmmo.text = currentFireTime.ToString();
            txtTotalAmmo.text = "/" + currentFireTime;
        }
    }

    public void Reset()
    {
        isDead = false;
        currentFireTime = fireTime;
        txtRemainAmmo.text = currentFireTime.ToString();
    }

    public void Dead()
    {
        isDead = true;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameStatus != EGameStatus.Live) return;
        if (isDead) return;
        if (isPlayer && GameManager.Instance.IsGunReloading) return;

        if (startDelayFire > 0)
        {
            startDelayFire -= Time.deltaTime;
            return;
        }

        RayEnemy();
        nextAttackTime -= Time.deltaTime;
        if (nextAttackTime <= 0)
        {
            Shoot();
            nextAttackTime = attackCooldown;
            if (isPlayer && currentFireTime <= 0)
            {
                GameManager.Instance.IsGunReloading = true;
                currentFireTime = fireTime;
            }
        }
    }

    private void RayEnemy()
    {
        ray = new Ray(firePoint.position, firePoint.forward);
        if (Physics.Raycast(ray, out hit, range, enemyLayer))
        {
            target = hit.collider;
            if (crossHairDot != null)
            {
                crossHairDot.color = Color.green;
            }

            // Optional: visualize ray
            Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.green, 0.1f);
            if (isPlayer) GameManager.Instance.IsRayHitEmeny = true;
        }
        else
        {
            target = null;
            if (crossHairDot != null)
            {
                crossHairDot.color = Color.white;
            }

            // Optional: visualize ray
            Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red, 0.1f);
            if (isPlayer) GameManager.Instance.IsRayHitEmeny = false;
        }
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
            txtRemainAmmo.text = currentFireTime.ToString();
            
            transform.DOKill();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, posZ);
            transform.DOLocalMoveZ(posZ - 0.02f, 0.1f).SetLoops(1, LoopType.Yoyo);

            crossHair.DOKill();
            crossHair.localScale = Vector3.one * 2;
            crossHair.DOScale(Vector3.one * 2 * 0.85f, 0.1f).SetLoops(1, LoopType.Yoyo);

            //stage 1 and last enemy
            if (GameManager.Instance.CurrenStage == 1 && GameManager.Instance.CurrentWave == 2 &&
                GameManager.Instance.TotalEnemiesKilled >= 3)
            {
                var ai = target.GetComponent<EnemyAI>();
                float currHP = ai.GetCurrentHP;
                if (bullet.damage - currHP >= 0)
                {
                    Debug.Log("2");
                    GameManager.Instance.CurrentGameStatus = EGameStatus.End;
                    ai.ShowHideObjects(false);
                    //fx last hit
                    if (cameraSlowMotionTransform != null)
                    {
                        cameraSlowMotionTransform.gameObject.SetActive(true);
                        cameraSlowMotionTransform.SetParent(bullet.transform);
                        cameraSlowMotionTransform.localPosition = new Vector3(-0.75f, 0, -7.5f);
                        bullet.speed /= 10;
                        // bullet.SetDirection(firePoint.forward, firePoint.position,
                        //     Vector3.Distance(target.transform.position, firePoint.position));
                        Debug.Log("3");
                    }

                    target = null;
                }
            }
        }
    }
}