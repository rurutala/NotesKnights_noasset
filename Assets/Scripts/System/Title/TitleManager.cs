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
            Debug.Log("'Option' オブジェクトが見つかりました！");
        }
        else
        {
            Debug.LogWarning("'Option' オブジェクトが見つかりませんでした。");
        }
        // DontDestroyOnLoadにあるDataManagerを削除
        DestroyDataManagerInDontDestroyOnLoad();

    }

    private void DestroyDataManagerInDontDestroyOnLoad()
    {

        // 一時オブジェクトを削除
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
        // 非アクティブなオブジェクトも含めてすべてのGameObjectを取得
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // アセットやエディタ上のオブジェクトを除外
            if (obj.hideFlags != HideFlags.None)
            {
                continue;
            }

            // シーンに存在するオブジェクトか確認
            if (string.IsNullOrEmpty(obj.scene.name))
            {
                continue; // シーンに属していない（アセットなど）のでスキップ
            }

            // オブジェクト名が一致するか確認
            if (obj.name == objectName)
            {
                return obj;
            }
        }

        return null; // 見つからなかった場合
    }
}
