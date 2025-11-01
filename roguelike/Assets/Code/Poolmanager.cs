using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Poolmanager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    private void Awake()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError($"Poolmanager: prefabs 배열이 비어 있습니다.");
            return;
        }
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++) {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        Debug.Log($"Poolmanager.Get 호출됨, index = {index}");
        if (index < 0 || index >= prefabs.Length)
        {
            Debug.LogError($"Poolmanager: 잘못된 인덱스 요청 ({index}), Prefabs 길이: {prefabs.Length}");
            return null;    
        }

        if (prefabs[index] == null)
        {
            Debug.LogError($"Poolmanager: Prefabs[{index}]가 비어 있습니다.");
            return null;
        }
        for (int i = 0; i < pools[index].Count; i++)
        {
            if (!pools[index][i].activeSelf)
            {
                GameObject obj = pools[index][i];
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(prefabs[index], transform);
        pools[index].Add(newObj);
        return newObj;
    }
}
