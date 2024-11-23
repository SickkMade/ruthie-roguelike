using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{

    void Start(){
        GameManager.Instance.SetPlayerSpawnEvent += SetPlayerPosition;
    }
    void OnDisable(){
        GameManager.Instance.SetPlayerSpawnEvent -= SetPlayerPosition;
    }

    void SetPlayerPosition(Node node){
        transform.position = node.room.Value.center;
    }
}
