using System;
using System.Runtime.InteropServices;
using Corecamerawrapper;
using Emgu.CV;
using Emgu.CV.CvEnum;


namespace Camera_test_core
{
    class Program
    {
        private Camera camera;

        static void Main(string[] args)
        {



            Mat a = Mat.Zeros(10, 10, DepthType.Cv8U, 3);
            Program p = new Program();

            p.Start();

        }


        unsafe
            public void Start()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            camera = new Camera();
            camera.@event += Camera_event2;

            //https://inphamousdevelopment.wordpress.com/2012/10/01/sending-callbacks-from-c-to-c/
            //https://stackoverflow.com/questions/26547215/how-to-pass-c-sharp-method-as-a-callback-to-cli-c-function
            camera.Grab();

            //camera.TestHandler += Camera_TestHandler;


        }

        unsafe
            private void Camera_event2(void* A_0, uint width, uint height)
        {
            byte[] managedArray = new byte[width * height];
            IntPtr ptr = new IntPtr(A_0);
            //  Marshal.Copy(managedArray, 0, ptr, (int)(width * height));

            Marshal.Copy(ptr, managedArray, 0, managedArray.Length);
            Mat img = new Mat((int)height, (int)width, DepthType.Cv8U, 1);
            img.SetTo(managedArray);
            CvInvoke.Imwrite("core.jpg", img);


        }

        void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            camera.Terminate();

            Console.WriteLine("exit");
        }


    }
}

