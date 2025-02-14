using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;


// UI controls            
    // Lock Screen:
        // displays screen saver, amount of notifications
        // enter = unlock phone (1st screen is active, menu is active)

    // Menu: 
        // Select and change phone screens
        // left/right = scroll through items/screens
        // enter = select menu item (selected screen is active)
        // down = menu is inactive, select something on the screen

    // Msgs Screen: 
        // display 3 last mesages received
        // down = select and scroll through messages (1st is always selected)
        // up = select menu left arrow
    
    // Conversation Screen:
        // display the full message received, and answer it (a few options)
        // left/right: select answer
        // enter: send answer

    // Map Screen:

    // Fishing minigame Screen:

public class UIcontroller : MonoBehaviour
{
    // access to list of sent messages
    [Header("Scripts")]
    public MessageSender messageSender;
    private List<MessageData> messagesFrozen = new List<MessageData>();
    private List<MessageData> messagesShifting = new List<MessageData>();
    private List<MessageData> messagesReversed = new List<MessageData>();

    // list containing all Screen objects
    private List<ScreenData> screens = new List<ScreenData>();

    // Clock component (small screen)
    [Header("Time-Related OBJS")]
    public GameObject clockParent;
    public TMP_Text clockText;
    // having the reference to the actual clock script would be useful
    // public ClockScript = $"{Clock.currentHour:D2}:{Clock.currentMinute:D2}:{Clock.currentSecond:D2}";

    // Menu component (small screen)
    [Header("Menu-Related OBJS")]
    public GameObject menuParent;
    public TMP_Text menuText;
    public GameObject leftMenu_hl;
    public GameObject rightMenu_hl;

    // subMenu component (main screen)
    [Header("SubMenu-Related OBJS")]
    public GameObject subMenuParent;
    public GameObject upButton_hl;
    public GameObject downButton_hl;

    // lock screen parent (main screen)
    [Header("LockScreen-Related OBJS")]
    public GameObject lockParent;

    // message screen parent (main screen)
    [Header("MessagesScreen-Related OBJS")]
    public GameObject messagesParent;

    // Received Messages top Notification
    [Header("Top Notification")]
    public GameObject notifA;
    public TMP_Text notifA_name;
    public TMP_Text notifA_msg;
    public TMP_Text notifA_time;
    // public GameObject notifA_pic;
    // only need a highligther on the 1st msg
    public GameObject notifA_highlight;

    // Received Messages middle Notification
    [Header("Middle Notification")]
    public GameObject notifB;
    public TMP_Text notifB_name;
    public TMP_Text notifB_msg;
    public TMP_Text notifB_time;
    // public GameObject notifB_pic;

    // Received Messages bottom Notification
    [Header("Bottom Notification")]
    public GameObject notifC;
    public TMP_Text notifC_name;
    public TMP_Text notifC_msg;
    public TMP_Text notifC_time;
    // public GameObject notifC_pic;

    // map screen parent (main screen)
    [Header("MapScreen-Related OBJS")]
    public GameObject mapParent;

    // UI status 
    [Header("Status")]
    public int currentScreen = 0;
    public int currentMessage = 0;

    public bool menuIsActive = false;
    private bool rightMenuSelected = false;
    private bool leftMenuSelected = false;

    public bool subMenuIsActive = false;
    private bool upButtonSelected = false;
    private bool downButtonSelected = false;

    public bool screenIsActive = false;


    // Status reset and setup
    // create screen objects
    // hide/show game objects
    void Start()
    {
        // get the list of sent messages
        messageSender = GetComponent<MessageSender>();
        
        // Create screen objects: title+ empty parent game object containing all related game objects
        screens.Add(new ScreenData("lock", lockParent));
        screens.Add(new ScreenData("msgs", messagesParent));
        screens.Add(new ScreenData("map", mapParent));

        // make sure the right objects are active
        clockParent.SetActive(true);
        lockParent.SetActive(true);

        // make sure the right objects are desactivated
        menuParent.SetActive(false);
        leftMenu_hl.SetActive(false);
        rightMenu_hl.SetActive(false);

        subMenuParent.SetActive(false);
        upButton_hl.SetActive(false);
        downButton_hl.SetActive(false);

        screens.ForEach(screen => screen.emptyParent.SetActive(false));

        notifA_highlight.SetActive(false);
        notifA.SetActive(false);
        notifC.SetActive(false);
        notifB.SetActive(false);
        
        UpdateContent();
    }
    
