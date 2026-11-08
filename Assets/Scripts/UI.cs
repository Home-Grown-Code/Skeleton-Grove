using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private GameObject gameOverUI;
    [Space]
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private TextMeshProUGUI KillCountText;
    private double TimeDisplayed = 0;
    private double LastTimeDisplayed = 0;
    private double TimeCLock = 0;
    private bool IsDead = false;

    private int KillCount;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!IsDead)
        {
        TimeCLock = TimeCLock + (double)Time.deltaTime;
        TimeDisplayed = TimeCLock - LastTimeDisplayed;
        }
        TimeText.text = TimeDisplayed.ToString("F0");
    }

    public void EnableGameOverUI()
    {
        IsDead = true;
        Time.timeScale = 0.2f;
        StartCoroutine(WaitAndStopTime());
    }

    private IEnumerator WaitAndStopTime()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        Time.timeScale = 1f;
        gameOverUI.SetActive(true);
    }

    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
        IsDead = false;
        LastTimeDisplayed = (double)Time.deltaTime;
    }

    public void AddKillCount()
    {
        if (!IsDead)
        {
        KillCount++;
        KillCountText.text = KillCount.ToString();
        }
    }
}