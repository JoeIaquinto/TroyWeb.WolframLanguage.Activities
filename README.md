# Wolfram Language Integration

Connect to the scientific, symbolic scripting language, Wolfram Language. Allows evaluation of complex, arbitrary expressions directly from workflows

# Description

The Wolfram Language + UiPath package allows connection from UiPath workflows to the Wolfram Language system. Enables users to perform advanced mathematical analysis, create rich visualisations, and open up the possibilities for deep integration between a symbolic language with immense capabilities and a sophisticated automation framework.

# What it does

This custom activity pack allows a user to launch or connect to a Wolfram Language kernel and interact as if they were working directly in the Wolfram Language REPL. The user can evaluate Wolfram Language expressions as strings or the Expression class provided with the WL .NET Interface. The results of this expression map back to .NET Types for use in the workflow. Expressions can also be evaluated to images, sound, or even custom .NET Objects.

There are also low level commands that allow interaction with the Wolfram Language on a packet level. The Wolfram Language / .NET interface uses Wolfram Symbolic Transfer Protocol to facilitate communication between the symbolic runtime of the WL kernel and the object-oriented .NET runtime. Direct manipulation of this protocol allows for complete and direct control of the Wolfram Language kernel.

The user may open a Wolfram Language kernel as a parent scope with evaluations inside, and the activity pack will take care of the setup and tear-down of the WL runtime process, or the user may open a Kernel and store it as a variable in their workflow for use later, closing it manually when required.

## Use Cases

The Wolfram Language has a tremendous range of domains it can interact with, including machine learning, image processing, natural language processing, scientific data processing, and much more. Examples that are provided with the project include: retrieving stock data and creating charts of historical prices, generating decay graphs of radioactive isotopes, performing queries to the online service Wolfram Alpha, and searching for the textual answer to a user's question in a knowledgebase article. The full documentation for the Wolfram Language is available online [here](https://reference.wolfram.com/language/)

## Setup

First, ensure you have the Wolfram Language system installed on your machine. This could either be from an existing Mathematica installation or by installing the [Free Wolfram Engine for Developers](https://www.wolfram.com/engine/), a non-production license to allow development on the Wolfram Language with a limited feature set.

Next, add the TroyWeb.WolframLanguage.Activities package to your project. Drag a Wolfram Language Scope activity into your sequence. The activity will attempt to locate the MathKernel.exe file in your Wolfram Language or Mathematica installation directory. If it is not found, provide this path to the parent scope activity to allow evaluations to be made. [Examples are also available on the project Github](https://github.com/JosephIaquinto/TroyWeb.WolframLanguage.Activities/tree/master/Examples)

## Activities

* Wolfram Language Scope
  * This is the parent activity that allows Evaluate and Packet Management activities to be used to communicate with an opened Kernel. Automatically handles starting and stopping the Wolfram kernel if one is not provided.
* Kernel category
  * Open Kernel
    * Opens a connection to the Wolfram Kernel and returns a kernel object that may be used in multiple Wolfram Language Scopes
  * Close Kernel
    * Closes an open kernel to free resources.
* Expression category
  * Create Expression
* Creates an Expr object of the provided Expression Type and name
  * Apply Expression
* Applies a provided Expr object to act on an array of arbitrary arguments and returns the resulting Expr object
* Evaluate category
  * Evaluate
* Evaluates a wolfram language expression provided as a string or Expr object as an Expr object
  * Evaluate to image
* Evaluates a wolfram language expression provided as a string or Expr object as an .NET Image object of provided height and width
  * Evaluate to input form
    * Evaluates a wolfram language expression provided as a string or Expr object as it would be entered as an ASCII string
  * Evaluate to output form
    * Evaluates a wolfram language expression provided as a string or Expr object as it would output as an ASCII string
* Low Level category
  * Packet Management category
    * New Packet
      * Indicate a new packet is starting on the communication buffer
    * End Packet
      * Indicate that the current packet on the communication buffer is closed
    * Flush Buffer
      * Push all buffered WSTP packets to the Wolfram Kernel
    * Wait for the next packet
      * Syncronously wait for a response packet to be received from the Wolfram Kernel
  * Get Data category
    * Get Expression
      * Get the expression at the front of the results queue
    * Get Value
      * Get the value at the front of the results queue
    * Peek Expression
      * Get the expression at the front of the results queue, but do not remove it
  * Put Data category
    * Put Argument Count
      * Add the count of arguments for the current function in the packet
    * Put Data
      * Add arbitrary data to the current packet
    * Put Function
      * Add a function call to the current packet
    * Put Next Expression Type
      * Indicate the type of the next expression that will be added to the packet
    * Put Reference
      * Insert a reference to a .NET object in order to send to the kernel. Mush have "Enable Object * References" wolfram language scope parameter enabled.
    * Put Symbol
      * Add a symbol to the current packet
    * Put Value
      * Add a value to the current packet

## Future Development

There are many processes you can use the Wolfram Language, but some basic workflows that many users would want are tedious to create in UiPath studio. I want to build a package of snippets / workflows to simplify standard processes such as changing the Wolfram Language's current directory, or exporting an evaluated object to the Wolfram Cloud. I would also like to connect the WL Kernel to a front-end so that the visualisations may be interacted with and used with standard UiPath activities.

## Benefits

Enables users to perform advanced mathematical analysis, create rich visualisations, and open up the possibilities for deep integration between a symbolic language with immense capabilities and a sophisticated automation framework. Provides full integration to Wolfram to extend UiPath capabilities into many different fields.

## Dependencies

The Wolfram Engine, available with Mathematica or the Free Wolfram Engine for Developers.
UiPath

## Compatibility

All UiPath versions (Tested on 2019.4+)
All Wolfram Language Versions from V4 and up.

NOTICE: I do not own the Wolfram Engine or any part therof. This was developed using the Free Wolfram Engine™ for Developers. You may not use this in production or for commercial use without purchasing a production wolfram engine license from the Wolfram Foundation.
