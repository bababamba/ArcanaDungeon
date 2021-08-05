using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaDungeon.Object;

public class EnemyControler : MonoBehaviour
{
    
    public Enemy Ene;
    public bool isEnemyturn = false;

  

    // Update is called once per frame
    void Update()
    {
        if(Ene.isEnemyturn ==false)
            isEnemyturn = false;
    }
    public void turn()
    {
        Ene.isEnemyturn = true;
    }
}
