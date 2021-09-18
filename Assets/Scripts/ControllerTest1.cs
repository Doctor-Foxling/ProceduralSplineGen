using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class ControllerTest1 : MonoBehaviour
{
    public GameObject plantPrefab;
    public float cameraMoveSpeed = 10;

    bool canCreate = true; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TraslateLeft()
    {
        Camera.main.transform.Translate(new Vector3(-cameraMoveSpeed * Time.deltaTime, 0, 0));
    }

    public void TraslateRight()
    {
        Camera.main.transform.Translate(new Vector3(cameraMoveSpeed * Time.deltaTime, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && canCreate)
        {
            var mp = Input.mousePosition;
            mp.z = 10;
            var pos = Camera.main.ScreenToWorldPoint(mp);
            pos.y = 0;
            var p = GameObject.Instantiate(plantPrefab, pos, Quaternion.identity);
            p.GetComponent<TestGrowth>().clickHeight = Camera.main.ScreenToWorldPoint(mp).y;
        }

        if (Input.mouseScrollDelta.y > 0.1)
            TraslateRight();
        if (Input.mouseScrollDelta.y < -0.1)
            TraslateLeft();
    }


    public void DenyCreation()
    {
        canCreate = false;
    }

    public void AllowCreation()
    {
        canCreate = true; 
    }
    
}
