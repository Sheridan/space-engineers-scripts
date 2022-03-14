// #include classes/display.cs
// #include classes/blocks/base/blocks_base.cs
// #include classes/block_power_info.cs
// #include helpers/human.cs

public class CBlockStatusDisplay : CDisplay
{
  public CBlockStatusDisplay() : base() {}

  private string getFunctionaBlocksStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
  {
    if(!group.isAssignable<IMyFunctionalBlock>()) { return ""; }
    string result = "";
    int pOn = 0;
    int fOn = 0;
    int wOn = 0;
    float powerConsumed = 0f;
    float powerMaxConsumed = 0f;
    foreach (IMyFunctionalBlock block in group.blocks())
    {
      if (block.Enabled)
      {
        pOn++;
        CBlockPowerInfo pInfo = new CBlockPowerInfo(block);
        powerConsumed += pInfo.currentConsume();
        powerMaxConsumed += pInfo.maxConsume();
      }
      if (block.IsFunctional) { fOn++; }
      if (block.IsWorking) { wOn++; }
    }
    result += $"PFW: {pOn}:{fOn}:{wOn} ";
    if(powerMaxConsumed > 0)
    {
      result += $"Consuming (now,max): {toHumanReadable(powerConsumed, EHRUnit.Power)}:{toHumanReadable(powerMaxConsumed, EHRUnit.Power)} ";
    }
    return result;
  }

  // private string getInventoryStatus(IMyInventory inventory)
  // {
  //   return $"Vol: {inventory.CurrentVolume} of {inventory.MaxVolume} ";
  // }

  private string getRotorsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
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

  private string getGasTanksStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyGasTank>()) { return ""; }
    string result = "";
    float capacity = 0;
    double filledRatio = 0;
    foreach (IMyGasTank block in group.blocks())
    {
      capacity += block.Capacity;
      filledRatio += block.FilledRatio;
    }
    result += $"Capacity: {toHumanReadable(capacity, EHRUnit.Volume)} "
            + $"Filled: {filledRatio/group.count()*100:f2}% ";
    return result;
  }

  private string getBatteryesStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyBatteryBlock>()) { return ""; }
    string result = "";
    float currentStored = 0;
    float maxStored = 0;
    foreach (IMyBatteryBlock block in group.blocks())
    {
      currentStored += block.CurrentStoredPower;
      maxStored += block.MaxStoredPower;
    }
    currentStored *= 1000000;
    maxStored *= 1000000;
    result += $"Capacity: {toHumanReadable(currentStored, EHRUnit.PowerCapacity)}:{toHumanReadable(maxStored, EHRUnit.PowerCapacity)} ";
    return result;
  }


  private string getInvertoryesStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
  {
    long volume = 0;
    long volumeMax = 0;
    int mass = 0;
    int items = 0;
    int inventoryes = 0;
    foreach (IMyTerminalBlock block in group.blocks())
    {
      if(block.HasInventory)
      {
        IMyInventory inventory;
        inventoryes = block.InventoryCount;
        for(int i = 0; i < inventoryes; i++)
        {
          inventory = block.GetInventory(i);
          volume    += inventory.CurrentVolume.ToIntSafe();
          volumeMax += inventory.MaxVolume.ToIntSafe();
          mass      += inventory.CurrentMass.ToIntSafe();
          items     += inventory.ItemCount;
        }
      }
    }
    if(inventoryes > 0)
    {
      mass *= 1000;
      return $"VMI: ({toHumanReadable(volume, EHRUnit.Volume)}:{toHumanReadable(volumeMax, EHRUnit.Volume)}):{toHumanReadable(mass, EHRUnit.Mass)}:{toHumanReadable(items)} from {inventoryes} ";
    }
    return "";
  }

  // private string getDrillsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
  // {
  //   if(!group.isAssignable<IMyShipDrill>()) { return ""; }
  //   string result = "";
  //   IMyInventory inventory;
  //   int volume = 0;
  //   int mass = 0;
  //   int items = 0;
  //   foreach (IMyShipDrill block in group.blocks())
  //   {
  //     inventory = block.GetInventory();
  //     volume += inventory.CurrentVolume.ToIntSafe();
  //     mass += inventory.CurrentMass.ToIntSafe();
  //     items += inventory.ItemCount;
  //     // volumes.Add($"{block.GetInventory().CurrentVolume:f2}");
  //     // result += getInventoryStatus(block.GetInventory());
  //   }
  //   result += $"VMI: {toHumanReadable(volume, "Л")}:{toHumanReadable(mass, "Кг")}:{toHumanReadable(items)} ";
  //   return result;
  // }

  private string getPistonsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
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

  private string getGyroStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyGyro>()) { return ""; }
    string result = "";
    float yaw = 0;
    float pitch = 0;
    float roll = 0;
    foreach (IMyGyro block in group.blocks())
    {
      yaw   += Math.Abs(block.Yaw);
      pitch += Math.Abs(block.Pitch);
      roll  += Math.Abs(block.Roll);
    }
    result += $"YPR: {yaw/group.count():f4}:{pitch/group.count():f4}:{roll/group.count():f4} ";
    return result;
  }

  private string getMergersStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
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

  private string getConnectorsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
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

  private string getProjectorsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
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

  private string getPowerProducersStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyPowerProducer>()) { return ""; }
    string result = "";
    float currentOutput = 0f;
    float maxOutput = 0f;
    foreach (IMyPowerProducer block in group.blocks())
    {
      CBlockPowerInfo pInfo = new CBlockPowerInfo(block);
      currentOutput += pInfo.currentProduce();
      maxOutput += pInfo.maxProduce();
    }
    result += $"Ген. энергии (now:max): {toHumanReadable(currentOutput, EHRUnit.Power)}:{toHumanReadable(maxOutput, EHRUnit.Power)} ";
    return result;
  }

  // private string getWeldersStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock
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

  public void showStatus<T>(CBlocksBase<T> group, int position) where T : class, IMyTerminalBlock
  {
    string result = $"[{group.subtypeName()}] ";
    if(!group.empty())
    {
      result += $"({group.count()}) "
             + getPistonsStatus<T>(group)
             + getConnectorsStatus<T>(group)
             + getMergersStatus<T>(group)
             + getProjectorsStatus<T>(group)
            //  + getDrillsStatus<T>(group)
             + getRotorsStatus<T>(group)
             + getGyroStatus<T>(group)
             + getBatteryesStatus<T>(group)
             + getGasTanksStatus<T>(group)
             + getPowerProducersStatus<T>(group)
             + getInvertoryesStatus<T>(group)
             + getFunctionaBlocksStatus<T>(group)
             ;
    }
    else
    {
      result += "Таких блоков нет";
    }
    echo_at(result, position);
  }
}
