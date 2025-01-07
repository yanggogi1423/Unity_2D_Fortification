using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Main Button")] public Button mainButton;

    [Header("Stage Button")] public Button [] stages;
    private void Start()
    {
        mainButton.onClick.AddListener(()=>SceneController.ChangeScene("Main"));

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM(AudioManager.Bgm.StageSelect, true);    
        }

        for (int i = 0; i < stages.Length; i++)
        {
            int tmp = i;
            stages[i].onClick.AddListener(()=>StageSelect(tmp));
        }
        
        //  Handle
        for (int i = 0; i < stages.Length; i++)
        {
            if (DataManager.CurStage < i)
            {
                stages[i].interactable = false;
            }
        }
    }

    public void StageSelect(int i)
    {
        SceneController.ChangeScene("Stage"+(i + 1));
    }
}
