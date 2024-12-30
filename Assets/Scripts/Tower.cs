using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Properties")]
    public int level;

    public int pop;

    [Header("Nexus")] private NexusInfo nexus;
    
    //  Cost 정보
    public int[] costList = { 10, 20, 30, 40 };
    
    //  인구수 정보
    public int[] popList = { 1, 2, 3, 4 };
    
    private void Start()
    {
        level = 0;
        pop = 0;

        nexus = GeneralManager.Instance.inGameManager.nexus;
    }

    public void Upgrade()
    {
        if (nexus.UpgradeTower(costList[level], popList[pop]))
        {
            level++;
            pop++;
        }
    }
}
