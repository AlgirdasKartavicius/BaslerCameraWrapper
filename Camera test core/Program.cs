using System;
using System.Runtime.InteropServices;
using Corecamerawrapper;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Camera_test_core
{
    internal class Program
    {
        private Camera _camera;
        private int _numberOfFrames = 0;

        private static void Main(string[] args)
        {
            Mat a = Mat.Zeros(10, 10, DepthType.Cv8U, 3);
            Program p = new Program();

            p.Start();

            Console.ReadLine();
        }

        public void Start()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            _camera = new Camera("Front1");
            _camera.SetExposure(10000);
            _camera.@event += Camera_event2;

            _camera.UpdateSettings();
            _camera.Grab();

            //camera.TestHandler += Camera_TestHandler;
        }

        private void Camera_event2(IntPtr ptr, int width, int height)
        {
            byte[] managedArray = new byte[width * height];
            Marshal.Copy(ptr, managedArray, 0, managedArray.Length);
            Mat img = new Mat(height, width, DepthType.Cv8U, 1);
            img.SetTo(managedArray);
            CvInvoke.Imwrite("core.jpg", img);
            _numberOfFrames++;
            if (_numberOfFrames == 5)
            {
                _camera.StopGrab();
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _camera.Terminate();

            Console.WriteLine("exit");
        }
    }
}