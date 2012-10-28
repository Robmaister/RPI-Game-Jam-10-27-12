using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using MineEscape.Graphics;
using MineEscape.Physics;
using MineEscape.Audio;

namespace MineEscape.Entities
{
	public class Player : Entity
	{
		private float[] lightVertices;
		private Mesh shadow;

		private float animTime;
		private float deltaTime;
		private Texture[] animTextures;
		private int animIndex;

		public bool drawShadow;

		public int health;

		public Source walkSource;

		public Player()
			: base(new Mesh(64, 64, Resources.Textures["Miner.png"]), 0, new Vector2(2000, 1500), new Vector2(64, 64), 256)
		{
			drawShadow = true;

			const int lightVerticesCount = 16;
			const float lightFoV = MathHelper.PiOver2;
			const float lightRadius = 400;

			List<float> lightVertList = new List<float>();
			lightVertList.Add(0);
			lightVertList.Add(0); //starts at the origin of player space.

			for (int i = 0; i < lightVerticesCount; i++)
			{
				float angle = (lightFoV * (float)i / ((float)lightVerticesCount - 1)) - (lightFoV * 0.5f) + MathHelper.PiOver2;
				lightVertList.Add((float)Math.Cos(angle) * lightRadius);
				lightVertList.Add((float)Math.Sin(angle) * lightRadius);
			}

			lightVertices = lightVertList.ToArray();

			animTime = 0.2f;

			animTextures = new Texture[]
			{
				Resources.Textures["Miner_walk1.png"],
				Resources.Textures["Miner_walk2.png"]
			};

			shadow = new Mesh(1024, 768, Resources.Textures["shadow.png"]);

			health = 5;

			walkSource = new Source();
			//walkSource.Position = new Vector3(position);
			walkSource.Relative = true;
			walkSource.Looping = true;
			walkSource.Buffer = Resources.Audio["walk.wav"];
		}

		public override void Update(float time)
		{
			base.Update(time);

			if (moving)
			{
				if (walkSource.State != OpenTK.Audio.OpenAL.ALSourceState.Playing)
					walkSource.Play();

				deltaTime += time;

				if (deltaTime >= animTime)
				{
					deltaTime = 0;
					animIndex = ++animIndex % animTextures.Length; //increment animIndex and make it wrap around to 0.
					mesh.texture = animTextures[animIndex];
				}
			}
			else if (walkSource.State == OpenTK.Audio.OpenAL.ALSourceState.Playing)
				walkSource.Stop();
		}

		public override void Draw()
		{
			base.Draw();

			if (drawShadow)
			{
				GL.PushMatrix();
				GL.MultMatrix(ref modelMatrix);

				GL.Enable(EnableCap.StencilTest);
				GL.ColorMask(false, false, false, false);
				GL.StencilFunc(StencilFunction.Never, 1, 0xFF);
				GL.StencilOp(StencilOp.Replace, StencilOp.Keep, StencilOp.Keep);

				GL.StencilMask(0xFF);
				GL.Clear(ClearBufferMask.StencilBufferBit);
				GL.EnableClientState(ArrayCap.VertexArray);
				GL.VertexPointer(2, VertexPointerType.Float, 0, lightVertices);
				GL.DrawArrays(BeginMode.TriangleFan, 0, lightVertices.Length / 2);
				GL.DisableClientState(ArrayCap.VertexArray);

				GL.Enable(EnableCap.AlphaTest);
				GL.AlphaFunc(AlphaFunction.Greater, 0.5f);
				GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
				GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
				mesh.Draw();

				GL.Disable(EnableCap.AlphaTest);

				GL.ColorMask(true, true, true, true);
				GL.StencilMask(0);
				GL.StencilFunc(StencilFunction.Equal, 0, 0xFF);

				GL.LoadMatrix(ref Matrix4.Identity);

				shadow.Draw();

				GL.Disable(EnableCap.StencilTest);
				GL.PopMatrix();
			}
		}
	}
}
