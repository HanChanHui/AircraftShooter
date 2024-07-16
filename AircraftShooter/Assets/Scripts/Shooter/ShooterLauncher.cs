using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System;

[Serializable]
public class ShooterJsonFiles
{
    public List<TextAsset> jsonFiles = new List<TextAsset>();
}

public class ShooterLauncher : MonoBehaviour
{
    public List<Shooter> shooters = new List<Shooter>();
    public List<ShooterJsonFiles> shooterJsonFilesList = new List<ShooterJsonFiles>(); // 각 Shooter의 JSON 파일 리스트

    [SerializeField] private float term = 1f;

    void Start()
    {
        for (int i = 0; i < shooters.Count; i++)
        {
            if (shooters[i] != null)
            {
                shooters[i].Init();
                //LoadParameters(i);
                shooters[i].StartCoroutine(ShooterRoutine(shooters[i], i));
            }
        }
    }

    private IEnumerator ShooterRoutine(Shooter shooter, int index)
    {
        List<BaseParameters> parametersList = LoadParameters(index);
        int currentParameterIndex = 0;

        while (true)
        {
            if (parametersList.Count > 0)
            {
                shooter.ApplyParameters(parametersList[currentParameterIndex]);
                shooter.StartShoot();

                while (shooter.IsRunning)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                currentParameterIndex = (currentParameterIndex + 1) % parametersList.Count;
                yield return new WaitForSeconds(term);
            }
            else
            {
                Debug.LogWarning("No parameters to apply for shooter " + index);
                yield break;
            }
        }
    }

    private List<BaseParameters> LoadParameters(int index)
    {
        List<BaseParameters> parametersList = new List<BaseParameters>();

        if (index >= 0 && index < shooterJsonFilesList.Count)
        {
            foreach (var textAsset in shooterJsonFilesList[index].jsonFiles)
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

        return parametersList;
    }
}
