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

		public LevelMap(float width, float height, Texture texture)
		{
			this.texture = texture;

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

			int widthI = texture.Size.Width;
			int heightI = texture.Size.Height;

			this.width = widthI;
			this.height = heightI;

			collisionMap = new bool[heightI][];
			float[] alphas = new float[widthI * heightI];
			GL.BindTexture(TextureTarget.Texture2D, texture);
			GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Alpha, PixelType.Float, alphas);
			//GL.ReadPixels(0, 0, widthI, heightI, PixelFormat.Alpha, PixelType.Float, alphas);
			for (int i = 0; i < collisionMap.Length; i++)
			{
				bool[] ba = new bool[widthI];
				for (int j = 0; j < ba.Length; j++)
				{
					ba[j] = alphas[i * widthI + j] <= 0.1f;
				}

				collisionMap[i] = ba;
			}
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
	}
}
