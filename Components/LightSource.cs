using System.Numerics;

namespace Silk.FirstSteps.Components
{
	public class LightSource : Component
	{
		public Vector3 LightColor { get; set; }
		public float Ambient { get; set; }
		public float Diffuse { get; set; }
		public float Specular { get; set; }
		public Vector3 Attenuation { get; set; }
		public Vector3 LightPosition
		{
			get => Vector3.Transform(field, gameObject.Transform.Rotation) + gameObject.Transform.Position;
			set;
		}

		public LightSource (Vector3 color, float ambient, float diffuse, float specular, Vector3 position)
		{
			LightColor = color;
			Ambient = ambient;
			Diffuse = diffuse;
			Specular = specular;
			LightPosition = position;
		}
	}
}
