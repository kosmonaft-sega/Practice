using System;
using UnityEngine;

public class rotationScript : MonoBehaviour
{
    [SerializeField] private GameObject bladePrefab;
    [SerializeField,Range(1,50)] int cubes = 8;
    [SerializeField,Range(0f,10f)] float radius = 5f;
    [SerializeField,Range(0f,500f)] float speed = 100f;
    [SerializeField] bool direction = true;
    [SerializeField,Range(0f,360f)] float angle = 360f;
    int start_cubes;

    void Awake()
    {
        start_cubes = cubes;
        if(bladePrefab != null)
        {
            for(float i = 0; i<angle; i+=angle/(float)start_cubes)
            {
                GameObject newobject = Instantiate(bladePrefab, transform.position, Quaternion.identity, transform);
                newobject.transform.localEulerAngles = new Vector3(0f,i,0f);
                newobject.transform.GetChild(0).transform.localPosition = new Vector3(radius,0f,0f);
            }
        }
        else
        {
            Debug.Log("Ошибка пырефаб не найден!");
        }
    }
    void Update()
    {
        var a = direction ? 1f : -1f;
        transform.Rotate(new Vector3(0f,a*speed*Time.deltaTime,0f));
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
            child.localEulerAngles = new Vector3(0f,angle/(float)start_cubes*(float)i,0f);
            i++;
        }
    }
}