using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using System.IO;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Maze mazePrefab;

    public Maze mazePrefab2;

    public Maze mazeInstance;

    public Maze mazeInstance2;

    public Player playerPrefab;

    public Player playerInstance;

    public Camera directionalCamera;

    public StreamReader SR;

    public StreamWriter sw;

    string fullpth = "Assets/Resources/test1";

    public GameObject gameoverText;
    public Text scoreText;
    public Image phase1;
    public Image phase2;
    public Image phase3;
    public Image done;

    public int score;
    public int phase;
    public int step;
    

    public string csv;
    

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

        if (false == File.Exists(fullpth + ".csv"))
        {
            sw = new StreamWriter(fullpth + ".csv");
        }
        else{
               sw= File.AppendText(fullpth + ".csv");
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        phase=1;
        step=1;
        sw.WriteLine("Key" + "," + "Time" + "," + "Position" + "," + "direction" + "," + "item"+ "," + "phase"+ "," + "step"+ "," + "score");
       
        isGameover = false;
        directionalCamera.rect = new Rect(0f, 0f, 0f, 0f);
        phase1.gameObject.SetActive(true);
        //StartCoroutine(BeginGame());
        GameManager.instance.BeginGame();

        //BeginGame();
    }

    public void BeginGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
        //Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        csv="Maze_1";
        mazeInstance = Instantiate(mazePrefab) as Maze;
        csv="Maze_2";
        mazeInstance2 = Instantiate(mazePrefab2) as Maze;
        mazeInstance.Generate();
        mazeInstance2.Generate();
        mazeInstance2.transform.position=new Vector3(6,0,0);
        //yield return StartCoroutine(mazeInstance.Generate());
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.localPosition = mazeInstance.GetCell(new IntVector2(0, 2)).transform.localPosition;  
        playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(0, 2)));
        //Camera.main.clearFlags = CameraClearFlags.Depth;
        //Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
    }

     public void RelearningReward()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
        //Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        csv="Maze_1";
        mazeInstance = Instantiate(mazePrefab) as Maze;
        csv="Maze_3";
        mazeInstance2 = Instantiate(mazePrefab2) as Maze;
        mazeInstance.Generate();
        mazeInstance2.Generate();
        mazeInstance2.transform.position=new Vector3(6,0,0);
        //yield return StartCoroutine(mazeInstance.Generate());
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.localPosition = mazeInstance2.GetCell(new IntVector2(0, 2)).transform.position; 
        playerInstance.currentCell= mazeInstance2.GetCell(new IntVector2(0, 2));;
        //Camera.main.clearFlags = CameraClearFlags.Depth;
        //Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
    }

    public void TestGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
        //Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        csv="Maze_1";
        mazeInstance = Instantiate(mazePrefab) as Maze;
        csv="Maze_4";
        mazeInstance2 = Instantiate(mazePrefab2) as Maze;
        mazeInstance.Generate();
        mazeInstance2.Generate();
        mazeInstance2.transform.position=new Vector3(6,0,0);
        //yield return StartCoroutine(mazeInstance.Generate());
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.localPosition = mazeInstance.GetCell(new IntVector2(0, 2)).transform.localPosition;  
        playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(0, 2)));
        //Camera.main.clearFlags = CameraClearFlags.Depth;
        //Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameover)
        {
            EndGame();

            if (Input.GetKeyDown(KeyCode.Space)) //normal mode 
            {
                RestartGame();
            }
        }

        //phase
        if (Input.GetKeyDown(KeyCode.Alpha1))

        {
            if(phase==1)
            {
                phase1.gameObject.SetActive(false);
                
                

            }
            else if(phase==2)
            {
                phase2.gameObject.SetActive(false);

            }
            else if(phase==3)
            {
                phase3.gameObject.SetActive(false);

            }



        }

        scoreText.text = "Score: " + score;

        
        
        // else if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     if (overheadCameraOn == false)
        //     {
        //         if (cameraOn == true)
        //         {
        //             Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
        //             cameraOn = false;
        //         }
        //         else
        //         {
        //             Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        //             cameraOn = true;
        //         }
        //     }


        // }
        // else if (Input.GetKeyDown(KeyCode.X))
        // {
        //     if (cameraOn == false)
        //     {
        //         if (overheadCameraOn == true)
        //         {
        //             directionalCamera.rect = new Rect(0f, 0f, 0f, 0f);
        //             overheadCameraOn = false;
        //         }
        //         else
        //         {
        //             directionalCamera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        //             overheadCameraOn = true;
        //         }
        //     }

        // }
    }


    public void EndGame()
    {
        isGameover = true;
        gameoverText.SetActive(true);
        

    }

    private void RestartGame()
    {
        isGameover = false;


        if(score>=12){
            score = 0;
            step=0;
            phase+=1;

        }
        
        gameoverText.SetActive(false);
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        Destroy(mazeInstance2.gameObject);
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }

        Debug.Log("phase"+phase+"\nstep: "+step);

        if(phase==1)
        {
            step+=1;
            BeginGame();

        }else if(phase ==2)
        {
            
            //Relearning
            step+=1;
            if(step==1)
            {
                phase2.gameObject.SetActive(true);

            }
            RelearningReward();


        }else if(phase ==3)
        {

            //test
            step+=1;
            if(step==1)
            {
                phase3.gameObject.SetActive(true);

            }
            TestGame();


        }else if(phase==4){

            done.gameObject.SetActive(true);

            //한 phase 가 끝남 test 저장 
            sw.Flush();
            sw.Close();
        }
         
    }

}
