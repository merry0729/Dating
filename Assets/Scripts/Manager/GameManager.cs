using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [Header("[ Manager ]")]
    public GameObject uiManagerPrefab;
    public GameObject sceneControlManagerPrefab;

    private void Awake()
    {
        Debug.Log($"Awake");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            Instantiate(uiManagerPrefab, transform);
            Instantiate(sceneControlManagerPrefab, transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
