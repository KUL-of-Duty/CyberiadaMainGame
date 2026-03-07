using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    public static ConsoleManager Instance;

    bool ConsoleVisible = false;
    [SerializeField] 
    GameObject ConsoleHUD;
    [SerializeField] 
    TMP_InputField ConsoleInputField;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (ConsoleVisible)
            {
                HideConsole();
            }
            else ShowConsole();
        }

        if(Input.GetKeyDown(KeyCode.Return) && ConsoleVisible)
        {
            SubmitCommand();
        }
    }

    private void ShowConsole()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        ConsoleHUD.SetActive(true);
    }   

    private void HideConsole()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ConsoleHUD.SetActive(false);
    }

    public void SubmitCommand()
    {
        ConsoleInputField.text = "";
    }
}
