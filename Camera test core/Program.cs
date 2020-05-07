using System;
using CoreCamera;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Camera_test_core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Program p = new Program();

            p.Start();

            Console.ReadLine();
        }

        private ICameraCore _camera;

        public void Start()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            _camera = new CameraCore("Front1", ImageArrived);
            _camera.SetExposure(10000);
            _camera.SetHeight(1000);
            _camera.SetWidth(1000);
            _camera.SetGain(200);

            _camera.UpdateSettings();
            _camera.Grab();
        }

        public void ImageArrived(byte[] bytes, int height, int width)
        {
            Mat img = new Mat(height, width, DepthType.Cv8U, 1);
            img.SetTo(bytes);
            Console.WriteLine("Saving test image");
            CvInvoke.Imwrite("test.jpg", img);
            _camera.StopGrab();
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (_camera.IsGrabbing)
            {
                _camera.StopGrab();
            }
            _camera.Terminate();

            Console.WriteLine("exit");
        }
    }
}