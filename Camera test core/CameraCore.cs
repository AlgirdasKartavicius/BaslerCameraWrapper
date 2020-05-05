using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Corecamerawrapper;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Camera_test_core
{
    public class CameraCore
    {
        private Camera _camera;
        private readonly Action<Mat> _imageAction;

        public bool IsGrabbing { get; set; }

        public CameraCore(string cameraName, Action<Mat> imageAction)
        {
            _camera = new Camera(cameraName);
            _camera.@event += Camera_event;
            _imageAction = imageAction;
        }

        public void SetExposure(long exp)
        {
            _camera.SetExposure(exp);
        }

        public void SetGain(long gain)
        {
            _camera.SetGain(gain);
        }

        public void SetHeight(long height)
        {
            _camera.SetHeight(height);
        }

        public void SetWidth(long width)
        {
            _camera.SetWidth(width);
        }

        public void Grab()
        {
            IsGrabbing = true;
            _camera.Grab();
        }

        public void StopGrab()
        {
            IsGrabbing = false;
            _camera.StopGrab();
        }

        public void UpdateSettings()
        {
            _camera.UpdateSettings();
        }

        private void Camera_event(IntPtr ptr, int width, int height)
        {
            byte[] managedArray = new byte[width * height];
            Marshal.Copy(ptr, managedArray, 0, managedArray.Length);
            Mat img = new Mat(height, width, DepthType.Cv8U, 1);
            img.SetTo(managedArray);
            _imageAction.Invoke(img);
        }

        public void Terminate()
        {
            _camera.Terminate();
        }
    }
}