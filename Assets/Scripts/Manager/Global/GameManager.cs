using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [Header("[ Manager ]")]
    public GameObject uiManagerPrefab;
    public GameObject sceneControlManagerPrefab;
    public GameObject objectPoolManagerPrefab;

    private void Awake()
    {
        Debug.Log($"Awake");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            Instantiate(uiManagerPrefab, transform);
            Instantiate(sceneControlManagerPrefab, transform);
            Instantiate(objectPoolManagerPrefab, transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
