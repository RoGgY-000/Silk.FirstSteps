using System.Numerics;

namespace Silk.FirstSteps
{
	public class Camera : GameObject
	{
		public float AspectRatio { get; set; }
		public float Yaw { get; set; }
		public float Pitch { get; set; }

		private float Zoom;

		public Matrix4x4 ViewMatrix
			=> Matrix4x4.CreateLookAt(Transform.Position, Transform.Position + Transform.Direction, Vector3.UnitY);

		public Matrix4x4 ProjectionMatrix
			=> Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Zoom), AspectRatio, 0.1f, 100.0f);

		public Camera (Vector3 position, float aspectRatio)
		{
			Transform.Position = position;
			AspectRatio = aspectRatio;
			Zoom = 45f;
		}

		public Camera ()
		{
			Transform.Position = new Vector3(0.0f, 0.0f, 3.0f);
			Zoom = 45f;

		}

		public void ModifyZoom (float zoomAmount)
		{
			//We don't want to be able to zoom in too close or too far away so clamp to these values
			Zoom = Math.Clamp(Zoom - zoomAmount, 1.0f, 45f);
		}

		public void ModifyDirection (float xOffset, float yOffset)
		{
			Yaw -= xOffset;
			Pitch -= yOffset;
			Pitch = Math.Clamp(Pitch, -89f, 89f);

			Transform.Rotation =
				Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(Yaw))
				* Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(Pitch));
		}
	}
}
