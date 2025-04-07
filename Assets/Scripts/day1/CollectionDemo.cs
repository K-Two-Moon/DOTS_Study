using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
/// <summary>
///   NativeList作业
/// </summary>
public struct NativeListJob : IJob
{
    public NativeList<int> lstValue;
    public void Execute()
    {
        for (int i = 0; i < 10; i++)
        {
            lstValue.Add(i);
        }
    }
}

/// <summary>
/// NativeQueue作业
/// </summary>
public struct NativeQueueJob : IJob
{
    public NativeQueue<int> que;
    public void Execute()
    {
        for (int i = 0; i < 10; i++)
        {
            que.Enqueue(i);
        }
    }
}

/// <summary>
/// 学生数据
/// </summary>
public struct StudentData
{
    public int ID { get; set; }
    public FixedString128Bytes name;

    public StudentData(int iD, FixedString128Bytes name)
    {
        ID = iD;
        this.name = name;
    }
}

/// <summary>
/// NativeHashMap作业
/// </summary>
public struct NativeHashMapJob : IJob
{
    public NativeHashMap<int, StudentData> data;
    public void Execute()
    {
        StudentData sd1 = new StudentData(1001, "鹏乾");
        StudentData sd2 = new StudentData(1002, "范峰");
        data.Add(sd1.ID, sd1);
        data.Add(sd2.ID, sd2);
    }
}

public class CollectionDemo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TestNativeListJob();

        //TestNativeQueueJob();
        TestNativeHashMapJob();
    }

    private void TestNativeListJob()
    {
        NativeList<int> lstValue=new NativeList<int>(Allocator.TempJob);
        NativeListJob nativeListJob = new NativeListJob {  lstValue= lstValue};
        var handle = nativeListJob.Schedule();
        handle.Complete();
        for (int i = 0; i < lstValue.Length; i++)
        {
            Debug.Log($"navitlist {lstValue[i]}");
        }
        lstValue.Dispose();
    }

    private void TestNativeQueueJob()
    {
        NativeQueue<int> que = new NativeQueue<int>(Allocator.TempJob);
        NativeQueueJob nativeListJob = new NativeQueueJob { que = que };
        var handle = nativeListJob.Schedule();
        handle.Complete();
        int count = que.Count;
        for (int i = 0; i <count; i++)
        {
            Debug.Log($"NativeQueue {que.Dequeue()}");
        }
        que.Dispose();
    }

    private void TestNativeHashMapJob()
    {
        NativeHashMap<int,StudentData> kVPairs=new NativeHashMap<int, StudentData>(32,Allocator.TempJob);
         NativeHashMapJob  nhm = new NativeHashMapJob { data = kVPairs };
        var handle = nhm.Schedule();
        handle.Complete();
        foreach (var item in kVPairs)
        {
            Debug.Log($"{item.Key},{item.Value.name}");
        }
        kVPairs.Dispose();
    }
}
