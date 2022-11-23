using Unity.Entities;
using Unity.Jobs;

public partial class LifetimeSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimulationEcbSystem;

    protected override void OnCreate()
    {
        endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        var ecb = endSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();


        Entities.ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) => {

            lifetime.Value -= deltaTime;

            if (lifetime.Value <= 0f)
            {
                ecb.DestroyEntity(entityInQueryIndex, entity);
            }

        }).ScheduleParallel();

        endSimulationEcbSystem.AddJobHandleForProducer(Dependency);
    }
}
