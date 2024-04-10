using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    SpriteRenderer sprite;
    [SerializeField] List<Sprite> num = new List<Sprite>();
    [SerializeField] Sprite box;
    [SerializeField] Sprite boom;
    [SerializeField] Sprite flag;

    public int myx;
    public int myy;
    public int mynum;
    public bool isMine;
    public bool isFlag;
    public bool isClick;

    public void Init(){
        sprite = GetComponent<SpriteRenderer>();

        sprite.sprite = box;
        isClick = false;
        isMine = false;
        isFlag = false;
    }

    public void Click()
    {
        if (isMine)
        {
            sprite.sprite = boom;
            return;
        }

        sprite.sprite = num[mynum];
        isClick = true;
    }

    public void Flag()
    {
        if (isClick) return;

        if (isFlag)
        {
            sprite.sprite = box;
            isFlag = false;
        }
        else
        {
            sprite.sprite = flag;
            isFlag = true;
        }
    }
}
