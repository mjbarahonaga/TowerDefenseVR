using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class DebugManager : MonoBehaviour
{
    [System.Serializable]
    public class EventToTry
    {
        public UnityEvent Event;

        [Button("Execute")]
        public void Execute() => Event.Invoke();
    }

    public List<EventToTry> EventList;
}
