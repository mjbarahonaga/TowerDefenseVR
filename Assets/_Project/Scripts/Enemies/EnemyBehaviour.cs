using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.Assertions;

public class EnemyBehaviour : MonoBehaviour, ISpawnable
{

    public DataEnemy Data;
    [ReadOnly, SerializeField] public bool Initialising = true;

    #region Private Variables
    [ReadOnly, SerializeField] private NavMeshAgent _navMeshAgent;
    [ReadOnly, SerializeField] private Animator _animator;
    [ReadOnly, SerializeField] private Vector3 _target;

    [ReadOnly, SerializeField] private int _idSpawn;
    [ReadOnly, SerializeField] private int _idWalk;
    [ReadOnly, SerializeField] private int _idDie;
    public Transform _myTransform;
    public Transform _myParentTransform;

    private int _reward;
    private int _currentHP;

    private TypePool _typePool;
    public TypePool MyPool { get => _typePool; set => _typePool = value; }
    #endregion

    private void Awake()
    {
        SetUpEnemy();
    }

    private void OnValidate()
    {
        Utils.ValidationUtility.SafeOnValidate(() =>
        {
            if (this == null) return;
            
            if(_navMeshAgent == null) _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            if(_animator == null) _animator = GetComponentInChildren<Animator>();
            if (_animator) 
            {
                _idSpawn = Animator.StringToHash("Spawn");
                _idWalk = Animator.StringToHash("Walk");
                _idDie = Animator.StringToHash("Die");
            }
            _myTransform.localPosition = Vector3.zero;
            SetUpEnemy();
        });
    }

    public void SetUpEnemy()
    {

        _navMeshAgent.speed = Data.Speed;
        _reward = Data.Reward;
        _currentHP = Data.HP;
    }

    
    // Before activate enemy
    public void Init(in Vector3 target, in Transform startPos)
    {
        Initialising = true;
        _navMeshAgent.isStopped = true;
        _target = target;
        //_myParentTransform.localPosition = _myParentTransform.TransformPoint(startPos.position);//_myParentTransform.InverseTransformPoint(startPos);
        //
        _navMeshAgent.updatePosition = false;

        _myParentTransform.localPosition = startPos.position;
        _myTransform.localPosition = Vector3.zero;
        _navMeshAgent.Warp(_myParentTransform.position);
        _myTransform.LookAt(target);

        _currentHP = Data.HP;

        MyStart();
    }

    private void MyStart()
    {
        Initialising = false; //< It is ready
        //OnValidate(); //< Check to get every reference and variable cached

        _ = AsyncStart();
    }

    async UniTask AsyncStart()
    {
        _animator.SetTrigger(_idSpawn);
        await UniTask.Delay((int)_animator.GetCurrentAnimatorStateInfo(0).length * 1000);
        _animator.SetTrigger(_idWalk);
        _navMeshAgent.SetDestination(_target);
        
        _navMeshAgent.updatePosition = true;
        _navMeshAgent.isStopped = false;
    }

    async UniTask AsyncDying()
    {
        GameManager.Instance.AddCoins(_reward);

        _navMeshAgent.isStopped = true;
        _animator.SetTrigger(_idDie);
        await UniTask.Delay((int)_animator.GetCurrentAnimatorStateInfo(0).length * 1000);

        GameManager.Instance.ReturnEnemyToPool(this);
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (damage > 0) return;

        _ = AsyncDying();
    }

}
