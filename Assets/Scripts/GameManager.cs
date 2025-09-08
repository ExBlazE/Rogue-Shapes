using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;
    private float timeAlive;

    public static GameManager Instance;

    void Awake()
    {
        // Singleton reference to this script
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        timeAlive = 0;
        score = 0;
    }

    void Update()
    {
        UpdateTime();
        UpdateScore();
    }

    public void AddScore(int addScore)
    {
        score += addScore;
    }

    public void UpdateTime()
    {
        timeAlive += Time.deltaTime;

        int timeMinute = (int)timeAlive / 60;
        int timeSeconds = (int)timeAlive % 60;
        
        timeText.SetText(timeMinute.ToString("D2") + ":" +  timeSeconds.ToString("D2"));
    }

    public void UpdateScore()
    {
        scoreText.SetText("Score: " + score.ToString("D2"));
    }
}
