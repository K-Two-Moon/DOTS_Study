using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

/// <summary>
/// 系统，负责逻辑的实现
/// </summary>
public partial struct DemoSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DemoDataTag>();//需要有该标签存在才会执行OnUpdate里面的代码
    }

    void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;//该方法只调用1次
        //查找组件
        foreach (var item in SystemAPI.Query<RefRW<DemoInsData>>())
        {
            //实例化cube
            state.EntityManager.Instantiate(item.ValueRW.insCube);
        }
    }
}
