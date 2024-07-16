using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

public class ShooterLauncher : MonoBehaviour
{
    [SerializeField]
    private List<string> jsonFiles = new List<string>(); // JSON 파일 경로 리스트

    private List<BaseParameters> parametersList = new List<BaseParameters>(); // 파라미터 리스트
    private int currentParameterIndex = 0;
    private Shooter shooter;

    private float term = 1f;


    private void Awake()
    {
        shooter = GetComponentInChildren<Shooter>();
    }
    void Start()
    {
        shooter.Init();
        LoadAllParameters();
        ApplyCurrentParameters();
        StartCoroutine(OnShooterLauncher());
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
            Debug.Log(shooter.IsRunning);
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

    // 모든 JSON 파일을 읽어들여 파라미터 리스트에 저장하는 메서드
    private void LoadAllParameters()
    {
        foreach (var filePath in jsonFiles)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                BaseParameters parameters = JsonConvert.DeserializeObject<BaseParameters>(json);
                if (parameters != null)
                {
                    parametersList.Add(parameters);
                }
            }
            else
            {
                Debug.LogWarning("File not found: " + filePath);
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
