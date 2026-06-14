using System.Numerics;

namespace Silk.FirstSteps
{
	public class Transform
	{
		public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
		public Quaternion Rotation { get; set; } = Quaternion.Identity;
		public Vector3 Front { get; } = new(0f, 0f, -1f);
		public Vector3 Direction => Vector3.Transform(Front, Rotation);
		public float Scale { get; set; } = 1f;

		public Matrix4x4 ModelMatrix => Matrix4x4.Identity 
			* Matrix4x4.CreateFromQuaternion(Rotation) 
			* Matrix4x4.CreateScale(Scale) 
			* Matrix4x4.CreateTranslation(Position);
	}
}
