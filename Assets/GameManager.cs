using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    Vector3 MouseDownPos;
    public bool isEnd = false;

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (isEnd) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = MousePosCheck();
            if (hit.collider == null) return;

            if (hit.collider.TryGetComponent<Box>(out Box box))
            {
                if (box.isFlag) return;

                if (box.isMine)
                {
                    End();
                    return;
                }

                box.Click();
                if (box.mynum == 0) Mine_Spawn.Instance.CheckZero(box.myx, box.myy);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = MousePosCheck();
            if (hit.collider == null) return;

            if (hit.collider.TryGetComponent<Box>(out Box box)) box.Flag();
        }
        if (Input.GetMouseButtonDown(2))
        {
            RaycastHit2D hit = MousePosCheck();
            if (hit.collider == null) return;

            if (hit.collider.TryGetComponent<Box>(out Box box)) Mine_Spawn.Instance.CheckFlag(box.myx, box.myy);
        }
    }

    public void End()
    {
        if(isEnd) return;

        Mine_Spawn.Instance.Boom();
        isEnd = true;
    }

    RaycastHit2D MousePosCheck()
    {
        MouseDownPos = Input.mousePosition;
        Vector2 pos = Camera.main.ScreenToWorldPoint(MouseDownPos);
        return Physics2D.Raycast(pos, Vector2.zero);
    }
}
