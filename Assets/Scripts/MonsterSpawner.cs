using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;

    [SerializeField] private GameObject[] pathTags;
    [SerializeField] private GameObject finalTarget;
    [SerializeField] private float spawnRate;
    [SerializeField] private int phase;

    private Coroutine monsterCoroutine;

    private void Awake()
    {
        //  무조건 찾도록 변경
        pathTags = GameObject.FindGameObjectsWithTag("path");

        if (finalTarget == null)
        {
            finalTarget = GameObject.FindGameObjectWithTag("final tag");
        }
        
        StartSpawnMonster();
    }

    public void StartSpawnMonster()
    {
        monsterCoroutine = StartCoroutine(MonsterSpawnCoroutine());
    }

    public void StopSpawnMonster()
    {
        if (monsterCoroutine == null) return;
        StopCoroutine(monsterCoroutine);
        monsterCoroutine = null;
    }

    private IEnumerator MonsterSpawnCoroutine()
    {
        while (true)
        {
            GameObject obj = Instantiate(GetRandomMonster(), transform.position, Quaternion.identity);
            Monster mob = obj.GetComponent<Monster>();

            if (mob.isFly)
            {
                mob.transform.position -= new Vector3(0, 0, 6);
            }

            mob.SetPath(pathTags);
            mob.SetFinalTarget(finalTarget);

            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SetPhase(int level, int stage)
    {
        //  TODO : 페이즈 설정. 레벨과 스테이지 관련하여 코드를 짜야한다.
    }

    private GameObject GetRandomMonster()
    {
        return monsterPrefabs[Random.Range(0, phase)];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
