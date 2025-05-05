using System.Reflection;
using System.Resources;
using System.Windows;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]


[assembly: AssemblyCompany(".NET Foundation")]
[assembly: AssemblyCopyright("Copyright (c) .NET Foundation and contributors. All rights reserved.")]
[assembly: AssemblyProduct("EyeDropper")]

#if DEBUG
    [assembly: AssemblyConfiguration("DEBUG")]
#else
    [assembly: AssemblyConfiguration("")]
#endif
[assembly: NeutralResourcesLanguage("en-US")]