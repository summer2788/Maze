using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Maze : MonoBehaviour
{
    
    

    public MazePassage passagePrefab;
    public MazeWall wallPrefab;

    public IntVector2 size;

    public MazeCell cellPrefab;
    public MazeCell cellGoalPrefab;
    public MazeCell cellStartPrefab;

    public Item bananaPrefab;
    public Item cheesePrefab;
    public Item burgerPrefab;

    private MazeCell[,] cells;


    public float generationStepDelay;
    public List<Dictionary<string, object>> data_Dialog;

    void Awake()
    {
        data_Dialog = CSVReader.Read("Maze_1");

        //for (int i = 0; i < data_Dialog.Count; i++)
        //{
        //    print(data_Dialog[i]["  cell  "].ToString());
        //}

        //문자열 자르기
        string str1 = data_Dialog[data_Dialog.Count-1]["  cell  "].ToString(); //(20,20) 이 저장됨
     
        str1=str1.Replace(")", "");
        str1=str1.Replace("(", "");
        str1=str1.Replace(" ", "");

        char sp = ',';
        string[] spstring = str1.Split(sp);

        size.x = int.Parse(spstring[0]);
        size.z = int.Parse(spstring[1]);


    }

    public MazeCell GetCell(IntVector2 coordinates)
    {
    
        return cells[coordinates.x, coordinates.z];
    }

    public void Generate()
    {
        //WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        //List<MazeCell> activeCells = new List<MazeCell>();
        //DoFirstGenerationStep(activeCells);
        //while (activeCells.Count > 0)
        //{
        //    yield return delay;
        //    DoNextGenerationStep(activeCells);
        //}

        //여기서 부터 새로운 코드 시작한다.
        int count = 0; 

        for (int z = 0; z < size.z; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                //yield return delay;
                MazeCell currentCell= GetCell(new IntVector2(x, z));     //세로로 미로 생성됨 
                if (currentCell == null)
                {
                    currentCell=CreateCell(new IntVector2(x, z));
                    // if(x==0 && z==0){
                    //     currentCell.transform.GetChild(0).GetComponent<Renderer>().material.color=Color.red;
                        
                    // }

                   

                    
                }

                // if(x==4 && z==4){
                //         currentCell.transform.GetChild(0).GetComponent<Renderer>().material.color=Color.blue;
                //     }


                if (currentCell.IsFullyInitialized)  //벽 생성 다됐다면 넘어감
                {
                    continue;
                }


                List<int> numbers = new List<int>();
                //북동남서 순서로 집어 넣음
                numbers.Add((int)data_Dialog[count]["N"]);
                numbers.Add((int)data_Dialog[count]["E"]);
                numbers.Add((int)data_Dialog[count]["S"]);
                numbers.Add((int)data_Dialog[count]["W"]);

                //만약 바나나 값이 1이라면 바나나를 생성
                if ((int)data_Dialog[count]["item"] == 1)
                {
                    CreateBanana(new IntVector2(x, z));
                }
                else if ((int)data_Dialog[count]["item"] == 2)
                {
                    CreateCheese(new IntVector2(x, z));
                }
                else if ((int)data_Dialog[count]["item"] == 3)
                {
                    CreateBurger(new IntVector2(x, z));
                }

                count++;

                for (int i = 0; i < 4; i++)
                {
                    if (numbers[i] == 0) //장애물을 만든다
                    {
                        MazeDirection direction = (MazeDirection)i;
                        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();  //임의의 방향을 벡터로 변환 하여 현재셀 coordinate에 더함->이웃 셀의 좌표가 된다
                        if (currentCell.GetEdge(direction) == null) //edge 가 없는 경우에만 edge 생성
                        {
                            if (ContainsCoordinates(coordinates))   //방향 범주값 검사
                            {
                                MazeCell neighbor = GetCell(coordinates);
                                if (neighbor == null)
                                {
                                    neighbor = CreateCell(coordinates); //cell을 만들어줌
                                    CreateWall(currentCell, neighbor, direction);

                                }
                                else
                                {
                                    CreateWall(currentCell, neighbor, direction);
                                    // No longer remove the cell here.
                                }
                            }
                            else //범주값 벗어남 ->가장자리 벽으로 생성
                            {
                                CreateWall(currentCell, null, direction);
                                // No longer remove the cell here.
                            }
                        }
                        

                    }
                    else //1이면 통로를 만든다. 
                    {
                        MazeDirection direction = (MazeDirection)i;
                        IntVector2 coordinates = currentCell.coordinates+ direction.ToIntVector2();  //임의의 방향을 벡터로 변환 하여 현재셀 coordinate에 더함->이웃 셀의 좌표가 된다
                        if (currentCell.GetEdge(direction) == null) //edge 가 없는 경우에만 edge 생성
                        {
                            MazeCell neighbor = GetCell(coordinates);
                            if (neighbor == null)
                            {
                                neighbor = CreateCell(coordinates); //cell을 만들어줌
                                CreatePassage(currentCell, neighbor, direction);

                            }
                            else
                            {
                                CreatePassage(currentCell, neighbor, direction);
                            }

                        }
                    }
                }

            
            }
        }


    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)  //벽 생성 다됐다면 activecells 리스트에서 제거
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();  //임의의 방향을 벡터로 변환 하여 현재셀 coordinate에 더함
        if (ContainsCoordinates(coordinates))   //방향 범주값 검사
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates); //cell을 만들어줌
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);
                // No longer remove the cell here.
            }
        }
        else //범주값 벗어남 ->가장자리 벽으로 생성
        {
            CreateWall(currentCell, null, direction);
            // No longer remove the cell here.
        }
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        //한 셀에서 다른 셀 통로 양방향으로 2개 생성
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell;
        
         if(coordinates.x==0 && coordinates.z==0 ){
             newCell = Instantiate(cellStartPrefab) as MazeCell;
      
        }
        else if(coordinates.x==4 && coordinates.z==4 ){
             newCell = Instantiate(cellGoalPrefab) as MazeCell;
        }
        else{
              newCell = Instantiate(cellPrefab) as MazeCell;

        }
        // MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
      
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + " " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }


    private Item CreateBanana(IntVector2 coordinates)
    {
        Item banana= Instantiate(bananaPrefab) as Item;
        banana.coordinates= coordinates;
        banana.name= " banana ";
        banana.transform.parent = transform;
        banana.transform.localPosition =
            new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);

        return banana;

    }

    private Item CreateCheese(IntVector2 coordinates)
    {
        Item cheese = Instantiate(cheesePrefab) as Item;
        cheese.coordinates = coordinates;
        cheese.name = " cheese ";
        cheese.transform.parent = transform;
        cheese.transform.localPosition =
            new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);

        return cheese;

    }

    private Item CreateBurger(IntVector2 coordinates)
    {
        Item burger = Instantiate(burgerPrefab) as Item;
        burger.coordinates = coordinates;
        burger.name = " burger ";
        burger.transform.parent = transform;
        burger.transform.localPosition =
            new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);

        return burger;

    }

    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }


}
