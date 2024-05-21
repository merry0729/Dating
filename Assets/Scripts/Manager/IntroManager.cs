using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [Header("Button")]
    public UIButton startButton;

    void Awake()
    {
        if (startButton == null)
            startButton = GameObject.Find("Start").GetComponent<UIButton>();

        startButton.OnClick += OnClickStart;
    }

    private void OnDestroy()
    {
        startButton.OnClick -= OnClickStart;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickStart()
    {
        Debug.Log($"OnClickStart");
    }
}
