# JoystickMerger
Based on the work of shaise. https://github.com/shaise/JoystickMerger

Join physical joysticks to one logical. Good for games that do not support more then one joystick.

This project was forked specific for merging two T.16000M Thrustmaster into one input device and using Steam to assign the buttons and axis to an "Xbox controller".

It got extended with an editor/generator, which allows non-developers to design their feeder in an UI.
The generator creates an executable running the vJoy mapping designed by you.

The project is based on vJoy logical joystick driver (http://vjoystick.sourceforge.net/site/) and SharpDX - a c# wrapper for directx. (http://sharpdx.org/)

# Using the generator
1. Click on "Clone or download" and download the zip.
2. Unpack the whole zip (not bin only).
3. Download and install vJoy.
4. Enable a logical vJoy device #1 with all axis, 32 buttons and 4 continous POVs. (reboot may required)
5. Go to the bin folder and run "JoystickMerger.Generator.exe".

# Creating your first feeder
1. Try out the auto-created configuration by clicking on "Generate". Choose a senseful location for the executable. You will probably use it for your real feeder later on. It should build successful. If not, try to simplify the auto-created configuration and try again.
2. Run that executable. (Your anti-virus may wants to scan it. And will find nothing, of course.)
3. Start the "vJoy Monitor" to see if anything happens.
4. Go back to the generator and start your real setup by removing the auto-created configuration by clicking on the red X and add new mapping by clicking on the blue "+".
5. Save the configuration XML and stop the feeder before clicking an "Generate".
6. Have fun.


# For devs: Compilation of the generator:
1. Compile using Microsoft Visual Studio 2015 (Can use community edition)
2. copy vJoyInterface.dll from packages/x64 to the executable folder
