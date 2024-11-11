using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class map_generator : MonoBehaviour
{
    public GameObject[] tilePrefabs; // プレハブの配列
    private int[,] mapData; // mapDataをここで宣言
    public string csvFilePath; // CSVファイルのパス

    // Start is called before the first frame update
    void Start()
    {
        // CSVファイルが読み込まれているか確認
        if (string.IsNullOrEmpty(csvFilePath))
        {
            Debug.LogError("CSV file path is missing!");
            return;
        }

        // CSVファイルを読み込む
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
                    tile.transform.parent = this.transform; // スクリプトがアタッチされたオブジェクトの子オブジェクトに設定
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
