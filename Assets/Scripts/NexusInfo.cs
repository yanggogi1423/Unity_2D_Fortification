using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NexusInfo : MonoBehaviour
{
    [Header("Properties")]
    public int maxHp;
    public int curHp;
    public int maxPop;
    public int curPop;
    public int curMoney;

    private bool isDead;
    
    [Header("HealthBar")]
    public GameObject healthBarPrefab;
    public Image healthProgress;
    private Transform healthBarTransform;
    
    //  UI
    public UIPlayerHp uiHp;

    [Header("Health Bar Settings")] [Tooltip("체력바의 Y 위치 오프셋을 설정")]
    [SerializeField] private float healthBarYOffset = 5f;
    
    [Header("Worker")]
    public Queue<Worker> workers = new Queue<Worker>();

    public GameObject nexusCanvas;

    private void Start()
    {
        maxHp = curHp = DataManager.NexusHp;
        maxPop  = DataManager.NexusPop;
        curMoney = 1000;
        curPop = 0;

        isDead = false;
        
        //  HealthBar Prefab Instantiate
        if (healthBarPrefab != null)
        {
            GameObject healthBarInstance = Instantiate(healthBarPrefab);
            healthBarTransform = healthBarInstance.transform;

            healthProgress = healthBarInstance.transform.Find("Progress").GetComponent<Image>();
            
            //  this를 부모로 설정
            healthBarTransform.SetParent(transform, false);

            RectTransform rect = healthBarTransform.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.anchoredPosition = new Vector2(0, healthBarYOffset);
            
            // 체력바가 항상 카메라를 향하도록 설정
            healthBarInstance.AddComponent<BillBoard>();
        }
        else
        {
            Debug.LogError("HealthBar Error");
        }

        if (uiHp == null)
        {
            Debug.LogError("UI 연결 안 됨");
        }
        
        //  UI Init
        uiHp.SetUIPlayerHp(curHp,maxHp);
        
        //  For Debug
        GeneralManager.Instance.inGameManager.UiUpdate();
    }

    public void GetDamage(int d)
    {
        curHp -= d;
        Debug.Log("공격받고 있습니다.");
        
        uiHp.SetUIPlayerHp(curHp,maxHp);
        
        if (healthProgress != null)
        {
            float ratio = curHp / (float)maxHp;
            healthProgress.fillAmount = ratio;
        }
        
        if (curHp < 0)
        {
            Destroy(healthBarTransform.gameObject);
            GeneralManager.Instance.inGameManager.GameOver();
            
            Time.timeScale = 0f;
        }
    }

    // public bool UsePop(int p)
    // {
    //     if (curPop + p > maxPop)
    //     {
    //         Debug.Log("인구수가 부족합니다.");
    //
    //         return false;
    //     }
    //
    //     curPop += p;
    //     GeneralManager.Instance.inGameManager.UiUpdate();
    //     return true;
    // }

    public void RechargePop(int p)
    {
        curPop -= p;
        GeneralManager.Instance.inGameManager.UiUpdate();
    }

    public bool UseMoneyAndPop(int m, int p)
    {
        if (curMoney - m < 0)
        {
            GeneralManager.Instance.alertManager.ShowAlert(0);
            return false;
        }
        if (curPop + p > maxPop)
        {
            GeneralManager.Instance.alertManager.ShowAlert(1);
            return false;
        }
        
        curPop += p;
        curMoney -= m;

        Debug.Log("구매 완료");
        GeneralManager.Instance.inGameManager.UiUpdate();

        return true;
    }
    
    public void GetMinerals(int m)
    {
        curMoney += m;
        GeneralManager.Instance.inGameManager.UiUpdate();
    }

    // public bool UseMinerals(int m)
    // {
    //     if (curMoney - m < 0)
    //     {
    //         Debug.Log("미네랄이 부족합니다.");
    //         return false;
    //     }
    //     
    //     curMoney -= m;
    //     GeneralManager.Instance.inGameManager.UiUpdate();
    //
    //     return true;
    // }

    private void CheckIsDead()
    {
        if (curHp <= 0)
        {
            isDead = true;
            GeneralManager.Instance.inGameManager.GameOver();
        }
    }

    public void RefundTower(int gold, int pop)
    {
        //  70%만 환불
        curMoney += (int)(gold * 0.7f);
        curPop -= pop;
        GeneralManager.Instance.inGameManager.UiUpdate();
    }
    
    //  WorldSpace UI
    public void SetVisibleCanvas(bool show)
    {
        nexusCanvas.SetActive(show);
    }
}
