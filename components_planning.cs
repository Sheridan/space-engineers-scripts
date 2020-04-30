// #include classes/main.cs
// #include classes/recipes.cs
// #include classes/display.cs
// #include classes/storages_group.cs

CRecipes recipes;
CDisplay lcd;
CStoragesGroup storage;
IMyAssembler targetAssembler;

public string program()
{
  lcd = new CDisplay();
  lcd.addDisplay("[Земля] Дисплей производства", 0, 0);
  targetAssembler = GridTerminalSystem.GetBlockWithName("[Земля] Master Сборщик 0") as IMyAssembler;
  storage = new CStoragesGroup("[Земля] БК Компоненты", "Компоненты");

  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  recipes = new CRecipes();
  recipes.add(FRecipe.LargeBlockArmorBlock(1000 * 4));
  recipes.add(FRecipe.Window3x3Flat(8));
  recipes.add(FRecipe.ArmorSide(64 * 16));
  recipes.add(FRecipe.ArmorCenter(512 * 16));
  recipes.add(FRecipe.LargeBlockArmorRoundCorner(32 * 16));

  recipes.add(FRecipe.LargeBlockRadioAntenna(4));
  recipes.add(FRecipe.SmallLight(16 + 2*4));
  recipes.add(FRecipe.LargeShipMergeBlock(4 + 16*2 + 4*2));
  recipes.add(FRecipe.LargePistonBase(8));
  recipes.add(FRecipe.LargeBlockGyro(16));

  recipes.add(FRecipe.LargeBlockWindTurbine(32));
  recipes.add(FRecipe.LargeBlockBatteryBlock(32));

  recipes.add(FRecipe.LargeBlockSmallContainer(16));
  recipes.add(FRecipe.LargeBlockLargeContainer(16));
  recipes.add(FRecipe.Connector(8));
  recipes.add(FRecipe.ConveyorTube(32));
  recipes.add(FRecipe.LargeBlockConveyor(32));

  recipes.add(FRecipe.Wheel5x5(4));
  recipes.add(FRecipe.Suspension5x5(4));

  return "Планирование производства";
}

public void main(string argument, UpdateType updateSource)
{
  bool assemblerProducing = targetAssembler.IsProducing;
  string state = assemblerProducing ? "Producing" : "Stopped";
  lcd.echo_at($"Assemblesr state: {state}", 0);
  lcd.echo_at("---", 1);
  int lcdIndex = 2;
  foreach(var component in recipes.sourceItems())
  {
    int inStorageAmount = storage.countItems(component.itemType());
    int needAmount = component.amount();
    int amount = needAmount - inStorageAmount;
    lcd.echo_at($"{component.itemType().ToString()}: {inStorageAmount} of {needAmount}", lcdIndex); lcdIndex++;
    if(amount > 0 && !assemblerProducing)
    {
      targetAssembler.AddQueueItem(
        MyDefinitionId.Parse(component.asBlueprintDefinition()),
        (double)amount);
    }
  }
}
