using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using Unity.NetCode;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct TestClientSystem : ISystem
{
    //[BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
       if (Input.GetKeyDown(KeyCode.T))
        {
            Entity rpcEntity = state.EntityManager.CreateEntity();

            state.EntityManager.AddComponentData(rpcEntity, new SimpleRPC {
                value = 56
            });
            state.EntityManager.AddComponentData(rpcEntity, new SendRpcCommandRequest());
            Debug.Log("Sending RPC...");
        }
    }

    //[BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
