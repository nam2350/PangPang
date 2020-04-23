using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  // OrderBy

public class DatabaseTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 해당 테이블의 id로 데이터 받아오기
        //var testData = DataService.Instance.GetData<Table.TestTable>(2);
        //Debug.Log("name : " + testData.name + ", age : " + testData.age);

        // 해당 테이블의 모든 데이터 받아오기
        //var testDataList = DataService.Instance.GetDataList<Table.TestTable>();
        //for (int i = 0; i < testDataList.Count; i++)
        //{
        //    Debug.Log("name : " + testDataList[i].name + ", age : " + testDataList[i].age);
        //}

        // 이름이 "C" 인 데이터 받아오기
        //var testData = DataService.Instance.GetDataList<Table.TestTable>().Find(x => x.name == "c");
        //Debug.Log("name : " + testData.name + ", age : " + testData.age);

        // 나이가 30 이하인 모든 데이터 받아오기
        //var testDataList = DataService.Instance.GetDataList<Table.TestTable>().FindAll(x => x.age <= 30);
        //for (int i = 0; i < testDataList.Count; i++)
        //{
        //    Debug.Log("name : " + testDataList[i].name + ", age : " + testDataList[i].age);
        //}

        // 받아온 테이블 데이터 정렬 ( 오름차순 )
        //var testDataList = DataService.Instance.GetDataList<Table.TestTable>().OrderBy(x => x.age).ToList();
        //for (int i = 0; i < testDataList.Count; i++)
        //{
        //    Debug.Log("name : " + testDataList[i].name + ", age : " + testDataList[i].age);
        //}

        // 받아온 테이블 데이터 정렬 ( 내림차순 )
        //var testDataList = DataService.Instance.GetDataList<Table.TestTable>().OrderByDescending(x => x.age).ToList();
        //for (int i = 0; i < testDataList.Count; i++)
        //{
        //    Debug.Log("name : " + testDataList[i].name + ", age : " + testDataList[i].age);
        //}

        // 나이가 40인 사람의 나이를 30으로 수정
        //var testData = DataService.Instance.GetDataList<Table.TestTable>().Find(x => x.age == 30);
        //testData.age = 40;
        //DataService.Instance.UpdateData(testData);

        // Insert
        //var testData = new Table.TestTable();
        //testData.name = "E";
        //testData.age = 50;
        //DataService.Instance.InsertData(testData);

        // Delete 이름이 "D"인 사람의 데이터 삭제
        //var testData = DataService.Instance.GetDataList<Table.TestTable>().Find(x => x.name == "d");
        //DataService.Instance.DeleteData<Table.TestTable>(testData.id);

    }


}
