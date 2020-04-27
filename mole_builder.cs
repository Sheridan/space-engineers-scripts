// #include classes/main.cs
// #include classes/waiter.cs
// #include classes/recipes.cs
// #include helpers/list.cs
// #include parts/mole_builder.cs

public enum EWorkState
{
  Wakeup,
  DisconnectWelderFoundation,
  StartWelding,
  Welding,
  StopWelding,
  ConnectWelderFoundation,
  DisconnectSupportFoundation,
  StartMoveBase,
  MoveBase,
  StopMoveBase,
  ConnectSupportFoundation,
  Sleep
}

EWorkState workState;
public EWorkState getNextState()
{
  switch (workState)
  {
    case EWorkState.Wakeup: return EWorkState.DisconnectWelderFoundation;
    case EWorkState.DisconnectWelderFoundation: return EWorkState.StartWelding;
    case EWorkState.StartWelding: return EWorkState.Welding;
    case EWorkState.Welding: return EWorkState.StopWelding;
    case EWorkState.StopWelding: return EWorkState.ConnectWelderFoundation;
    case EWorkState.ConnectWelderFoundation: return EWorkState.DisconnectSupportFoundation;
    case EWorkState.DisconnectSupportFoundation: return EWorkState.StartMoveBase;
    case EWorkState.StartMoveBase: return EWorkState.MoveBase;
    case EWorkState.MoveBase: return EWorkState.StopMoveBase;
    case EWorkState.StopMoveBase: return EWorkState.ConnectSupportFoundation;
    case EWorkState.ConnectSupportFoundation: return EWorkState.Sleep;
  }
  return EWorkState.Sleep;
}

public void switchToNextState()
{
  workState = getNextState();
  playSound("Security Klaxon");
  lcd.echo($"Switching to {workState.ToString()}");
}

public string boolStatusToString(bool val) { return val ? "Готово" : "В процессе"; }

public float pistonsSensetivity = 0.2f;
public bool checkPistonPos(float currentPos, float targetPos)
{
  return  currentPos <= targetPos + pistonsSensetivity &&
          currentPos >= targetPos - pistonsSensetivity;
}

public bool expandPistons(CBlockGroup<IMyPistonBase> pistons,
                          float length,
                          float velocity,
                          float stackSize = 1f)
{
  bool result = true;
  float realLength = (length - pistonHeadLength * stackSize) / stackSize;
  float realVelocity = velocity / stackSize;
  float currentPosition = 0;
  foreach (IMyPistonBase piston in pistons.blocks())
  {
    switch (piston.Status)
    {
      case PistonStatus.Stopped:
      case PistonStatus.Retracted:
      case PistonStatus.Retracting:
      case PistonStatus.Extended:
        {
          if (piston.CurrentPosition < realLength)
          {
            piston.Velocity = realVelocity;
            piston.MinLimit = 0f;
            piston.MaxLimit = realLength;
            piston.Extend();
          }
        }
        break;
    }
    currentPosition += piston.CurrentPosition;
    result = result && (piston.Status == PistonStatus.Extended ||
                       (piston.Status == PistonStatus.Extending && checkPistonPos(piston.CurrentPosition, realLength)));
  }
  currentPosition = currentPosition / pistons.count();
  lcd.echo($"[{pistons.purpose()}] Выдвигаются до {currentPosition:f2}->{realLength:f2}: {boolStatusToString(result)}");
  return result;
}

public bool retractPistons(CBlockGroup<IMyPistonBase> pistons,
                          float minLength,
                          float velocity,
                          float stackSize = 1f)
{
  bool result = true;
  float realLength = (minLength - pistonHeadLength * stackSize) / stackSize;
  float realVelocity = velocity / stackSize;
  float currentPosition = 0;
  foreach (IMyPistonBase piston in pistons.blocks())
  {
    switch (piston.Status)
    {
      case PistonStatus.Stopped:
      case PistonStatus.Extended:
      case PistonStatus.Extending:
      case PistonStatus.Retracted:
        {
          if (piston.CurrentPosition > realLength)
          {
            piston.Velocity = realVelocity;
            piston.MinLimit = realLength;
            piston.MaxLimit = 10f;
            piston.Retract();
          }
        }
        break;
    }
    currentPosition += piston.CurrentPosition;
    result = result && (piston.Status == PistonStatus.Retracted ||
                       (piston.Status == PistonStatus.Retracting && checkPistonPos(piston.CurrentPosition, realLength)));
  }
  currentPosition = currentPosition / pistons.count();
  lcd.echo($"[{pistons.purpose()}] Задвигаются до {currentPosition:f2}->{realLength:f2}: {boolStatusToString(result)}");
  return result;
}

