// System
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.UI;

// Project
// Alias

public static class EnumExtensions
{
    public static T GetNextEnumType<T>(this T enumType)
    {
        Array enumArray = Enum.GetValues(typeof(T));
        for (int i = 0; i < enumArray.Length - 1;i++)
        {
            if (enumType.Equals(enumArray.GetValue(i)))
            {
                return (T)enumArray.GetValue(i + 1);
            }
        }

        return (T) enumArray.GetValue(enumArray.Length - 1);
    }
}