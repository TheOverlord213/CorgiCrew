﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    List<GameObject> objectData = new  List<GameObject>();

    private bool objectWaitBool = false;
    public bool objectPickedUp = false;

    private float tempWaitChecker = 0.0f;

    private float gameOverTimerChecker = 0.0f;
    private readonly int maxGameTime = 120;
    public TextMeshProUGUI timerText;

    public GameObject corgiWins;
    public GameObject ownerWins;
    public GameObject draw;
    public GameObject waitingText;

    public Material transMat;

    private GameObject player;
    public GameObject pickedUpObject;
    public GameObject closeObj;

    public Slider progressSlider;
    public GameObject countdownDisplay;
    public bool startGame = false;
    private bool startedCountdown = false;
    private Vector3 playerItemAnchorPoint = new Vector3 (0,0,0);
    public int countownTime;
    

    private void Awake()
    {
        // sets up the objects data
        foreach (GameObject x in GameObject.FindGameObjectsWithTag("MovableObjects"))
        {
            objectData.Add(x);
            x.AddComponent<MovableObjectData>();
            x.AddComponent<Rigidbody>();
            x.AddComponent<SphereCollider>();

            x.GetComponent<SphereCollider>().isTrigger = true;
            float tempRadius = x.GetComponent<SphereCollider>().radius;
            x.GetComponent<SphereCollider>().radius = tempRadius * 1.5f;
        }

        player = GameObject.FindGameObjectWithTag("Player2");
    }

    // Start is called before the first frame update
    void Start()
    {
        progressSlider.maxValue = objectData.Count;
        progressSlider.value = 0;

        gameOverTimerChecker = maxGameTime;

        corgiWins.SetActive(false);
        ownerWins.SetActive(false);
        draw.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player2");
        }

        if (!objectWaitBool)
        {
            ObjectWait();
        }

        if (objectPickedUp)
        {
            SetObject();
        }

        if (GameObject.FindGameObjectWithTag("Player1") != null && GameObject.FindGameObjectWithTag("Player2") != null && !startedCountdown)
        {
            Debug.Log("both players exist");
            startedCountdown = true;
            waitingText.SetActive(false);
            countdownDisplay.SetActive(true);
            countdownDisplay.SetActive(true);
            StartCoroutine(CountDownToStart());
        }


        GameOverTimerCount();
        CheckCloseObj();
        QuitGame();
    }

    void CreateAnchorPoint()
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = playerPos.y + 2.0f;
        Vector3 playerDir = player.transform.forward;
        Quaternion playerRot = player.transform.rotation;
        float spawnDistance = 1.0f;

        playerItemAnchorPoint = playerPos + playerDir * spawnDistance - new Vector3(0, 1, 0);

    }

    void ObjectWait()
    {
        tempWaitChecker += Time.deltaTime;
        int seconds = Mathf.RoundToInt(tempWaitChecker % 60.0f);
        if (seconds == 2)
        {
            foreach (GameObject x in objectData)
            {       
                x.GetComponent<MovableObjectData>().objectInitialized = true;
                x.GetComponent<MovableObjectData>().transparentMat = transMat;
                x.GetComponent<MovableObjectData>().SetOrigin();
            }
            objectWaitBool = true;
        }
    }

    public void PlayerPickUpDetected()
    {
        if (closeObj != null)
        {
            objectPickedUp = true;
            pickedUpObject = closeObj;
            foreach (Collider c in closeObj.GetComponents<Collider>())
            {
                c.enabled = false;
            }
        }
    }

    public void DropObject()
    {
        objectPickedUp = false;
        foreach (Collider c in closeObj.GetComponents<Collider>())
        {
            c.enabled = true;
        }
        pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void SetObject()
    {
        CreateAnchorPoint();
        pickedUpObject.transform.position = playerItemAnchorPoint;
        pickedUpObject.transform.rotation = Quaternion.LookRotation(pickedUpObject.transform.position - player.transform.position);
        pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void TestDistance(GameObject newobj)
    {
        if (closeObj == null)
        {
            closeObj = newobj;
        }
        else
        {
            if (Vector3.Distance(newobj.transform.position,player.transform.position)
                < Vector3.Distance(closeObj.transform.position,player.transform.position))
            {
                closeObj = newobj;
            }
        }

    }

    void CheckCloseObj()
    {
        if (closeObj != null)
        {
            if (Vector3.Distance(player.transform.position,closeObj.transform.position) > 2f)
            {
                closeObj = null;
            }
        }
      
    }

    void QuitGame()
    {     
        if (Input.GetKey("escape"))
        {
            Debug.Log("Escaped");
            Application.Quit();
        }
    }

    public void SliderValueChange(bool increased)
    {
        if (increased)
            progressSlider.value = progressSlider.value + 1.5f;
        else if (!increased)
            progressSlider.value = progressSlider.value - 1f;
    }

    void GameOverTimerCount()
    {
        if(startGame)
        {
            //gameOverTimerChecker += Time.deltaTime;
            //int seconds = Mathf.RoundToInt(gameOverTimerChecker % 60.0f);
            //int tempNum = maxGameTime - seconds;
            //Debug.Log(maxGameTime);
            //timerText.text = tempNum.ToString();
            
            gameOverTimerChecker -= Time.deltaTime;
            //int seconds = Mathf.RoundToInt(gameOverTimerChecker % 60.0f);
            int seconds = Mathf.RoundToInt(gameOverTimerChecker);
            timerText.text = seconds.ToString();
            //if (seconds <= 0)
            if (seconds <= 0)
            {
                Debug.Log("Timer elappsed");
                float tempPercentage = progressSlider.maxValue / 2.0f;
                if (progressSlider.value < tempPercentage)
                {
                    ownerWins.SetActive(true);

                }
                else if (progressSlider.value > tempPercentage)
                {
                    corgiWins.SetActive(true);
                }
                else if (progressSlider.value == tempPercentage)
                {
                    draw.SetActive(true);
                }

                startGame = false;
                LoadMainMenu();
                gameOverTimerChecker = maxGameTime;
                PauseTime(true);

            }
        }
       
    }

    void PauseTime(bool timeIsFrozen)
    {
        if (timeIsFrozen)
            Time.timeScale = 0;
        else if (!timeIsFrozen)
            Time.timeScale = 1;
    }


    IEnumerator CountDownToStart()
    {
        while(countownTime>0)
        {
            countdownDisplay.GetComponent<TextMeshProUGUI>().text = countownTime.ToString();

            yield return new WaitForSecondsRealtime(1f);

            countownTime--;
        }

        countdownDisplay.GetComponent<TextMeshProUGUI>().text = "Go";
        yield return new WaitForSecondsRealtime(1f);
        startGame = true;
        countdownDisplay.SetActive(false);


    }


    public void LoadMainMenu()
    {
        StartCoroutine(LoadNewScene(0));
    }

    IEnumerator LoadNewScene(int sceneNumber)
    {
        yield return new WaitForSecondsRealtime(10f);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneNumber);

        while (!async.isDone)
            yield return null;
    }
}
