using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct GoInGameClientSystem : ISystem
{

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach ((RefRO<NetworkId> networkID, Entity entity) in
        SystemAPI.Query<RefRO<NetworkId>>().WithNone<NetworkStreamInGame>().WithEntityAccess())
        {
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
            Debug.Log("setting client as ingame");

            Entity rpcEntity = entityCommandBuffer.CreateEntity();
            entityCommandBuffer.AddComponent(rpcEntity, new GoInGameRequestRPC());
            entityCommandBuffer.AddComponent(rpcEntity, new SendRpcCommandRequest());
        }

        entityCommandBuffer.Playback(state.EntityManager);
        
    }
}

public struct GoInGameRequestRPC : IRpcCommand
{

}