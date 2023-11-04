using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camLookAt;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    private void LateUpdate() {
        camLookAt.localEulerAngles = new Vector3(yAxis.Value, camLookAt.localEulerAngles.y, camLookAt.localEulerAngles.z); 
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}