public bool turnMergers(CBlockGroup<IMyShipMergeBlock> mergers, bool enabled)
{
  bool result = true;
  foreach (IMyShipMergeBlock merger in mergers.blocks())
  {
    if (merger.Enabled != enabled)
    {
      merger.Enabled = enabled;
    }
    result = result && merger.IsConnected == enabled;
  }
  lcd.echo($"[{mergers.purpose()}] Переключаются: {boolStatusToString(result)}");
  return result;
}

public bool connectConnectors(CBlockGroup<IMyShipConnector> connectors)
{
  bool result = true;
  foreach (IMyShipConnector connector in connectors.blocks())
  {
    if (connector.Status != MyShipConnectorStatus.Connected)
    {
      connector.Enabled = true;
      connector.Connect();
    }
    result = result && connector.Status == MyShipConnectorStatus.Connected;
  }
  lcd.echo($"[{connectors.purpose()}] Замыкаются: {boolStatusToString(result)}");
  return result;
}

public bool disconnectConnectors(CBlockGroup<IMyShipConnector> connectors)
{
  bool result = true;
  foreach (IMyShipConnector connector in connectors.blocks())
  {
    if (connector.Status != MyShipConnectorStatus.Unconnected ||
        connector.Status != MyShipConnectorStatus.Connectable ||
       !connector.Enabled)
    {
      connector.Disconnect();
      connector.Enabled = false;
    }
    result = result && (connector.Status == MyShipConnectorStatus.Unconnected ||
                        connector.Status == MyShipConnectorStatus.Connectable);
  }
  lcd.echo($"[{connectors.purpose()}] Отмыкаются: {boolStatusToString(result)}");
  return result;
}

public bool turnProectors(bool enabled)
{
  bool result = true;
  foreach (IMyProjector projector in projectors.blocks())
  {
    if (projector.Enabled != enabled)
    {
      projector.Enabled = enabled;
    }
    result = result && projector.IsProjecting == enabled;
  }
  lcd.echo($"[{projectors.purpose()}] Переключаются: {boolStatusToString(result)}");
  return result;
}

public bool turnWelders(bool enabled)
{
  bool result = true;
  foreach (IMyShipWelder welder in welders.blocks())
  {
    if (welder.Enabled != enabled)
    {
      welder.Enabled = enabled;
    }
    result = result && welder.IsWorking == enabled;
  }
  lcd.echo($"[{welders.purpose()}] Переключаются: {boolStatusToString(result)}");
  return result;
}

// + Cycle welders
public enum EProjectionBlocks
{
  Total,
  Remaining,
  Buildable
}

int remainAfterThisStep;
public void goNextBuildStep()
{
  remainAfterThisStep = getTotalProjectedBlocksCount(EProjectionBlocks.Remaining) -
                        getTotalProjectedBlocksCount(EProjectionBlocks.Buildable);
}

public int getTotalProjectedBlocksCount(EProjectionBlocks state)
{
  int result = 0;
  foreach (IMyProjector projector in projectors.blocks())
  {
    switch (state)
    {
      case EProjectionBlocks.Total: result += projector.TotalBlocks; break;
      case EProjectionBlocks.Remaining: result += projector.RemainingBlocks; break;
      case EProjectionBlocks.Buildable: result += projector.BuildableBlocksCount; break;
    }
  }
  return result;
}

public bool checkBuildStepComplete()
{
  int remain = getTotalProjectedBlocksCount(EProjectionBlocks.Remaining) - remainAfterThisStep;
  if (remain < 0) { remainAfterThisStep += remain; }
  lcd.echo($"Left blocks for build. Step: {remain}. Remaining:{getTotalProjectedBlocksCount(EProjectionBlocks.Remaining)}. Total: {getTotalProjectedBlocksCount(EProjectionBlocks.Total)}");
  // bool remainDone = remainAfterThisStep == getTotalProjectedBlocksCount(EProjectionBlocks.Remaining);
  // if(remainDone)
  // {
  //   lcd.echo("Сварка слоя завершена досрочно!");
  //   turnWelders(welders, false);
  //   return true;
  // }
  // return rotateWelders() && remainDone;
  return rotateWelders() && remainAfterThisStep == getTotalProjectedBlocksCount(EProjectionBlocks.Remaining);
}

