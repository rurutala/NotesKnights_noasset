using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class moneyupate : MonoBehaviour
{
    // Start is called before the first frame update


    public TextMeshProUGUI text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = DataManager.Instance.money.ToString();
    }
}
