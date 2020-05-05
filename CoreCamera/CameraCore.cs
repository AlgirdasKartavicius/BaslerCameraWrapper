using System;
using System.Runtime.InteropServices;
using Corecamerawrapper;

namespace CoreCamera
{
    public class CameraCore : ICameraCore
    {
        /// <summary>
        /// Camera instance
        /// </summary>
        private readonly Camera _camera;

        /// <summary>
        /// Frame arrived action
        /// </summary>
        private readonly Action<byte[], int, int> _imageAction;

        /// <summary>
        /// Is camera currently grabbing
        /// </summary>
        public bool IsGrabbing { get; set; }

        public CameraCore(string cameraName, Action<byte[], int, int> imageAction)
        {
            _camera = new Camera(cameraName);
            _camera.@event += Camera_event;
            _imageAction = imageAction;
        }

        /// <summary>
        /// Set camera exposure time
        /// </summary>
        /// <param name="exposure">exposure value</param>
        public void SetExposure(long exposure)
        {
            _camera.SetExposure(exposure);
        }

        /// <summary>
        /// Set camera gain parameter
        /// </summary>
        /// <param name="gain">gain value</param>
        public void SetGain(long gain)
        {
            _camera.SetGain(gain);
        }

        /// <summary>
        /// Set frame height
        /// </summary>
        /// <param name="height">height value</param>
        public void SetHeight(long height)
        {
            _camera.SetHeight(height);
        }

        /// <summary>
        /// Set frame width
        /// </summary>
        /// <param name="width">width value</param>
        public void SetWidth(long width)
        {
            _camera.SetWidth(width);
        }

        /// <summary>
        /// Start frames grabbing
        /// </summary>
        public void Grab()
        {
            IsGrabbing = true;
            _camera.Grab();
        }

        /// <summary>
        /// Stop frames grabbing
        /// </summary>
        public void StopGrab()
        {
            IsGrabbing = false;
            _camera.StopGrab();
        }

        /// <summary>
        /// Write current settings to camera device
        /// </summary>
        public void UpdateSettings()
        {
            _camera.UpdateSettings();
        }

        /// <summary>
        /// Camera arrived event
        /// </summary>
        /// <param name="ptr">pointer to image array</param>
        /// <param name="width">frame width</param>
        /// <param name="height">frame height</param>
        private void Camera_event(IntPtr ptr, int width, int height)
        {
            byte[] managedArray = new byte[width * height];
            Marshal.Copy(ptr, managedArray, 0, managedArray.Length);

            _imageAction.Invoke(managedArray, width, height);
        }

        /// <summary>
        /// Terminate basler pylon
        /// </summary>
        public void Terminate()
        {
            _camera.Terminate();
        }
    }
}