using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKCube
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _front;
        private Vector3 _up;
        private float _pitch;
        private float _yaw;
        private float _moveSpeed;
        private float _rotateSpeed;

        public Camera(Vector3 position)
        {
            _position = position;
            _front = -Vector3.UnitZ;  // Mirando hacia -Z
            _up = Vector3.UnitY;      // Y es hacia arriba
            _pitch = 0f;
            _yaw = -90f;             // -90 para mirar hacia -Z
            _moveSpeed = 10.0f;       // Unidades por segundo
            _rotateSpeed = 60.0f;     // Grados por segundo
            UpdateVectors();
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _position + _front, _up);
        }

        public void HandleInput(KeyboardState keyboard, float deltaTime)
        {
            // Movimiento adelante/atrás
            if (keyboard.IsKeyDown(Keys.Up))
                _position += _front * _moveSpeed * deltaTime;
            if (keyboard.IsKeyDown(Keys.Down))
                _position -= _front * _moveSpeed * deltaTime;

            // Movimiento izquierda/derecha
            if (keyboard.IsKeyDown(Keys.Left))
                _position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * _moveSpeed * deltaTime;
            if (keyboard.IsKeyDown(Keys.Right))
                _position += Vector3.Normalize(Vector3.Cross(_front, _up)) * _moveSpeed * deltaTime;

            // Movimiento arriba/abajo
            if (keyboard.IsKeyDown(Keys.PageUp))
                _position += _up * _moveSpeed * deltaTime;
            if (keyboard.IsKeyDown(Keys.PageDown))
                _position -= _up * _moveSpeed * deltaTime;

            // Rotación izquierda/derecha
            if (keyboard.IsKeyDown(Keys.A))
                _yaw -= _rotateSpeed * deltaTime;
            if (keyboard.IsKeyDown(Keys.D))
                _yaw += _rotateSpeed * deltaTime;

            // Rotación arriba/abajo
            if (keyboard.IsKeyDown(Keys.W))
                _pitch += _rotateSpeed * deltaTime;
            if (keyboard.IsKeyDown(Keys.S))
                _pitch -= _rotateSpeed * deltaTime;

            // Limitar pitch para evitar que la cámara se voltee
            _pitch = Math.Clamp(_pitch, -89f, 89f);

            UpdateVectors();
        }

        private void UpdateVectors()
        {
            // Calcular el nuevo vector frontal
            _front.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
            _front.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            _front.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));
            _front = Vector3.Normalize(_front);
        }
    }
}
