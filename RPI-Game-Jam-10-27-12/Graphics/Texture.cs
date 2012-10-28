using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MineEscape.Graphics
{
	/// <summary>
	/// A class containing the information for an OpenGL texture.
	/// </summary>
	public class Texture
	{
		private TextureMinFilter minFilter;
		private TextureMagFilter magFilter;
		private TextureWrapMode wrapS;
		private TextureWrapMode wrapT;

		/// <summary>
		/// Gets the OpenGL handle to this texture.
		/// </summary>
		public int ID { get; private set; }

		public bool Exists
		{
			get
			{
				return ID != 0;
			}
		}

		/// <summary>
		/// Gets the size of this texture, in pixels.
		/// </summary>
		public Size Size { get; private set; }

		public Vector2 SizeVec
		{
			get
			{
				return new Vector2(Size.Width, Size.Height);
			}
		}

		/// <summary>
		/// Gets or sets the filter applied when the rendered texture is smaller than original size.
		/// </summary>
		public TextureMinFilter MinFilter
		{
			get
			{
				return minFilter;
			}

			set
			{
				minFilter = value;
				GL.BindTexture(TextureTarget.Texture2D, ID);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)minFilter);
				GL.BindTexture(TextureTarget.Texture2D, 0);
			}
		}

		/// <summary>
		/// Gets or sets the filter applied when the rendered texture is larger than original size.
		/// </summary>
		public TextureMagFilter MagFilter
		{
			get
			{
				return magFilter;
			}

			set
			{
				magFilter = value;
				GL.BindTexture(TextureTarget.Texture2D, ID);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)magFilter);
				GL.BindTexture(TextureTarget.Texture2D, 0);
			}
		}

		/// <summary>
		/// Gets or sets the way OpenGL handles texture coordinates larger than <c>1.0f</c> on the S(X) axis.
		/// </summary>
		public TextureWrapMode WrapS
		{
			get
			{
				return wrapS;
			}

			set
			{
				wrapS = value;
				GL.BindTexture(TextureTarget.Texture2D, ID);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)wrapS);
				GL.BindTexture(TextureTarget.Texture2D, 0);
			}
		}

		/// <summary>
		/// Gets or sets the way OpenGL will handle texture coordinates larger than <c>1.0f</c> on the T(Y) axis.
		/// </summary>
		public TextureWrapMode WrapT
		{
			get
			{
				return wrapT;
			}

			set
			{
				wrapT = value;
				GL.BindTexture(TextureTarget.Texture2D, ID);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)wrapT);
				GL.BindTexture(TextureTarget.Texture2D, 0);
			}
		}

		public static Texture Zero { get { return new Texture(); } }

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Texture class. Generates and stores a bitmap in VRAM.
		/// </summary>
		/// <param name="bmp">The bitmap to be copied to VRAM.</param>
		/// <param name="minFilter">A filter applied when the rendered texture is smaller than the texture at 100%.</param>
		/// <param name="magFilter">A filter applied when the rendered texture is larger than the texture at 100%.</param>
		/// <param name="wrapS">The way OpenGL will handle texture coordinates larger than <c>1.0f</c> on the S axis (X axis).</param>
		/// <param name="wrapT">The way OpenGL will handle texture coordinates larger than <c>1.0f</c> on the T axis (Y axis).</param>
		public Texture(Bitmap bmp, TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT)
		{
			this.Size = bmp.Size;

			//Generate a new texture ID
			ID = GL.GenTexture();

			//Texture parameters
			MinFilter = minFilter;
			MagFilter = magFilter;
			WrapS = wrapS;
			WrapT = wrapT;

			//Bind texture
			GL.BindTexture(TextureTarget.Texture2D, ID);

			//Send bitmap data up to VRAM
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			BitmapData bmpData = bmp.LockBits(new Rectangle(new Point(0, 0), Size), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Size.Width, Size.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
			bmp.UnlockBits(bmpData);

			//unbind texture
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		/// <summary>
		/// Initializes a new instance of the Texture class. Generates and stores a bitmap in VRAM.
		/// </summary>
		/// <param name="bmp">The bitmap to be copied to VRAM.</param>
		public Texture(Bitmap bmp)
			: this(bmp, TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Texture class. Generates and stores a bitmap in VRAM.
		/// </summary>
		/// <param name="path">The location of the texture on the hard drive.</param>
		public Texture(string path)
			: this(path, TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Texture class. Generates and stores a bitmap in VRAM.
		/// </summary>
		/// <param name="path">The location of the texture on the hard drive.</param>
		/// <param name="minFilter">A filter applied when the rendered texture is smaller than the texture at 100%.</param>
		/// <param name="magFilter">A filter applied when the rendered texture is larger than the texture at 100%.</param>
		/// <param name="wrapS">The way OpenGL will handle texture coordinates larger than <c>1.0f</c> on the S axis (X axis).</param>
		/// <param name="wrapT">The way OpenGL will handle texture coordinates larger than <c>1.0f</c> on the T axis (Y axis).</param>
		public Texture(string path, TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT)
			: this(new Bitmap(path), minFilter, magFilter, wrapS, wrapT)
		{
		}

		private Texture()
		{
		}

		public void Unload()
		{
			GL.DeleteTexture(ID);
			ID = 0;
		}

		#endregion

		#region Object Overrides

		public override bool Equals(object obj)
		{
			int? value = obj as int?;

			if (value.HasValue)
				return value == ID;
			else
				return false;
		}

		public override int GetHashCode()
		{
			return ID;
		}

		#endregion

		#region Operator Overloads

		/// <summary>
		/// Implicitly casts the Texture to it's OpenGL pointer. Used to make GL method calls cleaner.
		/// </summary>
		/// <param name="texture">A Texture struct.</param>
		/// <returns>The OpenGL pointer of texture.</returns>
		public static implicit operator int(Texture texture)
		{
			return texture.ID;
		}

		public static bool operator ==(Texture left, Texture right)
		{
			//returns true if they are the exact same Program or if both are null.
			if (object.ReferenceEquals(left, right))
				return true;

			//returns false if only one is null.
			if ((object)left == null || (object)right == null)
				return false;

			return left.ID == right.ID;
		}

		public static bool operator !=(Texture left, Texture right)
		{
			return !(left == right);
		}

		#endregion
	}
}
