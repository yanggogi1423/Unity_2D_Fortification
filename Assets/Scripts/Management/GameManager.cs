using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON

    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            //  존재하는 지 확인한다.
            _instance = FindObjectOfType<GameManager>();
            if (_instance != null) return _instance;

            //  존재하지 않는다면 생성한다.
            _instance = new GameManager().AddComponent<GameManager>();
            _instance.name = "GameManager";
        }

        return _instance;
    }

    private void Awake()
    {
        //  존재하지 않는다면 this를 GM으로
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            //  존재하지만 this가 아니라면 destroy
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] private List<Monster> monsters = new();

    public void AddMonsters(Monster m)
    {
        monsters.Add(m);
    }

    public void RemoveMonsters(Monster m)
    {
        monsters.Remove(m);
    }

}
