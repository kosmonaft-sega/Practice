using System;
using UnityEngine;

public class rotationScript : MonoBehaviour
{
    [SerializeField] private GameObject bladePrefab;
    [SerializeField,Range(1,25)] int cubes = 8;
    [SerializeField,Range(0f,10f)] float radius = 5f;
    [SerializeField,Range(0f,500f)] float speed = 100f;
    [SerializeField] bool direction = true;
    [SerializeField,Range(0f,360f)] float angle = 360f;
    int prev_cubes;

    void Awake()
    {
        prev_cubes = cubes;
        if(bladePrefab != null)
        {
            summon_cube();
        }
        else
        {
            Debug.Log("Ошибка префаб не найден!");
        }
    }
    void Update()
    {
        var a = direction ? 1f : -1f;
        transform.Rotate(new Vector3(0f,a*speed*Time.deltaTime,0f));
        if (prev_cubes != cubes)
        {
            summon_cube();
        }
    }

    void OnValidate()
    {
        foreach(Transform child in transform)
        {
            child.GetChild(0).localPosition = new Vector3(radius,0f,0f);
        }
        int i = 0;
        foreach(Transform child in transform)
        {
            child.localEulerAngles = new Vector3(0f,angle/(float)cubes*(float)i,0f);
            i++;
        }
    }

    void summon_cube()
    {
        int children = transform.childCount;
        GameObject newobject;
        if(children<cubes)
        {
            for(int i=0; i<cubes; i++)
            {
                if(i<children)
                {
                    newobject = transform.GetChild(i).gameObject;
                }
                else
                {
                    newobject = Instantiate(bladePrefab, transform.position, Quaternion.identity, transform);
                }
                newobject.transform.localEulerAngles = new Vector3(0f,angle/(float)cubes*(float)i,0f);
                newobject.transform.GetChild(0).transform.localPosition = new Vector3(radius,0f,0f);
            }
        }
        else if(children>cubes)
        {
            int i = 0;
            foreach(Transform child in transform)
            {
                child.transform.localEulerAngles = new Vector3(0f,angle/(float)cubes*(float)i,0f);
                child.transform.GetChild(0).transform.localPosition = new Vector3(radius,0f,0f);
                if(i>=cubes)
                {
                    Destroy(child.gameObject);
                }
                i++;
            }
        }
    }
}
