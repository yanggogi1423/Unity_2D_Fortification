using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour
{
    public GameObject target;
    public Vector3 curVector;
    public GameObject[] pathTags;

    public GameObject finalTarget;
    public Vector3 finalVector;
    
    public UnityEvent onAttack = new UnityEvent();
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float damage;
    [SerializeField] private int level;
    
    private void Start()
    {
        if (pathTags.Length == 0)
        {
            Debug.Log("Fatal Error : Path tag slots are empty.");
        }

        finalVector = finalTarget.transform.position - transform.position;
        FindPath();

        //  기본적으로 레벨은 1
        level = 1;
    }
    
    private void Update()
    {
        if (CalDist(target.transform) < 0.01)
        {
            FindPath();
        }
        
        Move();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, finalTarget.transform.position) > 0.3)
        {
            transform.position += curVector.normalized * moveSpeed;
        }
        else
        {   //  임시로 작성해놓음
            Die();
        }
    }

    private void Die()
    {
        GameManager.GetInstance().RemoveMonsters(this);
        Debug.Log("Touch!");
        Destroy(gameObject);
    }

    private void FindPath()
    {
        //  final Vector 업데이트
        finalVector = finalTarget.transform.position - transform.position;

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

    private float CalDist(Transform t)
    {
        return Vector3.Distance(t.position, transform.position);
    }

    private void Attack()
    {
        
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
}
