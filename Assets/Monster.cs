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
    
    
    private void Start()
    {
        if (pathTags.Length == 0)
        {
            Debug.Log("Fatal Error : Path tag slots are empty.");
        }

        finalVector = finalTarget.transform.position - transform.position;
        FindPath();
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
}
