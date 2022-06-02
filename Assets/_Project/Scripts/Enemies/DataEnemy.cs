using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data container for settings per enemy
/// </summary>
[CreateAssetMenu(fileName = "Enemy.asset", menuName = "TowerDefense/Enemy Configuration")]
public class DataEnemy : ScriptableObject
{
    public string Description;

    public int HP;

    public int Reward;

    public GameObject Prefab;
}
