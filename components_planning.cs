// #include classes/main.cs
// #include classes/recipes.cs
// #include classes/display.cs
// #include classes/blocks/assembler.cs
// #include classes/blocks/container.cs
// #include classes/blocks/base/blocks.cs
// #include classes/blocks/base/blocks_named.cs

CRecipes recipes;
CDisplay lcdAssembling;
CContainer storage;
CAssembler assembler;

public string program()
{
  lcdAssembling = new CDisplay();
  lcdAssembling.addDisplays("Производство");
  assembler = new CAssembler(new CBlocks<IMyAssembler>());
  storage = new CContainer(new CBlocksNamed<IMyCargoContainer>("Компоненты"));

  // Runtime.UpdateFrequency = UpdateFrequency.Update100;

  recipes = new CRecipes();
  recipes.add(FRecipe.LargeArmorBlock(4*32+32*32));
  recipes.add(FRecipe.Window3x3Flat(8));
  recipes.add(FRecipe.ArmorSide(32));
  recipes.add(FRecipe.ArmorCenter(32));
  recipes.add(FRecipe.LargeArmorRoundCorner(32));

  recipes.add(FRecipe.LargeRadioAntenna(4));
  recipes.add(FRecipe.SmallLight(64));
  recipes.add(FRecipe.LargeShipMergeBlock(16));
  recipes.add(FRecipe.LargePistonBase(16));
  recipes.add(FRecipe.LargeGyro(16));

  recipes.add(FRecipe.LargeWindTurbine(32));
  recipes.add(FRecipe.LargeBattery(32));
  recipes.add(FRecipe.SolarPanel(32));

  recipes.add(FRecipe.LargeSmallContainer(16));
  recipes.add(FRecipe.LargeLargeContainer(16));
  recipes.add(FRecipe.Connector(8));
  recipes.add(FRecipe.ConveyorTube(32));
  recipes.add(FRecipe.LargeConveyor(32));

  recipes.add(FRecipe.Wheel5x5(4));
  recipes.add(FRecipe.Suspension5x5(4));
  recipes.add(FRecipe.AtmosphericThrust(8));

  recipes.add(FRecipe.LargeWelder(16));
  recipes.add(FRecipe.LargeGrinder(16));
  recipes.add(FRecipe.LargeDrill(16));

  recipes.add(FRecipe.MedicalRoom(2));

  return "Планирование производства";
}

public void main(string argument, UpdateType updateSource)
{
  if(argument == "start") { process(); }
}

public void process()
{
  int i = 0;
  bool assemblerProducing = assembler.producing();
  string state = assemblerProducing ? "Producing" : "Stopped";
  lcdAssembling.echo_at($"Assemblesr state: {state}", i++);
  lcdAssembling.echo_at("---", i++);
  foreach(CComponentItem component in recipes.sourceItems())
  {
    int inStorageAmount = storage.items(component.itemType());
    int needAmount = component.amount();
    int amount = needAmount - inStorageAmount;
    lcdAssembling.echo_at($"{component.itemType().ToString()}: {inStorageAmount} of {needAmount} - {inStorageAmount/(needAmount/100f):f2}%", i++);
    if(amount > 0 && !assemblerProducing)
    {
      assembler.produce(component.asBlueprintDefinition(), amount);
    }
  }
}
