using UnityEngine;
using ColdironTools.Events;

public class AnimationEventCaller : MonoBehaviour
{
    public void RaiseEvent(GameEvent eventToRaise)
    {
        eventToRaise.Raise();
    }
}
