using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // DataManager����A�C�e��ID���擾���A�v���n�u�𐶐�����
        foreach (int itemID in DataManager.Instance.boughtItemIDs)
        {
            // �v���n�u�̃p�X��ݒ�
            string prefabPath = "prefabs/item/item" + itemID;

            // �v���n�u��Resources�t�H���_���烍�[�h
            GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);

            // �v���n�u�����݂���ꍇ�̂ݐ���
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
                Debug.Log($"{prefabPath} ����������܂����B");
            }
            else
            {
                Debug.LogError($"{prefabPath} �̃v���n�u��������܂���ł����B");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
