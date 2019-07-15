using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerInputHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("Player").SendMessage("OnCirclePressed");
    }
}
