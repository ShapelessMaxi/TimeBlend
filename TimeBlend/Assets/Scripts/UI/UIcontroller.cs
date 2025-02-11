using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; // Required for UI navigation

public class UIcontroller : MonoBehaviour
{
    public MessageSender messageSender;

    public List<ScreenData> screens = new List<ScreenData>();
    
    public GameObject lockParent;
    public GameObject messagesParent;
    public GameObject mapParent;

    public GameObject clockObject;
    public TMP_Text clockText;
    // public ClockScript = $"{Clock.currentHour:D2}:{Clock.currentMinute:D2}:{Clock.currentSecond:D2}";

    public GameObject menuObject;
    public TMP_Text menuText;

    public GameObject leftButtonHighlight;
    public GameObject rightButtonHighlight;


    public GameObject notificationA;
    public GameObject notificationA_highlight;
    public GameObject notificationA_profilepic;
    public TMP_Text notificationA_name;
    public TMP_Text notificationA_abreviatedMessage;
    public TMP_Text notificationA_timestamp;

    public GameObject notificationB;
    public GameObject notificationB_profilepic;
    public TMP_Text notificationB_name;
    public TMP_Text notificationB_abreviatedMessage;
    public TMP_Text notificationB_timestamp;

    public GameObject notificationC;
    public GameObject notificationC_profilepic;
    public TMP_Text notificationC_name;
    public TMP_Text notificationC_abreviatedMessage;
    public TMP_Text notificationC_timestamp;

    public int currentScreen = 0;
    private int currentMessageIndex = 0;
    private bool isUnlocked = false;
    private bool menuIsActive = false;
    private bool leftMenuSelected = false;
    private bool messageSelected = false;
    
    void Start()
    {
        // get the list of sent messages
        messageSender = GetComponent<MessageSender>();
        
        // Create screen objects
        // displayed menu title
        //empty aprent game object containing all related game objects to hide/display
        screens.Add(new ScreenData("lock", lockParent));
        screens.Add(new ScreenData("msgs", messagesParent));
        screens.Add(new ScreenData("map", mapParent));

        // make sure the right objects are hidden or shown
        clockObject.SetActive(true);
        lockParent.SetActive(true);
        menuObject.SetActive(false);
        leftButtonHighlight.SetActive(false);
        rightButtonHighlight.SetActive(false);
        notificationA.SetActive(false);
        notificationA_highlight.SetActive(false);
        notificationC.SetActive(false);
        notificationB.SetActive(false);
        foreach (ScreenData screen in screens)
        {
            screen.emptyParent.SetActive(false);
        }
        
        UpdateContent();
    }

