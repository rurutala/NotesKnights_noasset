using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class remainupdate : MonoBehaviour
{


    public TextMeshProUGUI text; 
    // Start is called before the first frame update
    void Start()
    {
        text.text = DataManager.Instance.remaincount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
