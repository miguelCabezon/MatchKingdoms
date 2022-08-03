# es.jacksparrot.dotween unity package

## Installation using the Unity Package Manager (Unity 2019.3+)
1. Open the Package Manager Window. 
2. Click on the + icon on the top left.
3. Select "Add package from git URL...".
4. In the imput field enter this: "https://github.com/JackSParrot/dotween-pkg.git"
5. Click on add and wait while the package is downloaded and imported.

## Installation using the Unity Package Manager (Unity 2018.1+)

The [Unity Package Manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html) (UPM) is a new method to manage external packages. It keeps package contents separate from your main project files.

1. Modify your project's `Packages/manifest.json` file adding this line:

    ```json
    "es.jacksparrot.json": "https://github.com/JackSParrot/dotween-pkg.git"
    ```

    Make sure it's still a valid JSON file. For example:

    ```json
    {
        "dependencies": {
            "com.unity.package-manager-ui": "2.2.0",
            "es.jacksparrot.json": "https://github.com/JackSParrot/dotween-pkg.git"
        }
    }
    ```

2. To update the package you need to delete the package lock entry in the `lock` section in `Packages/manifest.json`. The entry to delete could look like this:

    ```json
    "es.jacksparrot.dotween": {
      "hash": "a7ffd9287ac3c0ce1c68204873d24e540b88940d",
      "revision": "HEAD"
    }
    ```
# DISCLAIMER
## This package code does not belong to me, here is the author's README. I am just offering it as a package.

DOTween and DOTween Pro are copyright (c) 2014-2018 Daniele Giardini - Demigiant

// IMPORTANT!!! /////////////////////////////////////////////
// Upgrading DOTween from versions older than 1.2.000 ///////
// (or DOTween Pro older than 1.0.000) //////////////////////
-------------------------------------------------------------
If you're upgrading your project from a version of DOTween older than 1.2.000 (or DOTween Pro older than 1.0.000) please follow these instructions carefully.
1) Import the new version in the same folder as the previous one, overwriting old files. A lot of errors will appear but don't worry
2) Close and reopen Unity (and your project). This is fundamental: skipping this step will cause a bloodbath
3) Open DOTween's Utility Panel (Tools > Demigiant > DOTween Utility Panel) if it doesn't open automatically, then press "Setup DOTween...": this will run the upgrade setup
4) From the Add/Remove Modules panel that opens, activate/deactivate Modules for Unity systems and for external assets (Pro version only)

// GET STARTED //////////////////////////////////////////////

- After importing a new DOTween update, select DOTween's Utility Panel from the "Tools/Demigiant" menu (if it doesn't open automatically) and press the "Setup DOTween..." button to activate/deactivate Modules. You can also access a Preferences Tab from there to choose default settings for DOTween.
- In your code, add "using DG.Tweening" to each class where you want to use DOTween.
- You're ready to tween. Check out the links below for full documentation and license info.


// LINKS ///////////////////////////////////////////////////////

DOTween website (documentation, examples, etc): http://dotween.demigiant.com
DOTween license: http://dotween.demigiant.com/license.php
DOTween repository (Google Code): https://code.google.com/p/dotween/
Demigiant website (documentation, examples, etc): http://www.demigiant.com

// NOTES //////////////////////////////////////////////////////

- DOTween's Utility Panel can be found under "Tools > Demigiant > DOTween Utility Panel" and also contains other useful options, plus a tab to set DOTween's preferences