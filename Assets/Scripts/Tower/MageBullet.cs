using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Quaternion = UnityEngine.Quaternion;

public class MageBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attributes")] 
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private int bulletDamage = 30;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform bulletSpawnPoint;
    private bool isExplode;

    [Header("Explode")] 
    public GameObject explode;

    public Transform target;
    private Vector2 direction;
    
    // 방향을 설정하는 메서드
    //  방향 보정하는 방향으로 변경
    public void SetDirection(Transform other, Vector2 dir)
    {
        isExplode = false;
        target = other;
        direction = dir;
        StartCoroutine(DestroyObjectIfNotHit());
    }

    private void FixedUpdate()
    {
        if (direction == Vector2.zero || isExplode) return;
        if (target.GetComponent<Monster>().isDead || target == null)
        {
            return;
        }
        
        direction = (target.transform.position - transform.position);
        rb.velocity = direction.normalized * bulletSpeed;

        // 총알의 회전 설정 (방향 벡터 기준)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update()
    {
        if (target.GetComponent<Monster>().isDead || target == null)
        {
            StartCoroutine(DestroyCoroutine());
        }
    }

    private IEnumerator DestroyObjectIfNotHit()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    // 충돌 시 총알 파괴
    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster mob = other.gameObject.GetComponent<Monster>();
        if (mob != null)
        {
            //  오류 방지용 (이중 방지)
            isExplode = true;
            rb.velocity = Vector2.zero;
            Collider2D[] monsters = Physics2D.OverlapCircleAll(GetComponent<Rigidbody2D>().position, attackRange);
        
            
            explode.SetActive(true);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            
            foreach (var monster in monsters)
            {
                if (monster.CompareTag("Enemy") && !monster.GetComponent<Monster>().isFly)
                {
                    monster.GetComponent<Monster>().GetDamage(bulletDamage);
                }
            }
            
            StartCoroutine(DestroyCoroutine());
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }
}
