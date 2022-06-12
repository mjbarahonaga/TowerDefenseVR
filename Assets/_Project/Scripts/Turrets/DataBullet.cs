using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet.asset", menuName = "TowerDefense/Bullet Configuration")]
public class DataBullet : ScriptableObject
{
    public TypePool TypePool;
    public int Damage;
    public int Speed;
    public float LifeTime;
}
