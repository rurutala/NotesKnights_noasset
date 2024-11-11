using UnityEngine;
using UnityEngine.UI;

public class button_dis : MonoBehaviour
{
    private Button _button;

    void Start()
    {
        // このGameObjectにアタッチされているButtonコンポーネントを取得
        _button = GetComponent<Button>();

        if (_button != null)
        {
            // ボタンのクリックイベントにメソッドを登録
            _button.onClick.AddListener(DisableSelf);
        }
        else
        {
            Debug.LogError($"{gameObject.name} に Button コンポーネントが見つかりません。");
        }
    }

    // ボタンがクリックされたときに呼び出されるメソッド
    void DisableSelf()
    {
        _button.interactable = false; // ボタンを非アクティブ化
    }

}
