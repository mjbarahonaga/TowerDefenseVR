using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretLvl.asset", menuName = "TowerDefense/Turret LVL Configuration")]
public class DataLevelTurret : ScriptableObject
{
    public int Price;
    public DataBullet DataBullet;
    public Material MaterialTurret;
}
