using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerWrapper : MonoBehaviour
{
    public CircleCollider2D circleCollider2D;
    public SpriteRenderer spriteRenderer;

    [Header("UI Elements")] public GameObject towerCanvas;
    public GameObject archerBuild;
    public GameObject mageBuild;
    public GameObject buyButton;
    public GameObject cancelButton;

    [Header("Info Text")] public GameObject infoWrapper;
    public TMP_Text towerName;
    public TMP_Text towerDamage;
    public TMP_Text towerRange;
    public TMP_Text towerAtkType;

    [Header("For In Game")] public GameObject upgradeButton;
    public GameObject sellButton;
    public Image upgradeImage;

    [Header("Tower Objects")] public GameObject mageTower;
    public GameObject archerTower;

    [Header("Properties")] public bool isBuilt;

    //  isBuilt가 false일 때 0 : 아처, 1 : 마법사
    public int curSelected;

    private void Start()
    {
        isBuilt = false;
        curSelected = -1;
    }

    public void SetSelect(bool select)
    {
        // if (!select)
        // {
        //     ClickCancelButton();
        // }
        
        towerCanvas.SetActive(select);

        if (!isBuilt)
        {
            archerBuild.SetActive(select);
            mageBuild.SetActive(select);
        }
        else
        {
            sellButton.SetActive(select);
            upgradeButton.SetActive(select);
        }
    }

    //  mode는 현재 선택된 모드이다. (0 : 아처, 1 : 마법사)
    public void ClickTowerButton(int mode)
    {
        infoWrapper.SetActive(true);
        buyButton.SetActive(true);
        cancelButton.SetActive(true);

        curSelected = mode;
        
        archerBuild.GetComponent<Button>().interactable = true;
        mageBuild.GetComponent<Button>().interactable = true;
        
        if (mode == 0)
        {
            archerBuild.GetComponent<Button>().interactable = false;
            //  TODO : Archer 타워 text
            towerName.SetText("Archer Tower");
            towerDamage.SetText("Damage : 50 X " + archerTower.GetComponent<Tower>().level);
            towerRange.SetText("Range : All");
            towerAtkType.SetText("Type : One Target");
        }
        else if (mode == 1)
        {
            mageBuild.GetComponent<Button>().interactable = false;
            //  TODO : Mage 타워 text
            towerName.SetText("Mage Tower");
            towerDamage.SetText("Damage : 30 X " + mageTower.GetComponent<Tower>().level);
            towerRange.SetText("Range : Ground");
            towerAtkType.SetText("Type : Splash");
        }
    }

    public void ClickCancelButton()
    {
        //  버튼색 원래대로
        infoWrapper.SetActive(false);
        buyButton.SetActive(false);
        cancelButton.SetActive(false);

        archerBuild.GetComponent<Button>().interactable = true;
        mageBuild.GetComponent<Button>().interactable = true;
        curSelected = -1;
        
        towerCanvas.SetActive(false);

        GeneralManager.Instance.towerManager.curTowerWrapper = null;
    }

    public void Built()
    {
        //  Collider, SR 해제
        // circleCollider2D.enabled = false;
        spriteRenderer.enabled = false;

        isBuilt = true;
        sellButton.SetActive(true);
        upgradeButton.SetActive(true);

        archerBuild.SetActive(false);
        mageBuild.SetActive(false);
        buyButton.SetActive(false);
        cancelButton.SetActive(false);

        //  Build
        if (curSelected == 0)
        {
            archerTower.SetActive(true);
            if (!archerTower.GetComponent<Tower>().isFirstBuild)
            {
                archerTower.GetComponent<Tower>().Start();
            }
        }
        else if (curSelected == 1)
        {
            mageTower.SetActive(true);
            if (!mageTower.GetComponent<Tower>().isFirstBuild)
            {
                mageTower.GetComponent<Tower>().Start();
            }
        }
    }

    public void Sell()
    {
        //  Collider, SR 복원
        // circleCollider2D.enabled = true;
        spriteRenderer.enabled = true;
        upgradeButton.GetComponent<Button>().interactable = true;

        isBuilt = false;
        sellButton.SetActive(false);
        upgradeButton.SetActive(false);

        archerBuild.SetActive(true);
        mageBuild.SetActive(true);

        if (curSelected == 0)
        {
            archerTower.GetComponent<Tower>().Reset();
            archerTower.SetActive(false);
        }
        else if (curSelected == 1)
        {
            mageTower.GetComponent<Tower>().Reset();
            mageTower.SetActive(false);
        }

        //  init
        curSelected = -1;
        archerBuild.GetComponent<Button>().interactable = true;
        mageBuild.GetComponent<Button>().interactable = true;
    }

    public void Upgrade()
    {
        if (curSelected == 0)
        {
            if (archerTower.GetComponent<Tower>().Upgrade())
            {
                StartCoroutine(UpgradeCoroutine());
            }
        }
        else if (curSelected == 1)
        {
            if (mageTower.GetComponent<Tower>().Upgrade())
            {
                StartCoroutine(UpgradeCoroutine());
            }
        }
    }

    private bool CheckMaxLevel()
    {
        if (curSelected == 0)
        {
            if (archerTower.GetComponent<Tower>().level > archerTower.GetComponent<Tower>().maxLevel)
            {
                return false;
            }
        }
        else if (curSelected == 1)
        {
            if (mageTower.GetComponent<Tower>().level > mageTower.GetComponent<Tower>().maxLevel)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator UpgradeCoroutine()
    {
        int elapsed = 0;
        upgradeImage.fillAmount = 0f;
        
        upgradeButton.GetComponent<Button>().interactable = false;
        while (true)
        {
            if (elapsed >= 100)
            {
                upgradeImage.fillAmount = 1f;
                if (CheckMaxLevel())
                {
                    upgradeButton.GetComponent<Button>().interactable = true;    
                }
                
                yield break;
            }
            elapsed++;
            upgradeImage.fillAmount = elapsed / 100f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}