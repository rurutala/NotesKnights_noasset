using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class selectupdown : MonoBehaviour
{
    public int id;
    public bool up = true;
    private TextMeshProUGUI tmpComponent; // TMPコンポーネント
    // Start is called before the first frame update
    void Start()
    {
        tmpComponent = GetComponentInChildren<TextMeshProUGUI>();
        // PlayerPrefsから保存された値を取得
        string key = "UP" + id.ToString();
        if (PlayerPrefs.HasKey(key))
        {
            // 保存された値を取得し、bool型に変換
            up = PlayerPrefs.GetInt(key) == 1;
        }
        if (up)
        {
            tmpComponent.text = "↑";
        }
        else
        {
            tmpComponent.text = "↓";
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
            tmpComponent.text = "↑";
        }
        else
        {
            tmpComponent.text = "↓";
        }
        string key = "UP" + id.ToString();
        PlayerPrefs.SetInt(key, up ? 1 : 0);
        PlayerPrefs.Save();
    }
}