    // runs each frame
    // key mapped logic for interaction witht the phone
    void Update()
    {   
        // menu is active
        if (menuIsActive)
        {
            MenuControls();
        }
        
        // submenu is active
        else if (subMenuIsActive)
        {
            subMenuControls();
        }
        
        // mainscreen is active
        else if (screenIsActive)
        {
            // check wich screen is currently selected
            switch(currentScreen)
            {    
                // Lock Screen
                case 0:
                    // Enter: activate menu, switch to message screen
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        // change to message screen
                        currentScreen = 1;

                        // show the menu
                        menuParent.SetActive(true);

                        // hide the clock and lockscreen
                        clockParent.SetActive(false);
                        lockParent.SetActive(false);

                        // activate the menu, status check, hide/show objects 
                        ActivateMenu();

                        // update the content of the small and main screen
                        UpdateContent();
                    }

                    return;

                // Message Screen
                case 1:
                    // block the reception of messages
                    messagesFrozen = new List<MessageData>(messageSender.sentMessages);

                    // Up
                    if (subMenuIsActive)
                    {
                        
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow) && !subMenuIsActive)
                    {
                        ActivateMenu();
                    }

                    // Down
                    else if (Input.GetKeyDown(KeyCode.DownArrow) && subMenuIsActive)
                    {
                        // scroll through messages (shift list)
                        // replace with ShiftNotifDisplay()
                        // UpdateNotification(messagesShifting);
                    }
                    else if  (Input.GetKeyDown(KeyCode.DownArrow) && !subMenuIsActive && messageSender.sentMessages != null)
                    {
                        // activate submenu
                        ActivateSubMenu();
                    }
                    
                    // Enter
                    else if (Input.GetKeyDown(KeyCode.Return) && subMenuIsActive)
                    {
                        // select left 
                        if (upButtonSelected) 
                        {
                            // change screens
                            currentScreen = (currentScreen - 1 + screens.Count) % screens.Count;
                            UpdateContent();
                        }
                        // select rgiht
                        else if (downButtonSelected)
                        {
                            // change screens
                            currentScreen = (currentScreen + 1) % screens.Count;
                            UpdateContent();
                        }

                        // select 1st message
                        notifA_highlight.SetActive(true);
                    }

                    return;
                        
                // Map Screen
                case 2:

                    return;

                default:
                    return;
            }
        }

        // all other status are false, meaning the phone is locked
        // Down: unlock phone, activate menu
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // activate main menu
            ActivateMenu();

            // show submenu (still desativated)
            subMenuParent.SetActive(true);

            // hide the clock 
            clockParent.SetActive(false);
            // hide lockscreen pixel animation
            lockParent.SetActive(false);

            // change and update the screen
            currentScreen = 1;
            UpdateContent();
        }
    }
    
    // ui controls when the menu is active
    void MenuControls()
    {
        // Right: select right arrow key
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // status check
            menuIsActive = true;
            rightMenuSelected = true;
            leftMenuSelected = false;

            // game object activation
            rightMenu_hl.SetActive(true);
            leftMenu_hl.SetActive(false);
        }

        // Left: select left arrow key
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // status check
            menuIsActive = true;
            rightMenuSelected = false;
            leftMenuSelected = true;

            // game object activation
            leftMenu_hl.SetActive(true);
            rightMenu_hl.SetActive(false);
        }

        // Down: activate subMenu, desactivate menu
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DesactivateMenu();
            ActivateSubMenu();

            // extra things depending on the screen
            switch(currentScreen)
            {   
                // message screen: update the notifications, select up arrow 
                case 1:
                    upButton_hl.SetActive(true);
                    if (messageSender.sentMessages != null)
                    {
                        UpdateNotification(messageSender.sentMessages);
                    }
                    return;

                default:
                    return;
            }
        }

        // Enter: Confirm, Switch through the options (screens)
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // desactivate the current screen content
            screens[currentScreen].emptyParent.SetActive(false);
            
            // activate the previous or next screen
            if (leftMenuSelected) 
            {
                // activate the previous screen content
                currentScreen = (currentScreen - 1 + screens.Count) % screens.Count;
                UpdateContent();
            }
            else if (rightMenuSelected)
            {
                // activate the next screen content
                currentScreen = (currentScreen + 1) % screens.Count;
                UpdateContent();
            }
        }
    }

    // ui controls when the subMenu is active
    void subMenuControls()
    {
        // Right: activate screen, desactivate subMenu
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Menu status and objs off
            DesactivateMenu();

            // Screen status and objs on
            ActivateScreen(); 
        }

        // Up: higlight up arrow
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // update selection status
            downButtonSelected = false;
            upButtonSelected = true;

            // game object desactivation
            downButton_hl.SetActive(false);
            leftMenu_hl.SetActive(true);
        }

        // Down: highlight down arrow
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // update selection status
            downButtonSelected = true;
            upButtonSelected = false;

            // game object desactivation
            downButton_hl.SetActive(true);
            leftMenu_hl.SetActive(false);
        }

        // Enter: Scroll the list of message up or down
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (upButtonSelected)
            {
                // function that scroll up
                currentMessage = (currentMessage - 1 + screens.Count) % screens.Count;

                // UpdateNotification(messagesFrozen);

            }
            else if (downButtonSelected)
            {
                // function that scroll down
            }
        }
    }

    // ui controls when the screen is active
    void ScreenControls()
    {
        // Left, When screen is active
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // reactivate submenu
            ActivateSubMenu();
        }

        // Down
    }

    // set status, show/hide g objects
    void ActivateMenu() 
    {
            // status check
            menuIsActive = true;
            subMenuIsActive = false;
            screenIsActive = false;

            rightMenuSelected = false;
            leftMenuSelected = true;

            upButtonSelected = false;
            downButtonSelected = false;

            // game object activation
            menuParent.SetActive(true);

            leftMenu_hl.SetActive(true);
            rightMenu_hl.SetActive(false);

            upButton_hl.SetActive(false);
            downButton_hl.SetActive(false);

            notifA_highlight.SetActive(false);
    }
    
    void DesactivateMenu()
    {
        // status check
        menuIsActive = false;
        rightMenuSelected = false;
        leftMenuSelected = false;

        // game object desactivation
        rightMenu_hl.SetActive(false);
        leftMenu_hl.SetActive(false);
    }

    // set status, show/hide g objects
    void ActivateSubMenu()
    {
        // status check
        subMenuIsActive = true;
        menuIsActive = false;

        rightMenuSelected = false;
        leftMenuSelected = false;

        upButtonSelected = true;
        downButtonSelected = false;

        // game object activation
        downButton_hl.SetActive(true);

        rightMenu_hl.SetActive(false);
        leftMenu_hl.SetActive(false);
        upButton_hl.SetActive(false);
    } 
    
    void DesactivateSubMenu()
    {
        // status check
        subMenuIsActive = false;
        upButtonSelected = false;
        downButtonSelected = false;

        // game object desactivation
        rightMenu_hl.SetActive(false);
        leftMenu_hl.SetActive(false);
    }

    // set status, show/hide g objects
    void ActivateScreen()
    {
        // // status check
        // subMenuIsActive = true;
        // menuIsActive = false;
        // rightMenuSelected = false;
        // leftMenuSelected = false;
        // upButtonSelected = true;
        // downButtonSelected = false;

        // // game object activation
        // downButton_hl.SetActive(true);

        // // game object desactivation
        // rightMenu_hl.SetActive(false);
        // leftMenu_hl.SetActive(false);
        // upButton_hl.SetActive(false);
    } 

    void DesactivateScreen()
    {

    }

    // display main screen content
    void UpdateContent()
    {
        // update the content of the menu and main screen
        menuText.text = screens[currentScreen].title;
        screens[currentScreen].emptyParent.SetActive(true);

        switch(currentScreen)
        {
            case 0:
                // UpdateContent();
    
                return;
            case 1:
                UpdateNotification(messageSender.sentMessages);
                return;
        }    
    }

    // new function
    void UpdateNotification(List<MessageData> messages)
    {
        // Count of message list
        int messagesCount = messages.Count;

        // Display a max of 3 Notifications
        int displayCount = Mathf.Min(3, messagesCount);

        // Hide the notification objs if theres less than 1/2/3 messages
        notifA.SetActive(displayCount > 0);
        notifB.SetActive(displayCount > 1);
        notifC.SetActive(displayCount > 2);

        // Keep a reversed copy of the frozen message list
        messagesReversed = new List<MessageData>(messages);
        messagesReversed.Reverse();

        // Count up to 3, Display the correct messages at the correct spot
        for (int i = 0; i < displayCount; i++)
        {
            // Keep track of current messages
            MessageData currentMessage = messagesReversed[i];
            string truncatedMessage = currentMessage.fullMessage.Substring(0, 12);
            string timestamp = clockText.text.Substring(0, 5);
            
            switch(i) {
                case 0:
                    notifA_name.text = currentMessage.senderName;
                    notifA_time.text = timestamp;
                    notifA_msg.text = currentMessage.fullMessage.Substring(0, 24);
                    break;

                case 1:
                    notifB_name.text = currentMessage.senderName;
                    notifB_time.text = timestamp;
                    notifA_msg.text = truncatedMessage;
                    break;
                
                case 2:
                    notifC_name.text = currentMessage.senderName;
                    notifC_time.text = timestamp;
                    notifA_msg.text = truncatedMessage;
                    break;

                default:
                    // should never be anything else other than 1, 2 or 3
                    return;
            }
        }
    }

}