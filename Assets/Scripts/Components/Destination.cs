using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Destination : IComponentData
{
    public float3 Value;
}
