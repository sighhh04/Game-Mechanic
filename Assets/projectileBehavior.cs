using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehavior : MonoBehaviour
{
    
    public const int kLifeTime = 5000; // Alife for this number of cycles

    private int mLifeCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        mLifeCount = kLifeTime;
        
    }

    // Update is called once per frame
    void Update()
    {
    
        mLifeCount--;
        if (mLifeCount <= 0)
        {
            Destroy(transform.gameObject);  // kills self
        }

    }
}