int currentWelderIndex = 0;
int turnWeldersPerStep = 32;
double waitForNextWelderSecunds = 16;
// List<IMySlimBlock> currentBuildingBlocks;
public bool rotateWelders()
{
  IMyShipWelder welder;
  if (currentWelderIndex > 0)
  {
    lcd.echo($"Выключаются сварщики {currentWelderIndex - turnWeldersPerStep}:{currentWelderIndex}");
    for (int i = currentWelderIndex - turnWeldersPerStep; i < currentWelderIndex; i++)
    {
      welder = welders.blocks()[i];
      while (welder.Enabled)
      {
        welder.Enabled = false;
      }
    }
  }
  if (currentWelderIndex < welders.count())
  {
    playSound("Security Alarm");
    int maxWelderIndex = currentWelderIndex + turnWeldersPerStep;
    maxWelderIndex = maxWelderIndex > welders.count() ? welders.count() : maxWelderIndex;
    lcd.echo($"Включаются сварщики {currentWelderIndex}:{maxWelderIndex}");
    for (; currentWelderIndex < maxWelderIndex; currentWelderIndex++)
    {
      // if(currentWelderIndex == welders.Count) { break; }
      welder = welders.blocks()[currentWelderIndex];
      while (!welder.Enabled)
      {
        welder.Enabled = true;
      }
    }
    // currentWelderIndex += turnWeldersPerStep;
    waiter.wait(waitForNextWelderSecunds);
    return false;
  }
  currentWelderIndex = 0;
  return true;
}
// - Cycle welders

// + waiting
// double waitTo = 0;
// public double getCurrentSecunds()
// {
//   TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
//   return timeSpan.TotalSeconds;
// }
// public void wait(double seconds)
// {
//   waitTo = seconds + getCurrentSecunds();
// }
// public double leftWaitSecunds()
// {
//   return waitTo - getCurrentSecunds();
// }
// public bool waiting()
// {
//   // lcd.echo($"{waitTo}:{leftWaitSecunds()}");
//   if (waitTo > 0 && leftWaitSecunds() > 0)
//   {
//     return true;
//   }
//   waitTo = 0;
//   return false;
// }
// - waiting

public void playSound(string name)
{
  soundBlock.SelectedSound = name;
  soundBlock.Play();
}

// + cargo
public bool loadComponentsFromBase()
{
  bool result = true;
  CRecipes blocksRecipes = new CRecipes();
  // components = new Dictionary<string, int>();
  foreach (IMyProjector projector in projectors.blocks())
  {
    foreach (var block in projector.RemainingBlocksPerType)
    {
      blocksRecipes.add(FRecipe.fromString(block.Key.ToString(), block.Value));
    }
  }
  List<CRecipeSourceItem> neededItems = blocksRecipes.sourceItems();
  lcd.echo($"Загрузка компонентов: {neededItems.Count} наименований");
  IMyInventory dstInventory = componentsContainer.GetInventory();
  foreach (var neededItem in neededItems)
  {
    // string componentName = component.Key;
    int neededItemCount = neededItem.amount();
    MyItemType neededItemType = neededItem.asMyItemType();
    List<MyInventoryItem> dstItems = new List<MyInventoryItem>();
    dstInventory.GetItems(dstItems, x => x.Type.Equals(neededItemType));
    foreach (MyInventoryItem dstItem in dstItems)
    {
      neededItemCount -= dstItem.Amount.ToIntSafe();
      if (neededItemCount <= 0) { break; }
    }
    if (neededItemCount > 0)
    {
      foreach (IMyCargoContainer srcContainer in componentsSourceContainers)
      {
        IMyInventory srcInventory = srcContainer.GetInventory();
        List<MyInventoryItem> srcItems = new List<MyInventoryItem>();
        srcInventory.GetItems(srcItems, x => x.Type.Equals(neededItemType));
        foreach (MyInventoryItem srcItem in srcItems)
        {
          int srcItemCount = srcItem.Amount.ToIntSafe();
          lcd.echo($"{neededItemType.SubtypeId}: {srcContainer.CustomName}->{componentsContainer.CustomName} - {srcItemCount}");
          if (srcItemCount >= neededItemCount)
          {
            if (!dstInventory.TransferItemFrom(srcInventory, srcItem, neededItemCount)) { lcd.echo("Не перенеслось..."); }
            else { neededItemCount = 0; }
            break;
          }
          else
          {
            if (!dstInventory.TransferItemFrom(srcInventory, srcItem, srcItemCount)) { lcd.echo("Не перенеслось..."); }
            else { neededItemCount -= srcItemCount; }
          }
        }
        if (neededItemCount == 0) { break; }
      }
      result = result && neededItemCount == 0;
    }
    lcd.echo($"{neededItemType.SubtypeId}: need {neededItemCount} items.");
  }
  if (!result) { lcd.echo("Недостаточно материалов!"); }
  return result;
}
// - cargo

