using Silk.FirstSteps.Components;
using Silk.NET.OpenGL;

namespace Silk.FirstSteps.Objects
{
	public interface IRenderable
	{
		public unsafe void Render (GL gl, Camera camera, LightSource[] lights);
	}
}