using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainManager : Singleton<MainManager>
{
    public GameObject menuPanel;
    public GameObject gamePanel;
    [Space]
    public LineRendererController lineRendererController;
    public GameObject lineParent;
    public Transform ball;
    public Rigidbody2D ballRigidBody;
    public float raiseSpeed = 1;
    public float maxDistanceBetweenPoints = -4;
    [Space]
    public TextMeshProUGUI levelNumberText;
    public List<GameObject> levelObjects;
    public GameObject restartGamePanel;
    [Space]
    bool isGameMuted;
    public Image soundMuteButtonIcon;
    public AudioSource song;
    public AudioSource winSound;
    public AudioSource finishGameSound;
    public AudioSource loseSound;

    int currentLevel = 1;
    bool raisingLeftPoint;
    bool raisingRightPoint;

    void Update()
    {
        if (raisingLeftPoint)
        {
            if (lineRendererController.checkDistanceBetweenPoints(true) <= maxDistanceBetweenPoints && lineRendererController.CanMoveHigher(true))
            {
                lineRendererController.leftPoint.transform.Translate(Vector3.up * Time.deltaTime * raiseSpeed);
            }
        }

        if (raisingRightPoint)
        {
            if (lineRendererController.checkDistanceBetweenPoints(false) <= maxDistanceBetweenPoints && lineRendererController.CanMoveHigher(false))
            {
                lineRendererController.rightPoint.transform.Translate(Vector3.up * Time.deltaTime * raiseSpeed);
            }
        }

        //CHEAT
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextLevel();
        }
    }

    public void StartLevel(int level)
    {
        foreach (GameObject levelObject in levelObjects)
        {
            levelObject.SetActive(false);
        }
        levelObjects[currentLevel - 1].SetActive(true);
        levelNumberText.text = currentLevel.ToString("0");
        ResetLevel();
    }

    public void ResetLevel()
    {
        lineRendererController.leftPoint.position = new Vector2(-2, -4);
        lineRendererController.rightPoint.position = new Vector2(2, -4);
        lineParent.SetActive(true);
        ball.position = new Vector2(0, -3.6f);
        ball.gameObject.SetActive(true);
        ballRigidBody.velocity = Vector2.zero;
    }

    public void NextLevel()
    {
        currentLevel++;
        ball.gameObject.SetActive(false);
        PlaySound(0);
        if (currentLevel <= 6)
        {
            StartCoroutine(GoingToNextLevel());
        }
        else
        {
            StartCoroutine(EndingTheGame());
        }
    }

    IEnumerator GoingToNextLevel()
    {
        yield return new WaitForSeconds(0.5f);

        StartLevel(currentLevel);
    }

    IEnumerator EndingTheGame()
    {
        yield return new WaitForSeconds(0.5f);

        BeatTheGame();
    }

    public void LoseLevel()
    {
        PlaySound(1);
        ResetLevel();
    }

    void BeatTheGame()
    {
        PlaySound(2);   
        foreach (GameObject levelObject in levelObjects)
        {
            levelObject.SetActive(false);
        }
        restartGamePanel.SetActive(true);
        lineParent.SetActive(false);
        lineRendererController.leftPoint.position = new Vector2(0, -100);
        lineRendererController.rightPoint.position = new Vector2(0, -100);
        ball.gameObject.SetActive(false);
        levelNumberText.text = "YOU WIN!";
    }

    public void RestartGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        restartGamePanel.SetActive(false);
        currentLevel = 1;
        StartLevel(currentLevel);
    }

    public void RaiseLeftPoint()
    {
        raisingLeftPoint = true;
    }

    public void StopLeftPoint()
    {
        raisingLeftPoint = false;
    }

    public void RaiseRightPoint()
    {
        raisingRightPoint = true;
    }

    public void StopRightPoint()
    {
        raisingRightPoint = false;
    }

    public void PlaySound(int id)
    {
        if (isGameMuted)
        {
            return;
        }

        switch (id)
        {
            case 0:
                winSound.Play();
                break;
            case 1:
                loseSound.Play();
                break;

            case 2:
                finishGameSound.Play();
                break;
        }
    }

    public void MuteGame()
    {
        isGameMuted = !isGameMuted;
        if (isGameMuted)
        {
            soundMuteButtonIcon.color = Color.red;
            song.Pause();
        }
        else
        {
            soundMuteButtonIcon.color = Color.black;
            song.Play();
        }
    }
}
