#pragma once
#include <pylon/PylonIncludes.h>
using namespace Pylon;
using namespace System;

namespace Corecamerawrapper {
	public ref class Camera 
	{
	public: void Test() {

	}

	public: Camera()
	{
		Console::WriteLine(".NET CORE Basler camera wrapper");
		PylonInitialize();
	}



		

	};
}
