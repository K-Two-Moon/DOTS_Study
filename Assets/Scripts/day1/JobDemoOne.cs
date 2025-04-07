using TMPro.EditorUtilities;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// ��ҵ
/// </summary>
public struct MyJobOne : IJob
{
    public float a, b;
    public NativeArray<float> values;
    /// <summary>
    /// ִ��
    /// </summary>
    public void Execute()
    {
        values[0] = a + b;
    }
}

public class JobDemoOne : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TestMyJobOne();
    }

    private void TestMyJobOne()
    {
        //1. ׼������
        NativeArray<float> values = new NativeArray<float>(1, Allocator.TempJob);
        //2. ʵ������ҵ
        MyJobOne myJobOne = new MyJobOne {  a=4,b=4, values=values};
        //3. ������ҵ
        var handle = myJobOne.Schedule();
        //4. �ȴ���ҵ���
        handle.Complete();
        //5. ʹ��
        Debug.Log($"�������йص����ֺͣ�{values[0]}");
        //6. ��������
        values.Dispose();
    }
}
