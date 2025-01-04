using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
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
