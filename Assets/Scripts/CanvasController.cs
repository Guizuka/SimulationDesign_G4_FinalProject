using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{

    private void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;

        transform.LookAt(
            new Vector3(camPos.x, transform.position.y, -camPos.z)
            );
    }
}
