using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class NexusInfo : MonoBehaviour
{
    [Header("Properties")]
    public int maxHp;
    public int curHp;
    public int maxPop;
    public int curPop;
    public int curMoney;

    private bool isDead;

    private void Start()
    {
        maxHp = curHp = DataManager.NexusHp;
        maxPop = curPop = DataManager.NexusPop;
        curMoney = 0;

        isDead = false;
    }

    public void GetDamage(int d)
    {
        curHp -= d;
        Debug.Log("공격받고 있습니다.");
        if (curHp < 0)
        {
            GeneralManager.Instance.inGameManager.GameOver();
            
            Time.timeScale = 0f;
        }
}

    public bool UsePop(int p)
    {
        if (curPop - p < 0)
        {
            Debug.Log("인구수가 부족합니다.");

            return false;
        }

        curPop -= p;
        return true;
    }

    public void RechargePop(int p)
    {
        curPop += p;
    }

    public bool UpgradeTower(int m, int p)
    {
        if (curMoney - m < 0 || curPop - p < 0)
        {
            Debug.Log("돈이나 인구수가 부족합니다.");

            return false;
        }
        
        curPop -= p;
        curMoney -= m;

        Debug.Log("구매 완료");

        return true;
    }

    private void CheckIsDead()
    {
        if (curHp <= 0)
        {
            isDead = true;
            GeneralManager.Instance.inGameManager.GameOver();
        }
    }
}
