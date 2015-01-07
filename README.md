This mod allows to send chat messages and notifications from Programmable Blocks.

Simply place an Antenna on the same grid with your Programmable block, then add this to the beginning of your script:
```c#
const string TRANSMISSION_BEGIN_MARKER = ".~:/{{["; 
const string MESSAGE_MARKER = "MESG:";
const string NOTIFICATION_MARKER = "NOTF:";
void MSG(string text, bool notification = false)
{
    var antennas = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(antennas);
    if (antennas.Count == 0) return;
    IMyRadioAntenna antenna = antennas[0] as IMyRadioAntenna;
    string antennaName = antenna.CustomName;
    int markerIndex = antennaName.IndexOf(TRANSMISSION_BEGIN_MARKER);
    if (markerIndex > 0) {
        antennaName = antennaName.Substring(0, markerIndex);
    }
    antenna.SetCustomName(antennaName + TRANSMISSION_BEGIN_MARKER +
                          (notification ? NOTIFICATION_MARKER : MESSAGE_MARKER) + text);
}
```
Then use it like this to send chat message:
```c#
MSG("Hello world!");
```
or like this to send notification:
```c#
MSG("This will appear as notification", true);
```

Feel free to use this code any way you want, it's under "I don't care" license.
