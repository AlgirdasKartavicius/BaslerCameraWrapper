#pragma once
#include <pylon/PylonIncludes.h>
// Include files used by samples.
#include "../include/ConfigurationEventPrinter.h"
#include "../include/ImageEventPrinter.h"

using namespace Pylon;
using namespace System;
using namespace std;

using namespace System::Runtime::InteropServices;
namespace Corecamerawrapper {

	// Number of images to be grabbed.
	static const uint32_t c_countOfImagesToGrab = 12;
	public delegate void OnFrameArrived(IntPtr, Int32, Int32);

	CInstantCamera camera;
	public ref class Camera
	{

	public:event OnFrameArrived^ event;


	public: void Test() {

	}


	public: Camera()
	{

		Console::WriteLine(".NET CORE Basler camera wrapper");
		PylonInitialize();
		CGrabResultPtr ptrGrabResult;

	}

	public: void Grab() {
		try
		{
			// Create an instant camera object with the camera device found first.
			CInstantCamera camera(CTlFactory::GetInstance().CreateFirstDevice());

			// Print the model name of the camera.
			cout << "Using device " << camera.GetDeviceInfo().GetModelName() << endl;

			// The parameter MaxNumBuffer can be used to control the count of buffers
			// allocated for grabbing. The default value of this parameter is 10.
			camera.MaxNumBuffer = 10;

			// Start the grabbing of c_countOfImagesToGrab images.
			// The camera device is parameterized with a default configuration which
			// sets up free-running continuous acquisition.
			camera.StartGrabbing(c_countOfImagesToGrab);

			// This smart pointer will receive the grab result data.
			CGrabResultPtr ptrGrabResult;
			//Cimage fortmat converter
			CImageFormatConverter formatConverter;

			//Pylon image
			CPylonImage image;
			uint16_t cnt = 0;
			// Camera.StopGrabbing() is called automatically by the RetrieveResult() method
			// when c_countOfImagesToGrab images have been retrieved.
			while (camera.IsGrabbing())
			{
				// Wait for an image and then retrieve it. A timeout of 5000 ms is used.
				camera.RetrieveResult(5000, ptrGrabResult, TimeoutHandling_ThrowException);
				cnt++;
				// Image grabbed successfully?
				if (ptrGrabResult->GrabSucceeded())
				{					// Access the image data.
					cout << "SizeX: " << ptrGrabResult->GetWidth() << endl;
					cout << "SizeY: " << ptrGrabResult->GetHeight() << endl;

					OnFrame((IntPtr)ptrGrabResult->GetBuffer(), (Int32)ptrGrabResult->GetWidth(), (Int32)ptrGrabResult->GetHeight());

				}
				else
				{
					cout << "Error: " << ptrGrabResult->GetErrorCode() << " " << ptrGrabResult->GetErrorDescription() << endl;
				}
			}
		}
		catch (const GenericException& e)
		{
			// Error handling.
			cerr << "An exception occurred." << endl
				<< e.GetDescription() << endl;

		}

		// Comment the following two lines to disable waiting on exit.
		cerr << endl << "Press Enter to exit." << endl;
		while (cin.get() != '\n');
	}

		  void OnFrame(IntPtr i, Int32 width, Int32 height) {

			  event(i, width, height);
		  }

	public: void Terminate() {
		PylonTerminate();
	}

	public: void SetExposure(int exp) {

	}





	};
}
