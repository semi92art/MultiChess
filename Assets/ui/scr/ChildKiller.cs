﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildKiller : MonoBehaviour
{
    void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}
