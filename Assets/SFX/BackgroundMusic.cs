using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioClip[] Songs;
    // Start is called before the first frame update
    void Start()
    {
        SFXManager.instance.playRandSFX(Songs, transform, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
