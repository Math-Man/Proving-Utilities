using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCull : MonoBehaviour
{
    [SerializeField]private float cullTimer = 5.0f;
    void Start()
    {
        Destroy(gameObject, cullTimer);
    }

}
