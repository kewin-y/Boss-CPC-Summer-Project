// From https://youtu.be/mntS45g8OK4

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    bool SaveData<T>(string relativePath, T data);

    T LoadData<T>(string relativePath);
}
