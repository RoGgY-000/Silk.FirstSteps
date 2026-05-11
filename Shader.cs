using System;
using System.Collections.Generic;
using System.Text;
using Silk.NET.OpenGL;

namespace Silk.FirstSteps
{
    internal class Shader : IDisposable
    {
        private uint _handle;
        private GL _gl;

        public Shader (GL gl, string vertexPath, string fragmentPath)
        {
            _gl = gl;

            // Читаем файлы
            string vertexCode = File.ReadAllText(vertexPath);
            string fragmentCode = File.ReadAllText(fragmentPath);

            // Компилируем
            uint vertex = CompileShader(ShaderType.VertexShader, vertexCode);
            uint fragment = CompileShader(ShaderType.FragmentShader, fragmentCode);

            // Линкуем программу
            _handle = _gl.CreateProgram();
            _gl.AttachShader(_handle, vertex);
            _gl.AttachShader(_handle, fragment);
            _gl.LinkProgram(_handle);

            // Проверка на ошибки линковки
            _gl.GetProgram(_handle, GLEnum.LinkStatus, out int status);
            if ( status == 0 )
                throw new Exception($"Ошибка линковки программы: {_gl.GetProgramInfoLog(_handle)}");

            // Очистка (шейдеры уже внутри программы)
            _gl.DetachShader(_handle, vertex);
            _gl.DetachShader(_handle, fragment);
            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
        }

        public void Use () => _gl.UseProgram(_handle);

        public void Dispose () => _gl.DeleteProgram(_handle);

        private uint CompileShader (ShaderType type, string code)
        {
            uint handle = _gl.CreateShader(type);
            _gl.ShaderSource(handle, code);
            _gl.CompileShader(handle);

            _gl.GetShader(handle, ShaderParameterName.CompileStatus, out int status);
            if ( status == 0 )
                throw new Exception($"Ошибка компиляции {type}: {_gl.GetShaderInfoLog(handle)}");

            return handle;
        }
    }
}