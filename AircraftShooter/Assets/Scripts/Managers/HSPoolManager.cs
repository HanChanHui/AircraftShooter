using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSPoolManager : MonoBehaviour
{
    public static HSPoolManager Instance = null;

    [System.Serializable]
    public struct MemItem {
        public string type;
        public GameObject obj;
        public int count;
    };

    // 각 객체를 타입별로 관리하는 pool
    public Dictionary<string, MemoryPool> pools;
    // 초기화할 객체 정보 배열
    public MemItem[] items;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    public void MyStart() {
        InitItems();
    }

    /// <summary>
    /// Pool 초기화
    /// <summary>
    private void InitItems()
    {
        pools = new Dictionary<string, MemoryPool>();

        foreach(MemItem item in items)
        {
            pools.Add(item.type, new MemoryPool(item.obj, item.count, transform));
        }
    }
    /// <summary>
    /// Pool 객체 생성
    /// <summary>
    public GameObject NewItem(string _name)
    {
        if(pools.ContainsKey(_name))
        {
            return pools[_name].NewItem();
        }
        else
        {
            Debug.LogWarning($"No pool found for item type: {_name}");
            return null;
        }
    }
    /// <summary>
    /// 특정 타입의 새로운 객체 생성
    /// <summary>
    public T NewItem<T>(string _name)
    {
        GameObject obj = NewItem(_name);

        if(obj)
        {
            T objType = obj.GetComponent<T>();

            if(objType != null)
            {
                return objType;
            }
            else
            {
                return default(T);
            }
        }
        else
        {
            return default(T);
        }
    }
    /// <summary>
    /// 메모리 풀 반환
    /// <summary>
    public MemoryPool GetMemoryPool(string _name)
    {
        if(pools.ContainsKey(_name))
        {
            return pools[_name];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 객체 제거
    /// <summary>
    public void RemoveItem(string _name, GameObject _gameObject)
    {
        if(pools.ContainsKey(_name))
        {
            pools[_name].RemoveItem(_gameObject);
        }
    }
    /// <summary>
    /// 객체 개수 반환
    /// <summary>
    public int GetCount(string _name)
    {
        if(pools.ContainsKey(_name))
        {
            return pools[_name].count;
        }
        else
        {
            return -1;
        }
    }

}
