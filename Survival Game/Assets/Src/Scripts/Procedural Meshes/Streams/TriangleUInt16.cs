using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace ProceduralMeshes.Streams
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TriangleUInt16
    {
        public ushort x, y, z;

        public static implicit operator TriangleUInt16(int3 t) => new TriangleUInt16
        {
            x = (ushort)t.x,
            y = (ushort)t.y,
            z = (ushort)t.z
        };
    }   
}