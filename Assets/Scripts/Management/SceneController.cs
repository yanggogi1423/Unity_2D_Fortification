using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

/*
 *  Scene을 String으로 인해 변하게 할 수 있게 하는 스크립트입니다.
 *  따로 Object를 만들지 않고 사용할 수 있도록, 모든 methods를 static으로 선언합니다.
 */
public class SceneController : Singleton<SceneController>
{
    
    //  딜레이가 필요한 경우를 위한 변수
    private float delay = 3f;

    //  현재 씬 정보
    public static string NowScene;
    
    //  로딩 씬 같은 경우 다음 씬을 알아야 이동할 수 있음
    public static string NextScene;
    
    //  스테이지 정보
    public static string[] stageList = { "Stage_1", "Stage_2", "Stage_3", "Ending" };
    
    // 보스 테스트
    // public static string[] stageList = { "Stage_Boss", "Stage_2" };

    //  게임 시작 여부
    private bool isStart;

    [Header("Stage")]
    public int curSelectStage;

    public void Start()
    {
        //  Main 씬이 담기게 됨.
        NowScene = SceneManager.GetActiveScene().name;
        
        isStart = false;
    }

    public void FixedUpdate()
    {


    
        // if (NowScene == null)
        // {
        //     NowScene = SceneManager.GetActiveScene().name;
        //     if (StageInfoManager.StageInit)
        //     {
        //         StageInfoManager.StageInit = false;
        //     }
        // }
    
        // if (NowScene == "GameOver")
        // {
        //     GameObject.Find("ExitProgram").GetComponent<Button>().onClick.AddListener(ExitProgram);
        // }
        
    }

    // #region ForEnterGame
    //
    // //  게임 시작만을 위한 메소드
    // public void GoToGame()
    // {
    //     AudioManager.Instance.StopAllSfx();
    //     if (!isStart)
    //     {
    //         if (Input.GetKeyDown(KeyCode.Space))
    //         {
    //             return;
    //         }
    //
    //         //  Stage Menu일 때
    //         if (GeneralManager.Instance.stageInfoManager != null)
    //         {
    //             curSelectStage = GeneralManager.Instance.stageSelectManager.GetCurSelectStage();    
    //         }
    //         
    //         isStart = true;
    //         StartCoroutine(PreGoToGameCoroutine());
    //     }
    // }
    //
    // //  Restart Game
    // public void ReStartGame()
    // {
    //     AudioManager.Instance.StopAllSfx();
    //     
    //     Time.timeScale = 1f;
    //     // Debug.Log("Restart!");
    //     isStart = false;
    //
    //     GameManager.InGame = false;
    //     GameManager.InGameInit = false;
    //     
    //     GoToGame();
    // }
    //
    // //  게임 시작 전 로딩 창을 임의로 불러옴(Space 키를 사용하지 않게 하기 위함)
    // private IEnumerator PreGoToGameCoroutine()
    // {
    //     if (GeneralManager.Instance.stageInfoManager != null)
    //     {
    //         GeneralManager.Instance.stageInfoManager.SetStageMenuHide();
    //         yield return new WaitForSeconds(1.1f);
    //     }
    //     ChangeScene("Loading");
    //     StartCoroutine(GoToGameCoroutine());
    // }
    //
    // //  로딩 창이 시작되고 나서 스테이지를 바꿈
    // private IEnumerator GoToGameCoroutine()
    // {
    //     isStart = false;
    //     
    //     yield return new WaitForSeconds(2.8f);
    //
    //     //  Ending
    //     if (curSelectStage == 3)
    //     {
    //         ChangeScene(stageList[curSelectStage]);
    //     }
    //     else
    //     {
    //         //  게임 시작 직전 초기화 설정
    //         ChangeScene(stageList[curSelectStage]);
    //         GameManager.InGameInit = true;
    //     
    //         //  배경 음악
    //         switch (curSelectStage)
    //         {
    //             case 0 :
    //                 AudioManager.Instance.PlayBGM(AudioManager.Bgm.Stage1,true);
    //                 break;
    //             case 1 :
    //                 AudioManager.Instance.PlayBGM(AudioManager.Bgm.Stage2,true);
    //                 break;
    //             case 2:
    //                 AudioManager.Instance.PlayBGM(AudioManager.Bgm.Stage3,true);
    //                 break;
    //         }
    //     
    //         //  플레이어 할당
    //         GameManager.Instance.player = GameObject.Find("Player");
    //     
    //         //  추가적인 검증은 하지 않겠음
    //     }
    // }
    // #endregion
    
    
    //  Scene을 이동하는 전역 함수
    public static void ChangeScene(string sceneName)
    {
        AudioManager.Instance.StopAllSfx();
        AudioManager.Instance.RestoreAudioMixerSettings();
        if (sceneName == "Main")
        {
            // Debug.Log("MAIN!!");
            Time.timeScale = 1f;
            
            AudioManager.Instance.PlayBGM(AudioManager.Bgm.Main, true);
        }

        // if (GameManager.InGame)
        // {
        //     GameManager.InGame = false;
        // }

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
        
        // Debug.Log("Go to " + sceneName);
        SceneManager.LoadScene(sceneName);
        NowScene = sceneName;
        
    }

    //  프로그램 종료
    public static void ExitProgram()
    {
        Application.Quit();
    }

    //  다음 씬 정보를 저장하는 함수
    // public static void SetNextScene(string name)
    // {
    //     if (GameManager.TutorialEnd)
    //     {
    //         NextScene = "StageMenu";
    //     }
    //     else
    //     {
    //         NextScene = name;
    //     }
    // }

    //  다음 씬을 불러오는 함수
    public static void LoadNextScene()
    {
        AudioManager.Instance.StopAllSfx();
        ChangeScene(NextScene);
    }
    
}
