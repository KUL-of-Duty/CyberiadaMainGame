using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject mainButtonsRoot;

    private void Awake()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (mainButtonsRoot != null)
            mainButtonsRoot.SetActive(true);
    }

    private void Update()
    {
        if (optionsPanel == null)
            return;

        if (optionsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseSettings();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ConnectionMenu");
    }
    public void OpenSettings()
    {
        if (optionsPanel == null)
        {
            Debug.LogWarning($"{nameof(MainMenu)}: optionsPanel is not assigned. Assign it in the Inspector.");
            return;
        }

        optionsPanel.SetActive(true);

        if (mainButtonsRoot != null)
            mainButtonsRoot.SetActive(false);
    }

    public void CloseSettings()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (mainButtonsRoot != null)
            mainButtonsRoot.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    } 
}
