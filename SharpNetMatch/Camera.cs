
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    public class Camera
    {
        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        //static public Vector2 Location = Vector2.Zero;
        private SharpNetMatch _nm;
        public Camera(SharpNetMatch nm)
        {
            _zoom = 1.0f;
            Rotation = 0.0f;
            Pos = Vector2.Zero;
            _nm = nm;
        }

        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation { get; set; }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            Pos += amount;
        }
        // Get set position
        public Vector2 Pos { get; set; }
        public Matrix Transformation
        {
            get
            {
                _transform =       // Thanks to o KB o for this solution
                  Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                                             Matrix.CreateRotationZ(Rotation) *
                                             Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                             Matrix.CreateTranslation(new Vector3(_nm.GraphicsDevice.Viewport.Width * 0.5f, _nm.GraphicsDevice.Viewport.Height * 0.5f, 0));
                return _transform;
            }
        }

        //Offsets any cam location by a zoom scaled window bounds
        public Vector2 CamCenterOffset
        {
            get
            {
                return new Vector2((_nm.Window.ClientBounds.Height / Zoom)
                    * 0.5f, (_nm.Window.ClientBounds.Width / Zoom) * 0.5f);
            }
        }

        //Scales the mouse.X and mouse.Y by the same Zoom as everything.
        public Vector2 MouseCursorInWorld
        {
            get
            {
                return ScreenPosInWorld(new Vector2(_nm.mouseState.X, _nm.mouseState.Y));
            }
        }

        public Vector2 ScreenPosInWorld(Vector2 sPos)
        {
            sPos = new Vector2((sPos.X - _nm.Window.ClientBounds.Width / 2) / Zoom,
    (sPos.Y - _nm.Window.ClientBounds.Height / 2) / Zoom);
            sPos += Pos;
            Vector2.Transform(sPos, Transformation);
            return sPos;
        }

    }
}
