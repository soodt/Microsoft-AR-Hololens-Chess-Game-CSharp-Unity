using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnchor : MonoBehaviour
{
    public static BoardAnchor instance;

    // Start is called before the first frame update
    // Ensures this is the only instance of this is present.
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }
}
