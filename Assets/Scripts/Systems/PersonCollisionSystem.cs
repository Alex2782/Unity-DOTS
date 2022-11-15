using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

public partial class PersonCollisionSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct PersonCollisionJob : ITriggerEventsJob
    {
        public void Execute(TriggerEvent triggerEvent)
        {

        }
    }

    protected override void OnUpdate()
    {
        Dependency = new PersonCollisionJob
        {

        }.Schedule(stepPhysicsWorld.Simulation, Dependency);
    }
}
