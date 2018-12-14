using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance; //이 앱의 유일한 싱글턴

    [SerializeField]
    private Text ScoreText;

    [SerializeField]
    private PreviewBlock PreviewBoard;

    [SerializeField]
    private GameObject GameOverPanel;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetScore(int InputScore)
    {
        ScoreText.text = "Score : " + InputScore;
    }

    public void ShowPreviewBlock(BlockID InputID)
    {
        PreviewBoard.SetPreviewBlock(InputID);
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
