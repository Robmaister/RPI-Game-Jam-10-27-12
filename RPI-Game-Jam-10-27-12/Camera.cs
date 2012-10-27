using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MineEscape
{
	public class Camera
	{
		private Matrix4 projection;
		private Matrix4 view;

		private Vector2 position;
		private float angle;

		public Camera()
		{
			position = new Vector2(0, 0);
			view = Matrix4.Identity;
			projection = Matrix4.CreateOrthographicOffCenter(-512, 512, -384, 384, 0, 1);
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
				RebuildView();
			}
		}

		public float Rotation
		{
			get
			{
				return angle;
			}
			set
			{
				angle = value;
				RebuildView();
			}
		}

		public void LoadProjection()
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);
			GL.MatrixMode(MatrixMode.Modelview);
		}

		public void LoadView()
		{
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref view);
		}

		private void RebuildView()
		{
			view = Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(-position.X, -position.Y, 0);
		}
	}
}
