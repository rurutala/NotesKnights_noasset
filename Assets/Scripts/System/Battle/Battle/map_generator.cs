using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class map_generator : MonoBehaviour
{
    public GameObject[] tilePrefabs; // �v���n�u�̔z��
    private int[,] mapData; // mapData�������Ő錾
    public string csvFilePath; // CSV�t�@�C���̃p�X

    // Start is called before the first frame update
    void Start()
    {
        // CSV�t�@�C�����ǂݍ��܂�Ă��邩�m�F
        if (string.IsNullOrEmpty(csvFilePath))
        {
            Debug.LogError("CSV file path is missing!");
            return;
        }

        // CSV�t�@�C����ǂݍ���
        string[] lines = File.ReadAllLines(csvFilePath);

        int rows = lines.Length;
        int cols = lines[0].Split(',').Length;
        mapData = new int[rows, cols];

        for (int x = 0; x < rows; x++)
        {
            string[] values = lines[x].Split(',');
            for (int y = 0; y < cols; y++)
            {
                mapData[x, y] = int.Parse(values[y]);
            }
        }

        for (int x = 0; x < mapData.GetLength(0); x++)
        {
            for (int y = 0; y < mapData.GetLength(1); y++)
            {
                int tileType = mapData[x, y];
                if (tileType >= 0 && tileType < tilePrefabs.Length)
                {
                    Vector3 position = new Vector3(x, 0, y);
                    GameObject tile = Instantiate(tilePrefabs[tileType], position, Quaternion.identity);
                    tile.transform.parent = this.transform; // �X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g�̎q�I�u�W�F�N�g�ɐݒ�
                }
                else
                {
                    Debug.LogWarning($"Invalid tile type {tileType} at position ({x}, {y})");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
