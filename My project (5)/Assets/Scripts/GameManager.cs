using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    [Header("Settings")]
    public float timeLimit = 60f;
    public int coinsToWin = 10;

    private int coinsCollected = 0;
    private float currentTime;
    private bool gameActive = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentTime = timeLimit;
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        UpdateUI();
    }

    private void Update()
    {
        if (!gameActive) return;

        currentTime -= Time.deltaTime;
        timerText.text = "Tiempo: " + Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 0)
            TriggerGameOver();
    }

    public void AddCoin()
    {
        coinsCollected++;
        UpdateUI();

        if (coinsCollected >= coinsToWin)
            TriggerWin();
    }

    public void TriggerGameOver()
    {
        if (!gameActive) return;
        gameActive = false;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TriggerWin()
    {
        if (!gameActive) return;
        gameActive = false;
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void UpdateUI()
    {
        coinText.text = "Monedas: " + coinsCollected + "/" + coinsToWin;
    }
}