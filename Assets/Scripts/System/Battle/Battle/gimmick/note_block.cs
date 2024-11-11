using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class note_block : MonoBehaviour
{

    [SerializeField] private float modifierAmount;
    [SerializeField] private float duration;

    // 判定済みオブジェクトを格納するリスト
    private List<GameObject> affectedObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // すでに効果が適用されたオブジェクトは無視する
        if (affectedObjects.Contains(other.gameObject))
        {
            return; // すでに判定されているので何もしない
        }
        if (other.gameObject.tag == "Enemy")
        {
            var effect = other.gameObject.AddComponent<AttackDecreaseEffect>();
            effect.ApplyEffect(duration, modifierAmount);
            affectedObjects.Add(other.gameObject);
        }
        else if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<Unit>().isPlaced && other.isTrigger == true && other.sharedMaterial.name == "collision_material")
        {
            var effect = other.gameObject.AddComponent<AttackIncreaseEffect>();
            effect.ApplyEffect(duration, modifierAmount);
            affectedObjects.Add(other.gameObject);
        }
    }
}
