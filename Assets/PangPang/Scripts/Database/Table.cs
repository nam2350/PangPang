using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;   // Serializable 을 사용하기 위함 , Type 
using UnityEngine.Scripting;    // Preserve를 사용하기 위함
using SQLite.Attribute;

public class Table 
{
    // 동기화 방법이 다르기 때문에 나눠짐
    public static Type[] readOnlyTableArray = new Type[]
    {

    };

    public static Type[] writableTableArray = new Type[]
    {
        typeof(TestTable)
    };
    

    // Serializable : 해당 어트리뷰트가 붙어있어야만 Sqlcipher툴에서 해당 클래스에 접근이 가능하다.
    // Preserve : 해당 클래스가 사용되지 않더라도 빌드시 제외하는 걸 막아줌
    [Serializable, Preserve]
    public class TestTable : IKeyTableData, IInsertableData
    {
        public int GetTableId() { return id; }
        public void SetTableId(int id) { this.id = id; }

        public static int max;
        [Ignore]    // 해당 컬럼은 무시
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        [NotNull, PrimaryKey, Unique]
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
    }
}
