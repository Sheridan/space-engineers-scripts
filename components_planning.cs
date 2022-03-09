// #include classes/main.cs
// #include classes/recipes.cs
// #include classes/display.cs
// #include classes/storages_group.cs

CRecipes recipes;
CDisplay lcdAssembling;
CDisplay lcdPerBlock;
CStoragesGroup storage;
IMyAssembler targetAssembler;

public string program()
{
  lcdAssembling = new CDisplay();
  lcdAssembling.addDisplay($"[{structureName}] Дисплей Производство 0", 0, 0);
  lcdPerBlock = new CDisplay();
  lcdPerBlock.addDisplay($"[{structureName}] Дисплей Хранение 0", 0, 0);
  lcdPerBlock.addDisplay($"[{structureName}] Дисплей Хранение 1", 0, 1);
  targetAssembler = GridTerminalSystem.GetBlockWithName($"[{structureName}] Сборщик Master 00") as IMyAssembler;
  storage = new CStoragesGroup($"[{structureName}] Компоненты", "Компоненты");

  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  recipes = new CRecipes();
  recipes.add(FRecipe.LargeBlockArmorBlock(4*32+32*32));
  recipes.add(FRecipe.Window3x3Flat(8));
  recipes.add(FRecipe.ArmorSide(32));
  recipes.add(FRecipe.ArmorCenter(32));
  recipes.add(FRecipe.LargeBlockArmorRoundCorner(32));

  recipes.add(FRecipe.LargeBlockRadioAntenna(4));
  recipes.add(FRecipe.SmallLight(64));
  recipes.add(FRecipe.LargeShipMergeBlock(16));
  recipes.add(FRecipe.LargePistonBase(16));
  recipes.add(FRecipe.LargeBlockGyro(16));

  recipes.add(FRecipe.LargeBlockWindTurbine(32));
  recipes.add(FRecipe.LargeBlockBatteryBlock(32));
  recipes.add(FRecipe.SolarPanel(32));

  recipes.add(FRecipe.LargeBlockSmallContainer(16));
  recipes.add(FRecipe.LargeBlockLargeContainer(16));
  recipes.add(FRecipe.Connector(8));
  recipes.add(FRecipe.ConveyorTube(32));
  recipes.add(FRecipe.LargeBlockConveyor(32));

  recipes.add(FRecipe.Wheel5x5(4));
  recipes.add(FRecipe.Suspension5x5(4));
  recipes.add(FRecipe.AtmosphericThrust(8));

  recipes.add(FRecipe.MedicalRoom(2));

  return "Планирование производства";
}

public void main(string argument, UpdateType updateSource)
{
  bool assemblerProducing = targetAssembler.IsProducing;
  string state = assemblerProducing ? "Producing" : "Stopped";
  lcdAssembling.echo_at($"Assemblesr state: {state}", 0);
  lcdAssembling.echo_at("---", 1);
  int lcdAssemblingIndex = 2;
  foreach(CComponentItem component in recipes.sourceItems())
  {
    int inStorageAmount = storage.countItems(component.itemType());
    int needAmount = component.amount();
    int amount = needAmount - inStorageAmount;
    lcdAssembling.echo_at($"{component.itemType().ToString()}: {inStorageAmount} of {needAmount}", lcdAssemblingIndex); lcdAssemblingIndex++;
    if(amount > 0 && !assemblerProducing)
    {
      targetAssembler.AddQueueItem(
        MyDefinitionId.Parse(component.asBlueprintDefinition()),
        (double)amount);
    }
  }
}

// public void showPerItems(CComponentItem component)
// {
//   int lcdAssemblingIndex = 2;
//   foreach (CComponentItem component in recipes.sourceItems())
//   {
//     int inStorageAmount = storage.countItems(component.itemType());
//     int needAmount = component.amount();
//     int amount = needAmount - inStorageAmount;
//     lcdAssembling.echo_at($"{component.itemType().ToString()}: {inStorageAmount} of {needAmount}", lcdAssemblingIndex); lcdAssemblingIndex++;
//     if (amount > 0 && !assemblerProducing)
//     {
//       targetAssembler.AddQueueItem(
//         MyDefinitionId.Parse(component.asBlueprintDefinition()),
//         (double)amount);
//     }
//   }
// }
