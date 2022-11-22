using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
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
        [ReadOnly] public ComponentDataFromEntity<PersonTag> PersonGroup;
        public ComponentDataFromEntity<URPMaterialPropertyBaseColor> ColorGroup;
        public float Seed;


        public void Execute(TriggerEvent triggerEvent)
        {
            bool isEntityAPerson = PersonGroup.HasComponent(triggerEvent.EntityA);
            bool isEntityBPerson = PersonGroup.HasComponent(triggerEvent.EntityB);

            if (!isEntityAPerson || !isEntityBPerson) { return; }

            var random = new Random((uint)((1 + Seed) + (triggerEvent.BodyIndexA * triggerEvent.BodyIndexB)));

            random = ChangeMaterialColor(random, triggerEvent.EntityA);
            ChangeMaterialColor(random, triggerEvent.EntityB);
        }

        private Random ChangeMaterialColor(Random random, Entity entity)
        {
            if (ColorGroup.HasComponent(entity))
            {
                var colorComponent = ColorGroup[entity];
                colorComponent.Value.x = random.NextFloat(0, 1);
                colorComponent.Value.y = random.NextFloat(0, 1);
                colorComponent.Value.z = random.NextFloat(0, 1);
                ColorGroup[entity] = colorComponent;
            }

            return random;
        }

    }



    protected override void OnUpdate()
    {
        Dependency = new PersonCollisionJob
        {
            PersonGroup = GetComponentDataFromEntity<PersonTag>(true),
            ColorGroup = GetComponentDataFromEntity<URPMaterialPropertyBaseColor>(),
            Seed = System.DateTimeOffset.Now.Millisecond

        }.Schedule(stepPhysicsWorld.Simulation, Dependency);
    }
}
