//using Unity.Entities;
//using UnityEngine;

//[UpdateInGroup(typeof(GroupDemo2211))]
///// <summary>
///// SystemBasedemo
///// </summary>
//public partial class SystemBaseDemo : SystemBase
//{
//    private GameObject obj;
//    private int count;
//    private string msg;
//    protected override void OnCreate()
//    {
//        RequireForUpdate<Day4Tag>();
//        Debug.Log("SystemBase_OnCreate");

//    }
//    protected override void OnStartRunning()
//    {
//        Debug.Log("SystemBase_OnStartRunning");
//    }
//    protected override void OnUpdate()
//    {
//        Enabled = false;
//        Debug.Log("SystemBase_OnUpdate");
//    }
//    protected override void OnStopRunning()
//    {
//        Debug.Log("SystemBase_OnStopRunning");
//    }
//    protected override void OnDestroy()
//    {
//        Debug.Log("SystemBase_OnDestroy");
//    }
//}
