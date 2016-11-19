Cuke4Unity
==========

Cuke4Unity is a tool that lets you do Behaviour-Driven Development (BDD) in Unity. It is still very early in its development.

Prerequisites
-------------

Cuke4Unity is being developed using Unity 5.4, but ought to work with similar versions of Unity.

You will need to install Ruby and the cucumber 'gem' in order to be able to use Cuke4Unity.


Sample Invocation
-----------------

The calculator sample works (although its readme needs to be updated). To use it:

- Run the project (in Editor, on your computer, or on a device with working network sockets)
- Edit the Assets/Cuke4Nuke/Examples/Calculator/features/step_definitons/cucumber.wire file to list the IP of the device (or 'localhost' if you are running it on the same computer)
- open a command prompt and change to the "Assets/Cuke4Nuke/Examples/Calculator"
- run 'cucumber'
- tada! You can test what you are doing.

Third-Party Projects
--------------------

Cuke4Unity is a port/fork of the now-retired Cuke4Nuke, which was a project to speak to Cucumber using the 'Wire Protocol'.

Cuke4Unity includes a Unity-version of the Newtonsoft JSON parser.

It also included JsonLit (but this is only used in Unit Tests for Cuke4Unity and can be removed).

It includes Unity's Test Tools so that you can run Unit Tests.

It includes a Unity Fluent Assertion library, so that you can write very readable tests, and because I could find no way to use NUnit on devices in Unity 5.3 or newer.
