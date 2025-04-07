using TMPro.EditorUtilities;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// 作业
/// </summary>
public struct MyJobOne : IJob
{
    public float a, b;
    public NativeArray<float> values;
    /// <summary>
    /// 执行
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
        //1. 准备数据
        NativeArray<float> values = new NativeArray<float>(1, Allocator.TempJob);
        //2. 实例化作业
        MyJobOne myJobOne = new MyJobOne {  a=4,b=4, values=values};
        //3. 调用作业
        var handle = myJobOne.Schedule();
        //4. 等待作业完成
        handle.Complete();
        //5. 使用
        Debug.Log($"与男生有关的数字和：{values[0]}");
        //6. 销毁数据
        values.Dispose();
    }
}
