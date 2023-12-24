using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Slider difficultySlider;
    [SerializeField] Button doublePlayer, playerVsAI, doubleAI, playerVsAI1;

    private int difficultyLevel = 0;


    //Sounds
    public AudioClip buttonClick;

    void Start()
    {
        doublePlayer.onClick.AddListener(delegate { PlayGame(1); });
        playerVsAI.onClick.AddListener(delegate { PlayGame(2); });
        playerVsAI1.onClick.AddListener(delegate { PlayGame(2); });
        doubleAI.onClick.AddListener(delegate { PlayGame(3); });
    }
    public void PlayGame(int mode)
    {
        difficultyLevel = mode!= 2 ? 0 :difficultyLevel>=1?difficultyLevel:1;
        BoardManager.difficultyLevel = difficultyLevel;
        BoardManager.playMode = mode;
        Debug.Log(mode +": "+ difficultyLevel);
        SceneManager.LoadScene(1);

    }
    public void QuitGame()
    {
       Application.Quit();
    }

    public void selectedMode(int option)
    {

    }

    public void updateDifficulty()
    {
        difficultyLevel = (int)difficultySlider.value;
    }

    public void playButtonClick()
    {
        AudioSource source = transform.GetComponent<AudioSource>();
        source.clip = buttonClick;
        source.Play();
    }
}
