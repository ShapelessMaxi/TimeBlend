using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// UI controls            
        // Lock Screen:
        // lock animation, screen saver
        // enter/space = unlock phone (1st screen is active, menu is active)

        // Menu: 
        // Select and change phone screens
        // left/right = scroll through items/screens
        // enter/space = select menu item (selected screen is active)

        // Msgs Screen: 
        // display 3 last mesages received
        // down arrow key = select 1st message annd scroll through messages (1st is always selected)
        // up arrow key = select menu left arrow
        
        // Map Screen:

        // Fishing minigame Screen:

public class UIcontroller : MonoBehaviour
{
    // access to list of sent messages
    public MessageSender messageSender;

    // list containing all Screen objects
    private List<ScreenData> screens = new List<ScreenData>();

    // Clock component (small screen)
    public GameObject clockParent;
    public TMP_Text clockText;
    // having the reference to the actual clock script would be useful
    // public ClockScript = $"{Clock.currentHour:D2}:{Clock.currentMinute:D2}:{Clock.currentSecond:D2}";

    // Menu component (small screen)
    public GameObject menuParent;
    public TMP_Text menuText;
    public GameObject leftMenu_higlight;
    public GameObject rightMenu_higlight;

    // lock screen parent (main screen)
    public GameObject lockParent;

    // message screen parent (main screen)
    public GameObject messagesParent;

    // Received Messages top Notification
    public GameObject notifA;
    public TMP_Text notifA_name;
    public TMP_Text notifA_msg;
    public TMP_Text notifA_time;
    // public GameObject notifA_pic;
    // only need a highligther on the 1st msg
    public GameObject notifA_highlight;

    // Received Messages middle Notification
    public GameObject notifB;
    public TMP_Text notifB_name;
    public TMP_Text notifB_msg;
    public TMP_Text notifB_time;
    // public GameObject notifB_pic;

    // Received Messages bottom Notification
    public GameObject notifC;
    public TMP_Text notifC_name;
    public TMP_Text notifC_msg;
    public TMP_Text notifC_time;
    // public GameObject notifC_pic;

    // map screen parent (main screen)
    public GameObject mapParent;

    // UI status 
    public int currentScreen = 0;
    private bool menuIsActive = false;
    private bool rightMenuSelected = false;
    private bool leftMenuSelected = false;
    private int currentMessageIndex = 0;
    

    // Status reset and setup
    // create screen objects
    // hide/show game objects
    void Start()
    {
        // get the list of sent messages
        messageSender = GetComponent<MessageSender>();
        
        // Create screen objects
        // displayed menu title
        // empty parent game object containing all related game objects
        screens.Add(new ScreenData("lock", lockParent));
        screens.Add(new ScreenData("msgs", messagesParent));
        screens.Add(new ScreenData("map", mapParent));

        // make sure the right objects are active
        clockParent.SetActive(true);
        lockParent.SetActive(true);

        // make sure the right objects are desactivated
        menuParent.SetActive(false);
        leftMenu_higlight.SetActive(false);
        rightMenu_higlight.SetActive(false);
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
         switch(currentScreen)
            {   
                // Lock Screen;
                case 0:

                    // menu is active
                    if (menuIsActive)
                    {
                        MenuControls();                        
                    }
                    // menu is not active
                    else
                    {
                        // Enter
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            menuIsActive = true;
                            
                            // hide the clock and lockscreen
                            clockParent.SetActive(false);
                            lockParent.SetActive(false);
                            // show the menu
                            menuParent.SetActive(true);

                            // change to message screen
                            currentScreen = 1;

                            // update the content of the small and main screen
                            UpdateContent();
                        }
                    }

                    return;

                // Message Screen
                case 1:
                    // display notifications
                    NotifDisplay();

                    // menu is active
                    if (menuIsActive)
                    {
                        MenuControls();                      
                    }
                    // menu is not active
                    else
                    {
                        // Up                        
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            // status check
                            menuIsActive = true;
                            rightMenuSelected = false;
                            leftMenuSelected = true;

                            // game object desactivation
                            notifA_highlight.SetActive(false);
                            rightMenu_higlight.SetActive(false);
                            leftMenu_higlight.SetActive(true);
                        }

                        // Down
                        else if (Input.GetKeyDown(KeyCode.DownArrow) && messageSender.sentMessages != null)
                        {
                            // status check
                            menuIsActive = false;
                            rightMenuSelected = false;
                            leftMenuSelected = false;

                            // game object activation
                            notifA_highlight.SetActive(true);

                            // game object desactivation
                            rightMenu_higlight.SetActive(false);
                            leftMenu_higlight.SetActive(false);
                        }
                    }
                    return;

