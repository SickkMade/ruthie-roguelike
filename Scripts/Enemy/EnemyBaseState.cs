using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateMachine enemy);
    public abstract void UpdateState(EnemyStateMachine enemy);
    public abstract void OnCollisionEnter(EnemyStateMachine enemy);

}
