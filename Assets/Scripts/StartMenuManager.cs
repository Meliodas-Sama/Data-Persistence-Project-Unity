using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance;
    public static string playerNameText;
    private InputField nameInputField;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        nameInputField = FindObjectOfType<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() 
    {
        if (!string.IsNullOrWhiteSpace(nameInputField.text))
        {
            playerNameText = nameInputField.text;
            SceneManager.LoadScene(1);
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
