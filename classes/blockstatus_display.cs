// #include classes/display.cs
// #include helpers/human.cs

public class CBlockStatusDisplay : CDisplay
{
  public CBlockStatusDisplay() : base() {}

  private string getFunctionaBlocksStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if(!group.isAssignable<IMyFunctionalBlock>()) { return ""; }
    string result = "";
    int pOn = 0;
    foreach (IMyFunctionalBlock block in group.blocks())
    {
      if (block.Enabled) { pOn++; }
    }
    result += $"Power: {pOn} ";
    return result;
  }

  // private string getInventoryStatus(IMyInventory inventory)
  // {
  //   return $"Vol: {inventory.CurrentVolume} of {inventory.MaxVolume} ";
  // }

  private string getRotorsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyMotorStator>()) { return ""; }
    string result = "";
    List<string> rpm = new List<string>();
    List<string> angle = new List<string>();
    foreach (IMyMotorStator block in group.blocks())
    {
      float angleGrad = block.Angle * 180 / (float)Math.PI;
      rpm.Add($"{block.TargetVelocityRPM:f2}");
      angle.Add($"{angleGrad:f2}°");
    }
    result += $"Angle: {string.Join(":", angle)} "
            + $"RPM: {string.Join(":", rpm)} ";
    return result;
  }


  private string getDrillsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if(!group.isAssignable<IMyShipDrill>()) { return ""; }
    string result = "";
    IMyInventory inventory;
    int volume = 0;
    int mass = 0;
    int items = 0;
    foreach (IMyShipDrill block in group.blocks())
    {
      inventory = block.GetInventory();
      volume += inventory.CurrentVolume.ToIntSafe();
      mass += inventory.CurrentMass.ToIntSafe();
      items += inventory.ItemCount;
      // volumes.Add($"{block.GetInventory().CurrentVolume:f2}");
      // result += getInventoryStatus(block.GetInventory());
    }
    result += $"VMI: {toHumanReadable(volume, "Л")}:{toHumanReadable(mass, "Кг")}:{toHumanReadable(items)} ";
    return result;
  }

  private string getPistonsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyPistonBase>()) { return ""; }

    string result = "";
    List<string> positions = new List<string>();
    // List<string> velocityes = new List<string>();
    int statusStopped = 0;
    int statusExtending = 0;
    int statusExtended = 0;
    int statusRetracting = 0;
    int statusRetracted = 0;
    foreach (IMyPistonBase block in group.blocks())
    {
      switch(block.Status)
      {
        case PistonStatus.Stopped: statusStopped++; break;
        case PistonStatus.Extending: statusExtending++; break;
        case PistonStatus.Extended: statusExtended++; break;
        case PistonStatus.Retracting: statusRetracting++; break;
        case PistonStatus.Retracted: statusRetracted++; break;
      }
      positions.Add($"{block.CurrentPosition:f2}");
      // velocityes.Add($"{block.Velocity:f2}");
    }
    result += $"SeErR: {statusStopped}:{statusExtending}:{statusExtended}:{statusRetracting}:{statusRetracted} "
            + $"Pos: {string.Join(":", positions)} ";
            // + $"Vel: {string.Join(":", velocityes)} ";
    return result;
  }

  private string getGyroStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyGyro>()) { return ""; }
    string result = "";
    float yaw = 0;
    float pitch = 0;
    float roll = 0;
    foreach (IMyGyro block in group.blocks())
    {
      yaw += block.Yaw;
      pitch += block.Pitch;
      roll += block.Roll;
    }
    result += $"YPR: {yaw/group.count():f3}:{pitch/group.count():f3}:{roll/group.count():f3} ";
    return result;
  }

  private string getMergersStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyShipMergeBlock>()) { return ""; }
    string result = "";
    int connected = 0;
    foreach (IMyShipMergeBlock block in group.blocks())
    {
      if (block.IsConnected) { connected++; }
    }
    result += $"Connected: {connected} ";
    return result;
  }

  private string getConnectorsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyShipConnector>()) { return ""; }
    string result = "";
    int statusUnconnected = 0;
    int statusConnectable = 0;
    int statusConnected = 0;
    foreach (IMyShipConnector block in group.blocks())
    {
      switch (block.Status)
      {
        case MyShipConnectorStatus.Unconnected: statusUnconnected++; break;
        case MyShipConnectorStatus.Connectable: statusConnectable++; break;
        case MyShipConnectorStatus.Connected: statusConnected++; break;
      }
    }
    result += $"UcC: {statusUnconnected}:{statusConnectable}:{statusConnected} ";
    return result;
  }

  private string getProjectorsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyProjector>()) { return ""; }
    string result = "";
    int projecting = 0;
    List<string> blocksTotal = new List<string>();
    List<string> blocksRemaining = new List<string>();
    List<string> blocksBuildable = new List<string>();
    foreach (IMyProjector block in group.blocks())
    {
      if (block.IsProjecting) { projecting++; }
      blocksTotal.Add($"{block.TotalBlocks}");
      blocksRemaining.Add($"{block.RemainingBlocks}");
      blocksBuildable.Add($"{block.BuildableBlocksCount}");
    }
    result += $"Pr: {projecting} "
           + $"B: {string.Join(":", blocksBuildable)} "
           + $"R: {string.Join(":", blocksRemaining)} "
           + $"T: {string.Join(":", blocksTotal)} "
           ;
    return result;
  }

  // private string getWeldersStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  // {
  //   if (!group.isAssignable<IMyShipWelder>()) { return ""; }
  //   string result = "";
  //   int connected = 0;
  //   foreach (IMyShipWelder block in group.blocks())
  //   {
  //     if (block.IsConnected) { connected++; }
  //   }
  //   result += $"Connected: {connected} ";
  //   return result;
  // }

  public void showStatus<T>(CBlockGroup<T> group, int position) where T : class, IMyTerminalBlock
  {
    string result = $"[{group.subtypeName()}] {group.purpose()} ";
    if(!group.empty())
    {
      result += $"({group.count()}) "
             + getPistonsStatus<T>(group)
             + getConnectorsStatus<T>(group)
             + getMergersStatus<T>(group)
             + getProjectorsStatus<T>(group)
             + getDrillsStatus<T>(group)
             + getRotorsStatus<T>(group)
             + getGyroStatus<T>(group)
             + getFunctionaBlocksStatus<T>(group)
             ;
    }
    else
    {
      result += $" Группа {group.groupName()} пуста";
    }
    echo_at(result, position);
  }
}
