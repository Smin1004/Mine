using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance = null;
    public static ObjectPool Instance => _instance;

    [SerializeField] Transform parent;
    [SerializeField] GameObject Bullet;

    Queue<Box> unusedObjectQueue = new();
    Queue<Box> usedObjectQueue = new();

    private void Awake()
    {
        _instance = this;

        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for(int i = 0; i < initCount; i++)
        {
            unusedObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private Box CreateNewObject()
    {
        var newObj = Instantiate(Bullet).GetComponent<Box>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static void Reset(){
        foreach (var item in Instance.usedObjectQueue)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(Instance.transform);
            Instance.unusedObjectQueue.Enqueue(item);
        }
    }

    public static Box GetObject()
    {
        if(Instance.unusedObjectQueue.Count > 0)
        {
            var obj = Instance.unusedObjectQueue.Dequeue();
            obj.transform.SetParent(Instance.parent);
            obj.gameObject.SetActive(true);
            Instance.usedObjectQueue.Enqueue(obj);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(Instance.parent);
            Instance.usedObjectQueue.Enqueue(newObj);
            return newObj;
        }
    }

    public static void ReturnObject(Box obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.unusedObjectQueue.Enqueue(obj);
    }
}
