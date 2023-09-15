using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SingleTaunt : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] protected GameObject owner;

    public void Taunt(GameObject target)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        if (target.gameObject.CompareTag("Player"))
        {
            target.GetComponent<_PlayerAction>().selectedTarget = owner;
        }
        else if (target.gameObject.CompareTag("Enemy"))
        {
            target.GetComponent<_EnemyAction>().selectedTarget = owner;
        }

        target.GetComponent<CharacterStats>().DamageText(0, false, "taunt");
        target.GetComponent<CharacterStats>().tauntCheck = true;
    }
}
