using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEffectType
{
    BulletFX = 0,
    HitFX1,
    DamageFX1,
    DamageFX_Critical,
    CarBrokenFX,
    BulletRicochest,
}
[Serializable]
public class EffectPrefab
{
    public EEffectType type;
    public GameObject prefab;
}

public class EffectManager : SingletonMonoAwake<EffectManager>
{
    [SerializeField] private List<EffectPrefab> effectsPrefab = new List<EffectPrefab>();
    
    // Pool for effects
    private Dictionary<string, Queue<GameObject>> effectPool = new Dictionary<string, Queue<GameObject>>();

    

    public override void OnAwake()
    {
        base.OnAwake();
    }

    /// <summary>
    /// Preloads effects into the pool.
    /// </summary>
    /// <param name="effectPrefab">Effect prefab.</param>
    /// <param name="amount">Number of instances to preload.</param>
    public void PreloadEffect(EEffectType type, int amount)
    {
        var findEffect = effectsPrefab.Find(x => x.type == type);
        if (findEffect == null || findEffect.prefab == null)
        {
            Debug.Log("Effect not found");
            return;
        }

        string effectName = findEffect.prefab.name;
        if (!effectPool.ContainsKey(effectName))
        {
            effectPool[effectName] = new Queue<GameObject>();
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject effectInstance = Instantiate(findEffect.prefab, transform);
            effectInstance.SetActive(false);
            effectPool[effectName].Enqueue(effectInstance);
        }
    }

    /// <summary>
    /// Spawns an effect from the pool at the specified position and rotation.
    /// </summary>
    /// <param name="effectPrefab">Effect prefab.</param>
    /// <param name="position">Spawn position.</param>
    /// <param name="rotation">Spawn rotation.</param>
    public T PlayEffect<T>(EEffectType type, Vector3 position, Quaternion rotation)
    {
        var findEffect = effectsPrefab.Find(x => x.type == type);
        if (findEffect == null || findEffect.prefab == null)
        {
            Debug.Log("Effect not found");
            return default(T);
        }
        string effectName = findEffect.prefab.name;

        if (!effectPool.ContainsKey(effectName) || effectPool[effectName].Count == 0)
        {
            PreloadEffect(type, 1); // Preload if no available effects
        }

        GameObject effectInstance = effectPool[effectName].Dequeue();
        effectInstance.transform.position = position;
        effectInstance.transform.rotation = rotation;
        effectInstance.SetActive(true);

        // Schedule effect deactivation
        StartCoroutine(DeactivateEffect(effectInstance, findEffect.prefab));
        
        return effectInstance.GetComponent<T>();
    }

    /// <summary>
    /// Deactivates the effect after it has finished playing.
    /// </summary>
    /// <param name="effectInstance">Effect instance.</param>
    /// <param name="effectPrefab">Effect prefab.</param>
    private IEnumerator DeactivateEffect(GameObject effectInstance, GameObject effectPrefab)
    {
        ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            yield return new WaitForSeconds(2f); // Default time for non-particle effects
        }

        effectInstance.SetActive(false);
        effectPool[effectPrefab.name].Enqueue(effectInstance);
    }
}
