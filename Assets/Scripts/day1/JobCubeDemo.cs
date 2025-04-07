using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
[BurstCompile]
/// <summary>
/// �ٶ���ҵ
/// </summary>
public struct VelocityJob : IJob
{
    /// <summary>
    /// �����ٶ�
    /// </summary>
    public float speed;
    /// <summary>
    /// �ƶ�ʱ��
    /// </summary>
    public float time;
    /// <summary>
    /// �ٶ�
    /// </summary>
    public NativeArray<Vector3> velocity;
    /// <summary>
    /// ִ��
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
/// ������ҵ
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
        //1. ׼������
        NativeArray<Vector3> velocity = new NativeArray<Vector3>( cubeCount, Allocator.TempJob);
        //2. ʵ������ҵ
        VelocityJob velocityJob = new VelocityJob { speed=3, time=Time.deltaTime, velocity=velocity };
        //3. ������ҵ
        var handle = velocityJob.Schedule();
        //4. �ȴ���ҵ���
        handle.Complete();
        //5. ʹ��
        for (int i = 0; i < cubeCount; i++)
        {
            cubeLst[i].position += velocity[i];
        }
        //6. ��������
        velocity.Dispose();
    }
}
