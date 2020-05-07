using System;
using CoreCamera;

namespace BlazorAppCamera.Data
{
    public class CameraService

    {
        private readonly ICameraCore _camera;
        private Action<byte[]> _action;

        public CameraService()
        {
            _camera = new CameraCore("Front1", ImageArrived);
        }

        public void ImageArrived(byte[] bytes, int height, int width)
        {
            _action.Invoke(bytes);
        }

        public void StartGrab(Action<byte[]> action)
        {
            _action = action;
            _camera.SetExposure(50000);
            _camera.SetHeight(1000);
            _camera.SetWidth(1000);
            _camera.UpdateSettings();
            _camera.Grab();
        }
    }
}