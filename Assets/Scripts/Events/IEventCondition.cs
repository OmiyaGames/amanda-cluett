using UnityEngine;

public abstract class IEventCondition : MonoBehaviour
{
    public abstract bool Passed(UnlockEvent unlockEvent, GamePanel panel);
}
