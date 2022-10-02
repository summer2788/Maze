using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Maze mazePrefab;

    private Maze mazeInstance;

    public Player playerPrefab;

    private Player playerInstance;

    public Camera directionalCamera;

    public GameObject gameoverText;
    public Text scoreText;

    public int score;

    private bool cameraOn = true;
    private bool overheadCameraOn = false;
    public bool isGameover;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        isGameover = false;
        directionalCamera.rect = new Rect(0f, 0f, 0f, 0f);
        StartCoroutine(BeginGame());

        //BeginGame();
    }

    private IEnumerator BeginGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.localPosition = mazeInstance.GetCell(new IntVector2(4, 4)).transform.localPosition;  
        playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(4, 4)));
        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameover)
        {
            EndGame();
        }

        scoreText.text = "Score: " + score;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (overheadCameraOn == false)
            {
                if (cameraOn == true)
                {
                    Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
                    cameraOn = false;
                }
                else
                {
                    Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                    cameraOn = true;
                }
            }


        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (cameraOn == false)
            {
                if (overheadCameraOn == true)
                {
                    directionalCamera.rect = new Rect(0f, 0f, 0f, 0f);
                    overheadCameraOn = false;
                }
                else
                {
                    directionalCamera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                    overheadCameraOn = true;
                }
            }

        }
    }


    public void EndGame()
    {
        isGameover = true;
        gameoverText.SetActive(true);

    }

    private void RestartGame()
    {
        isGameover = false;
        score = 0;
        gameoverText.SetActive(false);
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }
        StartCoroutine(BeginGame());
    }

}
