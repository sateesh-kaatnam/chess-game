using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinBoardScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Button mainMenu;
    public TextMeshProUGUI textDisplay;
    public static string defaultText = "Default Text";
    void Start()
    {
        textDisplay.text = defaultText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
