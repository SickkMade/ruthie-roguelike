using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float weaponOffset = 2f;
    private bool canAttack = true;

    void Awake(){
        weaponHolder.SetActive(false);
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space) && canAttack){
            Attack();
            StartCoroutine(AttackCooldownCoroutine());
        }
    }

    void Attack(){
        GameManager.Instance.CallPlayerTakeDamage();
        canAttack = false;
        weaponHolder.SetActive(true);
        GameManager.Instance.CallPlayerPauseInputForSecondsEvent(attackCooldown);
        AudioManager.Instance.callSfxSource("hit");

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //set location of weapon
        Vector2 dir = Vector2.right * horizontal + Vector2.up * vertical;
        if(dir == Vector2.zero) dir = Vector2.right;
        weaponHolder.transform.localPosition = dir * weaponOffset;

        //set rotation of weapon
        float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
        weaponHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    IEnumerator AttackCooldownCoroutine(){
        yield return new WaitForSeconds(attackCooldown/2);
            weaponHolder.SetActive(false);
        yield return new WaitForSeconds(attackCooldown/2);
        canAttack = true;
    }
}
