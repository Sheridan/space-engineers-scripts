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
  recipes.add(FRecipe.HeavyArmorBlock(32));
  recipes.add(FRecipe.ArmorBlock(4*32+32*32*4));
  recipes.add(FRecipe.Window3x3Flat(8));
  recipes.add(FRecipe.ArmorSide(32));
  recipes.add(FRecipe.ArmorCenter(32));
  recipes.add(FRecipe.LargeArmorRoundCorner(32));

  recipes.add(FRecipe.LargeRadioAntenna(4));
  recipes.add(FRecipe.SmallLight(64));
  recipes.add(FRecipe.LargeShipMergeBlock(16));
  recipes.add(FRecipe.LargePistonBase(16));
  recipes.add(FRecipe.LargeGyro(16));
  recipes.add(FRecipe.OxygenFarm(16*2*4));

  recipes.add(FRecipe.LargeWindTurbine(32));
  recipes.add(FRecipe.LargeBattery(32*8+16*4));
  recipes.add(FRecipe.SolarPanel(32*8+72*4));
  recipes.add(FRecipe.LargeReactor(4));
  // recipes.add(FRecipe.SmallReactor(16));

  recipes.add(FRecipe.LargeSmallContainer(16));
  recipes.add(FRecipe.LargeLargeContainer(16));
  recipes.add(FRecipe.Connector(8));
  recipes.add(FRecipe.ConveyorTube(32));
  recipes.add(FRecipe.LargeConveyor(32));

  recipes.add(FRecipe.Wheel5x5(4));
  recipes.add(FRecipe.Suspension5x5(4));
  recipes.add(FRecipe.AtmosphericThrust(8));
  recipes.add(FRecipe.HydrogenThrust(8));
  // recipes.add(FRecipe.IonThrust(8));
  // recipes.add(FRecipe.IonThrustModule(32));

  recipes.add(FRecipe.LargeWelder(16));
  recipes.add(FRecipe.LargeGrinder(16));
  recipes.add(FRecipe.LargeDrill(16));
  recipes.add(FRecipe.LargeOreDetector(4));

  recipes.add(FRecipe.MedicalRoom(2));
  // recipes.add(FRecipe.GravGen(8));

  int refineryes = ((7+7+2)*2)*4;

  recipes.add(FRecipe.Assembler(4));
  recipes.add(FRecipe.Refinery(refineryes));
  recipes.add(FRecipe.PowerEfficiencyModule(16));
  recipes.add(FRecipe.SpeedModule(16));
  recipes.add(FRecipe.YieldModule(refineryes*4));

  recipes.add(CComponentItem.NATO_25x184mm(1000));
  // recipes.add(CComponentItem.NATO_5p56x45mm(200));
  recipes.add(CComponentItem.AutomaticRifleGun_Mag_20rd(200));
  recipes.add(CComponentItem.UltimateAutomaticRifleGun_Mag_30rd(200));
  recipes.add(CComponentItem.RapidFireAutomaticRifleGun_Mag_50rd(200));
  recipes.add(CComponentItem.PreciseAutomaticRifleGun_Mag_5rd(200));
  recipes.add(CComponentItem.SemiAutoPistolMagazine(200));
  recipes.add(CComponentItem.ElitePistolMagazine(200));
  recipes.add(CComponentItem.FullAutoPistolMagazine(200));


  return "Планирование производства";
}

public void main(string argument, UpdateType updateSource)
{
       if(argument == "start") { process(); }
  else if(argument == "clear") { clear(); }
}

public void process()
{
  int i = 0;
  bool assemblerProducing = assembler.producing();
  string state = assemblerProducing ? "Producing" : "Stopped";
  lcdAssembling.echo_at($"Assemblesr state: {state}", i++);
  lcdAssembling.echo_at("---", i++);
  foreach(CComponentItem c in recipes.sourceItems())
  {
    // debug(c.asBlueprintDefinition());
    int inStorageAmount = storage.items(c.itemType());
    int needAmount = c.amount();
    int amount = needAmount - inStorageAmount;
    lcdAssembling.echo_at($"{c.itemType().ToString()}: {inStorageAmount} of {needAmount} - {inStorageAmount/(needAmount/100f):f2}%", i++);
    if(amount > 0 && !assemblerProducing)
    {
      assembler.produce(c.asBlueprintDefinition(), amount);
    }
  }
  // foreach(KeyValuePair<string, SMinCurrentMax<float>> i in ammo)
  // {

  // }
}

public void clear()
{
  assembler.clear();
}