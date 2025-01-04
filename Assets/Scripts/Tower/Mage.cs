using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    [Header("Properties")] 
    [SerializeField] private float preAtkRange; //  Detection
    [SerializeField] private float atkRange;
    // [SerializeField] private int damage;

    private Animator animator;
    [SerializeField] private float attackCool;

    [Header("Movement")]
    public bool isPreAtk;
    public bool isAtk;
    public bool isSide;
    public bool isLeft;
    public bool isUp;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    private Coroutine attackCoroutine;
    
    //  Target
    public GameObject curTarget;
    public List<GameObject> enemyList;
    
    public void Start()
    {
        isPreAtk = false;
        isAtk = false;
        isSide = false;
        isUp = false;
        animator = GetComponent<Animator>();
        
        SetDirection();

        //  List Reference 가져오기
        enemyList = GeneralManager.Instance.inGameManager.enemies;

        attackCoroutine = null;
    }

    private void Update()
    {
        CheckPreDetection();
        CheckDetection();

        CheckDirection();
    }

    private void SetDirection()
    {
        animator.SetBool("isAtk", isAtk);
        animator.SetBool("isSide", isSide);
        animator.SetBool("isUp", isUp);
    }

    private void CheckPreDetection()
    {
        if (curTarget != null) return;
        
        //  가장 먼저 찾는 애를 먼저 공격
        foreach (var e in enemyList)
        {
            if (e == null) continue;
            if (CalDist(e.transform) < preAtkRange && e.GetComponent<Monster>().isTargeted < 1)
            {
                //  공중은 공격 못함
                if (e.GetComponent<Monster>().isFly)
                {
                    continue;
                }

                curTarget = e;
                isPreAtk = true;
                SetDirection();
                
                e.GetComponent<Monster>().isTargeted++;
                
                return;
            }
        }
    }

    private void CheckDetection()
    {
        if (curTarget == null) return;
        if (attackCoroutine != null) return;

        if (CalDist(curTarget.transform) < atkRange)
        {
            isAtk = true;
            SetDirection();

            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }
    }

    private void ResetTarget()
    {
        isPreAtk = false;
        isAtk = false;
        curTarget.GetComponent<Monster>().isTargeted--;
        curTarget = null;
    }

    private float CalDist(Transform t)
    {
        return Vector2.Distance(t.position, transform.position);
    }

    private void CheckDirection()
    {
        if(curTarget == null) return;
        
        Vector3 curVector = curTarget.transform.position - transform.position;
        
        float angle = Mathf.Atan2(curVector.y, curVector.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        
        isUp = false;
        isLeft = false;
        isSide = false;

        if (angle >= 45f && angle < 135f)
        {
            // 위 방향
            isUp = true;
            transform.localScale = new Vector3(1, 1, 1); 
        }
        else if (angle >= 135f && angle < 225f)
        {
            // 왼쪽 방향
            isLeft = true;
            isSide = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (angle >= 225f && angle < 315f)
        {
            isUp = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // 오른쪽 방향 (315~360, 0~45)
            isSide = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        SetDirection();
    }

    private void Attack()
    {
        if (curTarget == null) return;
        // 타겟 방향 계산
        Vector2 targetDirection = (curTarget.transform.position - transform.position);

        // 총알 생성
        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        MageBullet bulletScript = bulletObj.GetComponent<MageBullet>();
        
        Debug.Log("발사 !");

        bulletScript.SetDirection(curTarget.transform, targetDirection.normalized);
    }

    private IEnumerator AttackCoroutine()
    {
        Debug.Log("공격 시작");

        float elapsed = 3.1f;
        while (true)
        {
            if (!CheckTarget())
            {
                yield return new WaitForSeconds(elapsed);
                SetDirection();
                attackCoroutine = null;
                yield break;
            }
            isAtk = true;
            SetDirection();
            
            yield return new WaitForSeconds(0.9f);
            elapsed -= 0.9f;
            if (!CheckTarget())
            {
                yield return new WaitForSeconds(elapsed);
                SetDirection();
                attackCoroutine = null;
                yield break;
            }
            Attack();
            
            yield return new WaitForSeconds(0.15f);
            elapsed -= 0.15f;
            if (!CheckTarget())
            {
                yield return new WaitForSeconds(elapsed);
                SetDirection();
                attackCoroutine = null;
                yield break;
            }
            isAtk = false;
            SetDirection();
            
            yield return new WaitForSeconds(2f);
            elapsed -= 2f;
            if (!CheckTarget())
            {
                yield return new WaitForSeconds(elapsed);
                SetDirection();
                attackCoroutine = null;
                yield break;
            }
            elapsed = 3.1f;
        }
    }

    private bool CheckTarget()
    {
        //  현재 타겟이 없음 (오류 방지)
        if (curTarget == null)
        {
            return false;
        }

        //  몬스터가 죽음
        if (curTarget.GetComponent<Monster>().isDead)
        {
            ResetTarget();
            return false;
        }

        //  Detection 범위 밖으로 나감
        if (CalDist(curTarget.transform) > preAtkRange)
        {
            Debug.Log("벗어남");
            ResetTarget();
            return false;
        }

        return true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, preAtkRange);
        

        // 현재 타겟이 설정되어 있는 경우, 타겟으로 가는 선 생성
        if (curTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, curTarget.transform.position);
        }
    }
}
