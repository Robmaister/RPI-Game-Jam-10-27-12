using System;

using OpenTK.Graphics.OpenGL;

using MineEscape.Graphics;
using MineEscape.Physics;

namespace MineEscape
{
	public class LevelMap
	{
		private float[] vertices;
		private float[] texcoords;
		private byte[] indices;
		private Texture texture;

		private int width, height;
		private bool[][] collisionMap;

		public LevelMap(Texture texture, Texture colMap)
		{
			this.texture = texture;

			this.width = texture.Size.Width;
			this.height = texture.Size.Height;

			vertices = new float[]
			{
				0, height,
				0, 0,
				width, 0,
				width, height
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

			collisionMap = new bool[height][];
			float[] alphas = new float[width * height];
			GL.BindTexture(TextureTarget.Texture2D, colMap);
			GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Alpha, PixelType.Float, alphas);
			for (int i = 0; i < collisionMap.Length; i++)
			{
				bool[] ba = new bool[width];
				for (int j = 0; j < ba.Length; j++)
				{
					ba[j] = alphas[i * width + j] <= 0.1f;
				}

				collisionMap[i] = ba;
			}

			GL.BindTexture(TextureTarget.Texture2D, 0);
			colMap.Unload();
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

		public bool IsColliding(AABB bounds)
		{
			if (bounds.Left < 0 || bounds.Right >= width || bounds.Top >= height || bounds.Bottom < 0)
				return true;

			for (int i = (int)bounds.Bottom; i <= (int)bounds.Top; i++)
			{
				for (int j = (int)bounds.Left; j <= (int)bounds.Right; j++)
				{
					if (collisionMap[i][j])
						return true;
				}
			}

			return false;
		}

		public void Unload()
		{
			texture.Unload();
		}
	}
}
