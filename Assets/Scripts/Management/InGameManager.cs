using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [Header("Nexus")] public NexusInfo nexus;

    [Header("Tower")] public Tower[] towers;
    
    void Awake()
    {
        //  스테이지 정보 Set
        StageInfoManager.StageInfoSet();

        //  General Manager와 연결
        GeneralManager.Instance.SetInGameManager();
        
        //  Nexus 찾기
        nexus = GameObject.Find("Nexus").GetComponent<NexusInfo>();
        
        //  타워 찾기
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    private void FixedUpdate()
    {
        
    }

    private void CheckTowerClick()
    {
        
    }
}
