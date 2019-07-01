# TroyWeb.WolframLanguage.Activities

A set of custom UIPath activities to allow integration between RPA workflows and the Wolfram Language Engine using .NET/Link.

Example UiPath projects can be found in the Examples directory.

## The Sky Is The Limit

This may be a trivial example, but the .NET/Link interface essentially extends the Wolfram Language to .NET, as well as extending .NET to utilize the Wolfram Language system. This project is merely providing an interface from the UIPath RPA workflow system to .NET/Link so that the wolfram language may be used freely from UIPath. Anything you can do in Wolfram Language is now available inside your UIPath Robotic Workflows!

## Installation

After installing the [Wolfram Engine](https://www.wolfram.com/engine/), install the WolframLanguge.Activities NuGet package in UiPath Studio. You must import the Wolfram.NETLink namespace into the workflow you intend to use these activities. Add a parent scope activity to your workflow and provide a path to MathKernel.exe in your Wolfram Engine install location. From there, you may evaluate expressions as if you were using the Wolfram Engine directly. Have fun!

## Activities

### Wolfram Language Scope

Opens a wolfram engine kernel (MathKernel.exe). Requires path, optionally an argv style string array of arguments to open the kernel with.

### Evaluate <T>

Evaluates a string as a Wolfram Language expression in the parent scope's kernel.

### Evaluate To Image

### Evaluate To Input Form

### Evaluate To Output Form

See the Wolfram Language and .NET/Link documentation for more information. You need an installed copy of the Wolfram Engine on the machine where the robot will be running. Future work may include connection to a remote wolfram engine kernel.

NOTICE: I do not own the Wolfram Engine or any part therof. This was developed using the [Free Wolfram Engine™ for Developers](https://www.wolfram.com/legal/terms/wolfram-engine.html). You may not use this in production or for commercial use without purchasing a production wolfram engine license from the Wolfram Foundation.
