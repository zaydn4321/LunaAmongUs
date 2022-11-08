using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject player;

    public Button taskButton;

    public GameObject copyTask;
    public GameObject towerTask;
    public GameObject hackTask;
    public GameObject xordleTask;

    public GameObject taskSlider;
    public GameObject timerObj;
    public GameObject gameOverScreen;

    int completedTasks = 0;
    float totalTasks = 4;
    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;

        taskButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer
        time += Time.deltaTime;
        int sec = ((int)time) % 60;
        int min = ((int)time - sec) / 60;
        timerObj.GetComponent<Text>().text = min.ToString("00.##") + ":" + sec.ToString("00.##");
    }

    public void ActivateTaskBtn(Task t)
    {
        taskButton.gameObject.SetActive(true);
        taskButton.GetComponentInChildren<Text>().text = t.GetName(); // set button text to task name
        taskButton.onClick.AddListener(t.PerformTask);
    }

    public void DeactivateTaskBtn()
    {
        taskButton.onClick.RemoveAllListeners();
        taskButton.gameObject.SetActive(false);
    }

    public void CompletedTask()
    {
        DeactivateTaskBtn();
        player.GetComponent<PlayerController>().canMove = true;
        completedTasks++;

        // Update task bar
        taskSlider.GetComponent<Slider>().value = completedTasks / totalTasks;
        if (completedTasks == totalTasks)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        // Display time on game over screen
        Text t = GameObject.FindGameObjectsWithTag("Time")[0].GetComponent<Text>();
        t.text = timerObj.GetComponent<Text>().text;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TowerTaskDone()
    {
        towerTask.GetComponent<Task>().screen.SetActive(false);
        towerTask.SetActive(false);
        CompletedTask();
    }

    public void XordleTaskDone()
    {
        xordleTask.GetComponent<Task>().screen.SetActive(false);
        xordleTask.SetActive(false);
        CompletedTask();
    }
}
