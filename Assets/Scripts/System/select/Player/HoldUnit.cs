using UnityEngine;
using UnityEngine.UI;

public class HoldUnit : MonoBehaviour
{
    public int slotID;       // �X���b�gID
    public int unitID = -1;  // �L�����N�^�[ID�i-1�͖��I����Ԃ������j
    public string unitName;

    public GameObject uniton;
    public Image unitImage;  // �L�����N�^�[�摜
    public Button selectButton; // �X���b�g�I���{�^��

    private void Start()
    {
        selectButton.onClick.AddListener(OnSelect);
    }

    // �{�^���������ꂽ���̏���
    private void OnSelect()
    {
        // �������g�𑗐M
        UnitSelectManager.Instance.UnitsBuyOn(this);
    }

    // �L�����N�^�[�摜��ID���X�V
    public void UpdateUnit(int newID, Sprite newImage,string name)
    {
        if(newID == -1)
        {
            uniton.SetActive(false);
            unitID = -1;
            unitName = name;
            unitImage.sprite = null;
            return;
        }
        unitName = name;
        uniton.SetActive(true);
        unitID = newID;
        unitImage.sprite = newImage;
    }
}
