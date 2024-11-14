using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using System.IO;
using System.Text;
using static UnityEditor.PlayerSettings.Switch;
using UnityEngine.Networking;

[System.Serializable]
public class Languages
{
    public string language, languageLocalize;
    public List<string> value = new List<string>();
}
[System.Serializable]
public class SaveSingletoneData_language
{
    public List<Languages> languages = new List<Languages>();
}

public class Singletone : Singletone_base
{
    public static Singletone singletone;
     protected override void Awake() {
        base.Awake();
        if(singletone == null) {
            singletone = this;
            DontDestroyOnLoad(singletone);
        } else {
            Destroy(this);
        }
    }
    ////////////////////////////////////////////////////
   
    public Camera cam;
    public gameManager gm;

}
