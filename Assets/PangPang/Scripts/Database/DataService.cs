using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // File
using SqlCipher4Unity3D;
using System;
using System.Linq;   // ToList() 를 사용하기 위함
using System.Reflection;    // MethodInfo

public class DataService
{

    private static DataService instance;
    public static DataService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataService();
            }
            return instance;
        }
    }

    SQLiteConnection connection;
    public string databaseName = "PangDatabase.db";

    Dictionary<Type, Dictionary<int, IKeyTableData>> tableDic;



    // 게임이 시작될 때 자동으로 한번 해당 함수를 호출해주는 기능 ( static 함수에만 붙일 수 있다.)
    // BeforeSceneLoad : 모든 스크립트들의 Awake 보다 먼저 호출됨
    // AfterSceneLoad : 모든 스크립틀의 Awake와 Start 사이에 호출됨
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
    static void Init()
    {
        Instance.InitDB();
        Instance.InitDataForPlay();
    }

    public void InitDataForPlay()
    {
        if (connection == null)
        {
            connection = new SQLiteConnection(databaseName, "1234");    // 로컬에서 디비에 접속
        }
        tableDic = new Dictionary<Type, Dictionary<int, IKeyTableData>>();

        for (int i = 0; i < Table.readOnlyTableArray.Length; i++)
        {
            tableDic.Add(Table.readOnlyTableArray[i], new Dictionary<int, IKeyTableData>());
        }
        for (int i = 0; i < Table.writableTableArray.Length; i++)
        {
            tableDic.Add(Table.writableTableArray[i], new Dictionary<int, IKeyTableData>());
        }

        MethodInfo loadDataMethod = this.GetType().GetMethod("LoadData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (Type type in tableDic.Keys)
        {
            MethodInfo loadDataGenericMethod = loadDataMethod.MakeGenericMethod(type);
            loadDataGenericMethod.Invoke(this, new object[] { });
        }
    }


    // where 호출 조건
    public void LoadData<T>() where T : IKeyTableData, new ()
    {
        var dataList =  connection.Table<T>().ToList(); // List<T>
        for (int i = 0; i < dataList.Count; i++)
        {
            tableDic[typeof(T)].Add(dataList[i].GetTableId(), dataList[i]);
        }

        T table = new T();
        IInsertableData insertableData = table as IInsertableData;  // Table.TestTable -> IInsertableData 로 변환 시도
        if (insertableData != null) // Table.TestTable 이 IInsertableData 인터페이스를 상속받고 있다면
        {
            if (tableDic[typeof(T)].Count > 0)
            {
                insertableData.Max = tableDic[typeof(T)].Keys.Max();
            }
            else
            {
                insertableData.Max = -1;
            }
        }
    }

    public T GetData<T>(int id) where T : IKeyTableData
    {
        var dataDic = tableDic[typeof(T)];
        if (dataDic.ContainsKey(id) )
        {
            return (T)dataDic[id];
        }
        Debug.LogError("GetDta Error : No Data " + typeof(T) + ", id :" + id);
        return default(T);
    }

    public List<T> GetDataList<T>() where T : IKeyTableData
    {
        if (tableDic.ContainsKey(typeof(T)) == true)
        {
            return tableDic[typeof(T)].Values.Cast<T>().ToList();
        }
        Debug.LogError("GetDataList Error : No Table" + typeof(T));
        return default(List<T>);
    }

    public void UpdateData<T> (T table) where T : IKeyTableData
    {
        tableDic[typeof(T)][table.GetTableId()] = table;
        connection.Update(table);
    }

    public int InsertData<T>(T table) where T : IKeyTableData, IInsertableData
    {
        table.Max++;
        int nextId = table.Max;
        table.SetTableId(nextId);
        tableDic[typeof(T)].Add(nextId, table);
        connection.Insert(table);

        return nextId;
    }

    public void DeleteData<T>(int id) where T : IKeyTableData
    {
        tableDic[typeof(T)].Remove(id);
        connection.Delete<T>(id);
    }

    public void InitDB()
    {
        string streamingAssetsPath = "";

 // 플랫폼을 구분할 때 ( 플랫폼별 경로가 다름)
#if UNITY_EDITOR
        streamingAssetsPath = "Assets/StreamingAssets/" + databaseName;
        databaseName = "Assets/" + databaseName;

#elif UNITY_ANDROID
        streamingAssetsPath = "jar:file://" + Application.dataPath + "!/assets/" + databaseName;
        databaseName = Application.persistentDataPath + "/" + databaseName;

#elif UNITY_IOS
        streamingAssetsPath = Application.dataPath + "/Raw/" + databaseName;
        databaseName = Application.persistentDataPath + "/" + databaseName;

#endif

        if (File.Exists(databaseName) == false) // 최초 실행을 의미
        {

// Asset 폴더로 db 파일 복사
#if UNITY_EDITOR || UNITY_IOS
            File.Copy(streamingAssetsPath, databaseName); 

#elif UINTY_ANDROID
            WWW loadDB = new WWW(streamingAssetsPath);
            while (loadDB.isDone == false) {}
            File.WriteAllBytes(databaseName, loadDB.bytes);
            loadDB.Dispose();
            loadDB = null;
#endif


        }

    }

}
