using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance;
    public Transform m_canvas_mainMenu;
    public Transform m_canvas_instructions;

    //public GameObject sceneLoader;

    //private bool loadNextScene = false;

    [SerializeField]
    private int nextSceneNumber;

    // Start is called before the first frame update
    void Start()
    {
        ButtonSetup();
        m_canvas_mainMenu.gameObject.SetActive(true);
        m_canvas_instructions.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (loadNextScene)
        //{
        //    //SceneLoading();
        //}

        CheckQuit();
    }

    void ButtonSetup()
    {
        var playGame_button = m_canvas_mainMenu.transform.Find("PlayGameButton").GetComponent<Button>();
        playGame_button.onClick.AddListener(delegate ()
        {
            // load level 1 (the main menu is level 0)
            
            StartCoroutine(LoadNewScene(nextSceneNumber));
        });

        // instructions
        var instructions_button = m_canvas_mainMenu.transform.Find("InstructionButton").GetComponent<Button>();
        instructions_button.onClick.AddListener(delegate ()
        {
            m_canvas_mainMenu.gameObject.SetActive(false);
            m_canvas_instructions.gameObject.SetActive(true);
        });

        // return to the main menu
        var return_button = m_canvas_instructions.transform.Find("ReturnButton").GetComponent<Button>();
        return_button.onClick.AddListener(delegate ()
        {
            m_canvas_mainMenu.gameObject.SetActive(true);
            m_canvas_instructions.gameObject.SetActive(false);
        });

        // quit the game
        var quit_button = m_canvas_mainMenu.transform.Find("ExitButton").GetComponent<Button>();
        quit_button.onClick.AddListener(delegate ()
        {
            Application.Quit();
        });

    }

    IEnumerator LoadNewScene(int sceneNumber)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneNumber);

        while (!async.isDone)
            yield return null;
    }

    void CheckQuit()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

}
