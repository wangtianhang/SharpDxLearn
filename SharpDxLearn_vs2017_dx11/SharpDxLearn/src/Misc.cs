using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct VertexPositionColor
{
    public readonly SharpDX.Vector3 Position;
    public readonly SharpDX.Color4 Color;

    public VertexPositionColor(SharpDX.Vector3 position, SharpDX.Color4 color)
    {
        Position = position;
        Color = color;
    }
}
