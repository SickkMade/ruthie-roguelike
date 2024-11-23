using UnityEngine;

public class HuntState : EnemyBaseState
{
    public float huntTime = 5; //in seconds
    public float speed = 5;
    float timer;
    Vector2 direction;
    //Start function
    public override void EnterState(EnemyStateMachine enemy)
    {
        direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
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
        enemy.rb.MovePosition(enemy.rb.position + direction * speed * Time.deltaTime);

        if(timer>huntTime){
            enemy.ChangeState(enemy.idleState);
        }
    }
}
