<!-- <p align="center">
  <img width="100" alt="osu! logo" src="assets/lazer.png">
</p>
-->

# tatakae!

A fork of the [osu!(lazer)][osu!] game client specifically for arcade-style usage. 

Not affiliated with ppy Pty Ltd. or *osu!*.

The original README.md file distributed with osu!(lazer) can be found at [ORIGINAL_README.md](./ORIGINAL_README.md).

Contact relating to tatakae!: <me@lucyfaria.net>

[osu!]: https://osu.ppy.sh

## What is this?

tatakae! is:

- [x] a modified version of osu!(lazer), meant for kiosk/arcade/convention use
- [x] a hobby project maintained by a single person

tatakae! is NOT:

- [ ] a commercial product
- [ ] a replacement for osu!(lazer) (unless you're using this at an event)
- [ ] AFFILIATED WITH osu! OR ppy Pty Ltd.
- [ ] ENDORSED or APPROVED by ppy Pty Ltd. or peppy

## Licence and Legal

*tatakae!* (as of now, July 7, 2026) and its modifications to software is licensed under the MIT license, as is base *osu!*(lazer).

*tatakae!* uses osu-resources which is under a CC-BY-NC 4.0 license. 

If you are to use this fork at an event, contact peppy <pe@ppy.sh> beforehand as using the *osu!* logo and trademark without permission is illegal.

### Original licensing text

*osu!*'s code and framework are licensed under the [MIT licence](https://opensource.org/licenses/MIT). Please see [the licence file](LICENCE) for more information. [tl;dr](https://tldrlegal.com/license/mit-license) you can do whatever you want as long as you include the original copyright and license notice in any copy of the software/source.

Please note that this *does not cover* the usage of the "osu!" or "ppy" branding in any software, resources, advertising or promotion, as this is protected by trademark law.

Please also note that game resources are covered by a separate licence. Please see the [ppy/osu-resources](https://github.com/ppy/osu-resources) repository for clarifications.

## Contributions

tatakae! is a personal project of mine. I am aiming to get something ready for Anirevo in Nov. 2026 :)

If you would like to contribute, feel free! Please note that I have no obligation to accept outside contributions and this is mainly a pet project of mine. So best case just reach out to me (Lucy Faria) via opening a GitHub discussion or emailing me beforehand before working on something so that your efforts are not wasted.

No AI was used and will ever be used for this. Have fun programming.

## Running osu! (copied from base osu!)

If you are just looking to give the game a whirl, you can grab the latest release for your platform:

### Latest release:

| [Windows 10+ (x64)](https://github.com/ppy/osu/releases/latest/download/install.exe) | macOS 12+ ([Intel](https://github.com/ppy/osu/releases/latest/download/osu.app.Intel.zip), [Apple Silicon](https://github.com/ppy/osu/releases/latest/download/osu.app.Apple.Silicon.zip)) | [Linux (x64)](https://github.com/ppy/osu/releases/latest/download/osu.AppImage) | [iOS 13.4+](https://osu.ppy.sh/home/testflight) | [Android 5+](https://github.com/ppy/osu/releases/latest/download/sh.ppy.osulazer.apk) |
|--------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| ------------- | ------------- | ------------- |

<!-- You can also generally download a version for your current device from the [osu! site](https://osu.ppy.sh/home/download). -->

If your platform is unsupported or not listed above, there is still a chance you can run the release or manually build it by following the instructions below.

## Developing osu! (copied from base osu!)

### Prerequisites

Please make sure you have the following prerequisites:

- A desktop platform with the [.NET 8.0 SDK](https://dotnet.microsoft.com/download) installed.

When working with the codebase, we recommend using an IDE with intelligent code completion and syntax highlighting, such as the latest version of [Visual Studio](https://visualstudio.microsoft.com/vs/), [JetBrains Rider](https://www.jetbrains.com/rider/), or [Visual Studio Code](https://code.visualstudio.com/) with the [EditorConfig](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig) and [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) plugin installed.

### Downloading the source code

Clone the repository:

```shell
git clone https://github.com/ppy/osu
cd osu
```

To update the source code to the latest commit, run the following command inside the `osu` directory:

```shell
git pull
```

### Building

#### From an IDE

You should load the solution via one of the platform-specific `.slnf` files, rather than the main `.sln`. This will reduce dependencies and hide platforms that you don't care about. Valid `.slnf` files are:

- `osu.Desktop.slnf` (most common)
- `osu.Android.slnf`
- `osu.iOS.slnf`

Run configurations for the recommended IDEs (listed above) are included. You should use the provided Build/Run functionality of your IDE to get things going. When testing or building new components, it's highly encouraged you use the `osu! (Tests)` project/configuration. More information on this is provided [below](#contributing).

To build for mobile platforms, you will likely need to run `sudo dotnet workload restore` if you haven't done so previously. This will install Android/iOS tooling required to complete the build.

#### From CLI

You can also build and run *osu!* from the command-line with a single command:

```shell
dotnet run --project osu.Desktop
```

When running locally to do any kind of performance testing, make sure to add `-c Release` to the build command, as the overhead of running with the default `Debug` configuration can be large (especially when testing with local framework modifications as below).

If the build fails, try to restore NuGet packages with `dotnet restore`.

### Testing with resource/framework modifications

Sometimes it may be necessary to cross-test changes in [osu-resources](https://github.com/ppy/osu-resources) or [osu-framework](https://github.com/ppy/osu-framework). This can be quickly achieved using included commands:

Windows:

```ps
UseLocalFramework.ps1
UseLocalResources.ps1
```

macOS / Linux:

```ps
UseLocalFramework.sh
UseLocalResources.sh
```

Note that these commands assume you have the relevant project(s) checked out in adjacent directories:

```
|- osu            // this repository
|- osu-framework
|- osu-resources
```

### Code analysis

Before committing your code, please run a code formatter. This can be achieved by running `dotnet format` in the command line, or using the `Format code` command in your IDE.

We have adopted some cross-platform, compiler integrated analyzers. They can provide warnings when you are editing, building inside IDE or from command line, as-if they are provided by the compiler itself.

JetBrains ReSharper InspectCode is also used for wider rule sets. You can run it from PowerShell with `.\InspectCode.ps1`. Alternatively, you can install ReSharper or use Rider to get inline support in your IDE of choice.

