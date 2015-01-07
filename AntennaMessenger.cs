using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sandbox.Common;
using Sandbox.Common.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;


namespace AntennaMessaging
{
  [MyEntityComponentDescriptor(typeof(MyObjectBuilder_RadioAntenna))]
  public class AntennaMessenger : MyGameLogicComponent
  {
    const string TRANSMISSION_BEGIN_MARKER = ".~:/{{[";
    const string MESSAGE_MARKER = "MESG:";
    const string NOTIFICATION_MARKER = "NOTF:";
    const string DEBUG_MARKER = "BCST:";

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      return null;
    }

    public override void Close()
    {
      (Entity as IMyRadioAntenna).CustomNameChanged -= antenna_CustomNameChanged;
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      (Entity as IMyRadioAntenna).CustomNameChanged += antenna_CustomNameChanged;
      Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    string lastMessage = null;
    public void antenna_CustomNameChanged(IMyTerminalBlock antenna)
    {
      int markerPosition = antenna.CustomName.IndexOf(TRANSMISSION_BEGIN_MARKER);
      if (markerPosition < 0) { return; }

      string customName = antenna.CustomName.Substring(0, markerPosition);

      string message = antenna.CustomName.Substring(markerPosition + TRANSMISSION_BEGIN_MARKER.Length, antenna.CustomName.Length - TRANSMISSION_BEGIN_MARKER.Length - markerPosition);
      if (lastMessage != null && message == lastMessage)
      {
        return;
      }
      lastMessage = message;
      string header = message.Substring(0, MESSAGE_MARKER.Length);
      string data = message.Substring(MESSAGE_MARKER.Length, message.Length - MESSAGE_MARKER.Length);

      if (header == MESSAGE_MARKER)
      {
        MyAPIGateway.Utilities.ShowMessage(customName, data);
      }
      else if (header == NOTIFICATION_MARKER)
      {
        MyAPIGateway.Utilities.ShowNotification(data);
      }

      antenna.SetCustomName(customName);
    }

    public override void MarkForClose()
    {
    }

    public override void UpdateAfterSimulation()
    {
    }

    public override void UpdateAfterSimulation10()
    {
      int index = (Entity as IMyRadioAntenna).CustomName.IndexOf(TRANSMISSION_BEGIN_MARKER);
      if (index > 0 && lastMessage != null)
      {
        (Entity as IMyRadioAntenna).SetCustomName((Entity as IMyRadioAntenna).CustomName.Substring(0, index));
        lastMessage = null;
      }
    }

    public override void UpdateAfterSimulation100()
    {

    }

    public override void UpdateBeforeSimulation()
    {
    }

    public override void UpdateBeforeSimulation10()
    {
    }

    public override void UpdateBeforeSimulation100()
    {
    }

    public override void UpdateOnceBeforeFrame()
    {
    }
  }
}
