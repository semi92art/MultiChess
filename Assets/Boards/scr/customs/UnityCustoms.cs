﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ASmirnov
{
    static class UnityCustoms
    {
        /// <summary>
        /// Get size of game window whatever debug or any release platform
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetGameWindowSize()
        {
#if UNITY_EDITOR
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
#elif UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
        return new Vector2(Screen.width, Screen.height);
#endif
        }

        public static void DestroyAllChilds(GameObject obj)
        {
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(obj.transform.GetChild(i).gameObject);
#else
                GameObject.Destroy(obj.transform.GetChild(i).gameObject);
#endif
            }
        }

        public static void DestroyGameObjects(string name)
        {
            while (GameObject.Find(name) != null)
                GameObject.DestroyImmediate(GameObject.Find(name));
        }

        public static void DestroyAllChilds(Component comp)
        {
            DestroyAllChilds(comp.gameObject);
        }

        public static void SetSpriteRendererObjectSize(SpriteRenderer rend, Vector2 size)
        {
            rend.transform.localScale = Vector3.one;
            rend.transform.localScale = new Vector3(size.x / rend.bounds.size.x, size.y / rend.bounds.size.y, 1);
        }

        public static void SetSpriteRendererObjectSize(GameObject obj, Vector2 size)
        {
            if (obj.GetComponent<SpriteRenderer>() == null)
                return;
            SetSpriteRendererObjectSize(obj.GetComponent<SpriteRenderer>(), size);
        }
    }

}


