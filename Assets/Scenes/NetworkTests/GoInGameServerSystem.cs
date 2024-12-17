using UnityEngine;
using Unity.Entities;
using Unity.NetCode;
using Unity.Burst;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct GoInGameServerSystem : ISystem
{
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer eCB = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach ((RefRO<ReceiveRpcCommandRequest> rpc, Entity entity) in
        SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequestRPC>().WithEntityAccess())
        {
            eCB.AddComponent<NetworkStreamInGame>(rpc.ValueRO.SourceConnection);

            Debug.Log("Client connected to server.");

            eCB.DestroyEntity(entity);
        }

        eCB.Playback(state.EntityManager);
    }
}
