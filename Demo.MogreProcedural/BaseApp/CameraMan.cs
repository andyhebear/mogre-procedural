
using Mogre;
using System;


namespace Mogre_Procedural.Game.BaseApp
{
    public class CameraMan
    {
        private Camera mCamera;
        private bool mGoingForward;
        private bool mGoingBack;
        private bool mGoingRight;
        private bool mGoingLeft;
        private bool mGoingUp;
        private bool mGoingDown;
        private bool mFastMove;
        private bool mFreeze;

        public Camera Camera { get { return this.mCamera; } }
        public bool GoingForward { set { mGoingForward = value; } get { return mGoingForward; } }
        public bool GoingBack { set { mGoingBack = value; } get { return mGoingBack; } }
        public bool GoingLeft { set { mGoingLeft = value; } get { return mGoingLeft; } }
        public bool GoingRight { set { mGoingRight = value; } get { return mGoingRight; } }
        public bool GoingUp { set { mGoingUp = value; } get { return mGoingUp; } }
        public bool GoingDown { set { mGoingDown = value; } get { return mGoingDown; } }
        public bool FastMove { set { mFastMove = value; } get { return mFastMove; } }
        public bool Freeze { set { mFreeze = value; } get { return mFreeze; } }

        public CameraMan(Camera camera) { this.mCamera = camera; }
        public void UpdateCamera(float frameTime) {
            UpdateCamera(frameTime, null);
        }
        public void UpdateCamera(float frameTime, MoisManager input)
        {

            if (input != null) { this.UpdateKeys(input); }
            if (this.mFreeze) { return; }

            var move = Vector3.ZERO;
            if (this.mGoingForward) move += this.mCamera.Direction;
            if (this.mGoingBack) move -= this.mCamera.Direction;
            if (this.mGoingRight) move += this.mCamera.Right;
            if (this.mGoingLeft) move -= this.mCamera.Right;
            if (this.mGoingUp) move += this.mCamera.Up;
            if (this.mGoingDown) move -= this.mCamera.Up;

            move.Normalise();

            move *= 100;
            if (mFastMove) { move *= 6; } // With shift button pressed, move twice as fast.
            if (move != Vector3.ZERO) { mCamera.Move(move * frameTime); }
        }

        private void UpdateKeys(MoisManager input)
        {
            if (input.IsKeyDown(MOIS.KeyCode.KC_W) || input.IsKeyDown(MOIS.KeyCode.KC_UP)) {
                this.GoingForward = true; 
            }
            else { this.GoingForward = false; }
            if (input.IsKeyDown(MOIS.KeyCode.KC_S) || input.IsKeyDown(MOIS.KeyCode.KC_DOWN)) { this.GoingBack = true; }
            else { this.GoingBack = false; }
            if (input.IsKeyDown(MOIS.KeyCode.KC_A) || input.IsKeyDown(MOIS.KeyCode.KC_LEFT)) { this.GoingLeft = true; }
            else { this.GoingLeft = false; }
            if (input.IsKeyDown(MOIS.KeyCode.KC_D) || input.IsKeyDown(MOIS.KeyCode.KC_RIGHT)) { this.GoingRight = true; }
            else { this.GoingRight = false; }
            if (input.IsKeyDown(MOIS.KeyCode.KC_E) || input.IsKeyDown(MOIS.KeyCode.KC_PGUP)) { this.GoingUp = true; }
            else { this.GoingUp = false; }
            if (input.IsKeyDown(MOIS.KeyCode.KC_Q) || input.IsKeyDown(MOIS.KeyCode.KC_PGDOWN)) { this.GoingDown = true; }
            else { this.GoingDown = false; }
            if (input.IsKeyDown(MOIS.KeyCode.KC_LSHIFT) || input.IsKeyDown(MOIS.KeyCode.KC_RSHIFT)) { this.FastMove = true; }
            else { this.FastMove = false; }

            this.MouseMovement(input.MouseMoveX, input.MouseMoveY);
        }

        public void MouseMovement(int x, int y)
        {
            if (mFreeze) { return; }
            this.mCamera.Yaw(new Degree(-x * 0.15f));
            this.mCamera.Pitch(new Degree(-y * 0.15f));
        }
    }
}