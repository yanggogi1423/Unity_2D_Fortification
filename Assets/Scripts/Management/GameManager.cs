using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("Main")] 
    public Button startButton;

    public Button exitButton;
    
    
    
    private void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.Main, true);
        
    }

}
