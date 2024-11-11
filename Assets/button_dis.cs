using UnityEngine;
using UnityEngine.UI;

public class button_dis : MonoBehaviour
{
    private Button _button;

    void Start()
    {
        // ����GameObject�ɃA�^�b�`����Ă���Button�R���|�[�l���g���擾
        _button = GetComponent<Button>();

        if (_button != null)
        {
            // �{�^���̃N���b�N�C�x���g�Ƀ��\�b�h��o�^
            _button.onClick.AddListener(DisableSelf);
        }
        else
        {
            Debug.LogError($"{gameObject.name} �� Button �R���|�[�l���g��������܂���B");
        }
    }

    // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    void DisableSelf()
    {
        _button.interactable = false; // �{�^�����A�N�e�B�u��
    }

}
