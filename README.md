# AVoxEclipse
*Hands On Voxel Modeling With The Leap Motion Controller*

A Vox Eclipse lets you design block creations in 3D space by combining hand gestures with traditional button controls. Use your favorite hand to rotate, re-position and highlight your model. Click away with your other hand and command the tools 
needed to build out your block designs.

## Features
* Drawing Tools: Push, pop, paint and bubble blocks.
* Symmetry Mode: Keep your constructions nice and even.
* Save and Load: Let your models stick around to edit again and again!
* Color Selection: Add some flare using only slightly less color options than found on a Commadore 64.
* Multiple Control Options: Keyboard, mouse, gamepad, lefty or righty. Its your call.

## How do I edit and play with this thing?

Rather than the standard project location, the bulk of the scripting for this project is found under the “VisualStudio” folder. Open the solution in that location with Visual Studio 2013 or greater to start editing scripts. A post build step on the solution will copy the assembled DLL and editor DLL resources over to the “Assets/ManagedCode” and “Assets/Editor/ManagedCode” locations in the unity game directory detailed below.

The “UnityGame” directory contains the root of the project for editing in Unity3D. Open this location in Unity 5 or greater to start editing the core game.

Custom assets specific to the project can be found under these locations:
* “Assets/SSG/SSGVoxPuz” 
* “Assets/Editor/SSG/SSGVoxPuz”. 

Everything else is more or less imported from the asset store or another custom unity project.


## Can you just show me the fancy leap motion handy wavy code?

From Unity, open up the scene “VoxPuzDemo”. From here take a look at the “puzzle-controls” game object. This is the root of all of the game's heavy lifting. Here is a bief rundown on each of those controllers. Ones particualarly Leap Motion “centric” have been hilighted.
* **hint-controller:** That nifty hint box at the top of the screen
* **tool-controller:**  Logic around the currently selected user “tool” (Pencil, Brush, etc…)
* **menu-system:** the root of menu structure. Open up the “menu-controller” prefab for even more info
* **(LEAP!)pivot-controller:** moving the model around the screen
* **(LEAP!)rotation-controller:** rotating the model
* **(LEAP!)selection-controller:** All logic around the current block selection
* **interaction-global:** delegation logic for all “interactors” (pivot, rotation, selection, zoom…) 
* **zoom-controller:** zooming the camera in and out
* **persistance-controller:** saving / loading the modeling
* **modification-controller:** wrapper that handles all modifications to the model
* **modeling-controller:** logic around what blocks can be and are used for modification


## Why is this thing set up like this?

For more information on the modularization tool used to separate out this project, check out: 
[github.com/mdm373/UnityVSModuleBuilder/](github.com/mdm373/UnityVSModuleBuilder/)