CDisplay lcd;
const float blockHeight = 2.5f; // Me.CubeGrid.GridSize
const int structureHeightInBlocks = 10;
const float structureHeight = blockHeight * structureHeightInBlocks;
const float pistonHeadLength = 0.11f;
const float mergeBlockOffset = -0.05f;

const float mainPistonsInStack = 3f;
const float mainPistonsExpandVelocity = 1f;
const float mainPistonsRetractVelocity = 1f;
const float mainPistonsMinLength = blockHeight;
const float mainPistonsMaxLength = structureHeight + blockHeight;
const float connPistonsExpandVelocity = 1f;
const float connPistonsRetractVelocity = 2f;

const string componentsContainerName = "[Крот] БК";
const string componentsSourceContainersGroupName = "[Земля] БК Компоненты";
const string soundBlockName = "[Крот] Динамик";

IMyCargoContainer componentsContainer;
List<IMyCargoContainer> componentsSourceContainers;
IMySoundBlock soundBlock;
CWaiter waiter;

public string program()
{
  workState = EWorkState.Sleep;
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CDisplay();
  lcd.addDisplay("[Крот] Дисплей логов строительства 0", 0, 0);
  lcd.addDisplay("[Крот] Дисплей логов строительства 1", 1, 0);
  return "Управление строительством тоннеля";
}

// public void dis()
// {
//   Wakeup();
//   turnMergers(supportMergers, false);
//   retractPistons(supportMergersPistons, 0f, connPistonsRetractVelocity);
//   // turnMergers(weldersMergers, false);
//   // retractPistons(weldersMergersPistons, 0f, connPistonsRetractVelocity);

//   // turnMergers(logisticMergers, false);
//   // disconnectConnectors(logisticConnectors);
//   // retractPistons(logisticPistons, 0f, connPistonsRetractVelocity);
// }

// public void conn()
// {
//   Wakeup();
//   bool a =
//     expandPistons(supportMergersPistons, blockHeight - mergeBlockOffset - pistonHeadLength, connPistonsExpandVelocity) &&
//     turnMergers(supportMergers, true);
//   // bool a =
//   //   expandPistons(weldersMergersPistons, blockHeight - mergeBlockOffset - pistonHeadLength, connPistonsExpandVelocity) &&
//   //   turnMergers(weldersMergers, true);

//   // bool a = expandPistons(logisticPistons, blockHeight - mergeBlockOffset - pistonHeadLength, connPistonsExpandVelocity) &&
//   // turnMergers(logisticMergers, true) &&
//   // connectConnectors(logisticConnectors);
// }

