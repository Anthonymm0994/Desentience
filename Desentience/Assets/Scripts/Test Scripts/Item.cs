﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public Sprite icon = null;
    public GameObject itemPrefab;
    public bool isStackable() {
        return false;
    }
}
