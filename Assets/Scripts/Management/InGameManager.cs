using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InGameManager : MonoBehaviour
{
    [Header("Nexus")] public NexusInfo nexus;

    [Header("Tower")] public Tower[] towers;

    [Header("Spawner")] public MonsterSpawner[] spawners;

    [Header("Enemies")] public List<GameObject> enemies = new List<GameObject>();

    [Header("Map")] public Tilemap map;

    [Header("UI Elements")] public TMP_Text goldText;
    public TMP_Text popText;
    
    private void Awake()
    {
        //  스테이지 정보 Set
        StageInfoManager.StageInfoSet();

        //  General Manager와 연결
        GeneralManager.Instance.SetInGameManager();
        
        //  Nexus 찾기
        nexus = GameObject.Find("Nexus").GetComponent<NexusInfo>();
        
        //  Map 찾기
        if (map == null) map = GameObject.Find("ground").GetComponent<Tilemap>();
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.Stage, true);
    }

    public void StartWave()
    {
        foreach (var s in spawners)
        {
            s.StartSpawnMonster();
        }
    }

    public void StopWave()
    {
        foreach (var s in spawners)
        {
            s.StopSpawnMonster();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    //  For Debug
    public void UiUpdate()
    {
        goldText.SetText("" + nexus.curMoney);
        popText.SetText(nexus.curPop + " / " + nexus.maxPop);
    }
}
