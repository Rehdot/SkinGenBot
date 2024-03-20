**SkinGenBot** is a minecraft skin generator which is both a discord bot and console app.
On startup, you can choose whether you want it to run in console mode or bot mode.

**Console Mode**:
- Saves skin files to the build's working directory
- Supports the same features as bot mode

**Bot Mode**:
- Does not save skin files to your device
- Requires setup with bot token on first run of the build
- Supports all of the following commands

**Commands**:
- ?skin (hex)
- ?random
- ?splitgen (hex) (hex)
- ?splitgenrandom
- ?gradient (hex) (hex)
- ?gradientrandom

**Build Dependencies**:
- Requires all .png files in the Graphics folder to be EmbeddedResources
- Uses DisCatSharp (https://github.com/Aiko-IT-Systems/DisCatSharp)
