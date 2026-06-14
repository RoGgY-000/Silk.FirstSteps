using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Silk.FirstSteps.Components;
using Silk.NET.OpenGL;

namespace Silk.FirstSteps.Objects
{
	public class Plane : GameObject
	{
		Model model;

		public Plane (GL gl)
		{
			model = new Model(gl, "../net10.0/Assets/floor.model");
			model.Shader = Shader.Default;
		}

		public unsafe void Render (GL gl, Camera camera, LightSource[] lights)
		{
			foreach ( Mesh mesh in model.Meshes )
			{
				mesh.Bind();
				model.Shader.Use();
				model.Shader.SetUniform("uModel", Transform.ModelMatrix);
				model.Shader.SetUniform("uView", camera.ViewMatrix);
				model.Shader.SetUniform("uProjection", camera.ProjectionMatrix);
				model.Shader.SetUniform("viewPos", camera.Transform.Position);
				model.Shader.SetUniform("lightCount", lights.Length);
				model.Shader.SetUniform("material.ambient", new Vector3(0.2f));
				model.Shader.SetUniform("material.diffuse", new Vector3(0.5f));
				model.Shader.SetUniform("material.specular", new Vector3(0.2f));
				model.Shader.SetUniform("material.shininess", 16.0f);
				for ( int i = 0; i < lights.Length; i++ )
				{
					string arrayIdx = $"lights[{i}].";
					model.Shader.SetUniform(arrayIdx + "position", lights[i].LightPosition);
					model.Shader.SetUniform(arrayIdx + "color", lights[i].LightColor / 255f);
					model.Shader.SetUniform(arrayIdx + "ambient", lights[i].Ambient);
					model.Shader.SetUniform(arrayIdx + "diffuse", lights[i].Diffuse);
					model.Shader.SetUniform(arrayIdx + "specular", lights[i].Specular);
					model.Shader.SetUniform(arrayIdx + "constant", lights[i].Attenuation.X);
					model.Shader.SetUniform(arrayIdx + "linear", lights[i].Attenuation.Y);
					model.Shader.SetUniform(arrayIdx + "quadratic", lights[i].Attenuation.Z);
				}
				gl.DrawElements(PrimitiveType.Triangles, (uint) mesh.Indices.Length, DrawElementsType.UnsignedInt, (void*) 0);
			}
		}
	}
}
