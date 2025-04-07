//using Unity.Entities;
//using UnityEngine;
//using UnityEngine.Scripting;

//[UpdateInGroup(typeof(GroupDemo2211))]
///// <summary>
///// ISystemDemo
///// </summary>
//public partial struct ISystemDemo : ISystem,ISystemStartStop
//{
     
//    public void OnStartRunning(ref SystemState state)
//    {
//        Debug.LogWarning("OnStartRunning");
//    }

//    public void OnStopRunning(ref SystemState state)
//    {
//        Debug.LogWarning("OnStopRunning");
//    }

//    void OnCreate(ref SystemState state)
//    {
//        state.RequireForUpdate<Day4Tag>();
//        Debug.LogWarning("OnCreate");
//    }

//    void OnDestroy(ref SystemState state)
//    {
//        Debug.LogWarning("OnDestroy");
//    }

//    void OnUpdate(ref SystemState state)
//    {
//        state.Enabled = false;
//        GameObject obj;
//        Debug.LogWarning("OnUpdate");
//    }
//}
