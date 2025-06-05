using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerAutoFire : SingletonMonoAwake<PlayerAutoFire>
{
    public Transform gunBarrel; // Starting point of the ray
    public float maxRayLength = 10f; // Maximum length of the ray
    public LayerMask hitLayers; // Layers to detect with the ray
    private LineRenderer lineRenderer;

    public System.Action<bool> onFire;
    public System.Action<bool> onOverheat;
    
    private bool fire = false;
    private bool isOverheat = false;
    private float overheatTime = 10f;
    
    public string HitEnemyName = String.Empty;

    private Dictionary<string, CapsuleCollider> enemiesDead;
    
    private float fireTimer = 0f;
    public override void OnAwake()
    {
        base.OnAwake();
        Debug.Log("PlayerAutoFire INIT");
        // Initialize the LineRenderer
        // lineRenderer = GetComponent<LineRenderer>();
        // lineRenderer.startWidth = 0.03f;
        // lineRenderer.endWidth = 0.03f;
        // lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        // lineRenderer.startColor = Color.yellow;
        // lineRenderer.endColor = Color.yellow;
        
        enemiesDead = new Dictionary<string, CapsuleCollider>();
    }

    void Update()
    {
       // if(lineRenderer == null) return;
        
        // Start position of the ray
        Vector3 start = gunBarrel.position;

        // Default end position (if no hit)
        Vector3 end = gunBarrel.position + gunBarrel.forward * maxRayLength;

        // Perform the raycast
        if (isOverheat == false)
        {
            if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out RaycastHit hit, maxRayLength, hitLayers))
            {
                // Update the end position to the hit point
                end = hit.point;
           
                if (fire == false)
                {
                    HitEnemyName = hit.transform.name;
                    if (enemiesDead.ContainsKey(HitEnemyName))
                    {
                        if (enemiesDead[HitEnemyName].enabled)
                        {
                            onFire?.Invoke(true);
                            fire = true;
                        }
                    }
                    else
                    {
                        enemiesDead[HitEnemyName] = hit.transform.GetComponent<CapsuleCollider>();
                        if (enemiesDead[HitEnemyName].enabled)
                        {
                            onFire?.Invoke(true);
                            fire = true;
                        }
                    }
                }
                else
                {
                    if (enemiesDead.ContainsKey(HitEnemyName))
                    {
                        if (!enemiesDead[HitEnemyName].enabled)
                        {
                            onFire?.Invoke(false);
                            fire = false;
                        }
                    }
                }
            }
            else
            {
                if (fire)
                {
                    HitEnemyName = string.Empty;
                    onFire?.Invoke(false);
                }

                fire = false;
            }
        }

        //check overheat
        if (fire && !isOverheat)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= overheatTime)
            {
                //overheat
                isOverheat = true;
                onOverheat?.Invoke(true);
                onFire?.Invoke(false);
            }
        }
       

        // Update the LineRenderer to draw the ray
        // lineRenderer.SetPosition(0, start); // Start of the ray
        // lineRenderer.SetPosition(1, end);  // End of the ray
    }

    public void CoolingDownCompleted()
    {
        fire = false;
        isOverheat = false;
        fireTimer = 0;
        onOverheat?.Invoke(false);
    }
}
