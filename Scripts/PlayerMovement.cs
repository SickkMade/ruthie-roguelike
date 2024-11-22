
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed;
    private bool canMove = true;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start(){
        GameManager.Instance.PlayerPauseInputForSecondsEvent += PauseMovementForSeconds;
    }

    void OnDisable(){
        GameManager.Instance.PlayerPauseInputForSecondsEvent -= PauseMovementForSeconds;
    }

    void FixedUpdate(){
        Move();
    }

    void Move(){
        if(!canMove) return;
        float horiz = Input.GetAxisRaw("Horizontal");
        Vector2 dir = Vector2.right * horiz + Vector2.up * Input.GetAxisRaw("Vertical");
        if(horiz > 0){ //flip player
            spriteRenderer.flipX = false;
        } else if (horiz < 0){
            spriteRenderer.flipX = true;
        }
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * dir.normalized);
    }

    void PauseMovementForSeconds(float seconds){
        StartCoroutine(PauseForSeconds(seconds/2));
    }

    IEnumerator PauseForSeconds(float seconds){
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

}
