using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] int score;
    [SerializeField] int highscore;
    public Color[] template = { new Color32(255, 81, 81, 255), new Color32(255, 129, 82, 255), new Color32(255, 233, 82, 255), new Color32(163, 255, 82, 255), new Color32(82, 207, 255, 255), new Color32(170, 82, 255, 255) };

    [SerializeField] int currentTarget = 0;
    [SerializeField] int currentFirst = 0;
    [SerializeField] int currentLast = 0;
    [SerializeField] int theNumberOfNumber = 0;

    private UIController uiController;

    private float time;
    [SerializeField] float timeOfGame;


    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<UIController>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        UpdateSlider();

        if(time < 0)
        {
            GameOver();
        }
    }

    public void UpdateSlider()
    {
        uiController.UpdateSlider(time);
    }

    public void SetSlider()
    {
        uiController.SetSlider(timeOfGame);
    }

    public void OnPressHandle(int index)
    {
        if(theNumberOfNumber == 2)
        {
            if (currentFirst == -1)
            {
                currentFirst = index;
                uiController.ShowNumberFirst(index, true);
            }else
            {
                currentLast = index;
                uiController.ShowNumberLast(index, true);

                Check();
            }
        }
        else if (theNumberOfNumber == 1)
        {
            currentFirst = index;
            uiController.ShowNumberFirst(index, true);
            Check();
        }
    }

    public void Check()
    {
        if (theNumberOfNumber == 2)
        {
            if(currentFirst * 10 + currentLast == currentTarget)
            {
                UpdateScore();
                StartCoroutine(StartAfterTime());
                return;
            }
            else
            {
                GameOver();
            }
        }
        else if (theNumberOfNumber == 1)
        {
            if (currentFirst == currentTarget)
            {
                UpdateScore();
                StartCoroutine(StartAfterTime());
                return;
            }
            else
            {
                GameOver();
            }
        }

        
    }

    public void HandleShowNumber(int value)
    {

    }

    public void GameOver()
    {
        Time.timeScale = 0;
        SoundController.Instance.PlayAudio(SoundController.Instance.gameOver, 0.8f, false);
        uiController.GameOver();
        Reset();
    }

    public void UpdateScore()
    {
        score++;
        if(highscore < score)
        {
            highscore = score;
            PlayerPrefs.SetInt("score", highscore);
            uiController.UpdateHighScore(highscore);
        }
        uiController.UpdateScore(score);
    }

    IEnumerator StartAfterTime()
    {
        yield return new WaitForSeconds(.5f);
        NextTurn();
    }
    public void NextTurn()
    {
        currentFirst = -1;
        currentLast = -1;
        uiController.ShowNumberFirst(0, false);
        uiController.ShowNumberLast(0, false);

        currentTarget = Random.Range(0, 20);
        uiController.UpdateNumber(IntToText(currentTarget));

        time = timeOfGame;

        if (currentTarget > 9)
            theNumberOfNumber = 2;
        else theNumberOfNumber = 1;
    }

    private string[] listNumbers = {"Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                                    "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty"};
    public string IntToText(int value)
    {
        if (value < 0 || value > listNumbers.Length) return listNumbers[0];

        return listNumbers[value];
    }

    public void Reset()
    {
        Time.timeScale = 1;

        time = timeOfGame;
        SetSlider();
        score = 0;
        uiController.UpdateScore(score);
        uiController.UpdateHighScore(PlayerPrefs.GetInt("score"));

        StartCoroutine(StartAfterTime());
    }

}
