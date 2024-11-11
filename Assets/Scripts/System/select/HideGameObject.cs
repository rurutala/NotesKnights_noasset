using UnityEngine;

public class HideGameObject : MonoBehaviour
{
    public GameObject targetObject;       // 非表示にするゲームオブジェクト
    public bool UnitsInteractable = false; // フラグ

    public void OnButtonPressed()
    {        // UnitsInteractableフラグがオンならば、シーン上のすべてのBuyUnitを探してEnableButton()を呼ぶ
        if (UnitsInteractable)
        {
            // シーン上のすべてのBuyUnitを見つける
            BuyUnit[] buyUnits = FindObjectsOfType<BuyUnit>();

            // 各BuyUnitに対してEnableButton()を呼ぶ
            foreach (BuyUnit buyUnit in buyUnits)
            {
                buyUnit.EnableButton();
            }
        }
        if (targetObject != null)
        {
            // ゲームオブジェクトを非表示にする
            targetObject.SetActive(false);
        }


    }
}
