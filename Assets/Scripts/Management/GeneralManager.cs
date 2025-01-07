using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : Singleton<GeneralManager>
{
    [Header("Global Manager")]
    //  Global Manager
    public GameManager gameManager;
    
    [Header("Local Manager")]
    //  Local Manager
    public InGameManager inGameManager;
    public TowerManager towerManager;
    public AlertManager alertManager;

    public void SetGlobalManager()
    {
        gameManager = GameManager.Instance;
    }

    public void SetInGameManager()
    {
        inGameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();
        towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
        alertManager = GameObject.Find("InGameManager").GetComponent<AlertManager>();
    }

    public void SetVariousManager()
    {
        
    }
}
