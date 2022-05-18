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

    public string IntToText(int value)
    {
        switch(value)
        {
            case 0: return "Zero"; break;
            case 1: return "One"; break;
            case 2: return "Two"; break;
            case 3: return "Three"; break;
            case 4: return "Four"; break;
            case 5: return "Five"; break;
            case 6: return "Six"; break;
            case 7: return "Seven"; break;
            case 8: return "Eight"; break;
            case 9: return "Nine"; break;
            case 10: return "Ten"; break;
            case 11: return "Eleven"; break;
            case 12: return "Twelve"; break;
            case 13: return "Thirteen"; break;
            case 14: return "Fourteen"; break;
            case 15: return "Fifteen"; break;
            case 16: return "Sixteen"; break;
            case 17: return "Seventeen"; break;
            case 18: return "Eighteen"; break;
            case 19: return "Nineteen"; break;
            case 20: return "Twenty"; break;
            default: return "Zero"; break;
        }
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
