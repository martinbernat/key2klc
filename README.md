# Slovak (Apple) - Parallels.klc
This custom Slovak keyboard layout provides full compatibility with Apple keyboard layouts when used on Windows PCs or within Parallels virtual environments.

# key2klc
Windows keyboard layout extractor - creates .klc files from all layouts in the system.

## Installing Slovak (Apple) Keyboard Layout
- Download and install the **Microsoft Keyboard Layout Creator** (MKLC)
- Load the Slovak .klc file from this repo into MKLC (File → Load Source File)
- From the menu, select Project → Build DLL and Setup Package to generate and install the custom layout

## Creating a Custom Keyboard Layout (based on existing Windows layouts)
- Download and install the Microsoft Keyboard Layout Creator (MKLC)
- Open the **key2klc** project in Visual Studio
- (Optional) Modify the C# source code to export only the layouts you are interested in by adjusting the "name" variable
- Build the **key2klc** project
- Run key2klc.exe from the command line to export .klc files from the system
- Open the exported .klc file in MKLC (File → Load Source File)
- Adjust the layout as needed
- Build and install the layout via Project → Build DLL and Setup Package
