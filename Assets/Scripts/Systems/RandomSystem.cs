using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class RandomSystem : SystemBase
{
    public NativeArray<Unity.Mathematics.Random> RandomArray { get; private set; }

    protected override void OnCreate()
    {
        int maxJobs = JobsUtility.MaxJobThreadCount;
        var randomArray = new Unity.Mathematics.Random[maxJobs];
        var seed = new System.Random();


        for (int i = 0; i < maxJobs; i++)
        {
            randomArray[i] = new Unity.Mathematics.Random((uint)seed.Next());
        }

        RandomArray = new NativeArray<Random>(randomArray, Allocator.Persistent);
    }


    protected override void OnDestroy()
    {
        RandomArray.Dispose();
    }


    protected override void OnUpdate()
    {

    }
}
