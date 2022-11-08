using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Task : MonoBehaviour
{
    public enum TaskType
    {
        Tower,
        CopyCode,
        Hack,
        Hint,
        Xordle
    }

    public TaskType taskType;

    public GameObject screen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if they hit escape, exit the task
        if (Input.GetButton("Cancel"))
        {
            GameManager.instance.player.GetComponent<PlayerController>().canMove = true;
            screen.SetActive(false);
        }
    }

    public string GetName()
    {
        switch (taskType)
        {
            case TaskType.Tower:
                return "Towers of Hanoi";
            case TaskType.CopyCode:
                return "Finish Your Coding Assignment";
            case TaskType.Hack:
                return "Hack Mr. Luna's Computer";
            case TaskType.Hint:
                return "View Password Hint";
            case TaskType.Xordle:
                return "Play Xordle";
        }
        return "";
    }

    // if player is near the task, show the task button 
    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.instance.ActivateTaskBtn(this);
    }

    // if player leaves the task area, hide the task button
    void OnTriggerExit2D(Collider2D other)
    {
        GameManager.instance.DeactivateTaskBtn();
    }

    public void PerformTask()
    {
        screen.SetActive(true);
        GameManager.instance.player.GetComponent<PlayerController>().canMove = false;

        switch (taskType)
        {
            case TaskType.CopyCode:
                screen.GetComponentInChildren<Button>().onClick.AddListener(SubmitCopyTask);
                break;
            case TaskType.Hack:
                screen.GetComponentInChildren<Button>().onClick.AddListener(SubmitHackTask);
                break;
        }
    }

    public void SubmitCopyTask()
    {
        TMP_InputField codeInput = screen.GetComponentInChildren<TMP_InputField>();
        if (codeInput.text == "System.out.println(\"Hello World!\");")
        {
            screen.SetActive(false);
            gameObject.SetActive(false);
            GameManager.instance.CompletedTask();
        }
        else
        {
            codeInput.text = "";
        }
    }

    public void SubmitHackTask()
    {
        InputField passwordInput = GameObject.FindGameObjectsWithTag("PasswordInput")[0].GetComponent<InputField>();
        if (passwordInput.text == "towerxordle")
        {
            screen.SetActive(false);
            gameObject.SetActive(false);
            GameManager.instance.CompletedTask();
        }
        else
        {
            passwordInput.text = "";
        }
    }
}
