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
    public Transform rectangleParent;
    public Transform rectangleShape;
    public Transform rectangleMovePosition;
    [Space]
    public Transform ball;
    public Rigidbody2D ballRigidBody;
    public float raiseSpeed = 1;
    public float moveSpeed = 2;
    public float rotateSpeed = 1;
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
    bool raisingLeft;
    bool raisingRight;

    void FixedUpdate()
    {
        if (raisingLeft || raisingRight)
        {
            if (rectangleParent.position.y < 5)
            {
                rectangleMovePosition.Translate(Vector2.up * Time.deltaTime * raiseSpeed);
                rectangleParent.position = Vector2.Lerp(rectangleParent.position, rectangleMovePosition.position, Time.deltaTime * moveSpeed);
            }
        }

        if (raisingLeft)
        {
            rectangleShape.Rotate(new Vector3(0, 0, -rotateSpeed));
        }

        if (raisingRight)
        {
            rectangleShape.Rotate(new Vector3(0, 0, rotateSpeed));
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
        rectangleMovePosition.position = new Vector2(0, -4);
        rectangleParent.position = new Vector2(0, -4);
        rectangleShape.eulerAngles = new Vector3(0, 0, 0);
        rectangleParent.gameObject.SetActive(true);
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
        rectangleParent.gameObject.SetActive(false);
        rectangleParent.position = new Vector2(0, -100);
        rectangleMovePosition.position = new Vector2(0, -100);
        rectangleShape.eulerAngles = new Vector3(0, 0, 0);
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
        raisingLeft = true;
    }

    public void StopLeftPoint()
    {
        raisingLeft = false;
    }

    public void RaiseRightPoint()
    {
        raisingRight = true;
    }

    public void StopRightPoint()
    {
        raisingRight = false;
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
