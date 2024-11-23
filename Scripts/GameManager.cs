using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Node dungeonRootNode;
    public delegate void emptyDelegate();
    public delegate void nodeDelegate(Node node);
    public delegate void floatDelegate(float value);
    public event emptyDelegate PlayerTakeDamage;
    public event nodeDelegate SetPlayerSpawnEvent;
    public event floatDelegate PlayerPauseInputForSecondsEvent;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        } else{
            Instance = this;
        }
    }

    public PlayerData playerData = new PlayerData{
        playerPos = Vector3.zero,
        maxHealth = 10,
        currentHealth = 10,
    };

    public void CallSetPlayerSpawnEvent(Node node){
        SetPlayerSpawnEvent?.Invoke(node);
    }

    public void CallPlayerTakeDamage(int damage){
        playerData.currentHealth -= damage;
        PlayerTakeDamage?.Invoke();
    }

    public void CallPlayerPauseInputForSecondsEvent(float value){
        PlayerPauseInputForSecondsEvent.Invoke(value);
    }
}

public struct PlayerData{
        public Vector3 playerPos;
        public int maxHealth;
        public int currentHealth;
    }
