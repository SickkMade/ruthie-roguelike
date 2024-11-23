using UnityEngine;

public class IdleState : EnemyBaseState
{
    public float waitTimeMax = 5;
    public float waitTimeMin = 3;
    float waitTime;

    float timer;
    //Start function
    public override void EnterState(EnemyStateMachine enemy)
    {
        waitTime = Random.Range(waitTimeMin, waitTimeMax);
        timer = 0;
    }

    //on collision enter
    public override void OnCollisionEnter(EnemyStateMachine enemy)
    {
        throw new System.NotImplementedException();
    }

    //update function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        timer += Time.deltaTime;
        if(timer > waitTime){
            enemy.ChangeState(enemy.huntState);
        }
    }
}
