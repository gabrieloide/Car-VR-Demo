using UnityEngine;

public class Screen : MonoBehaviour
{
    public static Screen Instance { get; private set; }
    public GameObject currentActiveScreen;   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        currentActiveScreen = GameObject.Find("HomeScreen");
        var screens = GameObject.Find("Screens");
        
        for (var i = 0; i < screens.transform.childCount; i++)
        {
            screens.transform.GetChild(i).gameObject.SetActive(false);
            
        }
        
        currentActiveScreen.SetActive(true);
    }

}
