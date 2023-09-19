using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SingleBleed : NormalAttack
{
    [Header("Effects")]
    [SerializeField] private float bleedRate;
    [SerializeField] private int bleedTurns;

    public override void Attack(GameObject target)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        CalculateDamage(target);

        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= bleedRate)
        {
            _Bleed bleed = FindObjectOfType<_Bleed>();

            if (targetStats.bleedStack < bleed.bleedLimit)
            {
                targetStats.TakeDamage(damage, critCheck, "bleed");
                targetStats.bleedStack += bleedTurns;

                StatusEffectManager statusEffect = FindObjectOfType<StatusEffectManager>();
                statusEffect.SpawnEffectsBar(targetStats, bleedTurns, "bleed");

                if (targetStats.bleedStack > bleed.bleedLimit)
                {
                    targetStats.bleedStack = bleed.bleedLimit;
                }
            }
        }
        else
        {
            targetStats.TakeDamage(damage, critCheck, null);
        }

        critCheck = false;
    }
}