using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private int pop;
    
    public GameObject workerPrefab;

    public NexusInfo nexus;
    // public GameObject mineral;
    
    private void Awake()
    {
        
    }

    public void Employ()
    {
        if (nexus.UseMoneyAndPop(cost, pop))
        {
            GameObject obj = Instantiate(workerPrefab, transform.position, Quaternion.identity);
            Worker worker = obj.GetComponent<Worker>();
        
            // worker.SetTargets(nexus, mineral);
            nexus.workers.Enqueue(worker);

            Debug.Log("Use " +cost +" "+pop);
            
            return;
        }
        Debug.Log("돈이나 인구수가 부족합니다.");
    }

    public void Fire()
    {
        if (nexus.workers.Count <= 0)
        {
            Debug.Log("해고할 인력이 없음");
            return;
        }

        nexus.RechargePop(pop);
        nexus.workers.Dequeue().Fire();
    }
}
