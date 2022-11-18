using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour
{

    public MazeCell currentCell;
    public GameObject item;

    private MazeDirection currentDirection;
    private MazeDirection tempDirection;

    public bool isMove;
    public bool isTurn;
    public bool isPick;
    private bool isThrow;
    private int isContact; //아이템과의 접촉 여부 portal1, portal2, banana, hamburger 1 2 3 4

    private string direction;
    private string itemName;

    
    
    

    public IEnumerator coroutine;

    Stack<GameObject> pickedlist;

    //public Camera directionalCamera;

    //private bool cameraOn =true;
    //private bool overheadCameraOn = false;

    public void Awake()
    {
        // fullpth= Application.dataPath+ "/Resources/test1.csv";

        //  if (false == File.Exists(fullpth))
        // {
        //     GameManager.instance.sw = new StreamWriter(fullpth);
        // }
        // else{
        //        GameManager.instance.sw= File.AppendText(fullpth);
        // }

        
    }
    public void Start()
    {
        pickedlist = new Stack<GameObject>();

        isContact=0;
        isMove = true;
        isTurn = true;
        isPick = true;
        isThrow= true;

        
        //StartCoroutine("CountTime",2);
        currentDirection=MazeDirection.South;
        Debug.Log(currentDirection);
        transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

    }

    // IEnumerator CountTime(float delayTime)
    // {
    //     Debug.Log("Time : " + Time.time);
    //     //s 신호 받음->움직임 부드럽게 
    //     currentDirection = tempDirection;

    //     if(isMove)
    //         Move(currentDirection);
    //     if(isTurn)
    //         Look(currentDirection);
    //     if (isPick)
    //         pickUp(item);
    //     if (isThrow)
    //         throwItem();

    //     isPick = false;
    //     isTurn = false;
    //     isMove = false;
    //     isThrow = false;

    //     if (currentCell.name == "Maze Cell 0 0")
    //     {
    //         StopCoroutine("CountTime");
    //         GameManager.instance.sw.Flush();
    //         GameManager.instance.sw.Close();
    //         GameManager.instance.isGameover = true;

    //     }

    //     if(!GameManager.instance.isGameover)
    //         GameManager.instance.sw.WriteLine("S" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName);

    //     yield return new WaitForSecondsRealtime(delayTime);
    //     StartCoroutine("CountTime",2);
    // }

    IEnumerator Slowmove(Vector3 pos) 
    {
        while (transform.localPosition != pos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 0.1f);
            yield return null;
            if (Vector3.Distance(transform.localPosition, pos) < 0.5f)
            {
                transform.localPosition = pos;
                isMove=true;
            }
            
        }

        
        
    }

    IEnumerator Slowturn(Quaternion vec)
    {
        while (transform.localRotation != vec)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, vec, 0.1f);
            GameManager.instance.directionalCamera.transform.localRotation = transform.localRotation * Quaternion.Euler(new Vector3(90, 0, 0));
            yield return null;
            if (Quaternion.Angle(transform.localRotation,vec) < 5f)
            {
                transform.localRotation = vec;
            }

        }

        isTurn=true;

    }

    private void Look(MazeDirection direction)
    {
        coroutine=Slowturn(direction.ToRotation());
        StartCoroutine(coroutine);
        //transform.localRotation = direction.ToRotation();
        //GameManager.instance.directionalCamera.transform.localRotation = direction.ToRotation() * Quaternion.Euler(new Vector3(90, 0, 0));
        currentDirection = direction;
    }

    public void SetLocation(MazeCell cell)
    {
        currentCell = cell;
        coroutine=Slowmove(cell.transform.position);
        StartCoroutine(coroutine);
        //transform.localPosition = cell.transform.localPosition;
    }

    private void Move(MazeDirection direction)
    {
        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            SetLocation(edge.otherCell);
        }
        else{
            isMove=true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameover)
        {
          

            if (Input.GetKeyDown(KeyCode.UpArrow)) //go forward 
            {
                if(isMove & isTurn){
                isMove=false;
                GameManager.instance.sw.WriteLine("4" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName+ "," + GameManager.instance.phase+ "," + GameManager.instance.step+"," + GameManager.instance.score);
                Move(currentDirection);

                }
                

                //Move(currentDirection);
                //if (currentCell.name == "Maze Cell 0 0")
                //{
                //    StopCoroutine("CountTime");
                //    GameManager.instance.sw.Flush();
                //    GameManager.instance.sw.Close();
                //    GameManager.instance.isGameover = true;
                    
                //}
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if(isTurn){
                isTurn=false;
                tempDirection = currentDirection.GetNextCounterclockwise();
                Debug.Log(tempDirection);
                Look(currentDirection.GetNextCounterclockwise());
                GameManager.instance.sw.WriteLine("1" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName+ "," + GameManager.instance.phase+ "," + GameManager.instance.step+"," + GameManager.instance.score);

                }
                
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if(isTurn){
                isTurn=false;
                tempDirection = currentDirection.GetNextClockwise();
                Debug.Log(tempDirection);
                Look(currentDirection.GetNextClockwise());
                GameManager.instance.sw.WriteLine("2" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName+ "," + GameManager.instance.phase+ "," + GameManager.instance.step+"," + GameManager.instance.score);

                }
                
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                if(isContact!=0)
                {
                    if(isContact==1)
                    {

                        if((GameManager.instance.step%2==1) ||  GameManager.instance.phase==3)
                        {
                            StopCoroutine(GameManager.instance.playerInstance.coroutine);
                            GameManager.instance.playerInstance.gameObject.SetActive(false);
                            GameManager.instance.playerInstance.transform.localPosition = GameManager.instance.mazeInstance2.GetCell(new IntVector2(0, 0)).transform.position;  
                            GameManager.instance.playerInstance.gameObject.SetActive(true);
                            GameManager.instance.playerInstance.currentCell=GameManager.instance.mazeInstance2.GetCell(new IntVector2(0, 0));
                            GameManager.instance.playerInstance.isMove=true;
                            StartCoroutine(coroutine);
                            isContact = 0;

                        }

                    }
                    else if(isContact==2)
                    {
                        if((GameManager.instance.step%2==0)|| GameManager.instance.phase==3)
                        {
                            StopCoroutine(GameManager.instance.playerInstance.coroutine);
                            GameManager.instance.playerInstance.gameObject.SetActive(false);
                            GameManager.instance.playerInstance.transform.localPosition = GameManager.instance.mazeInstance2.GetCell(new IntVector2(0, 4)).transform.position;  
                            GameManager.instance.playerInstance.gameObject.SetActive(true);
                            GameManager.instance.playerInstance.currentCell=GameManager.instance.mazeInstance2.GetCell(new IntVector2(0, 4));
                            GameManager.instance.playerInstance.isMove=true;
                            //Debug.Log(coroutine);
                            StartCoroutine(coroutine);
                            isContact = 0;

                        }

                    }else if(isContact==3)
                    {

                        if(GameManager.instance.itemcontrol>=2 || GameManager.instance.phase==3 )
                        {
                            Debug.Log("itemcontrol: "+GameManager.instance.itemcontrol);
                            GameManager.instance.itemcontrol+=1;

                            
                            pickUp(item);
                            if(GameManager.instance.itemcontrol ==4)
                            {
                                GameManager.instance.itemcontrol=0;
                            }
                            GameManager.instance.sw.WriteLine("3" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName+ "," + GameManager.instance.phase+ "," + GameManager.instance.step+"," + GameManager.instance.score);
                            GameManager.instance.isGameover=true;
                            

                        }

                    }else if(isContact==4)
                    {

                        if(GameManager.instance.itemcontrol<2 || GameManager.instance.phase==3 )
                        {
                            Debug.Log("itemcontrol: "+GameManager.instance.itemcontrol);
                            GameManager.instance.itemcontrol+=1;

                         
                            pickUp(item);
                            GameManager.instance.sw.WriteLine("3" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName+ "," + GameManager.instance.phase+ "," + GameManager.instance.step+"," + GameManager.instance.score);
                            GameManager.instance.isGameover=true;

                        }

                    }
                }
            }

            // if (!isContact)
            // {
            //     if (Input.GetKeyDown(KeyCode.Alpha3)) //버리기(toggle)
            //     {
            //         if (pickedlist.Count != 0)
            //         {
                        
            //             item = pickedlist.Peek().gameObject;
            //             itemName = item.name;
            //             throwItem();
            //         }
            //         GameManager.instance.sw.WriteLine("3" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName+ "," + GameManager.instance.phase+ "," + GameManager.instance.step+"," + GameManager.instance.score);
            //     }
            // }

            //  if (currentCell.name == "Maze Cell 4 4")
            // {
            //     GameManager.instance.sw.Flush();
            //     GameManager.instance.sw.Close();
            //     GameManager.instance.isGameover = true;

            // }
        }


        //else if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    if (overheadCameraOn == false)
        //    {
        //        if (cameraOn == true)
        //        {
        //            Camera.main.rect = new Rect(0f, 0f, 0f, 0f);
        //            cameraOn = false;
        //        }
        //        else
        //        {
        //            Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        //            cameraOn = true;
        //        }
        //    }


        //}
        //else if (Input.GetKeyDown(KeyCode.X))
        //{
        //    if (cameraOn == false)
        //    {
        //        if (overheadCameraOn == true)
        //        {
        //            directionalCamera.rect = new Rect(0f, 0f, 0f, 0f);
        //            overheadCameraOn = false;
        //        }
        //        else
        //        {
        //            directionalCamera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        //            overheadCameraOn = true;
        //        }
        //    }

        //}
    }

    public void pickUp(GameObject item)
    {
        pickedlist.Push(item);
        item.SetActive(false);
      
        if(item.name == " banana ")
        {

            GameManager.instance.score += 1;


        }else if(item.name == " burger ")
        {

            GameManager.instance.score += 4;


        }
        
    }

    public void throwItem()
    {
        GameObject tempitem = pickedlist.Pop();
        tempitem.SetActive(true);
        tempitem.transform.localPosition = transform.localPosition;
        GameManager.instance.score -= 1;
    }

    private void OnTriggerStay(Collider other)
    {
        
        // if (Input.GetKeyDown(KeyCode.Alpha3)) // 아이템 줍기
        // {
            
        //     item = other.gameObject;
        //     itemName = other.name;
        //     pickUp(item);
        //     GameManager.instance.sw.WriteLine("3" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName);
        //     GameManager.instance.isGameover=true;
        // }

        

        
            
            
            if(other.name=="portal1" )
            {

                isContact=1;
                

            }else if(other.name == "portal2")
            {
                isContact=2;
                

            }else if(other.name == " banana ")
            {
                
                isContact=3;
                item = other.gameObject;
                itemName = other.name;
               
            }
            else if(other.name == " burger ")
            {

                  isContact=4;
                  item = other.gameObject;
                    itemName = other.name;


            }


        
    }


  

    private void OnTriggerExit(Collider other)
    {
         if(other.name=="portal1" )
            {

                isContact = 0;
                Debug.Log(isContact);

            }else if(other.name == "portal2")
            {

                isContact = 0;
                Debug.Log(isContact);

        

            }else if(other.name == " banana ")
            {

                isContact = 0;
               
                
               
            }
            else if(other.name == " burger ")
            {

                isContact = 0;

               


            }


        
    }
    
    

}

