using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using MEC;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform Target;

    #region Private Variables
    [ReadOnly, SerializeField] private NavMeshAgent _navMeshAgent;
    [ReadOnly, SerializeField] private Animator _animator;
    
    private CoroutineHandle _coroutine;

    //private int _idSpawn;
    private int _idWalk;
    private int _idDie;
    #endregion


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
            
            //if(Target == null) Target = 
            //if(Target != null && _navMeshAgent != null) _navMeshAgent.destination = Target.position;
        });
    }


    private void Start()
    {
        // Check to get every reference and variable caching
        OnValidate();
        _navMeshAgent.isStopped = true;
        _ = AsyncStart();
    }

    async UniTask AsyncStart()
    {
        //await UniTask.WaitUntil(() => this._animator.GetCurrentAnimatorStateInfo(0).IsName("spawn") == true);
        await UniTask.Delay((int)_animator.GetCurrentAnimatorStateInfo(0).length * 1000);
        _animator.SetTrigger(_idWalk);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(Target.position);
    }

    IEnumerator<float> StartMovement() 
    {
        _navMeshAgent.isStopped = true;
        yield return Timing.WaitForSeconds(1);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(Target.position);
        yield return 0f;
    }
}
