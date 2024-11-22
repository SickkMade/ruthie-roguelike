using System;
using System.Collections;
using UnityEngine;

public class Library 
{
    //unused
    public static IEnumerator ReverseBoolAfterSeconds(Action<bool> setBoolean, bool value, float seconds){
        yield return new WaitForSeconds(seconds);
        setBoolean(!value);
    }
}
