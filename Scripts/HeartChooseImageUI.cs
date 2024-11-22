using UnityEngine;
using UnityEngine.UI;

public class HeartChooseImageUI : MonoBehaviour
{
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite halfHeart;
    [SerializeField]
    private Sprite emptyHeart;
    [SerializeField]
    private Image image;


    public void ChangeHeartType(HeartType type){
        if (type == HeartType.Full){
            image.overrideSprite = fullHeart;
        } 
        else if(type == HeartType.Half){
            image.overrideSprite = halfHeart;
        }
         else if(type == HeartType.Empty){
            image.overrideSprite = emptyHeart;
        }
    }
}

public enum HeartType {
    Full, Half, Empty
}
