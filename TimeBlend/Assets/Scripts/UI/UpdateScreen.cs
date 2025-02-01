using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpdateScreen : MonoBehaviour
{
    public List<ScreenData> screens = new List<ScreenData>();
    public int currentScreen = 0;

    public TMP_Text menuText;

    void Start()
    {
        // Create screen objects
        // menu title, empty parent object containing all relevant game objects
        screens.Add(new ScreenData("msgs", GameObject.Find("Messages")));
        screens.Add(new ScreenData("map", GameObject.Find("Map")));
        screens.Add(new ScreenData("test01", GameObject.Find("Map")));
        screens.Add(new ScreenData("test02", GameObject.Find("Map")));
    }

    public void HidePreviousContent()
    {
        screens[currentScreen].emptyParent.SetActive(false);  
    }

    public void UpdateContent()
    {
        // Set the text of the menuText
        menuText.text = screens[currentScreen].title;

        // show related game objects
        screens[currentScreen].emptyParent.SetActive(true);
    }

    public void NextScreen()
    {
        // Hide the previous screen
        HidePreviousContent();
        
        // Increment the index and loop back
        currentScreen = (currentScreen + 1) % screens.Count;

        UpdateContent();
    }

    public void PreviousScreen()
    {
        // Hide the current screen
        HidePreviousContent();

        // Decrement the index and loop back
        currentScreen = (currentScreen - 1 + screens.Count) % screens.Count;

        UpdateContent();
    }
}
