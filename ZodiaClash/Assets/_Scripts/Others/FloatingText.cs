using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float destroyTime;

    private void Start()
    {
        destroyTime = 0.75f;

        Destroy(gameObject, destroyTime); //destroy after a certain time
    }
}
