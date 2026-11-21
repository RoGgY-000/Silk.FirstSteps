using System.Numerics;

namespace Silk.FirstSteps.Components
{
	public class LightSource : Component
	{
		public Vector3 LightColor { get; set; }
		public float Ambient => BrightnessModifiers.X * Brightness;
		public float Diffuse => BrightnessModifiers.Y * Brightness;
		public float Specular => BrightnessModifiers.Z * Brightness;
		public float Brightness { get; set; }
		public Vector3 BrightnessModifiers { get; set; }
		public Vector3 Attenuation { get; set; }
		public Vector3 LightPosition
		{
			get => Vector3.Transform(field, gameObject.Transform.Rotation) + gameObject.Transform.Position;
			set;
		}

		public LightSource (Vector3 color, Vector3 position)
		{
			LightColor = color;
			LightPosition = position;
			Brightness = 2f;
			BrightnessModifiers = new(0.2f, 0.5f, 0.3f);
		}
	}
}