public void main(string argument, UpdateType updateSource)
{
  if (waiter != null && waiter.waiting()) { return; }
  // wait(60);
  if (argument == "go") { workState = EWorkState.Wakeup; return; }
  // if (argument == "dis") { dis(); return; }
  // if (argument == "conn") { conn(); return; }
  bool result = false;
  switch (workState)
  {
    case EWorkState.Wakeup:                      result = Wakeup();                      break;
    case EWorkState.DisconnectWelderFoundation:  result = DisconnectWelderFoundation();  break;
    case EWorkState.StartWelding:                result = StartWelding();                break;
    case EWorkState.Welding:                     result = Welding();                     break;
    case EWorkState.StopWelding:                 result = StopWelding();                 break;
    case EWorkState.ConnectWelderFoundation:     result = ConnectWelderFoundation();     break;
    case EWorkState.DisconnectSupportFoundation: result = DisconnectSupportFoundation(); break;
    case EWorkState.StartMoveBase:               result = StartMoveBase();               break;
    case EWorkState.MoveBase:                    result = MoveBase();                    break;
    case EWorkState.StopMoveBase:                result = StopMoveBase();                break;
    case EWorkState.ConnectSupportFoundation:    result = ConnectSupportFoundation();    break;
    case EWorkState.Sleep:                       result = Sleep();                       break;
  }
  if (result) { switchToNextState(); }
}

public bool Wakeup()
{
  waiter = new CWaiter(lcd);
  currentWelderIndex = 0;
  initGroups();
  componentsContainer = self.GridTerminalSystem.GetBlockWithName(componentsContainerName) as IMyCargoContainer;
  componentsSourceContainers = getGroupBlocks<IMyCargoContainer>(componentsSourceContainersGroupName, false);
  soundBlock = self.GridTerminalSystem.GetBlockWithName(soundBlockName) as IMySoundBlock;
  return true;
}


public bool DisconnectWelderFoundation()
{
  return
    turnMergers(weldersMergers, false) &&
    retractPistons(weldersMergersPistons, 0f, connPistonsRetractVelocity);
}

int buildedLinesInBlocks;
public bool StartWelding()
{
  if (turnProectors(true) &&
      loadComponentsFromBase() &&
      disconnectConnectors(mainConnectors))
  {
    buildedLinesInBlocks = 1;
    goNextBuildStep();
    return true;
  }
  return false;
}

public bool Welding()
{
  if (expandPistons(mainPistons, mainPistonsMinLength + buildedLinesInBlocks * blockHeight, mainPistonsExpandVelocity, mainPistonsInStack) &&
      checkBuildStepComplete())
  {
    if (buildedLinesInBlocks == structureHeightInBlocks)
    {
      return true;
    }
    buildedLinesInBlocks++;
    goNextBuildStep();
  }
  return false;
}

public bool StopWelding()
{
  return
    turnProectors(false) &&
    turnWelders(false);
}

public bool ConnectWelderFoundation()
{
  return
    expandPistons(weldersMergersPistons, blockHeight - mergeBlockOffset - pistonHeadLength, connPistonsExpandVelocity) &&
    turnMergers(weldersMergers, true);
}

public bool DisconnectSupportFoundation()
{
  return
    turnMergers(supportMergers, false) &&
    retractPistons(supportMergersPistons, 0f, connPistonsRetractVelocity) &&
    turnMergers(logisticMergers, false) &&
    disconnectConnectors(logisticConnectors) &&
    retractPistons(logisticPistons, 0f, connPistonsRetractVelocity);
}

public bool StartMoveBase()
{
  return true;
}

public bool MoveBase()
{
  return
    retractPistons(mainPistons, mainPistonsMinLength, mainPistonsRetractVelocity, mainPistonsInStack);
}

public bool StopMoveBase()
{
  return true;
}

public bool ConnectSupportFoundation()
{
  return
    expandPistons(supportMergersPistons, blockHeight - mergeBlockOffset - pistonHeadLength, connPistonsExpandVelocity) &&
    turnMergers(supportMergers, true) &&
    expandPistons(logisticPistons, blockHeight - mergeBlockOffset - pistonHeadLength, connPistonsExpandVelocity) &&
    turnMergers(logisticMergers, true) &&
    connectConnectors(logisticConnectors) &&
    connectConnectors(mainConnectors);
}

public bool Sleep()
{
  lcd.echo("Sleep");
  return false;
}

public List<T> getGroupBlocks<T>(string groupName, bool sameConstructAsMe = true) where T : class, IMyTerminalBlock
{
  IMyBlockGroup group = GridTerminalSystem.GetBlockGroupWithName(groupName);
  List<T> blocks = new List<T>();
  if (sameConstructAsMe) { group.GetBlocksOfType<T>(blocks, x => x.IsSameConstructAs(Me)); }
  else { group.GetBlocksOfType<T>(blocks); }
  return blocks;
}
