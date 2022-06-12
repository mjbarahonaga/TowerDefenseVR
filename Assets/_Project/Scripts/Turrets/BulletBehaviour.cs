
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

public class BulletBehaviour : MonoBehaviour, ISpawnable
{
    public DataBullet Data;

    private Transform _myTransform;
    private TypePool _pool;
    private int _damage;
    private int _speed;
    private float _lifeTime;
    private CoroutineHandle _coroutine;

    public TypePool MyPool { get => _pool; set => _pool = value; }

    private void Awake()
    {
        SetUp(Data);
        _myTransform = GetComponent<Transform>();
    }

    public void Init(DataBullet data, Transform from, Transform To)
    {
        SetUp(data);
        _myTransform.position = from.position;
        _myTransform.LookAt(To);

        _coroutine = Timing.RunCoroutine(Movement(),Segment.LateUpdate);
    }

    public void SetUp(DataBullet data)
    {
        _pool = data.TypePool;
        _damage = data.Damage;
        _speed = data.Speed;
        _lifeTime = data.LifeTime; 
    }

    IEnumerator<float> Movement()
    {
        float time = Time.time + _lifeTime;
        Vector3 forward = _myTransform.forward;
        while (time > Time.time)
        { 
            _myTransform.position += forward * _speed * Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        Deactivated();
    }

    private void Deactivated()
    {
        GameManager.Instance.ReturnToPool(MyPool, this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Deactivated();
        if(other.TryGetComponent(out EnemyBehaviour enemy))
        {
            enemy.TakeDamage(_damage);
            Timing.KillCoroutines(_coroutine);

        }
    }
}
