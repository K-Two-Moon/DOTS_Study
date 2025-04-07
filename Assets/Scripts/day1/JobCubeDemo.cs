using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
[BurstCompile]
/// <summary>
/// 速度作业
/// </summary>
public struct VelocityJob : IJob
{
    /// <summary>
    /// 定义速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 移动时间
    /// </summary>
    public float time;
    /// <summary>
    /// 速度
    /// </summary>
    public NativeArray<Vector3> velocity;
    /// <summary>
    /// 执行
    /// </summary>
    public void Execute()
    {
        for (int i = 0; i < velocity.Length; i++)
        {
            velocity[i] += Vector3.up * speed*time;
        }
    }
}

/// <summary>
/// 测试作业
/// </summary>
public class JobCubeDemo : MonoBehaviour
{
    private int cubeCount = 100;
    public GameObject insCube;
    private List<Transform> cubeLst;
    void Start()
    {
        cubeLst=new List<Transform>();
        for (int i = 0; i < cubeCount; i++)
        {
            Transform cube=Instantiate(insCube).transform;
            cube.position = Random.insideUnitSphere * 10;
            cubeLst.Add(cube);
        }
    }

    void Update()
    {
        TestVelocityJob();
    }

    private void TestVelocityJob()
    {
        //1. 准备数据
        NativeArray<Vector3> velocity = new NativeArray<Vector3>( cubeCount, Allocator.TempJob);
        //2. 实例化作业
        VelocityJob velocityJob = new VelocityJob { speed=3, time=Time.deltaTime, velocity=velocity };
        //3. 调用作业
        var handle = velocityJob.Schedule();
        //4. 等待作业完成
        handle.Complete();
        //5. 使用
        for (int i = 0; i < cubeCount; i++)
        {
            cubeLst[i].position += velocity[i];
        }
        //6. 销毁数据
        velocity.Dispose();
    }
}
