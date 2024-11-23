using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    EnemyBaseState currentState;

    [Header("Hunt State")]
    [SerializeField] float huntTime = 5; //in seconds
    [SerializeField] float huntSpeed = 5; //in seconds
    [Header("Idle State")]
    [SerializeField] float waitTimeMax = 5;
    [SerializeField] float waitTimeMin = 3;

    public Rigidbody2D rb;

    public HuntState huntState = new();
    public IdleState idleState = new();
    public WalkingState walkingState = new();


    void Start(){
        currentState = idleState;

        huntState.huntTime = huntTime;
        huntState.speed = huntSpeed;

        idleState.waitTimeMax = waitTimeMax;
        idleState.waitTimeMin = waitTimeMin;

        rb = GetComponent<Rigidbody2D>();
    }

    public void ChangeState(EnemyBaseState state){
        currentState = state;
        currentState.EnterState(this);
    }

    void Update(){
        currentState.UpdateState(this);
    }

}
