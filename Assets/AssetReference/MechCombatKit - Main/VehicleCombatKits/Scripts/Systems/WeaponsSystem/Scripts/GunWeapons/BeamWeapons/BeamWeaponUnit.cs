﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// A weapon unit for beam weapons.
    /// </summary>
    [DefaultExecutionOrder(50)]
    public class BeamWeaponUnit : HitScanWeaponUnit
    {
        [Header("Beam Parameters")]

        [SerializeField]
        protected float maxBeamLevel = 1;
        public float MaxBeamLevel { set { maxBeamLevel = value; } }

        [SerializeField]
        protected LineRenderer lineRenderer;

        [SerializeField]
        protected string beamRendererTextureKey = "_MainTex";

        [SerializeField]
        protected BeamHitEffectController hitEffect;

        [Header("Visual Effects")]

        [SerializeField]
        protected bool overrideEffectsColors = false;

        [ColorUsage(true, true)]
        [SerializeField]
        protected Color effectsColorOverride = Color.white;

        [SerializeField]
        protected List<AnimatedRenderer> effectsRenderers = new List<AnimatedRenderer>();

        protected BeamState beamState = BeamState.Off;
        public BeamState BeamState
        {
            get { return beamState; }
        }

        protected float beamStateStartTime = 0;

        protected float beamLevel = -1; // Start at a value that will be changed on first use

        protected bool triggered = false;

        [SerializeField]
        protected Vector2 beamScrollSpeed = new Vector2(-5, 0);

        [SerializeField]
        protected Vector2 beamTiling = new Vector2(0.005f, 0);

        [Header("Continuous")]

        [SerializeField]
        protected float beamFadeInTime = 0.15f;

        [SerializeField]
        protected AnimationCurve beamFadeInCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField]
        protected float beamFadeOutTime = 0.33f;

        [SerializeField]
        protected AnimationCurve beamFadeOutCurve = AnimationCurve.Linear(0, 1, 1, 0);

        [Header("Pulsed")]

        [SerializeField]
        protected bool isPulsed;

        [SerializeField]
        protected float pulseDuration = 0.75f;

        [SerializeField]
        protected AnimationCurve pulseCurve = AnimationCurve.Linear(0, 1, 1, 1);

        [Header("Audio")]

        [SerializeField]
        protected AudioSource beamActiveAudio;

        [SerializeField]
        protected float maxBeamActiveAudioVolume = 1;

        [SerializeField]
        protected AudioSource beamStartedAudio;

        [SerializeField]
        protected AudioSource beamStoppedAudio;

        [Header("Beam Controller Events")]

        public UnityEvent onBeamStarted;

        public UnityEvent onBeamStopped;

        public UnityEvent onBeamActive;

        public FloatEventHandler onBeamLevelSet;


        protected override void Reset()
        {
            base.Reset();

            // **************** Set up the line renderer ******************

            // Look for a line renderer
            lineRenderer = GetComponentInChildren<LineRenderer>();
           
            // If no line renderer present, load a beam from Resources
            if (lineRenderer == null)
            {
                LineRenderer lineRendererResource = Resources.Load<LineRenderer>("BeamLineRenderer");

                // Create a beam and add it as a child
                if (lineRendererResource != null)
                {
                    lineRenderer = Instantiate(lineRendererResource, transform);
                    lineRenderer.name = "BeamLineRenderer";

                    effectsRenderers.Add(new AnimatedRenderer(lineRenderer, "_Color"));
                }
            }

            // If resource not found, create one
            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.useWorldSpace = false;
            }

            effectsRenderers.Add(new AnimatedRenderer(lineRenderer, "_Color"));

            spawnPoint = lineRenderer.transform;

            // **************** Set up the hit effect ******************

            // Look for a line renderer
            hitEffect = GetComponentInChildren<BeamHitEffectController>();

            // If no hit effect present, load one from Resources
            if (hitEffect == null)
            {
                BeamHitEffectController hitEffectResource = Resources.Load<BeamHitEffectController>("BeamHitEffect");

                // Create a beam and add it as a child
                if (hitEffectResource != null)
                {
                    hitEffect = Instantiate(hitEffectResource, transform);
                    hitEffect.name = "HitEffect";     // otherwise it has (Clone) in name
                }
            }

            if (hitEffect != null)
            {
                Renderer[] renderers = hitEffect.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    effectsRenderers.Add(new AnimatedRenderer(renderer, "_Color"));
                }
            }
        }



        // Called when scene starts
        protected override void Start()
        {
            base.Start();

            if (rootTransform == null) rootTransform = transform.root;

            // Turn the beam off at the start
            SetBeamLevel(0);

            if (beamActiveAudio != null)
            {
                beamActiveAudio.volume = 0;
                beamActiveAudio.loop = true;
                beamActiveAudio.Play();
            }
        }

        /// <summary>
        /// Set the beam level.
        /// </summary>
        /// <param name="level">Beam level.</param>
        public virtual void SetBeamLevel(float level)
        {

            if (Mathf.Approximately(beamLevel, level)) return;


            // Set the color
            for (int i = 0; i < effectsRenderers.Count; ++i)
            {
                if (Mathf.Approximately(level, 0))
                {
                    effectsRenderers[i].renderer.enabled = false;
                }
                else
                {
                    effectsRenderers[i].renderer.enabled = true;

                    Color c;

                    if (overrideEffectsColors)
                    {
                        c = effectsColorOverride;
                    }
                    else
                    {
                        c = effectsRenderers[i].GetColor();
                    }

                    if (effectsRenderers[i].preserveAlpha)
                    {
                        c.a = level * effectsRenderers[i].BaseAlpha;
                    }
                    else
                    {
                        c.a = level;
                    }

                    effectsRenderers[i].SetColor(c);
                }

            }

            // Update hit effect
            if (hitEffect != null)
            {
                hitEffect.SetLevel(level);
            }

            beamLevel = Mathf.Clamp(level, 0, maxBeamLevel);

            // Call event
            onBeamLevelSet.Invoke(beamLevel);
        }

        protected override void OnHit(RaycastHit hit)
        {
            base.OnHit(hit);
            //Debug.Log("OnHit");
            UpdateBeamPositions(spawnPoint.position, hit.point);

            // Update hit effect
            if (hitEffect != null)
            {
                hitEffect.SetActivation(true);
                hitEffect.OnHit(hit);
            }
        }


        protected override void OnNoHit()
        {
            base.OnNoHit();
            UpdateBeamPositions(lineRenderer.transform.position, lineRenderer.transform.position + lineRenderer.transform.forward * currentRange);
            if (hitEffect != null) hitEffect.SetActivation(false);
        }


        protected virtual void UpdateBeamPositions(Vector3 start, Vector3 end)
        {
            if (lineRenderer != null)
            {
                lineRenderer.startWidth = 2f;
                lineRenderer.endWidth = 2f;
                lineRenderer.SetPosition(0, lineRenderer.transform.InverseTransformPoint(start));
                lineRenderer.SetPosition(1, lineRenderer.transform.InverseTransformPoint(end));
            }
        }


        public override void StartTriggering()
        {
            base.StartTriggering();
            triggered = true;
        }


        public override void StopTriggering()
        {
            triggered = false;
        }


        public override bool CanTrigger
        {
            get
            {
                if (isPulsed)
                {
                    return (beamState != BeamState.Pulsing);
                }
                else
                {
                    return true;
                }
            }
        }

        public override void TriggerOnce()
        {
            if (isPulsed)
            {
                if (beamState != BeamState.Pulsing)
                {
                    SetBeamState(BeamState.Pulsing);
                }
            }
        }


        protected virtual void SetBeamState(BeamState newBeamState)
        {
            switch (newBeamState)
            {
                case BeamState.FadingIn:

                    if (beamStartedAudio != null) beamStartedAudio.Play();
                    beamStateStartTime = Time.time - beamLevel * beamFadeInTime;    // Assume linear fade in/out
                    onBeamStarted.Invoke();
                    break;

                case BeamState.FadingOut:

                    if (beamStoppedAudio != null) beamStoppedAudio.Play();
                    beamStateStartTime = Time.time - (1 - beamLevel) * beamFadeOutTime;     // Assume linear fade in/out
                    break;

                case BeamState.Sustaining:

                    beamStateStartTime = Time.time;
                    break;

                case BeamState.Off:

                    beamStateStartTime = Time.time;
                    onBeamStopped.Invoke();
                    break;

                case BeamState.Pulsing:

                    beamStateStartTime = Time.time;
                    if (beamStartedAudio != null) beamStartedAudio.Play();
                    onBeamStarted.Invoke();
                    break;

            }

            beamState = newBeamState;
        }

        protected virtual void LateUpdate()
        {
            // Handle beam transitions
            switch (beamState)
            {
                case BeamState.FadingIn:

                    if (triggered)
                    {
                        float fadeInAmount = (Time.time - beamStateStartTime) / beamFadeInTime;
                        if (fadeInAmount > 1)
                        {
                            SetBeamLevel(1);
                            SetBeamState(BeamState.Sustaining);
                        }
                        else
                        {
                            SetBeamLevel(Mathf.Clamp(fadeInAmount, 0, 1));
                        }
                    }
                    else
                    {
                        SetBeamState(BeamState.FadingOut);
                    }

                    break;

                case BeamState.FadingOut:

                    if (triggered)
                    {
                        SetBeamState(BeamState.FadingIn);
                    }
                    else
                    {
                        float fadeOutAmount = (Time.time - beamStateStartTime) / beamFadeOutTime;
                        if (fadeOutAmount > 1)
                        {
                            SetBeamLevel(0);
                            SetBeamState(BeamState.Off);
                        }
                        else
                        {
                            SetBeamLevel(Mathf.Clamp(1 - fadeOutAmount, 0, 1));
                        }
                    }

                    break;

                case BeamState.Sustaining:

                    if (triggered)
                    {
                        SetBeamLevel(1);
                    }
                    else
                    {
                        SetBeamState(BeamState.FadingOut);
                    }

                    break;

                case BeamState.Off:

                    if (triggered && !isPulsed)
                    {
                        SetBeamState(BeamState.FadingIn);
                    }
                    else
                    {
                        SetBeamLevel(0);
                    }

                    break;

                case BeamState.Pulsing:

                    float pulseAmount = (Time.time - beamStateStartTime) / pulseDuration;
                    if (pulseAmount > 1)
                    {
                        SetBeamState(BeamState.Off);
                        SetBeamLevel(0);
                    }
                    else
                    {
                        SetBeamLevel(pulseCurve.Evaluate(pulseAmount));
                    }
                    break;
            }

            if (beamState != BeamState.Off)
            {
                float length = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
                float scrollSpeed = beamScrollSpeed.x;
                float nextTiling = beamTiling.x * length;

                lineRenderer.material.SetTextureOffset(beamRendererTextureKey, new Vector2((Time.time * scrollSpeed) % 1.0f, 0f));
                lineRenderer.material.SetTextureScale(beamRendererTextureKey, new Vector2(nextTiling, 1));
            }

            if (beamState != BeamState.Off)
            {
                onBeamActive.Invoke();
            }

            if (beamState != BeamState.Off)
            {
                HitScan(beamState != BeamState.FadingOut);
            }

            timeBasedDamageHealing = true;

            if (beamActiveAudio != null)
            {
                beamActiveAudio.volume = beamLevel * maxBeamActiveAudioVolume;
            }
        }
    }

}
