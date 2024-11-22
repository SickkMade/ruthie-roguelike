using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeartsCreatorUI : MonoBehaviour
{
    [SerializeField]
    private GameObject HeartsUI;
    private List<HeartChooseImageUI> hearts = new();

    void Start(){
        for(int i = 0; i < GameManager.Instance.playerData.maxHealth/2; i++){
            Instantiate(HeartsUI, this.transform, false).TryGetComponent<HeartChooseImageUI>(out HeartChooseImageUI heart);
            heart.ChangeHeartType(HeartType.Full);
            hearts.Add(heart);
        }
        if(GameManager.Instance.playerData.maxHealth % 2 == 1){ //if odd
            hearts[^1].ChangeHeartType(HeartType.Half);
        }

        GameManager.Instance.PlayerTakeDamage += SetHearts;
    }

    void OnDestroy(){
        GameManager.Instance.PlayerTakeDamage -= SetHearts;
    }

    void SetHearts(){
        int newHealth = GameManager.Instance.playerData.currentHealth;
        int maxHealth = GameManager.Instance.playerData.maxHealth;
        for(int i = 0; i < maxHealth; i++){
            int indexToPrefab = Mathf.FloorToInt(i/2);
            if(i <= newHealth){
                if(i % 2 == 0)
                    hearts[indexToPrefab].ChangeHeartType(HeartType.Half);
                else
                    hearts[indexToPrefab].ChangeHeartType(HeartType.Full);
            }
            else{
                hearts[indexToPrefab].ChangeHeartType(HeartType.Empty);
            }
        }
    }
}
