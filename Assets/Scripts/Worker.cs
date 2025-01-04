using System.Collections;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public bool isArrived;

    public Transform nexus;
    public Transform minerals;
    public Transform target;

    [SerializeField] private int gold;
    [SerializeField] private int pop;
    
    [Header("Movement")]
    public Animator animator;
    public bool isLeft;
    public bool isSide;
    public bool isUp;
    public bool isFired;
    
    public Vector3 curVector;
    [SerializeField] private float moveSpeed;
    
    private void Start()
    {
        if (minerals == null) minerals = GameObject.Find("Mineral").transform;
        if (nexus == null) nexus = GameObject.Find("Nexus").transform;  
        
        isArrived = true;
        target = minerals;

        isFired = false;

        animator = GetComponent<Animator>();
    }

    // public void SetTargets(Transform n, Transform m)
    // {
    //     nexus
    // }

    private void FixedUpdate()
    {
        if (!isFired)
        {
            CheckPosition();
            Move();
            CheckDirection();
        }
    }

    public void Fire()
    {
        isFired = true;
        StartCoroutine(FiredCoroutine());
    }

    private IEnumerator FiredCoroutine()
    {
        animator.SetBool("isFired", true);
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    } 
    
    private float CalDist(Transform t)
    {
        return Vector2.Distance(t.position, transform.position);
    }

    private void CheckPosition()
    {
        if (CalDist(target) < 1f)
        {
            if (target == nexus)
            {
                nexus.GetComponent<NexusInfo>().GetMinerals(gold);
            }
            isArrived = !isArrived;
        }
        
        if (isArrived)
        {
            target = minerals;
        }
        else
        {
            target = nexus;
        }
    }

    
    private void Move()
    {
        curVector = target.transform.position - transform.position;
        
        float tmp = transform.position.z;
        Vector2 newVector = ((Vector2)curVector).normalized * moveSpeed;
        transform.position += new Vector3(newVector.x, newVector.y, 0);
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
        
        animator.SetBool("isSide", isSide);
        animator.SetBool("isUp", isUp);
    }
    
}
