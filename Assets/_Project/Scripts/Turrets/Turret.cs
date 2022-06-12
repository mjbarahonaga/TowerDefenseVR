using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Turret : MonoBehaviour
{
    public DataBullet Data;

    public float HowManyShotsBySecond = 1;

    public Transform PositionExitBullets;

    public List<EnemyBehaviour> Targets;

    private UniTask _task;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyBehaviour enemy))
        {
            Targets.Add(enemy);
        }
         return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyBehaviour enemy))
        {
            if(Targets.Contains(enemy)) Targets.Remove(enemy);
        }
        return;
    }

    async UniTask MyUpdate()
    {
        while (true)
        {
            if(Targets.Count > 0)
            {
                if (Shot())
                {
                    await UniTask.Delay((int)(1000f / HowManyShotsBySecond));
                }
            }
            else await UniTask.WaitForEndOfFrame(this);
        }
    }

    private void Start()
    {
        _task = MyUpdate();
        
       
    }

    private void OnDestroy()
    {
        
    }

    private bool Shot()
    {

        int length = Targets.Count;
        Transform enemy = null;
        for (int i = 0; i < length; i++)
        {
            if (Targets[i].IsDie)
            {
                Targets.Remove(Targets[i]);
                continue;
            }
            if (Targets[i].isActiveAndEnabled)
            {
                enemy = Targets[i].transform;
                break;  
            }
        }
        if (enemy == null) return false;

        var bullet = GameManager.Instance.GetFromPool(TypePool.Default_Bullets_Pool).GetComponent<BulletBehaviour>();
        bullet.gameObject.SetActive(true);
        bullet.Init(Data, PositionExitBullets, enemy);
        return true;
    }
}
