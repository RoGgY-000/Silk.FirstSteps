using System;
using System.Collections.Generic;
using System.Text;
using Silk.NET.OpenGL;

namespace Silk.FirstSteps
{
	public class Mesh : IDisposable
	{
		public Mesh (GL gl, float[] vertices, uint[] indices, List<Texture> textures)
		{
			GL = gl;
			Vertices = vertices;
			Indices = indices;
			Textures = textures;
			SetupMesh();
		}

		public float[] Vertices { get; private set; }
		public uint[] Indices { get; private set; }
		public IReadOnlyList<Texture> Textures { get; private set; }
		public VertexArrayObject<float, uint> VAO { get; set; }
		public BufferObject<float> VBO { get; set; }
		public BufferObject<uint> EBO { get; set; }
		public GL GL { get; }

		public unsafe void SetupMesh ()
		{
			EBO = new BufferObject<uint>(GL, Indices, BufferTargetARB.ElementArrayBuffer);
			VBO = new BufferObject<float>(GL, Vertices, BufferTargetARB.ArrayBuffer);
			VAO = new VertexArrayObject<float, uint>(GL, VBO, EBO);

			// Общий размер (stride) теперь равен 8 * sizeof(float)
			VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 8, 0); // Позиция (offset = 0)
			VAO.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 8, 3); // UV-координаты (offset = 3)
			VAO.VertexAttributePointer(2, 3, VertexAttribPointerType.Float, 8, 5); // Нормали (offset = 5)
		}

		public void Bind ()
		{
			VAO.Bind();
		}

		public void Dispose ()
		{
			Textures = null;
			VAO.Dispose();
			VBO.Dispose();
			EBO.Dispose();
		}
	}
}
