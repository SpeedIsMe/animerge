using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class keeps track of the cells in a row
public class TileRow : MonoBehaviour
{
    public TileCell[] cells {get; private set;}

    public void Awake()
    {
        cells = GetComponentsInChildren<TileCell>();
    }
}
