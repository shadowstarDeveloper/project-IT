using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Singletone_base : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("== Import Datas ==")]
    [Header("Language data")]
    [HideInInspector] public int languageIndex = 1;
    public List<Languages> languages;
    const string langURL = "https://docs.google.com/spreadsheets/d/1_Kyp_wAzFgtP1CJtvLllGZQR8Kn1eOdAL31XhLcRNkc/";
    public event System.Action LanguageLocalized = () => { };

    protected virtual void Awake() {
        InitLang();
    }
    void InitLang() {
        int langIndex = PlayerPrefs.GetInt("languageIndex", -1);
        if (langIndex <= -1) {
            langIndex = 1;
        }
        int systemIndex = languages.FindIndex(x => x.language.ToLower() == Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) {
            systemIndex = 0;
        }
        int index = langIndex == -1 ? systemIndex : langIndex;
        SetLanguageIndex(index);
    }

    public void SetLanguageIndex(int index) {
        languageIndex = index;

        LanguageLocalized();
        //save();
    }

    [ContextMenu("Save Json file")]
    public void SaveSingletoneData() {

        SaveSingletoneData_language save = new SaveSingletoneData_language();
        for (int i = 0; i < languages.Count; i++) {
            save.languages.Add(languages[i]);
        }
        CreateJsonToFile(Application.dataPath + "", "Language", ObjToJson(save));

        //SetLanguageIndex(languageIndex);
    }
    [ContextMenu("Load Json file")]
    public void LoadSingletoneData() {

        string _path = Application.streamingAssetsPath;

        if (Application.platform == RuntimePlatform.WindowsEditor) {

        }
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            _path = Application.dataPath + "/Raw";
        }
        //런타임, ios 
        SaveSingletoneData_language lang = JsonUtility.FromJson<SaveSingletoneData_language>(Resources.Load<TextAsset>("Json/Language").text);
        languages = lang.languages;
    }
    public T LoadFileToJson<T>(string path, string fileName) {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
    public string ObjToJson(object obj) {
        return JsonUtility.ToJson(obj);
    }
    public void CreateJsonToFile(string path, string FileName, string Jsondata) {
        FileStream filestream = new FileStream(string.Format("{0}/{1}.json", path, FileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(Jsondata);
        filestream.Write(data, 0, data.Length);
        filestream.Close();
        Debug.Log("Save complete : " + FileName);
    }
    /////////////////////////////////////////////////////////////////
    IEnumerator getGoogledataCoroutine(string url, string type) {
        UnityWebRequest www = UnityWebRequest.Get(url + "export?format=tsv");
        yield return www.SendWebRequest();
        //Debug.Log(www.downloadHandler.text);
        switch (type) {
            case "language":
            setLanguageList(www.downloadHandler.text);
            break;

        }
    }
    protected void setLanguageList(string tsv) {
        // Debug.Log(tsv);
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        Debug.Log("language pack loaded. total : " + rowSize);

        int colummSize = row[0].Split('\t').Length;
        string[,] Sentence = new string[rowSize, colummSize];
        for (int i = 0; i < rowSize; i++) {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < colummSize; j++) {
                Sentence[i, j] = column[j];
            }
        }
        //클래스 리스트
        languages = new List<Languages>();
        for (int i = 0; i < colummSize; i++) {
            Languages data = new Languages();
            data.language = Sentence[0, i];
            data.languageLocalize = Sentence[1, i];

            for (var j = 1; j < rowSize; j++) {
                data.value.Add(Sentence[j, i]);
            }
            languages.Add(data);
        }
    }
    public string languageTxt(string key) {
        int keyIndex;
        try {
            keyIndex = languages[0].value.FindIndex(x => x.ToLower() == key.ToLower());
            string txt = languages[languageIndex + 1].value[keyIndex];
            //Debug.Log(front_txt + txt + back_txt);
            txt = txt.ToString().Replace("//", "\n");
            return txt.ToString().Replace("\r", "");
        }
        catch (ArgumentOutOfRangeException) {
            return $"error_key : {key}";
        }


    }

    [ContextMenu("Import language data from google sheets")]
    void getLanguage() {
        StartCoroutine(getGoogledataCoroutine(langURL, "language"));
    }
}
