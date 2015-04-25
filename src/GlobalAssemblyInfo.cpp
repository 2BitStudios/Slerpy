using namespace System;
using namespace System::Reflection;

[assembly:AssemblyProduct(L"Slerpy")];

[assembly:AssemblyCompany(L"2Bit Studios")];
[assembly:AssemblyCopyright(L"Copyright ©  2015")];
[assembly:AssemblyTrademark(L"")];

[assembly:CLSCompliantAttribute(true)];

#if _DEBUG
[assembly:AssemblyConfiguration(L"Debug")];
#else
[assembly:AssemblyConfiguration(L"Release")];
#endif
