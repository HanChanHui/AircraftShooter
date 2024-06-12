using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
 
    public static GameManager Instance {
        get {          
            if (_Instance == null) {
                    // 인스턴스 존재 여부 확인
                    _Instance = (GameManager)FindObjectOfType(typeof(GameManager));
 
                    // 아직 생성되지 않았다면 인스턴스 생성
                    if (_Instance == null) {
                        // 새로운 게임오브젝트를 만들어서 싱글톤 Attach
                        var singletonObject = new GameObject();
                        _Instance = singletonObject.AddComponent<GameManager>();
                        singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                    }
                }

                return _Instance;
        }
    }


    private void Start() 
    {
        HSPoolManager.Instance.MyStart();
    }

}
