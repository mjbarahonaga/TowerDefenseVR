using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

public class EnemyBehaviour : MonoBehaviour
{
    
    #region Private Variables
    [ReadOnly, SerializeField] private NavMeshAgent _navMeshAgent;
    [ReadOnly, SerializeField] private Animator _animator;
    [ReadOnly, SerializeField] private Vector3 _target;

    //private int _idSpawn
    private int _idWalk;
    private int _idDie;
    private new Transform transform;

    private int _reward;
    private int _currentHP;
    #endregion

    private void Awake() => transform = gameObject.transform;

    private void OnValidate()
    {
        Utils.ValidationUtility.SafeOnValidate(() =>
        {
            if (this == null) return;
            
            if(_navMeshAgent == null) _navMeshAgent = GetComponent<NavMeshAgent>();
            if(_animator == null) _animator = GetComponent<Animator>();
            if (_animator) 
            {
                //_idSpawn = Animator.StringToHash("spawn");
                _idWalk = Animator.StringToHash("Walk");
                _idDie = Animator.StringToHash("Die");
            }
            
        });
    }

    // Before activate enemy
    public void Init(Vector3 target, Vector3 startPos, DataEnemy data)
    {
        _target = target;
        transform.position = startPos;
        transform.LookAt(target);

        _reward = data.Reward;
        _currentHP = data.HP;
    }

    private void Start()
    {
        // Check to get every reference and variable caching
        OnValidate();
        _navMeshAgent.isStopped = true;
        _target = GameManager.Instance.RespawnLocations[0].Target.position; //< To debug, delete later
        _ = AsyncStart();
    }

    async UniTask AsyncStart()
    {
        await UniTask.Delay((int)_animator.GetCurrentAnimatorStateInfo(0).length * 1000);
        _animator.SetTrigger(_idWalk);
        _navMeshAgent.SetDestination(_target);
        _navMeshAgent.isStopped = false;
    }

    

}
