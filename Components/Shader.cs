using System.Numerics;
using Silk.NET.OpenGL;

namespace Silk.FirstSteps
{
	public class Shader : IDisposable
	{
		public static GL Gl;
		public static Shader Default => _default.Value;


		private static readonly Lazy<Shader> _default
			= new Lazy<Shader>(()
				=> new Shader("../net10.0/Graphics/Shaders/shader.vert", "../net10.0/Graphics/Shaders/lighting.frag"));
		private uint _handle;

		public Shader (string vertexPath, string fragmentPath)
		{
			uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
			uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
			_handle = Gl.CreateProgram();
			Gl.AttachShader(_handle, vertex);
			Gl.AttachShader(_handle, fragment);
			Gl.LinkProgram(_handle);
			Gl.GetProgram(_handle, GLEnum.LinkStatus, out var status);
			if ( status == 0 )
			{
				throw new Exception($"Program failed to link with error: {Gl.GetProgramInfoLog(_handle)}");
			}

			Gl.DetachShader(_handle, vertex);
			Gl.DetachShader(_handle, fragment);
			Gl.DeleteShader(vertex);
			Gl.DeleteShader(fragment);
		}

		public void Use ()
		{
			Gl.UseProgram(_handle);
		}

		public void SetUniform (string name, int value)
		{
			int location = Gl.GetUniformLocation(_handle, name);
			if ( location == -1 )
			{
				throw new Exception($"{name} uniform not found on shader.");
			}
			Gl.Uniform1(location, value);
		}

		public unsafe void SetUniform (string name, Matrix4x4 value)
		{
			//A new overload has been created for setting a uniform so we can use the transform in our shader.
			int location = Gl.GetUniformLocation(_handle, name);
			if ( location == -1 )
			{
				throw new Exception($"{name} uniform not found on shader.");
			}
			Gl.UniformMatrix4(location, 1, false, (float*) &value);
		}

		public void SetUniform (string name, float value)
		{
			int location = Gl.GetUniformLocation(_handle, name);
			if ( location == -1 )
			{
				throw new Exception($"{name} uniform not found on shader.");
			}
			Gl.Uniform1(location, value);
		}

		public void SetUniform (string name, Vector3 value)
		{
			int location = Gl.GetUniformLocation(_handle, name);
			if ( location == -1 )
			{
				throw new Exception($"{name} uniform not found on shader.");
			}
			Gl.Uniform3(location, value.X, value.Y, value.Z);
		}

		public void Dispose ()
		{
			Gl.DeleteProgram(_handle);
		}

		private uint LoadShader (ShaderType type, string path)
		{
			string src = File.ReadAllText(path);
			uint handle = Gl.CreateShader(type);
			Gl.ShaderSource(handle, src);
			Gl.CompileShader(handle);
			string infoLog = Gl.GetShaderInfoLog(handle);
			if ( !string.IsNullOrWhiteSpace(infoLog) )
			{
				throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
			}

			return handle;
		}
	}
}