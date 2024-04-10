using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    [SerializeField] InputField mapSize;
    [SerializeField] InputField mineCount;

    public void Reset()
    {
        Mine_Spawn.Instance.row_column = int.Parse(mapSize.text);
        Mine_Spawn.Instance.mine = int.Parse(mineCount.text);

        Mine_Spawn.Instance.InitMine();
    }
}