    void Update()
    {   
        if (!isUnlocked) 
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                UnlockPhone();
            }
        }
        else 
        {
            // Detect right arrow key
            if (!messageSelected)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    menuIsActive = true;
                    HighlightRightButton();
                }
                // Detect left arrow key
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    menuIsActive = true;
                    HighlightLeftButton();
                }
                // Detect down arrow key
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (screens[currentScreen].title == "msgs") 
                    {
                    menuIsActive = false;
                    HighlightMessage();
                    }
                }
                // Detect enter and space keys
                else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    if (menuIsActive) 
                    {
                    ConfirmMenuSelection();
                    }
                }
            }
            else
            {
                // Detect up arrow key
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    menuIsActive = true;
                    messageSelected = false;
                    currentMessageIndex = 0;
                    notificationA_highlight.SetActive(false);
                    leftMenuSelected = true;
                    leftButtonHighlight.SetActive(true);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ShiftNotificationsDown();
                }
            }   
        }

        int messageCount = messageSender.sentMessages.Count;
        int displayCount = Mathf.Min(3, messageCount);

        if (!messageSelected){
            for (int i = 0; i < displayCount; i++)
            {
                // Access messages in reverse order (newest first)
                int index = messageCount - 1 - i;

                // Map positions: newest = "A", second newest = "B", third newest = "C"
                string notificationPosition = GetNotificationPosition(i);
                UpdateNotification(notificationPosition, messageSender.sentMessages[index]);
            }
        }

        // Hide notifications that are no longer needed
        if (displayCount < 3) notificationC.SetActive(false);
        if (displayCount < 2) notificationB.SetActive(false);
    }

    public void ShiftNotificationsDown()
    {
        int messageCount = messageSender.sentMessages.Count;
        if (messageCount > 2)
        {
            // Update the starting index: Shift down the notifications by incrementing the index
            currentMessageIndex++;

            // Loop to update notification positions (A, B, C)
            for (int i = 0; i < Mathf.Min(3, messageCount); i++)
            {
                // Calculate the index for the new "A", "B", "C"
                int index = currentMessageIndex + i;

                // Map positions: newest = "A", second newest = "B", third newest = "C"
                string notificationPosition = GetNotificationPosition(i);
                UpdateNotification(notificationPosition, messageSender.sentMessages[index]);
            }
        }
    }

    public void UpdateNotification(string motificationIdentifier, MessageData message)
    {
        // target the correct game object
        if (motificationIdentifier == "A") {
            notificationA.SetActive(true);
            notificationA_name.text = message.senderName;
            notificationA_timestamp.text = clockText.text;
            notificationA_abreviatedMessage.text = message.fullMessage.Substring(0, 12);
        } 
        else if (motificationIdentifier == "B") 
        {
            notificationB.SetActive(true);
            notificationB_name.text = message.senderName;
            notificationB_timestamp.text = clockText.text;
            notificationB_abreviatedMessage.text = message.fullMessage.Substring(0, 12);
        }
        else if (motificationIdentifier == "C") 
        {
            notificationC.SetActive(true);
            notificationC_name.text = message.senderName;
            notificationC_timestamp.text = clockText.text;
            notificationC_abreviatedMessage.text = message.fullMessage.Substring(0, 12);
        }
    }

    public void HighlightLeftButton()
    {
        leftMenuSelected = true;
        leftButtonHighlight.SetActive(true);
        rightButtonHighlight.SetActive(false);
    }

    public void HighlightRightButton()
    {
        leftMenuSelected = false;
        rightButtonHighlight.SetActive(true);
        leftButtonHighlight.SetActive(false);
    }

    public void HighlightMessage()
    {
        messageSelected = true;
        rightButtonHighlight.SetActive(false);
        leftButtonHighlight.SetActive(false);
        notificationA_highlight.SetActive(true);
    }
    
    public void UnlockPhone()
    {
        // keep status of lockscreen opened
        isUnlocked = true;
        // remove lockscreen from the list of screens available
        screens.RemoveAt(0);
        
        // hide the clock + lockscreen and show the menu
        lockParent.SetActive(false);
        clockObject.SetActive(false);
        menuObject.SetActive(true);

        // decide which screen is opened first
        currentScreen = 0;
        // update the content of the menu and main screen
        UpdateContent();
    }
    
    // Helper method to map index to notification position
    string GetNotificationPosition(int index)
    {
        switch (index)
        {
            case 0: return "A";
            case 1: return "B";
            case 2: return "C";
            default: return "D"; 
        }
    }
    
    public void UpdateContent()
    {
        // update the content of the menu and main screen
        menuText.text = screens[currentScreen].title;
        screens[currentScreen].emptyParent.SetActive(true);
    }

    public void ConfirmMenuSelection()
    {
        if (leftMenuSelected)
        {
            PreviousScreen();
        }
        else
        {
            NextScreen();
        }
    }

    public void NextScreen()
    {
        screens[currentScreen].emptyParent.SetActive(false);
        currentScreen = (currentScreen + 1) % screens.Count;
        UpdateContent();
    }

    public void PreviousScreen()
    {
        screens[currentScreen].emptyParent.SetActive(false);
        currentScreen = (currentScreen - 1 + screens.Count) % screens.Count;
        UpdateContent();
    }
}

