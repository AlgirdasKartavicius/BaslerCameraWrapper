using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreCamera;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace BlazorAppCamera.Data
{
    public class CameraService

    {
        private ICameraCore _camera;
        private Action<string> _action;
        private int _cnt = 0;
        private int _number = 0;

        public CameraService()
        {
            _camera = new CameraCore("Front1", ImageArrived);
        }

        public void ImageArrived(byte[] bytes, int height, int width)
        {
            _cnt++;
            if (_cnt > _number) return;
            var path = "example.jpg";
            Mat img = new Mat(height, width, DepthType.Cv8U, 1);
            img.SetTo(bytes);
            CvInvoke.Imwrite(path, img);

            _action.Invoke(path);
        }

        public void StartGrab(Action<string> action, int number)
        {
            _action = action;
            _number = number;
            _camera.SetExposure(50000);
            _camera.SetHeight(1000);
            _camera.SetWidth(1000);
            //_camera.SetGain(200);

            _camera.UpdateSettings();
            _camera.Grab();
        }
    }
}