using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointVisualizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        //sphere.transform.localPosition = transform.localPosition;

        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(.3f, .3f, .3f);
            sphere.transform.position = child.transform.position;
            sphere.transform.SetParent(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
