using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    SaveData bestPlayer ;
    
    private bool m_GameOver = false;
    private string currentPlayerName;


    // Start is called before the first frame update
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

        currentPlayerName = StartMenuManager.playerNameText;
        bestPlayer = new SaveData();
        LoadBestScore();
        BestScoreText.text = $"Best Score: {bestPlayer.bestPlayerName} : {bestPlayer.bestPlayerScore}";
    }

    [System.Serializable]
    class SaveData
    {
        public string bestPlayerName;
        public int bestPlayerScore;
    }

    public void SaveBestScroe()
    {
        if(m_Points >= bestPlayer.bestPlayerScore)
        {
            SaveData temp = new SaveData();
            temp.bestPlayerName = currentPlayerName;
            temp.bestPlayerScore = m_Points;

            string json = JsonUtility.ToJson(temp);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData temp;
            temp = JsonUtility.FromJson<SaveData>(json);
            bestPlayer.bestPlayerName = temp.bestPlayerName;
            bestPlayer.bestPlayerScore = temp.bestPlayerScore;
        }
        else
        {
            bestPlayer.bestPlayerName = "";
            bestPlayer.bestPlayerScore = 0;
        }
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
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        SaveBestScroe();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
