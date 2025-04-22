/*********************************************************					
* DevUtils.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_Occulus;
using Dev_System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static DevDefine;

public static class DevUtils					
{

    /// <summary>
    /// 매개변수중 Max값 얻기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static T FindMax<T>(params T[] values) where T : IComparable<T>
    {
        if (values.Length == 0)
        {
            throw new ArgumentException("At least one value must be provided.", nameof(values));
        }

        T max = values[0];
        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].CompareTo(max) > 0)
            {
                max = values[i];
            }
        }
        return max;
    }



    /// <summary>
    /// String문자열을 Enum으로 캐스팅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T StringToEnum<T>(string value)
    {
        T result = (T)Enum.Parse(typeof(T), value);
        if(System.Enum.IsDefined(typeof(T), result)== false)
        {
            UnityEngine.Debug.LogError("The string does not exist in the enum.");
            return default;
        }
        else
        {
            return result;
        }
    }



    /// <summary>
    /// TMP용 텍스트의 Color변경
    /// </summary>
    /// <param name="hex"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex, float alpha = 255f)
    {
        // '#' 기호 제거
        hex = hex.Replace("#", "");

        // 16진수 문자열을 Color로 변환
        Color color = new Color();
        if (UnityEngine.ColorUtility.TryParseHtmlString("#" + hex, out color))
        {
            color.a = alpha; // alpha 값을 매개변수로 받은 값으로 설정
            return color;
        }
        else
        {
            // 유효한 16진수 색상 코드가 아닌 경우, 기본값으로 흰색 반환
            return new Color(1f, 1f, 1f, alpha);
        }

        //if (color.Contains("#") == false)
        //    color = $"#{color}";

        //ColorUtility.TryParseHtmlString(color, out Color parsedColor);

        //return parsedColor;
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static void MakeMask(this ref LayerMask mask, List<DevDefine.ELayer> list)
    {
        foreach (DevDefine.ELayer layer in list)
            mask |= (1 << (int)layer);
    }

    public static void AddLayer(this ref LayerMask mask, DevDefine.ELayer layer)
    {
        mask |= (1 << (int)layer);
    }

    public static void RemoveLayer(this ref LayerMask mask, DevDefine.ELayer layer)
    {
        mask &= ~(1 << (int)layer);
    }

    public static string GetTag(ETag tag)
    {
        return tag.ToString();
    }

    public static void CameraFarDistance(float  dist)
    {
        if(CameraRigMaster.Instance != null)
        {
            CameraRigMaster.Instance.pHeadCamera.farClipPlane = dist;
        }
    }

    public static void OculusPosLock(bool lockPos)
    {
        OVRManager.instance.usePositionTracking = lockPos;
    }

    public static Sprite Texture2DToSprite(Texture2D texture)
    {
        Sprite changeSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return changeSprite != null ? changeSprite : null;
    }

}//end of class					
					
					
			