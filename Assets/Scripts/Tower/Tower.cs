using System;
using System.Collections;
using System.Dynamic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Type")] [Tooltip("0 : 아처, 1 : 메이지")]
    [SerializeField]
    private int towerType;
    
    [Header("Properties")]
    public int level;

    public int maxLevel = 4;
    public int gold;
    public int totalPop;
    public int pop;

    [Header("Nexus")] private NexusInfo nexus;

    [Header("Archers")] 
    [SerializeField] private GameObject[] lv1Archers;
    [SerializeField] private GameObject[] lv2Archers;
    [SerializeField] private GameObject[] lv3Archers;
    [SerializeField] private GameObject[] lv4Archers;

    [Header("First Build")] 
    public bool isFirstBuild = true;

    private GameObject[][] archerPointer = new GameObject[4][];
    
    //  Cost 정보
    public int[] costList0 = { 10, 20, 30, 40 };
    public int[] costList1 = { 10, 20, 30, 40 };
    
    //  인구수 정보
    public int[] popList0 = { 1, 2, 3, 4 };
    public int[] popList1 = { 2, 4, 6, 8 };

    private Animator animator;
    
    public void Start()
    {
        level = 0;
        pop = 0;
        totalPop = 0;
        gold = 0;

        animator = GetComponent<Animator>();

        nexus = GeneralManager.Instance.inGameManager.nexus;
        
        //  Archer Pointer
        archerPointer[0] = lv1Archers;
        archerPointer[1] = lv2Archers;
        archerPointer[2] = lv3Archers;
        archerPointer[3] = lv4Archers;

        isFirstBuild = false;
        
        Upgrade();
    }

    private void SetArchers()
    {
        foreach (var a in archerPointer[level - 1])
        {
            a.SetActive(true);
            if (towerType == 0)
            {
                a.GetComponent<Archer>().Start();
            }
            else if (towerType == 1)
            {
                a.GetComponent<Mage>().Start();
            }
        }
    }

    public void Upgrade()
    {
        if (level >= maxLevel)
        {
            Debug.Log("Max Level");
            return;
        }
        
        if (towerType == 0)
        {
            if (nexus.UseMoneyAndPop(costList0[level], popList0[pop]))
            {
                Debug.Log("Upgrade " + costList0[level] + " " + popList0[level]);
                gold += costList0[level];
                totalPop = popList0[pop];
                level++;
                pop++;
                
                //  원래 pop을 돌려놓는다 (nexus)
                if (pop > 0)
                {
                    nexus.RechargePop(popList0[pop - 1]);
                }
            
                animator.SetInteger("level", level);

                StartCoroutine(UpgradeCoroutine());
            }
        }
        else if (towerType == 1)
        {
            if (nexus.UseMoneyAndPop(costList1[level], popList1[pop]))
            {
                Debug.Log("Upgrade " + costList1[level] + " " + popList1[level]);
                gold += costList1[level];
                totalPop = popList1[pop];
                level++;
                pop++;
                
                //  원래 pop을 돌려놓는다 (nexus)
                if (pop > 0)
                {
                    nexus.RechargePop(popList0[pop - 1]);
                }
            
                animator.SetInteger("level", level);

                StartCoroutine(UpgradeCoroutine());
            }
        }
    }

    private IEnumerator UpgradeCoroutine()
    {
        if (level < 3)
        {
            DisableAllUnit();
            yield return new WaitForSeconds(1.35f);
            SetArchers();
        }
        else
        {
            DisableAllUnit();
            yield return new WaitForSeconds(2.1f);
            SetArchers();
        }
    }

    public void DisableAllUnit()
    {
        for (int i = 0; i < archerPointer.Length; i++)
        {
            foreach (var a in archerPointer[i])
            {
                a.SetActive(false);
            }
        }
    }

    public void Reset()
    {
        level = 0;
        pop = 0;
        
        DisableAllUnit();
        
        GeneralManager.Instance.inGameManager.nexus.RefundTower(gold, totalPop);
        gold = 0;
        totalPop = 0;

    }
}
