
using UnityEngine;

public class NpcWander : MonoBehaviour
{
    [SerializeField] Collider2D lineOfSight;
    [SerializeField] float wanderSize;
    [SerializeField] float wanderTime;

    void OnTriggerEnter2D(){
        
    }

    void Update(){
        Wander();
    }


    void Wander(){
        Vector3 point = GetRandomPoint() + transform.position;
        float timer = 0;
        if(timer < wanderTime){ //we can break if seen here aswell
            Debug.Log(timer);
            timer += Time.deltaTime;
            Vector3.Lerp(transform.position, point, timer / wanderTime);
        }
    }

    Vector3 GetRandomPoint(){
        return Vector3.up * Random.Range(0, wanderSize) + Vector3.right * Random.Range(0, wanderSize) + Vector3.forward;
    }

}
