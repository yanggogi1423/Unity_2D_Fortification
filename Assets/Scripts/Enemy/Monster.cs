using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public bool isFinal;
    
    [Header("Events")]
    public UnityEvent onAttack = new UnityEvent();
    
    [Header("Properties")]
    [SerializeField] private int maxHp;
    [SerializeField] private int curHp;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int level; //  아직 사용 안함 (레벨링 없음)
    [SerializeField] public bool isFly;

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

    [Header("HealthBar")]
    public GameObject healthBarPrefab;
    public Image healthProgress;
    private Transform healthBarTransform;

    [Header("Health Bar Settings")] [Tooltip("체력바의 Y 위치 오프셋을 설정")]
    [SerializeField] private float healthBarYOffset = 5f;

    //  초기 값 0, 최댓 값 3 (Handling은 공격자 스크립트에서)
    [Header("Targeted")] public int isTargeted;
    
    protected virtual void Start()
    {
        if (pathTags.Length == 0)
        {
            Debug.Log("Fatal Error : Path tag slots are empty.");
        }

        finalVector = finalTarget.transform.position - transform.position;
        FindPath();

        //  기본적으로 레벨은 1 -> 추후에 웨이브나 스테이지에 맞추어 변경할 수 있도록.
        level = 1;

        //  애니메이터
        animator = GetComponent<Animator>();

        isLeft = false;
        isSide = false;
        isUp = false;
        isAttack = false;
        isDead = false;

        isFinal = false;
        
        isTargeted = 0;
        
        //  InGameManager에 추가
        GeneralManager.Instance.inGameManager.enemies.Add(gameObject);
        
        //  HealthBar Prefab Instantiate
        if (healthBarPrefab != null)
        {
            GameObject healthBarInstance = Instantiate(healthBarPrefab);
            healthBarTransform = healthBarInstance.transform;

            healthProgress = healthBarInstance.transform.Find("Progress").GetComponent<Image>();
            
            //  this를 부모로 설정
            healthBarTransform.SetParent(transform, false);

            RectTransform rect = healthBarTransform.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.anchoredPosition = new Vector2(0, healthBarYOffset);
            
            // 체력바가 항상 카메라를 향하도록 설정
            healthBarInstance.AddComponent<BillBoard>();
        }
        else
        {
            Debug.LogError("HealthBar Error");
        }
    }

    private void Update()
    {
        if (!isDead)
        {
            if (!isAttack)
            {
                if (CalDist(target.transform) < 0.45f)
                {
                    FindPath();
                }
            }
        }
    }
    
    protected virtual void FixedUpdate()
    {
        CheckDirection();
        
        Bounds tilemapBounds = 
            GeneralManager.Instance.inGameManager.map.GetComponent<Renderer>().bounds;
            
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, tilemapBounds.min.x +0.5f, tilemapBounds.max.x -0.5f),
            Mathf.Clamp(transform.position.y, tilemapBounds.min.y+0.5f, tilemapBounds.max.y-0.5f),
            transform.position.z
        );
        
        if (isFinal)
        {
            curVector = finalTarget.transform.position - transform.position;
        }
        else
        {
            curVector = target.transform.position - transform.position;
        }
        
        if (!isDead)
        {
            if (!isAttack)
            {
                Move();
            }
        }
    }

    private void Move()
    {
        //  공격 중이지 않을 때만 이동
        if (!isAttack)
        {
            float tmp = transform.position.z;
            Vector2 newVector = ((Vector2)curVector).normalized * moveSpeed;
            transform.position += new Vector3(newVector.x, newVector.y, 0);
        }
    }

    private void CheckDirection()
    {
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
            if (!isDead)
            {
                healthBarTransform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
        else if (angle >= 135f && angle < 225f)
        {
            // 왼쪽 방향
            isLeft = true;
            isSide = true;
            transform.localScale = new Vector3(1, 1, 1);
            if (!isDead)
            {
                healthBarTransform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
        else if (angle >= 225f && angle < 315f)
        {
            isUp = false;
            transform.localScale = new Vector3(1, 1, 1);
            if (!isDead)
            {
                healthBarTransform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            // 오른쪽 방향 (315~360, 0~45)
            isSide = true;
            transform.localScale = new Vector3(-1, 1, 1);
            if (!isDead)
            {
                healthBarTransform.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            }
        }
        
        animator.SetBool("isSide", isSide);
        animator.SetBool("isUp", isUp);
    }

    
    protected void Die()
    {
        isDead = true;

        GetComponent<PolygonCollider2D>().enabled = false;
        
        // 체력바 제거
        if (healthBarTransform != null)
        {
            Destroy(healthBarTransform.gameObject);
        }
        
        GeneralManager.Instance.inGameManager.enemies.Remove(gameObject);
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
        if (isFinal)
        {
            curVector = finalTarget.transform.position - transform.position;
            return;
        }
        
        //  final Vector 업데이트
        finalVector = finalTarget.transform.position - transform.position;

        float minDist = CalDist(finalTarget.transform);
        int idx = -1;

        for (int i = 0; i < pathTags.Length; i++)
        {
            if (Vector3.Dot((pathTags[i].transform.position - transform.position).normalized, finalVector.normalized) >
                0f)
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
            isFinal = true;
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
        return Vector2.Distance(t.position, transform.position);
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

    public void GetDamage(int d)
    {
        curHp -= d;

        if (healthProgress != null)
        {
            float ratio = curHp / (float)maxHp;
            healthProgress.fillAmount = ratio;
        }
        
        if (curHp <= 0)
        {
            Die();
        }
    }
    
    protected virtual void Attack()
    {
        // 기본 공격 로직
    }
    
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 현재 타겟이 설정되어 있는 경우, 타겟으로 가는 선 생성
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
    
}