                // Map Screen
                case 2:
                    // menu is active
                    if (menuIsActive)
                    {
                        MenuControls();                        
                    }
                    // menu is not active
                    else
                    {
                        // Up                        
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            // status check
                            menuIsActive = true;
                            rightMenuSelected = false;
                            leftMenuSelected = true;

                            // game object desactivation
                            notifA_highlight.SetActive(false);
                            rightMenu_higlight.SetActive(false);
                            leftMenu_higlight.SetActive(true);
                        }
                    }
                    return;

                default:
                    return;
            }
    }

    // handle the logic of displaying the message notifications
    void NotifDisplay()
    {
        // Compte le nombre de messages recus
        int messageCount = messageSender.sentMessages.Count;

        // Afficher un maximum de 3 notification
        // Retourne le nombre entier le plus petit entre 3 et le nombres de messages recus
        int displayCount = Mathf.Min(3, messageCount);

        // Cacher les notifications superflues
        if (displayCount < 3) notifC.SetActive(false);
        if (displayCount < 2) notifB.SetActive(false);

        // count up to 3 (displayed notification)
        for (int i = 0; i < displayCount; i++)
        {
            // Access messages in reverse order (newest first)
            int msgIndex = messageCount - 1 - i;
            WriteNotification(i, messageSender.sentMessages[msgIndex]);
        }

    }
    
    // Write the correct info on the correct game object
    void WriteNotification(int index, MessageData message)
    {
        switch(index) {
            case 0:
                notifA.SetActive(true);
                notifA_name.text = message.senderName;
                notifA_time.text = clockText.text;
                notifA_msg.text = message.fullMessage.Substring(0, 12);
                return;

            case 1:
                notifB.SetActive(true);
                notifB_name.text = message.senderName;
                notifB_time.text = clockText.text;
                notifB_msg.text = message.fullMessage.Substring(0, 12);
                return;
            
            case 2:
                notifC.SetActive(true);
                notifC_name.text = message.senderName;
                notifC_time.text = clockText.text;
                notifC_msg.text = message.fullMessage.Substring(0, 12);
                return;

            default:
                // should never be anything else other than 1, 2 or 3
                return;
        }
    }

    // ui controls when the menu is active
    void MenuControls()
    {
        // Right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // status check
            menuIsActive = true;
            rightMenuSelected = true;
            leftMenuSelected = false;

            // game object activation
            rightMenu_higlight.SetActive(true);
            // game object desactivation
            leftMenu_higlight.SetActive(false);
        }

        // Left
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // status check
            menuIsActive = true;
            rightMenuSelected = false;
            leftMenuSelected = true;

            // game object activation
            leftMenu_higlight.SetActive(true);
            // game object desactivation
            rightMenu_higlight.SetActive(false);
        }

        // Down
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // status check
            menuIsActive = false;
            rightMenuSelected = false;
            leftMenuSelected = false;

            // game object desactivation
            rightMenu_higlight.SetActive(false);
            leftMenu_higlight.SetActive(false);

            switch(currentScreen)
            {   
                // message screen
                case 1:
                    notifA_highlight.SetActive(true);
                    return;

                default:
                    return;
            }
        }

        // Enter
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            screens[currentScreen].emptyParent.SetActive(false);
            
            // select left 
            if (leftMenuSelected) 
            {
                // change screens
                currentScreen = (currentScreen - 1 + screens.Count) % screens.Count;
                UpdateContent();
            }
            // select rgiht
            else if (rightMenuSelected)
            {
                // change screens
                currentScreen = (currentScreen + 1) % screens.Count;
                UpdateContent();
            }
        }
    }

    // display main screen content
    void UpdateContent()
    {
        // update the content of the menu and main screen
        menuText.text = screens[currentScreen].title;
        screens[currentScreen].emptyParent.SetActive(true);
    }

    // to update
    // scrol through messages / shift the messages up 
    void ShiftNotifications()
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
                int msgIndex = currentMessageIndex + i;

                WriteNotification(i, messageSender.sentMessages[msgIndex]);
            }
        }
    }

}