using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyEffect : MonoBehaviour
{
    private ParticleSystem enemyDestroyEffectParticleSystem;

    private void Awake()
    {
        enemyDestroyEffectParticleSystem = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Set Ammo Hit Effect from passed in AmmoHitEffectSO details
    /// </summary>
    public void SetEnemyDestroyEffect(EnemyDestroyEffectSO enemyDestroyEffect)
    {
        // Set hit effect color gradient
        SetEnemyDestroyEffectColorGradient(enemyDestroyEffect.colorGradient);

        // Set hit effect particle system starting values
        SetEnemyDestroyEffectParticleStartingValues(enemyDestroyEffect.duration, enemyDestroyEffect.startParticleSize, enemyDestroyEffect.startParticleSpeed,enemyDestroyEffect.startLifetime, enemyDestroyEffect.effectGravity, enemyDestroyEffect.maxParticleNumber);

        // Set hit effect particle system particle burst particle number
        SetEnemyDestroyEffectParticleEmission(enemyDestroyEffect.emissionRate, enemyDestroyEffect.burstParticleNumber);

        // Set hit effect particle sprite
        SetEnemyDestroyEffectParticleSprite(enemyDestroyEffect.sprite);


        SetEnemyDestroyEffectVelocityOverLifeTime(enemyDestroyEffect.velocityOverLifetimeMin, enemyDestroyEffect.velocityOverLifetimeMax);

    }

    /// <summary>
    /// Set the hit effect particle system color gradient
    /// </summary>
    private void SetEnemyDestroyEffectColorGradient(Gradient gradient)
    {
        // Set colour gradient
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = enemyDestroyEffectParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = gradient;
    }


    /// <summary>
    /// Set hit effect particle system starting values
    /// </summary>
    private void SetEnemyDestroyEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startLifetime, float effectGravity, int maxParticles)
    {
        ParticleSystem.MainModule mainModule = enemyDestroyEffectParticleSystem.main;

        // Set particle system duration
        mainModule.duration = duration;

        // Set particle start size
        mainModule.startSize = startParticleSize;

        // Set particle start speed
        mainModule.startSpeed = startParticleSpeed;

        // Set particle start lifetime
        mainModule.startLifetime = startLifetime;

        // Set particle starting gravity
        mainModule.gravityModifier = effectGravity;

        // Set max particles
        mainModule.maxParticles = maxParticles;
    }

    /// <summary>
    /// Set hit effect particle system particle burst particle number
    /// </summary>
    private void SetEnemyDestroyEffectParticleEmission(int emissionRate, float burstParticleNumber)
    {
        ParticleSystem.EmissionModule emissionModule = enemyDestroyEffectParticleSystem.emission;

        // Set particle burst number
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);

        // Set particle emission rate
        emissionModule.rateOverTime = emissionRate;
    }

    /// <summary>
    /// Set hit effect particle system sprite
    /// </summary>
    private void SetEnemyDestroyEffectParticleSprite(Sprite sprite)
    {
        // Set particle burst number
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = enemyDestroyEffectParticleSystem.textureSheetAnimation;

        textureSheetAnimationModule.SetSprite(0, sprite);
    }

    /// <summary>
    /// Set the hit effect velocity over lifetime
    /// </summary>
    private void SetEnemyDestroyEffectVelocityOverLifeTime(Vector3 minVelocity, Vector3 maxVelocity)
    {
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = enemyDestroyEffectParticleSystem.velocityOverLifetime;

        // Define min max X velocity
        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveX.constantMin = minVelocity.x;
        minMaxCurveX.constantMax = maxVelocity.x;
        velocityOverLifetimeModule.x = minMaxCurveX;

        // Define min max Y velocity
        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = minVelocity.y;
        minMaxCurveY.constantMax = maxVelocity.y;
        velocityOverLifetimeModule.y = minMaxCurveY;

        // Define min max Z velocity
        ParticleSystem.MinMaxCurve minMaxCurveZ = new ParticleSystem.MinMaxCurve();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = minVelocity.z;
        minMaxCurveZ.constantMax = maxVelocity.z;
        velocityOverLifetimeModule.z = minMaxCurveZ;

    }
}
