
using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Turret : MonoBehaviour
{

    public List<DataLevelTurret> LVLs = new List<DataLevelTurret>();
    private DataLevelTurret _currentLVL;

    public DataBullet Data;

    public float HowManyShotsBySecond = 1;

    public Transform PositionExitBullets;

    public List<EnemyBehaviour> Targets;

    private MeshRenderer _renderer;

    private CoroutineHandle _coroutine;

    public BuyOptions RefBuyOptions;

    int _currentLevelIndex;

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

    IEnumerator<float> MyUpdate()
    {
        while (true)
        {
            if(Targets.Count > 0)
            {
                if (Shot())
                {
                    yield return Timing.WaitForSeconds(1f / HowManyShotsBySecond);
                }
            }
            yield return Timing.WaitForOneFrame;
        }
    }
    private void Awake()
    {
        
        _renderer = GetComponent<MeshRenderer>();
        
    }

    private void OnEnable()
    {
        _currentLevelIndex = 0;
        _currentLVL = LVLs[_currentLevelIndex];
        SetUp(_currentLVL);
        if(!RefBuyOptions) RefBuyOptions = transform.parent.gameObject.GetComponentInChildren<BuyOptions>();
        RefBuyOptions?.UpdatePrice(LVLs[_currentLevelIndex + 1]);    // Next LVL
        RefBuyOptions.transform.parent.gameObject.SetActive(false);
        RefBuyOptions.gameObject.SetActive(true);
        GameManager.OnFinish += ResetTurret;
        _coroutine = Timing.RunCoroutine(MyUpdate(), Segment.SlowUpdate);
    }

    

    private void OnDisable()
    {
        RefBuyOptions?.UpdatePrice(LVLs[0]);
        GameManager.OnFinish -= ResetTurret;
    }

    public void ResetTurret() => transform.parent.gameObject.SetActive(false);


    private void OnDestroy()
    {
        Timing.KillCoroutines(_coroutine);
    }

    private bool Shot()
    {

        int length = Targets.Count;
        Transform enemy = null;
        for (int i = 0; i < length; ++i)
        {
            if (Targets[i].IsDie == true)
            {
                Targets.Remove(Targets[i]);
                --length;
                continue;
            }
            if (Targets[i].isActiveAndEnabled)
            {
                enemy = Targets[i].transform;
                break;  
            }
        }
        if (enemy == null) return false;
        transform.LookAt(enemy);
        var bullet = GameManager.Instance.GetFromPool(TypePool.Default_Bullets_Pool).GetComponent<BulletBehaviour>();
        bullet.gameObject.SetActive(true);
        bullet.Init(Data, PositionExitBullets, enemy);
        return true;
    }

    private void SetUp(DataLevelTurret lvl)
    {
        _renderer.material = lvl.MaterialTurret;
        Data = lvl.DataBullet;
        _currentLVL = lvl;
    }

    public void NextLevel()
    {
        int limit = LVLs.Count - 1;
        if (_currentLevelIndex > limit) return;

        ++_currentLevelIndex;

        if(_currentLevelIndex != limit) RefBuyOptions?.UpdatePrice(LVLs[_currentLevelIndex + 1]);
        else RefBuyOptions.gameObject.SetActive(false);

        RefBuyOptions.transform.parent.gameObject.SetActive(false);

        SetUp(LVLs[_currentLevelIndex]);

    }
}
