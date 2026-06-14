using System.Numerics;
using Silk.FirstSteps.Components;
using Silk.NET.OpenGL;

namespace Silk.FirstSteps.Objects
{
	public class Lamp : GameObject, IRenderable
	{
		public LightSource light;
		public Model model;

		public Lamp (GL gl)
		{
			model = new Model(gl, "../net10.0/Assets/cube.model");
			model.Shader = new Shader("../net10.0/Graphics/Shaders/shader.vert", "../net10.0/Graphics/Shaders/shader.frag");
			light = new LightSource(new(255f, 255f, 255f), 0.2f, 0.75f, 0.5f, Vector3.Zero);
			light.Attenuation = new(1.0f, 0.09f, 0.032f);
			light.gameObject = this;
		}
		public Lamp (GL gl, Vector3 Color) : this(gl)
		{
			light.LightColor = Color;
		}

		public unsafe void Render (GL gl, Camera camera, LightSource[] lights)
		{
			foreach ( Mesh mesh in model.Meshes )
			{
				mesh.Bind();
				model.Shader.Use();
				model.Shader.SetUniform("lightColor", light.LightColor / 255f);
				model.Shader.SetUniform("uModel", Transform.ModelMatrix);
				model.Shader.SetUniform("uView", camera.ViewMatrix);
				model.Shader.SetUniform("uProjection", camera.ProjectionMatrix);
				gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) mesh.Vertices.Length);
			}
		}
	}
}
