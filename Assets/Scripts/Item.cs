using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public IntVector2 coordinates;

    private void OnTriggerEnter(Collider other)
    {  
        if((this.name=="portal1") || (this.name == "portal2"))
        {
           
            StopCoroutine(GameManager.instance.playerInstance.coroutine);
            GameManager.instance.playerInstance.gameObject.SetActive(false);
            GameManager.instance.playerInstance.transform.localPosition = GameManager.instance.mazeInstance2.GetCell(new IntVector2(0, 0)).transform.position;  
            GameManager.instance.playerInstance.gameObject.SetActive(true);
            GameManager.instance.playerInstance.currentCell=GameManager.instance.mazeInstance2.GetCell(new IntVector2(0, 0));
            GameManager.instance.playerInstance.isMove=true;

        }
    }

    //    print(this.name);
    //    Destroy(this.gameObject);
    //    GameManager.instance.score += 1;
    
}
