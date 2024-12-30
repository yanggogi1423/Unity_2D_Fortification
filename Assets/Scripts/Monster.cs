using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
 * 이동 관련 메소드 포함
 */
public class Monster : MonoBehaviour
{
    [Header("Targets")]
    public GameObject target;
    public Vector3 curVector;
    public GameObject[] pathTags;

    public GameObject finalTarget;
    public Vector3 finalVector;
    
    [Header("Events")]
    public UnityEvent onAttack = new UnityEvent();
    
    [Header("Properties")]
    [SerializeField] private int maxHp;
    [SerializeField] private int curHp;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int level;

    [Header("Animation")]
    public Animator animator;
    public bool isLeft;
    public bool isSide;
    public bool isUp;
    public bool isAttack;
    public bool isDead;

    [Header("Attack")] 
    protected Coroutine attackCoroutine;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackCool;
    [SerializeField] protected float attackRange;
    
    protected virtual void Start()
    {
        if (pathTags.Length == 0)
        {
            Debug.Log("Fatal Error : Path tag slots are empty.");
        }

        finalVector = finalTarget.transform.position - transform.position;
        FindPath();

        //  기본적으로 레벨은 1
        level = 1;

        //  애니메이터
        animator = GetComponent<Animator>();

        isLeft = false;
        isSide = false;
        isUp = false;
        isAttack = false;
        isDead = false;
    }

    private void Update()
    {
        CheckDirection();
        if (!isDead)
        {
            if (!isAttack)
            {
                if (CalDist(target.transform) < 0.01)
                {
                    FindPath();
                }
                Move();
            }

            if (curHp <= 0)
            {
                Die();
            }
        }
    }

    private void Move()
    {
        //  공격 중이지 않을 때만 이동
        if (!isAttack)
        {
            transform.position += curVector.normalized * moveSpeed;
        }
    }

    private void CheckDirection()
    {
        float angle = Mathf.Atan2(curVector.y, curVector.x) * Mathf.Rad2Deg;

        //  -180 ~ 180 범위의 각도를 0 ~ 360도로 변환
        if (angle < 0) angle += 360f;

        //  각도에 따라 상하좌우 방향 판별
        isUp = angle > 315 || angle <= 45;              
        isLeft = angle > 225 && angle <= 315;           
        isSide = (angle > 45 && angle <= 135) ||
                 (angle > 225 && angle <= 330);

        if (isLeft)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        animator.SetBool("isSide",isSide);
        animator.SetBool("isUp",isUp);
    }
    
    private void Die()
    {
        isDead = true;
        isAttack = false;
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        animator.SetBool("isDead", true);
        
        yield return new WaitForSeconds(1.1f);
        GameManager.Instance.RemoveMonsters(this);
        Destroy(gameObject);
    }

    private void FindPath()
    {
        //  final Vector 업데이트
        finalVector = finalTarget.transform.position - transform.position;

        if (target == finalTarget)
        {
            SetVector();
            return;
        }

        float minDist = CalDist(finalTarget.transform);
        int idx = -1;

        for (int i = 0; i < pathTags.Length; i++)
        {
            if (Vector3.Dot((pathTags[i].transform.position - transform.position).normalized, finalVector.normalized) >
                0)
            {
                if (target == null)
                {
                    minDist = CalDist(pathTags[i].transform);
                    idx = i;
                }
                else if (minDist > CalDist(pathTags[i].transform) && target != pathTags[i])
                {
                    minDist = CalDist(pathTags[i].transform);
                    idx = i;
                }
            }
        }

        //  만일 target이 정해지지 않았다면 무조건 final일 수 밖에 없음
        if (idx == -1)
        {
            target = finalTarget;
        }
        else
        {
            target = pathTags[idx];
        }
        SetVector();
    }

    private void SetVector()
    {
        curVector = target.transform.position - transform.position;
    }

    protected float CalDist(Transform t)
    {
        return Vector3.Distance(t.position, transform.position);
    }

    public void SetLevel(int i)
    {
        level = i;
        //  TODO : 이후에 레벨을 통한 공격력 및 방어력 공식 만들어야 한다.
    }

    public void SetPath(GameObject[] os)
    {
        pathTags = os;
    }

    public void SetFinalTarget(GameObject o)
    {
        finalTarget = o;
    }
    
    protected virtual void Attack()
    {
        // 기본 공격 로직
    }
    
}
