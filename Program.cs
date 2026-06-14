using System.Numerics;
using Silk.FirstSteps.Components;
using Silk.FirstSteps.Objects;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Silk.FirstSteps
{
	class Program
	{
		private static IWindow window;
		public static GL Gl;
		private static IKeyboard primaryKeyboard;
		private static Vector2 LastMousePosition;

		private static Lamp lamp;
		private static Lamp lamp1;
		private static Lamp lamp2;
		private static Lamp lamp3;

		private static Objects.Plane Floor;
		private static Sphere Sphere;

		private static Camera camera;
		private static void Main (string[] args)
		{
			var options = WindowOptions.Default;
			options.PreferredDepthBufferBits = 24;
			options.Samples = 16;
			options.Title = "My first game engine";
			options.VSync = true;
			options.Size = new(1920, 1080);
			options.WindowBorder = WindowBorder.Hidden;
			options.WindowState = WindowState.Fullscreen;
			window = Window.Create(options);

			window.Load += OnLoad;
			window.Update += OnUpdate;
			window.Render += OnRender;
			window.FramebufferResize += OnFramebufferResize;
			window.Closing += OnClose;

			window.Run();

			window.Dispose();
		}

		private static void OnLoad ()
		{
			Gl = GL.GetApi(window);
			Shader.Gl = Gl;
			Gl.Enable(EnableCap.Multisample);

			IInputContext input = window.CreateInput();
			primaryKeyboard = input.Keyboards.FirstOrDefault();
			if ( primaryKeyboard != null )
			{
				primaryKeyboard.KeyDown += KeyDown;
			}
			for ( int i = 0; i < input.Mice.Count; i++ )
			{
				input.Mice[i].Cursor.CursorMode = CursorMode.Raw;
				input.Mice[i].MouseMove += OnMouseMove;
				input.Mice[i].Scroll += OnMouseWheel;
			}

			camera = new Camera(new(0f, 1f, 5f), (float) window.Size.X/ window.Size.Y);

			lamp = new Lamp(Gl, new(0f,0f,255f));
			lamp.Transform.Scale = 0.2f;

			lamp1 = new Lamp(Gl, new(255f, 0f, 0f));
			lamp1.Transform.Scale = 0.2f;

			lamp2 = new Lamp(Gl, new(0f, 255f, 0f));
			lamp2.Transform.Scale = 0.2f;

			lamp3 = new Lamp(Gl, new(255f, 255f, 255f));
			lamp3.Transform.Scale = 0.2f;

			Floor = new Objects.Plane(Gl);
			Floor.Transform.Position -= Vector3.UnitY;

			Sphere = new Sphere(Gl, 0.5f);
		}

		private static unsafe void OnUpdate (double deltaTime)
		{
			var moveSpeed = 2.5f * (float) deltaTime;

			if ( primaryKeyboard.IsKeyPressed(Key.W) )
			{
				//Move forwards
				camera.Transform.Position += moveSpeed * camera.Transform.Direction;
			}
			if ( primaryKeyboard.IsKeyPressed(Key.S) )
			{
				//Move backwards
				camera.Transform.Position -= moveSpeed * camera.Transform.Direction;
			}
			if ( primaryKeyboard.IsKeyPressed(Key.A) )
			{
				//Move left
				camera.Transform.Position -= Vector3.Normalize(Vector3.Cross(camera.Transform.Direction, Vector3.UnitY)) * moveSpeed;
			}
			if ( primaryKeyboard.IsKeyPressed(Key.D) )
			{
				//Move right
				camera.Transform.Position += Vector3.Normalize(Vector3.Cross(camera.Transform.Direction, Vector3.UnitY)) * moveSpeed;
			}

			lamp.Transform.Position = Vector3.UnitX;
			lamp1.Transform.Position = -Vector3.UnitX;
			lamp2.Transform.Position = Vector3.UnitZ;
			lamp3.Transform.Position = -Vector3.UnitZ;
		}

		private static unsafe void OnRender (double deltaTime)
		{
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			Gl.Enable(EnableCap.DepthTest);
			Gl.Enable(EnableCap.CullFace);
			Gl.CullFace(TriangleFace.Back);

			var floorModel = Floor.Transform.ModelMatrix;
			var view = camera.ViewMatrix;
			var projection = camera.ProjectionMatrix;

			lamp.Render(Gl, camera, Array.Empty<LightSource>());
			lamp1.Render(Gl, camera, Array.Empty<LightSource>());
			lamp2.Render(Gl, camera, Array.Empty<LightSource>());
			lamp3.Render(Gl, camera, Array.Empty<LightSource>());
			Floor.Render(Gl, camera, [lamp.light, lamp1.light, lamp2.light, lamp3.light]);
			Sphere.Render(Gl, camera, [lamp.light, lamp1.light, lamp2.light, lamp3.light]);
		}

		private static void OnFramebufferResize (Vector2D<int> newSize)
		{
			Gl.Viewport(newSize);
			camera.AspectRatio = (float)newSize.X / newSize.Y;
		}

		private static unsafe void OnMouseMove (IMouse mouse, Vector2 position)
		{
			float sensivity = 0.1f;
			if ( LastMousePosition == default )
			{
				LastMousePosition = position;
			}
			else
			{
				float xOffset = (position.X - LastMousePosition.X) * sensivity;
				float yOffset = (position.Y - LastMousePosition.Y) * sensivity;
				LastMousePosition = position;
				camera.ModifyDirection(xOffset, yOffset);
			}
		}

		private static unsafe void OnMouseWheel (IMouse mouse, ScrollWheel scrollWheel)
		{

		}

		private static void OnClose ()
		{

		}

		private static void KeyDown (IKeyboard keyboard, Key key, int arg3)
		{
			if ( key == Key.Escape )
			{
				window.Close();
			}
		}
	}
}
