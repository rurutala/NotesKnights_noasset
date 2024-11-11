using UnityEngine;

public class HideGameObject : MonoBehaviour
{
    public GameObject targetObject;       // ��\���ɂ���Q�[���I�u�W�F�N�g
    public bool UnitsInteractable = false; // �t���O

    public void OnButtonPressed()
    {        // UnitsInteractable�t���O���I���Ȃ�΁A�V�[����̂��ׂĂ�BuyUnit��T����EnableButton()���Ă�
        if (UnitsInteractable)
        {
            // �V�[����̂��ׂĂ�BuyUnit��������
            BuyUnit[] buyUnits = FindObjectsOfType<BuyUnit>();

            // �eBuyUnit�ɑ΂���EnableButton()���Ă�
            foreach (BuyUnit buyUnit in buyUnits)
            {
                buyUnit.EnableButton();
            }
        }
        if (targetObject != null)
        {
            // �Q�[���I�u�W�F�N�g���\���ɂ���
            targetObject.SetActive(false);
        }


    }
}
