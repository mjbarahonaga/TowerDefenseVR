using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnemy
{
    Ghoul,
    Ghoul_Boss,
    Ghoul_Festering,
    Ghoul_Groresque,
    Ghoul_Scavenger
}

/// <summary>
/// Data container for settings per enemy
/// </summary>
[CreateAssetMenu(fileName = "Enemy.asset", menuName = "TowerDefense/Enemy Configuration")]
public class DataEnemy : ScriptableObject
{
    public TypeEnemy Type;

    public int HP;

    public float Speed;

    public int Reward;

    public GameObject Prefab;
}
