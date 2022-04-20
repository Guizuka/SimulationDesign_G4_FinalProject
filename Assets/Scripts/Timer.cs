using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float timer = 0f;
    Text txtTimer;

    // Start is called before the first frame update
    void Start()
    {
       txtTimer = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //txtTimer.text = Mathf.Round(timer).ToString();
    }
}
