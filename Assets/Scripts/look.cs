using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class look : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform mRoot;
    public bool reverFace = false;


    void Start()
    {
        mRoot = transform;

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 v = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        //transform.LookAt(v);
        Vector3 targetPos = mRoot.position + Camera.main.transform.rotation * (reverFace?Vector3.back:Vector3.forward);
        Vector3 targetOrientation = Camera.main.transform.rotation * Vector3.up;
        mRoot.LookAt(targetPos, targetOrientation);
    }

}
