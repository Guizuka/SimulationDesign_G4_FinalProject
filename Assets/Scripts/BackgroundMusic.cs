
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    Sounds[] musics;

    Sounds sounds = new Sounds();

    // Start is called before the first frame update
    void Start()
    {

        sounds.LoadSounds(musics);
        sounds.PlayRandomSound(musics);

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
