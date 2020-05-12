# JoystickMerger
Based on the work of shaise. https://github.com/shaise/JoystickMerger

Join two physical joysticks to one logical. Good for games that do not support more then one joystick.

This project was forked specific for merging two T.16000M Thrustmaster into one input device and using Steam to assign the buttons and axes to an Xbox controller.

The project is based on vJoy logical joystick driver (http://vjoystick.sourceforge.net/site/) and SharpDX - a c# wrapper for directx. (http://sharpdx.org/)

# Compilation:
1. Compile using Microsoft Visual Studio 2015 (Can use community edition)
2. copy vJoyInterface.dll from packages/x86 or packages/x64 to the executable folder
