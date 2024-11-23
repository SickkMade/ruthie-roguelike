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

    void SetHearts()
    {
        int currentHealth = GameManager.Instance.playerData.currentHealth;
        int maxHealth = GameManager.Instance.playerData.maxHealth;
        
        for(int i = 0; i < maxHealth/2; i++)
        {
            if(currentHealth >= (i + 1) * 2)
            {
                hearts[i].ChangeHeartType(HeartType.Full);
            }
            else if(currentHealth == (i * 2) + 1)
            {
                hearts[i].ChangeHeartType(HeartType.Half);
            }
            else
            {
                hearts[i].ChangeHeartType(HeartType.Empty);
            }
        }
    }
}
