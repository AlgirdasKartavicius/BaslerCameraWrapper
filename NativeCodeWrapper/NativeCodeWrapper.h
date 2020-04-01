#pragma once
#include <pylon/PylonIncludes.h>
//using namespace Pylon;
//#include <iostream>
using namespace System;

namespace NativeCodeWrapper {
	public ref class Class1
	{

	public: void Test()
	{
		Pylon::PylonInitialize();

		Console::WriteLine("test");
	}
		  // TODO: Add your methods for this class here.
	};
}
