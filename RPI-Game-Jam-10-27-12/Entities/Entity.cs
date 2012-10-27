using System;
using System.Collections.Generic;

using OpenTK;

using MineEscape.Graphics;
using MineEscape.Physics;
using OpenTK.Graphics.OpenGL;

namespace MineEscape.Entities
{
	public abstract class Entity
	{
		protected Mesh mesh;
		protected float angle, moveSpeed;
		protected Vector2 position, size;
		protected Matrix4 modelMatrix;
		protected AABB boundingBox;

		protected Entity(Mesh mesh, float angle, Vector2 position, Vector2 size, float moveSpeed)
		{
			this.mesh = mesh;
			this.angle = angle;
			this.position = position;
			this.size = size;
			this.moveSpeed = moveSpeed;
			UpdateModelMatrix();
		}

		public AABB BoundingBox { get { return boundingBox; } }
		public Vector2 Position { get { return position; } set { position = value; UpdateModelMatrix(); } }
		public Vector2 Size { get { return size; } }

		protected void UpdateModelMatrix()
		{
			float halfSizeX = size.X * 0.5f;
			float halfSizeY = size.Y * 0.5f;
			boundingBox = new AABB(-halfSizeX + position.X, halfSizeX + position.X, halfSizeY + position.Y, -halfSizeY + position.Y);
			modelMatrix = Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(position.X, position.Y, 0);
		}

		public void Draw()
		{
			GL.PushMatrix();
			GL.MultMatrix(ref modelMatrix);
			mesh.Draw();
			GL.PopMatrix();
		}

		public void Update(float time)
		{
		}

		public void MoveUp(float time)
		{
			angle = 0;
			position.Y += moveSpeed * time;
			UpdateModelMatrix();
		}

		public void MoveDown(float time)
		{
			angle = MathHelper.Pi;
			position.Y -= moveSpeed * time;
			UpdateModelMatrix();
		}

		public void MoveLeft(float time)
		{
			angle = MathHelper.PiOver2;
			position.X -= moveSpeed * time;
			UpdateModelMatrix();
		}

		public void MoveRight(float time)
		{
			angle = MathHelper.ThreePiOver2;
			position.X += moveSpeed * time;
			UpdateModelMatrix();
		}

		public void MoveUpLeft(float time)
		{
			angle = MathHelper.PiOver4;
			position += Vector2.Normalize(new Vector2(-1, 1)) * moveSpeed * time;
			UpdateModelMatrix();
		}

		public void MoveUpRight(float time)
		{
			angle = 7 * MathHelper.PiOver4;
			position += Vector2.Normalize(new Vector2(1, 1)) * moveSpeed * time;
			UpdateModelMatrix();
		}

		public void MoveDownLeft(float time)
		{
			angle = 3 * MathHelper.PiOver4;
			position += Vector2.Normalize(new Vector2(-1, -1)) * moveSpeed * time;
			UpdateModelMatrix();
		}

		public void MoveDownRight(float time)
		{
			angle = 5 * MathHelper.PiOver4;
			position += Vector2.Normalize(new Vector2(1, -1)) * moveSpeed * time;
			UpdateModelMatrix();
		}
	}
}
