using System.Collections.Generic;
using UnityEngine;

public class TolerableUPitem : MonoBehaviour
{

    public int tolerable_up;

    public void Start()
    {
        GameFlowManager.Instance.TolerableEnemiesCount += tolerable_up;
    }
}
