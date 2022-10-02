using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour
{

    private MazeCell currentCell;
    public GameObject item;

    private MazeDirection currentDirection;
    private MazeDirection tempDirection;

    private bool isMove;
    private bool isTurn;
    public bool isPick;
    private bool isThrow;
    private bool isContact; //아이템과의 접촉 여부

    private string direction;
    private string itemName;

    string fullpth = "Assets/Resources/test1";
    StreamWriter sw;

    Stack<GameObject> pickedlist;

    //public Camera directionalCamera;

    //private bool cameraOn =true;
    //private bool overheadCameraOn = false;

    public void Awake()
    {
        if (false == File.Exists(fullpth))
        {
            sw = new StreamWriter(fullpth + ".csv");
        }
    }
    public void Start()
    {
        pickedlist = new Stack<GameObject>();

        isMove = false;
        isTurn = false;
        isPick = false;
        isThrow= false;

        sw.WriteLine("Key" + "," + "Time" + "," + "Position" + "," + "direction" + "," + "item");
        StartCoroutine("CountTime",2);

    }

    IEnumerator CountTime(float delayTime)
    {
        Debug.Log("Time : " + Time.time);
        //s 신호 받음->움직임 부드럽게 
        currentDirection = tempDirection;

        if(isMove)
            Move(currentDirection);
        if(isTurn)
            Look(currentDirection);
        if (isPick)
            pickUp(item);
        if (isThrow)
            throwItem();

        isPick = false;
        isTurn = false;
        isMove = false;
        isThrow = false;

        if (currentCell.name == "Maze Cell 0 0")
        {
            StopCoroutine("CountTime");
            sw.Flush();
            sw.Close();
            GameManager.instance.isGameover = true;

        }

        if(!GameManager.instance.isGameover)
            sw.WriteLine("S" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName);

        yield return new WaitForSecondsRealtime(delayTime);
        StartCoroutine("CountTime",2);
    }

    IEnumerator Slowmove(Vector3 pos) 
    {
        while (transform.localPosition != pos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 0.1f);
            yield return null;
            if (Vector3.Distance(transform.localPosition, pos) < 0.5f)
            {
                transform.localPosition = pos;
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

    }

    private void Look(MazeDirection direction)
    {
        StartCoroutine(Slowturn(direction.ToRotation()));
        //transform.localRotation = direction.ToRotation();
        //GameManager.instance.directionalCamera.transform.localRotation = direction.ToRotation() * Quaternion.Euler(new Vector3(90, 0, 0));
        currentDirection = direction;
    }

    public void SetLocation(MazeCell cell)
    {
        currentCell = cell;
        StartCoroutine(Slowmove(cell.transform.localPosition));
        //transform.localPosition = cell.transform.localPosition;
    }

    private void Move(MazeDirection direction)
    {
        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            SetLocation(edge.otherCell);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameover)
        {
          

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log(currentCell.name);
                sw.WriteLine("4" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName);
                isMove = true;

                //Move(currentDirection);
                //if (currentCell.name == "Maze Cell 0 0")
                //{
                //    StopCoroutine("CountTime");
                //    sw.Flush();
                //    sw.Close();
                //    GameManager.instance.isGameover = true;
                    
                //}
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isTurn = true;
                tempDirection = currentDirection.GetNextCounterclockwise();
                //Look(currentDirection.GetNextCounterclockwise());
                sw.WriteLine("1" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                isTurn = true;
                tempDirection = currentDirection.GetNextClockwise();
                //Look(currentDirection.GetNextClockwise());
                sw.WriteLine("2" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName);
            }

            if (!isContact)
            {
                if (Input.GetKeyDown(KeyCode.Alpha3)) //버리기(toggle)
                {
                    if (pickedlist.Count != 0)
                    {
                        isThrow = true;
                        item = pickedlist.Peek().gameObject;
                        itemName = item.name;
                    }
                    sw.WriteLine("3" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName);
                }
            }
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
        isContact = false;
        GameManager.instance.score += 1;
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
        
        if (Input.GetKeyDown(KeyCode.Alpha3)) // 아이템 줍기
        {
            isPick = true;
            item = other.gameObject;
            itemName = other.name;
            sw.WriteLine("3" + "," + Time.time + "," + currentCell.name + "," + currentDirection + "," + itemName);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        isContact = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isContact = false;
    }

}

