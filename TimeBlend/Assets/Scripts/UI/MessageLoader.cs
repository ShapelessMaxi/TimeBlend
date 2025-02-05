using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MessageLoader : MonoBehaviour
{
    public List<MessageData> allMessages = new List<MessageData>();
    public List<MessageData> randomMessages = new List<MessageData>();
    public List<MessageData> locationMessages = new List<MessageData>();
    
    public (float min, float max) timeRange = (0f, 5f);
    
    void Start()
    {
        LoadMessages();
    }

    void LoadMessages()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Messages.json");

        if (File.Exists(filePath))
        {
            // Read the content of the JSON file
            string jsonText = File.ReadAllText(filePath);

            // Deserialize the JSON into a MessageList object
            MessageList messageList = JsonUtility.FromJson<MessageList>("{\"allMessages\":" + jsonText + "}");

            // Add all messages to the allMessages list
            allMessages = messageList.allMessages;

            // Categorize messages based on their type
            foreach (var message in allMessages)
            {
                if (message.messageType == "random")
                {
                    randomMessages.Add(message);
                }
                else if (message.messageType == "location")
                {
                    locationMessages.Add(message);
                }
            }
        }
        else
        {
            Debug.LogError("Messages file not found: " + filePath);
        }
    }
}
