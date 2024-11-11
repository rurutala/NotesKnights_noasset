using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rest_block : MonoBehaviour
{

    [SerializeField] private float modifierAmount;
    [SerializeField] private float duration;
    // Start is called before the first frame update

    // ����ς݃I�u�W�F�N�g���i�[���郊�X�g
    private List<GameObject> affectedObjects = new List<GameObject>();
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // ���łɌ��ʂ��K�p���ꂽ�I�u�W�F�N�g�͖�������
        if (affectedObjects.Contains(other.gameObject))
        {
            return; // ���łɔ��肳��Ă���̂ŉ������Ȃ�
        }
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Unit>().isPlaced && other.isTrigger == true && other.sharedMaterial.name == "collision_material")
        {
            Debug.Log("rest_block_on");
            var effect = other.gameObject.AddComponent<AttackDecreaseEffect>();
            effect.ApplyEffect(duration, modifierAmount);
            affectedObjects.Add(other.gameObject);
        }
        else if(other.gameObject.tag == "Enemy")
        {
            var effect = other.gameObject.AddComponent<AttackIncreaseEffect>();
            effect.ApplyEffect(duration, modifierAmount);
            affectedObjects.Add(other.gameObject);
        }
    }
}
