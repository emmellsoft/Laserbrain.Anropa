# Laserbrain.ANROPA
Use this library to build a simple REST based "remote procedure call" client-server application.

Your application will consist of three parts (preferably three projects, as in the provided demo):

1. COMMON: The interface(s) to your server method(s).
2. SERVER: The server code implementing the interfaces.
3. CLIENT: The client code calling the server via the interfaces.

The idea is that your client code should not need to implement anything at all, just hand over the IService interfaces to the Anropa-framework and then be able to call the methods on those interfaces.

# Usage
---
You will find three Visual Studio solutions in the src folder:
* Laserbrain.Anropa
* Laserbrain.Anropa.Demo.Client
* Laserbrain.Anropa.Demo.Server

The `Laserbrain.Anropa` is the library itself; this is the code you need in your own application.
The `Laserbrain.Anropa.Demo.Client` and `Laserbrain.Anropa.Demo.Server` implements a simple demo for you to try out the framework.
The demo is split in two solutions for easy debugging using two Visual Studio instances.

The `Laserbrain.Anropa.Demo.Server` keeps the `Laserbrain.Anropa.Demo.Common` project, also used by the `Client` project. It contains the common test service interfaces.

