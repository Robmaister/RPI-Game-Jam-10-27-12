using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MineEscape.Graphics
{
	public class Mesh
	{
		private float[] vertices;
		private float[] texcoords;
		private byte[] indices;
		private Texture texture;

		public Mesh(float width, float height, Texture texture)
		{
			this.texture = texture;
			float halfX = width * 0.5f;
			float halfY = height * 0.5f;

			vertices = new float[]
			{
				-halfX, halfY,
				-halfX, -halfY,
				halfX, -halfY,
				halfX, halfY
			};

			texcoords = new float[]
			{
				0, 1,
				0, 0,
				1, 0,
				1, 1
			};

			indices = new byte[]
			{
				0, 1, 2,
				0, 2, 3
			};
		}

		public void Draw()
		{
			GL.BindTexture(TextureTarget.Texture2D, texture);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);

			GL.VertexPointer(2, VertexPointerType.Float, 8, vertices);
			GL.TexCoordPointer(2, TexCoordPointerType.Float, 8, texcoords);

			GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedByte, indices);

			GL.DisableClientState(ArrayCap.VertexArray);
			GL.DisableClientState(ArrayCap.TextureCoordArray);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
	}
}
