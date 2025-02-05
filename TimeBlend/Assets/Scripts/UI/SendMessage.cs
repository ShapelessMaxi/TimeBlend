using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SendMessage : MonoBehaviour
{
    private MessageLoader messageLoader; // Fix variable name (camel case)

    public List<MessageData> sentMessages = new List<MessageData>();

    public (float min, float max) timeRange = (0f, 5f);

    void Start()
    {
        messageLoader = FindObjectOfType<MessageLoader>(); // Assign MessageLoader reference

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
        int r = UnityEngine.Random.Range(0, messageLoader.randomMessages.Count); // Use messageLoader.randomMessages
        if (!messageLoader.randomMessages[r].isSent && messageLoader.randomMessages[r].canBeSent) 
        {
            // keep track of the message status
            messageLoader.randomMessages[r].isSent = true;
            messageLoader.randomMessages[r].canBeSent = false;
            sentMessages.Add(messageLoader.randomMessages[r]);
        }
    }
}
