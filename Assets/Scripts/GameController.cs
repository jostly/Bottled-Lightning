using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Graphic gameOverLabel;
    public Text scoreText;
    public Text warningsText;
    public Color[] warningsColors;
    public int maxAllowedWarnings;
    public Character[] characters;

    public MinMax conveyorSpeeds;
    public float timeToMaxSpeed;

    private int score;
    private int warnings;
    private bool gameOver;
    private float startTime;

    public static GameController Instance { get; set; }

    public void OutsideBounds(GameObject obj)
    {
        var battery = obj.GetComponent<Battery>();
        if (obj.tag == "Battery" || obj.tag == "Package-1" || obj.tag == "Package-2")
        {
            if (battery != null)
            {
                AddScore(-battery.scoreValue);
            }
            GetComponent<AudioSource>().Play();
            AddWarning();
            Destroy(obj);
        }
    }

    public void RegisterDelivery(int num)
    {
        AddScore(1000 * num);
        warnings = Mathf.Max(warnings - 1, 0);
        UpdateWarnings();
    }

    public void AddScore(int score)
    {
        this.score += score;
        UpdateScore();
    }


    private void AddWarning()
    {
        if (!gameOver)
        {
            warnings += 1;
            UpdateWarnings();
            if (warnings > maxAllowedWarnings)
            {
                GameOver();
            }
        }
    }

    private void UpdateScore()
    {
        scoreText.text = "" + score;
    }

    private void UpdateWarnings()
    {
        warningsText.text = (warnings == 0) ? "No warnings" : ("Warnings: " + warnings);
        warningsText.color = warningsColors[Mathf.Min(warnings, warningsColors.Length - 1)];
    }

    private void GameOver()
    {
        gameOver = true;
        DeepSetState(gameOverLabel, true);
        foreach (var c in characters)
        {
            c.enabled = false;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startTime = Time.time;
        DeepSetState(gameOverLabel, false);
        warnings = 0;
        score = 0;
        gameOver = false;
        UpdateWarnings();
        UpdateScore();
    }

    private void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Intro");
        }
    }

    private void FixedUpdate()
    {
        float conveyorSpeed = conveyorSpeeds.Lerp((Time.time - startTime) / timeToMaxSpeed);
        foreach (var obj in GameObject.FindGameObjectsWithTag("Powered Belt"))
        {
            var c = obj.GetComponent<SurfaceEffector2D>();
            if (c != null)
            {
                if (c.speed > 0f)
                {
                    c.speed = conveyorSpeed;
                } else if (c.speed < 0f)
                {
                    c.speed = -conveyorSpeed;
                }
            }
        }
    }

    private void DeepSetState(Graphic root, bool state)
    {
        root.enabled = state;
        foreach (var c in root.GetComponentsInChildren<Graphic>())
        {
            c.enabled = state;
        }
    }
}
