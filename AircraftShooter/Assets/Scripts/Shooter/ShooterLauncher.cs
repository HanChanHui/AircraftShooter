using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System;

public class ShooterLauncher : MonoBehaviour
{
    [SerializeField] private List<Shooter> shooters = new List<Shooter>();

    [SerializeField] private List<TextAsset> jsonFiles = new List<TextAsset>(); // JSON 파일 리스트

    private List<BaseParameters> parametersList = new List<BaseParameters>(); // 파라미터 리스트
    private int currentParameterIndex = 0;
    private Shooter shooter;

    [SerializeField] private float term = 1f;


    private void Awake()
    {
        shooter = GetComponentInChildren<Shooter>();
    }
    void Start()
    {
        foreach (var shooter in shooters)
        {
            if (shooter != null)
            {
                shooter.Init();
            }
        }

        LoadAllParameters();
        ApplyCurrentParameters();
        if(parametersList.Count > 0)
        {
            StartCoroutine(OnShooterLauncher());
        }
        
    }

    private IEnumerator OnShooterLauncher()
    {
        shooter.StartShoot();
        StartCoroutine(CoReStart());
        yield return null;
    }

    private IEnumerator CoReStart()
    {
        while(true)
        {
            if(!shooter.IsRunning)
            {
                yield return new WaitForSeconds(term);
                NextParameters();
                StartCoroutine(OnShooterLauncher());
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    private void LoadAllParameters()
    {
        foreach (var textAsset in jsonFiles)
        {
            if (textAsset != null)
            {
                try
                {
                    string json = textAsset.text;

                    // TypeNameHandling을 설정하여 타입 정보를 포함
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };

                    BaseParameters parameters = JsonConvert.DeserializeObject<BaseParameters>(json, settings);
                    if (parameters != null)
                    {
                        parametersList.Add(parameters);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error loading JSON from TextAsset: " + textAsset.name + " - " + ex.Message);
                }
            }
            else
            {
                Debug.LogWarning("TextAsset is null.");
            }
        }
    }

    // 현재 파라미터를 자식 오브젝트 Shooter에 적용하는 메서드
    private void ApplyCurrentParameters()
    {
        if (parametersList.Count == 0)
        {
            Debug.LogWarning("No parameters to apply.");
            return;
        }

        BaseParameters currentParameters = parametersList[currentParameterIndex];
        if (shooter != null)
        {
            shooter.ApplyParameters(currentParameters);
            Debug.Log("Applied parameters from: " + jsonFiles[currentParameterIndex]);
        }
        else
        {
            Debug.LogWarning("No Shooter component found in children.");
        }
    }

    // 다음 파라미터로 전환하는 메서드
    public void NextParameters()
    {
        currentParameterIndex = (currentParameterIndex + 1) % parametersList.Count;
        ApplyCurrentParameters();
    }
}
