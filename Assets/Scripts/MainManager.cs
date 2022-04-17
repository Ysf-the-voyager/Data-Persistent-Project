using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    // þu anki oyuncunun ekrandaki adý ve skoru
    public TextMeshProUGUI currentPlayer;
    public Text ScoreText;

    public Text bestScoreText;

    public string bestPlayer;
    public int bestScore;

    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private void Awake()
    {
        LoadRank();
    }

    void Start()
    {   
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        SetBestRank();
        currentPlayer.text = PlayerDataHandler.instance.currentPlayerName;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        PlayerDataHandler.instance.currentScore= m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        CheckBestScore();
        GameOverText.SetActive(true);
    }

    public void SetBestRank()
    {
        if (bestPlayer == null && bestScore == 0)
        {
            bestScoreText.text = "";
        }
        else 
        {
            bestScoreText.text = $"Best Score -{bestPlayer}:{bestScore}";
        }      
    }

    void CheckBestScore()
    { 
        int CurrentScore = PlayerDataHandler.instance.currentScore;
        string CurrentPlayer = PlayerDataHandler.instance.currentPlayerName;
        if (CurrentScore > bestScore)
        {
            bestPlayer = CurrentPlayer;
            bestScore = CurrentScore;

            bestScoreText.text = $"Best Score -{bestPlayer}:{bestScore}";
            SaveRank(bestPlayer,bestScore);
        }
    }
    public void LoadRank()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
           string json= File.ReadAllText(path);
           SaveData data= JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.s_theBestPlayer;
            bestScore = data.s_theBestScore;
        }
    }
    
    void SaveRank(string theBestPlayer,int theBestScore)
    {
        SaveData data = new SaveData();
        data.s_theBestPlayer = theBestPlayer;
        data.s_theBestScore = theBestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);       
    }

    [System.Serializable]
    class SaveData
    {
        public string s_theBestPlayer;
        public int s_theBestScore;
    }
}
