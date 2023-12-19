using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public int x_idx;
    public int y_idx;
    public int z_idx;
    public GameObject cube;
    GameObject cube_in;
    void Update()
    {
        // x,y,z must >=1 
        // Create(10,4,1);
        if(Input.GetKeyDown("k")){
            Create(x_idx,y_idx,z_idx);
            Debug.Log("keydown");
        }
    }

    // Update is called once per frame
    void Create(int x, int y, int z)
    {
        for(int i=1; i<=x; i++){
            for(int j=1; j<=y; j++){
                for(int k=1;k<=z; k++){
                    cube_in = Instantiate(cube, new Vector3(i, j-1, k), Quaternion.identity);
                    cube_in.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }
}
