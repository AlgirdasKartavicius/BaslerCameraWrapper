#pragma once
#include <pylon/PylonIncludes.h>
// Include files used by samples.
#include "../include/ConfigurationEventPrinter.h"
#include "../include/ImageEventPrinter.h"
// Settings to use Basler GigE cameras.
#include <pylon/gige/BaslerGigEInstantCamera.h>
typedef Pylon::CBaslerGigEInstantCamera Camera_t;
#include <pylon/gige/BaslerGigECamera.h>
#include <string>
#include <iostream>
using namespace Pylon;
using namespace System;
using namespace std;

using namespace System::Runtime::InteropServices;
namespace Corecamerawrapper {
	// Number of images to be grabbed.
	static const uint32_t c_countOfImagesToGrab = 12;
	public delegate void OnFrameArrived(IntPtr, Int32, Int32);

	IPylonDevice* pdevice;

	public ref class Camera
	{
	public:event OnFrameArrived^ event;

		  INT64 Exposure;
		  String^ CameraName;

		  void OnFrame(IntPtr i, Int32 width, Int32 height) {
			  event(i, width, height);
		  }

	public: Camera(String^ cameraName)
	{
		PylonInitialize();

		PylonAutoInitTerm autoInitTerm;
		CDeviceInfo info;
		CameraName = cameraName;

		info.SetUserDefinedName(GetCameraName());

		CTlFactory& TlFactory = CTlFactory::GetInstance();
		ITransportLayer* pTl
			= TlFactory.CreateTl(CBaslerGigECamera::DeviceClass());
		DeviceInfoList_t lstDevices;
		lstDevices.push_back(info);

		pTl->EnumerateDevices(lstDevices);
		if (lstDevices.empty()) {
			cerr << "No devices found" << endl;
			exit(1);
		}
		pdevice = pTl->CreateDevice(lstDevices[0]);

		Console::WriteLine(".NET CORE Basler camera wrapper");
		CGrabResultPtr ptrGrabResult;
	}
		  String_t GetCameraName() {
			  IntPtr ptrToNativeString = Marshal::StringToHGlobalAnsi(CameraName);
			  char* nativeString = static_cast<char*>(ptrToNativeString.ToPointer());

			  String_t name(nativeString);
			  return name;
		  }

	public: void Grab() {
		try
		{
			CInstantCamera camera(pdevice);

			// Create an instant camera object with the first found camera device matching the specified device class.
		//k

			//info.SetFriendlyName("Front");
			//info.SetDeviceClass(Camera_t::DeviceClass());

			//UpdateSettings(camera);*/

			// Print the model name of the camera.
			cout << "Using device " << camera.GetDeviceInfo().GetModelName() << endl;

			// The parameter MaxNumBuffer can be used to control the count of buffers
			// allocated for grabbing. The default value of this parameter is 10.
			camera.MaxNumBuffer = 10;

			// Start the grabbing of c_countOfImagesToGrab images.
			// The camera device is parameterized with a default configuration which
			// sets up free-running continuous acquisition.

			// This smart pointer will receive the grab result data.
			CGrabResultPtr ptrGrabResult;
			//Cimage fortmat converter
			CImageFormatConverter formatConverter;

			//Pylon image
			CPylonImage image;
			uint16_t cnt = 0;
			// Camera.StopGrabbing() is called automatically by the RetrieveResult() method
			//camera.StartGrabbing(GrabStrategy_OneByOne);
			camera.StartGrabbing(10);
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

	public: void StopGrab() {
		try
		{
			CInstantCamera camera(pdevice);
			camera.StopGrabbing();
		}
		catch (const GenericException& e)
		{
			cerr << "An exception occurred." << endl
				<< e.GetDescription() << endl;
		}
	}

	public: void Terminate() {
		PylonTerminate();
	}

	public: void UpdateSettings() {
		CDeviceInfo info;
		info.SetDeviceClass(Camera_t::DeviceClass());
		info.SetUserDefinedName(GetCameraName());
		// Create an instant camera object with the first found camera device that matches the specified device class.
		Camera_t camera(CTlFactory::GetInstance().CreateFirstDevice(info));
		camera.Open();
		camera.ExposureTimeRaw.SetValue(Exposure);
		camera.GainRaw.SetValue(camera.GainRaw.GetMax());
		camera.Close();
	}

	public: void SetExposure(INT64 exp) {
		Exposure = exp;
	}
	};
}
