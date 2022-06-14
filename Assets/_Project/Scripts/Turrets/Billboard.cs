using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Billboard : MonoBehaviour
{
    private Transform _myTransform;
    private Transform _character;
    public CoroutineHandle _coroutine;

    private void OnEnable()
    {
        _myTransform = GetComponent<Transform>();
        _character = GameManager.Instance.XROrigin.transform;
        _coroutine = Timing.RunCoroutine(MyUpdate(), Segment.SlowUpdate);
    }

    private void OnDisable()
    {
        Timing.KillCoroutines(_coroutine);
    }

    IEnumerator<float> MyUpdate()
    {
        while (true)
        {
            _myTransform.LookAt(_character);
            yield return Timing.WaitForOneFrame;
        }
    }
}
