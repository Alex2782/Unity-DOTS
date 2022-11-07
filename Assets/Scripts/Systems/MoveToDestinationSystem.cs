using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class MoveToDestinationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        float deltaTime = Time.DeltaTime;

        
        
        Entities.ForEach((ref Translation translation, ref Rotation rotation, in Destination destination, in MovementSpeed speed) => {

            if (math.all(destination.Value == translation.Value)) return;

            float3 toDestination = destination.Value - translation.Value;
            rotation.Value = quaternion.LookRotation(toDestination, new float3(0, 1, 0));

            float3 movement = math.normalize(toDestination) * speed.Value * deltaTime;

            if (math.length(movement) >= math.length(toDestination))
            {
                translation.Value = destination.Value;
            }
            else
            {
                translation.Value += movement;
            }

        }).ScheduleParallel();
    }
}
