using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMessage : MonoBehaviour
{
    public List<ScreenData.MessageData> randomMessages = new List<ScreenData.MessageData>();
    public List<ScreenData.MessageData> locationMessages = new List<ScreenData.MessageData>();

    private int timer; 
    public (float min, float max) timeRange = (0f, 20f);

    void Start()
    {
        // Create all randomly sent message objects:
        // can be sent bool
        // sender name
        // time received at (hour, minute, second)
        // abbreviated message displayed on main msg screen
        // full mesage displayed when opening the message
        randomMessages.Add(new ScreenData.MessageData(true, "Kriss", (09, 20, 14), "Hey where ar", "Hey where are u did you forget our date again??"));
        randomMessages.Add(new ScreenData.MessageData(false, "Kriss", (09, 23, 03), "im literally", "im literally waiting for you here at the cafe..."));
        // 
        // Create all location based message objects:
        locationMessages.Add(new ScreenData.MessageData(false, "HungTopFun", (09, 20, 14), "Hey sexy, you", "Hey sexy, your like 10 meters away, looking?"));

        // coroutine for almost randomly sent messages
        StartCoroutine(MessageRoutine());
    }

    IEnumerator MessageRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(timeRange.min, timeRange.max);
            yield return new WaitForSeconds(waitTime);
            
            SendRandomMessage();
        }
    }

    void SendRandomMessage()
    {
        Debug.Log("Random timed message triggered!");
        int r = UnityEngine.Random.Range(0, randomMessages.Count);
        if (randomMessages[r].canBeSent) {
            createNotification(randomMessages[r]);
        }
    }

    void createNotification(ScreenData.MessageData message)
    {
        // keep track of the message status
        message.canBeSent = false;

        Debug.Log(message.senderName);
        // create a copy of the message template game object
        // change displayed text
        // change timestamp according to phone clock
    }   

    // Update is called once per frame
    void Update()
    {
        // decide on the logic behind sending the messages....
        // options:
        // (block certain message with the bool can be sent)
        // 1- randomly. set a timer, and release a message every ~1-3min. (hey are we still doing the pre at your place later?)
        // 2- location based. set spots on the map, when you walk by them, send. (heey i just saw you at the coffee shop)
        // 3- probably a mix of both actually
    }
}
