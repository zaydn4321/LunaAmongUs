using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class XordleInput : MonoBehaviour
{
    public InputField input;
    EventSystem system;
    bool wait = false;
    Dictionary<KeyCode, string> keyNames = new Dictionary<KeyCode, string>();
    public XordleRow r;
    bool enable = false;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputField>();
        input.interactable = false;
        system = EventSystem.current;

        // Add all keys to a dictionary
        foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (!keyNames.ContainsKey(k))
            {
                keyNames.Add(k, k.ToString());
            }
        }

        // Map number keys to string values of the numbers
        for (int i = 0; i < 10; i++)
        {
            keyNames[(KeyCode)((int)KeyCode.Alpha0 + i)] = i.ToString();
            keyNames[(KeyCode)((int)KeyCode.Keypad0 + i)] = i.ToString();
        }

        // Include string values of necessary characters
        keyNames[KeyCode.Ampersand] = "&";
        keyNames[KeyCode.Caret] = "^";
        keyNames[KeyCode.Pipe] = "|";
        keyNames[KeyCode.Backslash] = "\\";
        keyNames[KeyCode.Equals] = "=";
    }

    // Update is called once per frame
    void Update()
    {
        if (enable && system.currentSelectedGameObject == gameObject && !wait)
        {
            foreach (KeyCode k in keyNames.Keys)
            {
                if (Input.GetKeyDown(k))
                {
                    Selectable next = null;
                    if (k == KeyCode.Backspace) 
                    {
                        input.text = ""; // Delete text in the current box
                        next = input.FindSelectableOnLeft(); // Box to be selected next is on the left
                    }
                    else if (k == KeyCode.Return)
                    {
                        r.CheckRow();
                        break;
                    }
                    if (keyNames[k].Length == 1) // if key is a printable character
                    {
                        int x = 0;
                        KeyCode kk = k;
                        if (int.TryParse(keyNames[k], out x))
                        {
                            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
                            {
                                switch (x)
                                {
                                    case 6:  // shift + 6 --> ^
                                        kk = KeyCode.Caret;
                                        break;
                                    case 7:  // shift + 7 --> &
                                        kk = KeyCode.Ampersand;
                                        break;
                                }
                            }
                        }
                        else if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
                        {
                            switch (k)
                            {
                                case KeyCode.Backslash: // Shift + \ --> |
                                    kk = KeyCode.Pipe;
                                    break;
                            }
                        }
                        input.text = keyNames[kk];
                        next = input.FindSelectableOnRight(); // Box to be selected next is on the right
                    }

                    if (next != null)
                    {
                        XordleInput xordleInput = next.GetComponent<XordleInput>();
                        if (xordleInput != null)
                        {
                            // Select the next box
                            system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));

                            // Prevent this keypress from affecting the next box that will be selected
                            xordleInput.WaitFrame();
                        }
                    }

                }
            }
        }
        wait = false;
    }

    public void WaitFrame()
    {
        wait = true;
    }

    public void Disable()
    {
        input.interactable = false;
        enable = false;
    }
    
    public void Enable()
    {
        input.interactable = true;
        enable = true;
    }

    public void Green()
    {
        // Make box green

        ColorBlock cb = input.colors;
        cb.normalColor = Color.green;
        cb.disabledColor = Color.green;
        input.colors = cb;
    }

    public void Yellow()
    {
        // Make box yellow

        ColorBlock cb = input.colors;
        cb.normalColor = Color.yellow;
        cb.disabledColor = Color.yellow;
        input.colors = cb;
    }
}
