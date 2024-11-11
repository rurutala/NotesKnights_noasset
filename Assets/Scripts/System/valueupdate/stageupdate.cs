using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class stageupdate : MonoBehaviour
{
    // Start is called before the first frame update


    public TextMeshProUGUI text;

    void Start()
    {
        text.text = DataManager.Instance.clearcount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
