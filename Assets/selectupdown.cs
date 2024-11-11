using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class selectupdown : MonoBehaviour
{
    public int id;
    public bool up = true;
    private TextMeshProUGUI tmpComponent; // TMP�R���|�[�l���g
    // Start is called before the first frame update
    void Start()
    {
        tmpComponent = GetComponentInChildren<TextMeshProUGUI>();
        // PlayerPrefs����ۑ����ꂽ�l���擾
        string key = "UP" + id.ToString();
        if (PlayerPrefs.HasKey(key))
        {
            // �ۑ����ꂽ�l���擾���Abool�^�ɕϊ�
            up = PlayerPrefs.GetInt(key) == 1;
        }
        if (up)
        {
            tmpComponent.text = "��";
        }
        else
        {
            tmpComponent.text = "��";
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changedirection()
    {
        up = !up;
        if (up)
        {
            tmpComponent.text = "��";
        }
        else
        {
            tmpComponent.text = "��";
        }
        string key = "UP" + id.ToString();
        PlayerPrefs.SetInt(key, up ? 1 : 0);
        PlayerPrefs.Save();
    }
}
