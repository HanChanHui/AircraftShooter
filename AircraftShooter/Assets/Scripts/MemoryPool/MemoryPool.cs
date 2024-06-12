using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

//-----------------------------------------------------------------------------------------
// 메모리 풀 클래스
// 용도 : 특정 게임오브젝트를 실시간으로 생성과 삭제하지 않고,
//      : 미리 생성해 둔 게임오브젝트를 재활용하는 클래스입니다.
//-----------------------------------------------------------------------------------------
//MonoBehaviour 상속 안받음. IEnumerable 상속시 foreach 사용 가능
//System.IDisposable 관리되지 않는 메모리(리소스)를 해제 함
public class MemoryPool : IEnumerable, System.IDisposable
{
    PoolItem[] table;
    public PoolItem[] Table { get {return this.table;} }
    public int count{ get {return this.table.Length; } }

    private Transform myParent;

    /// <summary>
    /// 열거자 기본 재정의
    /// <summary>
    public IEnumerator GetEnumerator()
    {
        if(table == null)
        {
            yield break;
        }
        int count = table.Length;

        for(int i = 0; i < count; i++)
        {
            PoolItem item = table[i];
            if(item.active)
            {
                yield return item.gameObject;
            }
        }
    }
    
    public MemoryPool(Object _original, int _count, Transform _parent = null)
    {
        myParent = _parent;
        this.Create(_original, _count);
    }

    /// <summary>
    /// 메모리 풀 생성
    /// original : 미리 생성해 둘 원본 소스
    /// count : 풀 최고 갯수
    /// <summary>
    public void Create(Object _original, int _count)
    {
        Dispose();
        table = new PoolItem[_count];

        for(int i = 0; i < _count; i++)
        {
            PoolItem item = new PoolItem();
            item.active = false;
            item.gameObject = GameObject.Instantiate(_original) as GameObject;
            item.gameObject.SetActive(false);
            table[i] = item;

            if(myParent)
            {
                item.gameObject.transform.SetParent(myParent);
            }
        }

        for(int i = 0; i < _count; i++)
        {
            IMemoryPool mpObject = table[i].gameObject.GetComponent<IMemoryPool>();
            if(mpObject != null)
            {
                mpObject.MPStart();
            }
        }
    }
    /// <summary>
    /// 새 아이템 요청 - 쉬고 있는 객체를 반납한다.
    /// <summary>
    public GameObject NewItem()
    {
        if(table == null)
        {
            return null;
        }
        int _count = table.Length;
        for(int i = 0; i < _count; i++)
        {
            PoolItem item = table[i];
            if(item.active == false)
            {
                item.active = true;
                item.gameObject.SetActive(true);
                return item.gameObject;
            }
        }

        return null;
    }

    public PoolItem NewObject()
    {
        if(table == null)
        {
            return null;
        }
        int _count = table.Length;
        for(int i = 0; i < _count; i++)
        {
            PoolItem item = table[i];
            if(item.active == false)
            {
                item.active = true;
                item.gameObject.SetActive(true);
                return item;
            }
        }

        return null;
    }
    /// <summary>
    /// 아이템 사용 종료 : 사용하던 객체를 쉬게한다.
    /// gameObject : NewItem으로 얻었던 객체
    /// <summary>
    public void RemoveItem(GameObject _gameObject)
    {
        if(table == null || _gameObject == null)
        {
            return;
        }
        int _count = table.Length;

        for(int i = 0; i < _count; i++)
        {
            PoolItem item = table[i];
            if(item.gameObject == _gameObject)
            {
                item.active = false;
                item.gameObject.SetActive(false);
                break;
            }
        }
    }

    /// <summary>
    /// 모든 아이템 사용 종료 : 모든 객체를 쉬게한다.
    /// <summary>
     public void ClearItem()
    {
        if(table == null)
        {
            return;
        }
        int _count = table.Length;

        for(int i = 0; i < _count; i++)
        {
            PoolItem item = table[i];
            if(item != null && item.active)
            {
                item.active = false;
                item.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 메모리 풀 삭제
    /// <summary>

    public void Dispose()
    {
        if(table == null)
        {
            return;
        }
        int _count = table.Length;

        for(int i = 0; i < _count; i++)
        {
            PoolItem item = table[i];
            GameObject.Destroy(item.gameObject);
        }
        table = null;
    }

    public void SetActiveAll()
    {
        if(table == null)
        {
            return;
        }
        int _count = table.Length;

        for(int i = 0; i < _count; i++)
        {
            PoolItem item = table[i];
            if(item != null)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    public PoolItem GetItem(int _index)
    {
        return table[_index];
    }

}
