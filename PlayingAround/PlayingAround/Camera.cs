using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PlayingAround
{
    public class Camera
    {
        private Matrix view;
        private Matrix projection;
        public float leftrightRot;
        public float updownRot;
        private float rotationSpeed = 0.005f;
        private float sensibility = 0.5f;
        private float moveSpeed = 1f;
        private float fieldOdView = MathHelper.PiOver4;
        private float aspectRatio = 4f / 3f;
        private float nearPlane = 1;
        private float farPlane = 1000;
        public Vector3 position;
        private Vector3 target;
        public Vector3 up;
        int width;
        int height;

        private MouseState originalMouseState;


        public Camera(Viewport viewport, Vector3 position, Vector3 up, float updownAngle = 0, float leftrightAngle = 0)
        {
            this.position = position;
            this.leftrightRot = leftrightAngle;
            this.updownRot = updownAngle;
            this.up = up;
            this.width = viewport.Width;
            this.height = viewport.Height;

            Mouse.SetPosition(viewport.Width / 2, viewport.Height / 2);
            originalMouseState = Mouse.GetState();

            projection = Matrix.CreatePerspectiveFieldOfView(fieldOdView, aspectRatio, nearPlane, farPlane);
            UpdateViewMatrix();
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();                

            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;
                leftrightRot -= rotationSpeed * xDifference;
                updownRot -= rotationSpeed * yDifference;
                Mouse.SetPosition(width / 2, height / 2);
                UpdateViewMatrix();
                currentMouseState = originalMouseState;
            }

            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))      //Forward
            {
                AddToCameraPosition(new Vector3(0, 0, -sensibility));

            }
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))    //Backward
            {
                AddToCameraPosition(new Vector3(0, 0, sensibility));

            }
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))   //Right
            {
                AddToCameraPosition(new Vector3(sensibility, 0, 0));

            }
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))    //Left
            {
                AddToCameraPosition(new Vector3(-sensibility, 0, 0));

            }
            if (keyState.IsKeyDown(Keys.Q))                                     //Up
            {
                AddToCameraPosition(new Vector3(0, sensibility, 0));

            }
            if (keyState.IsKeyDown(Keys.Z))                                     //Down
            {
                AddToCameraPosition(new Vector3(0, -sensibility, 0));

            }
        }

        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            target = position + cameraRotatedTarget;
            up = Vector3.Transform(cameraOriginalUpVector, cameraRotation);
            view = Matrix.CreateLookAt(position, target, up);
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            position += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }

        public Matrix View
        {
            get { return view; }
        }        

        public Matrix Projection
        {
            get { return projection; }
        }

        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        public float Sensibility
        {
            get { return sensibility; }
            set { sensibility = value; }
        }

        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }
    }
}
