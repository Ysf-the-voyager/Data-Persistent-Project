using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI uguiText;
    [SerializeField] TextMeshProUGUI uguiBestScore;
    string bestPlayerName;
    int bestPlayerScore;

    private void Awake()
    {
        Load();
    }
    void Start()
    {   
        uguiBestScore.text = $"Best Score - {bestPlayerName} : {bestPlayerScore}";
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log(path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start()
    {
        SceneManager.LoadScene(1);
        PlayerDataHandler.instance.currentPlayerName = uguiText.text;
    }
    void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
         
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestPlayerName = data.s_theBestPlayer;
            bestPlayerScore = data.s_theBestScore;
        }
    }

    [System.Serializable]
    class SaveData 
    {
        public string s_theBestPlayer;
        public int s_theBestScore;   
    }
}
