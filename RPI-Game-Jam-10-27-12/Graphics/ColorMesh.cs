using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MineEscape.Graphics
{
	public class ColorMesh
	{
		private float[] vertices;
		private byte[] indices;
		public Color4 color;

		public ColorMesh(float width, float height)
		{
			float halfX = width * 0.5f;
			float halfY = height * 0.5f;

			vertices = new float[]
			{
				-halfX, halfY,
				-halfX, -halfY,
				halfX, -halfY,
				halfX, halfY
			};

			indices = new byte[]
			{
				0, 1, 2,
				0, 2, 3
			};
		}

		public void Draw()
		{
			GL.EnableClientState(ArrayCap.VertexArray);

			GL.Color4(color);

			GL.VertexPointer(2, VertexPointerType.Float, 0, vertices);

			GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedByte, indices);

			GL.DisableClientState(ArrayCap.VertexArray);
		}
	}
}
