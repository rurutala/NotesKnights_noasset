using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class TitleManager : MonoBehaviour
{
    public GameObject option;

    async void Start()
    {

        PlayerPrefs.DeleteAll();
        //await UniTask.Delay(3000);
        AudioManager.Instance.PlayBGM(AudioManager.Instance.bgmList[1]);
        option = FindOptionObject("Option");

        if (option != null)
        {
            Debug.Log("'Option' �I�u�W�F�N�g��������܂����I");
        }
        else
        {
            Debug.LogWarning("'Option' �I�u�W�F�N�g��������܂���ł����B");
        }
        // DontDestroyOnLoad�ɂ���DataManager���폜
        DestroyDataManagerInDontDestroyOnLoad();

    }

    private void DestroyDataManagerInDontDestroyOnLoad()
    {

        // �ꎞ�I�u�W�F�N�g���폜
        Destroy(DataManager.Instance.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            option_onoff();
        }
    }

    public void option_onoff()
    {
        if (option != null)
        {
            option.SetActive(!option.activeSelf);
        }
    }

    private GameObject FindOptionObject(string objectName)
    {
        // ��A�N�e�B�u�ȃI�u�W�F�N�g���܂߂Ă��ׂĂ�GameObject���擾
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // �A�Z�b�g��G�f�B�^��̃I�u�W�F�N�g�����O
            if (obj.hideFlags != HideFlags.None)
            {
                continue;
            }

            // �V�[���ɑ��݂���I�u�W�F�N�g���m�F
            if (string.IsNullOrEmpty(obj.scene.name))
            {
                continue; // �V�[���ɑ����Ă��Ȃ��i�A�Z�b�g�Ȃǁj�̂ŃX�L�b�v
            }

            // �I�u�W�F�N�g������v���邩�m�F
            if (obj.name == objectName)
            {
                return obj;
            }
        }

        return null; // ������Ȃ������ꍇ
    }
}
