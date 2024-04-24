# Welcome to Lithium
Lithium - is a simple logger library for your .NET project.
## Install
### NuGet
You can install Lithium library from NuGet using command:

    dotnet add package Link.Lithium --version 1.0.2

## Usage example
Below this article you can find usage example for Lithium logger

    using Lithium;  
    using Lithium.Enums;  
      
    Log.Start(Level.TRACE);  
      
    Log.T("TRACE MESSAGE");  
    Log.D("DEBUG MESSAGE");  
    Log.I("INFO MESSAGE");  
    Log.W("WARNING MESSAGE");  
    Log.E("ERROR MESSAGE");  
      
    Log.Stop();
