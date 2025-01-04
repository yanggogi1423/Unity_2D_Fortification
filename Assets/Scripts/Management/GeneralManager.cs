using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : Singleton<GeneralManager>
{
    //  Global Manager
    public GameManager gameManager;
    
    //  Local Manager
    public InGameManager inGameManager;
    public TowerManager towerManager;

    public void SetGlobalManager()
    {
        gameManager = GameManager.Instance;
    }

    public void SetInGameManager()
    {
        inGameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();
        towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
    }
}
