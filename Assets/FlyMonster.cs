using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMonster : Monster
{
    [SerializeField] private GameObject explode;
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
                
                Debug.Log("공격 코루틴 시작");
        
                GeneralManager.Instance.inGameManager.nexus.GetDamage(damage);
                if (explode != null)
                {
                    explode.SetActive(true);
                }
                Die();
            }
        }
    }
}
