using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mine_Spawn : MonoBehaviour
{
    public static Mine_Spawn Instance { get; private set; }


    [SerializeField] Transform max_xy;
    [SerializeField] Transform temp;
    [SerializeField] Box box;

    private Vector3 now_transform;
    private float now_x = 0;
    private float now_y = 0;
    private int[,] map;
    Box[,] block;
    List<Box> mineList = new();

    public Vector3 mine_transform;
    public Vector3 spawn_transform;
    public int mine;
    public int row_column;
    public float divide_x = 0;
    public float divide_y = 0;

    public enum MineType
    {
        Normal,
        Mine = -1,
    }

    private void Awake()
    {
        Instance = this;
    }

    public void InitMine(){
        spawn_transform = Vector3.zero;
        map = new int[row_column, row_column];
        block = new Box[row_column, row_column];

        

        for (int mineCount = 0; mineCount < mine; ++mineCount)
        {
            int ran_x = Random.Range(0, row_column);
            int ran_y = Random.Range(0, row_column);


            if (map[ran_x, ran_y] != -1)
            {
                map[ran_x, ran_y] = (int)MineType.Mine;

                for (int i = ran_y - 1; i < ran_y + 2; i++)
                {
                    if (i < 0) i++;
                    if (row_column <= i) break;

                    for (int j = ran_x - 1; j < ran_x + 2; j++)
                    {
                        if (j < 0) j++;
                        if (row_column <= j) break;
                        if (map[j, i] != -1) map[j, i]++;
                    }
                }
            }
            else mineCount--;
        }

        Spawn();
    }

    void Spawn()
    {
        divide_x = max_xy.transform.lossyScale.x / row_column;
        divide_y = max_xy.transform.lossyScale.y / row_column;

        now_y -= divide_y / 2;
        now_transform = Vector3.zero;
        box.transform.localScale = new Vector3(divide_x, divide_y, 1);

        for (int i = 0; i < row_column; i++)
        {
            now_x = 0;
            divide_x = 0;
            now_y += divide_y;

            for (int j = 0; j < row_column; j++)
            {
                now_transform = new Vector3(now_x + divide_x, now_y);
                spawn_transform = transform.position + now_transform;
                divide_x += max_xy.transform.lossyScale.x / row_column;

                if (map[i, j] == -1)
                {
                    block[i, j] = Instantiate(box, spawn_transform, transform.rotation);
                    block[i, j].isMine = true;
                    block[i, j].mynum = map[i, j];
                    mineList.Add(block[i, j]);
                }
                else
                {
                    block[i, j] = Instantiate(box, spawn_transform, transform.rotation);

                    block[i, j].myx = i;
                    block[i, j].myy = j;
                    block[i, j].mynum = map[i, j];
                }
            }
        }
    }

    public void CheckZero(int x, int y)
    {
        for (int i = y - 1; i <= y + 1; i++)
        {
            if (i < 0) i++;
            if (row_column <= i) break;
            
            for (int j = x - 1; j <= x + 1; j++)
            {
                if (j < 0) j++;
                if (row_column <= j) break;
                if (map[j, i] == -1) continue;

                var b = block[j, i].GetComponent<Box>();

                if (b.isClick) continue;

                b.Click();
                if (b.mynum == 0) CheckZero(j, i);
            }
        }
    }

    public void CheckFlag(int x, int y)
    {
        int flagCount = 0;

        for (int i = y - 1; i <= y + 1; i++)
        {
            if (i < 0) i++;
            if (row_column <= i) break;
            
            for (int j = x - 1; j <= x + 1; j++)
            {
                if (j < 0) j++;
                if (row_column <= j) break;

                var b = block[j, i].GetComponent<Box>();

                if(b.isFlag) flagCount++;
            }
        }

        if(block[x, y].GetComponent<Box>().mynum == flagCount) CheckMine(x, y);
    }

    public void CheckMine(int x, int y)
    {
        for (int i = y - 1; i <= y + 1; i++)
        {
            if (i < 0) i++;
            if (row_column <= i) break;
            
            for (int j = x - 1; j <= x + 1; j++)
            {
                if (j < 0) j++;
                if (row_column <= j) break;
                var b = block[j, i].GetComponent<Box>();

                if (b.isClick || b.isFlag) continue;

                b.Click();
                if (b.mynum == 0) CheckZero(j, i);
                if(b.isMine) GameManager.Instance.End();
            }
        }
    }

    public void Boom(){
        foreach(var m in mineList){
            m.Click();
        }
    }
}
