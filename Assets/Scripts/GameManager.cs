using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    private float timeAlive = 0;

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
        // Update the time and score UI every frame
        UpdateTime();
        UpdateScore();
    }

    // Method to add score
    public void AddScore(int addScore)
    {
        score += addScore;
    }

    // Method to update time UI
    public void UpdateTime()
    {
        timeAlive += Time.deltaTime;

        // Split time into minutes and seconds
        int timeMinute = (int)timeAlive / 60;
        int timeSeconds = (int)timeAlive % 60;
        
        // Show time text in a two digit format
        timeText.SetText(timeMinute.ToString("D2") + ":" +  timeSeconds.ToString("D2"));
    }

    // Method to update score UI
    public void UpdateScore()
    {
        // Show score text in a two digit format
        scoreText.SetText("Score: " + score.ToString("D2"));
    }

    // Public method to get player alive time in other scripts
    public float GetTimeAlive()
    {
        return timeAlive;
    }
}
