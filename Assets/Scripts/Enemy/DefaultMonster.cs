using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMonster : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Attack();
        
    }

    private void Attack()
    {
        if (attackRange > CalDist(finalTarget.transform))
        {
            if (!isAttack)
            {
                Debug.Log("충돌");
                isAttack = true;
                animator.SetBool("isMove", false);
                animator.SetBool("isAttack", isAttack);
                attackCoroutine = StartCoroutine(AttackCoroutine(GeneralManager.Instance.inGameManager.nexus));
            }
            // else
            // {
            //     Debug.Log("충돌 해제");
            //     isAttack = false;
            //     animator.SetBool("isMove", true);
            //     animator.SetBool("isAttack", isAttack);
            //     
            //     StopCoroutine(attackCoroutine);
            // }
        }
        else
        {
            if (isAttack)
            {
                Debug.Log("충돌 해제");
                isAttack = false;
                animator.SetBool("isMove", true);
                animator.SetBool("isAttack", isAttack);
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
            }
        }
    }
    
    //  Collision & Attack
    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (!isAttack)
    //     {
    //         var tmp = other.gameObject.GetComponent<NexusInfo>();
    //         if (tmp != null)
    //         {
    //             Debug.Log("충돌");
    //             curVector = Vector3.zero;
    //             isAttack = true;
    //             animator.SetBool("isMove", false);
    //             animator.SetBool("isAttack", isAttack);
    //             attackCoroutine = StartCoroutine(AttackCoroutine(tmp));
    //         }
    //     }
    // }

    private IEnumerator AttackCoroutine(NexusInfo n)
    {
        Debug.Log("공격 코루틴 시작");
        while (true)
        {
            if (isDead)
            {
                yield break;
            }
            n.GetDamage(damage);
            yield return new WaitForSeconds(1.17f);
        }
    }

    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if (attackCoroutine != null)
    //     {
    //         isAttack = false;
    //         animator.SetBool("isAttack",isAttack);
    //         StopCoroutine(attackCoroutine);
    //     }
    // }
}
