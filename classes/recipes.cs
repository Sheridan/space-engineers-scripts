// #include classes/components.cs

public class CRecipe
{
  public CRecipe(string blueprint) { m_blueprint = blueprint; m_sourceItems = new List<CComponentItem>(); }
  public void addItem(CComponentItem item) { m_sourceItems.Add(item); }
  public string blueprint() { return m_blueprint; }
  public List<CComponentItem> sourceItems() { return m_sourceItems; }
  private string m_blueprint;
  private List<CComponentItem> m_sourceItems;
}

public class FRecipe
{
  static public CRecipe fromString(string itemString, int amount = 1)
  {
    switch (itemString)
    {
      case "MyObjectBuilder_CubeBlock/LargeBlockArmorBlock"         : return ArmorBlock                 (amount);
      case "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorBlock"    : return HeavyArmorBlock            (amount);
      case "MyObjectBuilder_InteriorLight/SmallLight"               : return SmallLight                 (amount);
      case "MyObjectBuilder_ConveyorConnector/ConveyorTube"         : return ConveyorTube               (amount);
      case "MyObjectBuilder_MergeBlock/LargeShipMergeBlock"         : return LargeShipMergeBlock        (amount);
      case "MyObjectBuilder_ShipConnector/Connector"                : return Connector                  (amount);
      case "MyObjectBuilder_Conveyor/LargeBlockConveyor"            : return LargeConveyor              (amount);
      case "MyObjectBuilder_CubeBlock/ArmorCorner"                  : return ArmorCorner                (amount);
      case "MyObjectBuilder_CubeBlock/ArmorInvCorner"               : return ArmorInvCorner             (amount);
      case "MyObjectBuilder_CubeBlock/ArmorSide"                    : return ArmorSide                  (amount);
      case "MyObjectBuilder_CubeBlock/ArmorCenter"                  : return ArmorCenter                (amount);
      case "MyObjectBuilder_CargoContainer/LargeBlockLargeContainer": return LargeLargeContainer        (amount);
      case "MyObjectBuilder_CargoContainer/LargeBlockSmallContainer": return LargeSmallContainer        (amount);
      case "MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna"    : return LargeRadioAntenna          (amount);
      case "MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock"    : return LargeBattery               (amount);
      case "MyObjectBuilder_WindTurbine/LargeBlockWindTurbine"      : return LargeWindTurbine           (amount);
      case "MyObjectBuilder_Gyro/LargeBlockGyro"                    : return LargeGyro                  (amount);
      case "MyObjectBuilder_CubeBlock/Window3x3Flat"                : return Window3x3Flat              (amount);
      case "MyObjectBuilder_Wheel/Wheel5x5"                         : return Wheel5x5                   (amount);
      case "MyObjectBuilder_MotorSuspension/Suspension5x5"          : return Suspension5x5              (amount);
      case "MyObjectBuilder_ExtendedPistonBase/LargePistonBase"     : return LargePistonBase            (amount);
      case "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner"   : return LargeArmorRoundCorner      (amount);
      case "MyObjectBuilder_MedicalRoom/LargeMedicalRoom"           : return MedicalRoom                (amount);
      case "MyObjectBuilder_SolarPanel/LargeBlockSolarPanel"        : return SolarPanel                 (amount);
      case "MyObjectBuilder_Thrust/LargeBlockLargeAtmosphericThrust": return AtmosphericThrust          (amount);
      case "MyObjectBuilder_ShipWelder/LargeShipWelder"             : return LargeWelder                (amount);
      case "MyObjectBuilder_ShipGrinder/LargeShipGrinder"           : return LargeGrinder               (amount);
      case "MyObjectBuilder_Drill/LargeBlockDrill"                  : return LargeDrill                 (amount);
      case "MyObjectBuilder_Thrust/LargeBlockLargeThrust"           : return IonThrust                  (amount);
      case "MyObjectBuilder_Thrust/LargeBlockLargeHydrogenThrust"   : return HydrogenThrust             (amount);
      case "MyObjectBuilder_OreDetector/LargeOreDetector"           : return LargeOreDetector           (amount);
      case "MyObjectBuilder_Thrust/LargeBlockSmallModularThruster"  : return IonThrustModule            (amount);
      case "MyObjectBuilder_Reactor/LargeBlockLargeGenerator"       : return LargeReactor               (amount);
      case "MyObjectBuilder_Reactor/LargeBlockSmallGenerator"       : return SmallReactor               (amount);
      // case "MyObjectBuilder_GravityGeneratorSphere/"                : return GravGenSphere              (amount);
      case "MyObjectBuilder_GravityGenerator/"                      : return GravGen                    (amount);
      case "MyObjectBuilder_UpgradeModule/LargeEnergyModule"        : return PowerEfficiencyModule      (amount);
      case "MyObjectBuilder_UpgradeModule/LargeProductivityModule"  : return SpeedModule                (amount);
      case "MyObjectBuilder_UpgradeModule/LargeEffectivenessModule" : return YieldModule                (amount);
      case "MyObjectBuilder_Assembler/LargeAssembler"               : return Assembler                  (amount);
      case "MyObjectBuilder_Refinery/LargeRefinery"                 : return Refinery                   (amount);
      case "MyObjectBuilder_OxygenFarm/LargeBlockOxygenFarm"        : return OxygenFarm                 (amount);
    }
    throw new System.ArgumentException("Не знаю такой строки", itemString);
  }
  static public CRecipe SingleItem(CComponentItem item, int amount = 1)
  {
    CRecipe recipe = new CRecipe(item.asBlueprintDefinition());
    recipe.addItem(item);
    return recipe;
  }

  static public CRecipe LargePistonBase(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ExtendedPistonBase/LargePistonBase");
    recipe.addItem(FComponentItem.Computer(2 * amount));
    recipe.addItem(FComponentItem.Motor(4 * amount));
    recipe.addItem(FComponentItem.LargeTube((8+4) * amount)); // + top
    recipe.addItem(FComponentItem.Construction(10 * amount));
    recipe.addItem(FComponentItem.SteelPlate((10+15) * amount)); // + top
    return recipe;
  }
  static public CRecipe Wheel5x5(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Wheel/Wheel5x5");
    recipe.addItem(FComponentItem.LargeTube(8 * amount));
    recipe.addItem(FComponentItem.Construction(30 * amount));
    recipe.addItem(FComponentItem.SteelPlate(16 * amount));
    return recipe;
  }
  static public CRecipe Suspension5x5(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_MotorSuspension/Suspension5x5");
    recipe.addItem(FComponentItem.Motor(20 * amount));
    recipe.addItem(FComponentItem.SmallTube(30 * amount));
    recipe.addItem(FComponentItem.LargeTube(20 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(70 * amount));
    return recipe;
  }
  static public CRecipe Window3x3Flat(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/Window3x3Flat");
    recipe.addItem(FComponentItem.BulletproofGlass(196 * amount));
    recipe.addItem(FComponentItem.Girder(40 * amount));
    return recipe;
  }
  static public CRecipe LargeGyro(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Gyro/LargeBlockGyro");
    recipe.addItem(FComponentItem.Computer(5 * amount));
    recipe.addItem(FComponentItem.Motor(4 * amount));
    recipe.addItem(FComponentItem.MetalGrid(50 * amount));
    recipe.addItem(FComponentItem.LargeTube(4 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(600 * amount));
    return recipe;
  }
  static public CRecipe LargeWindTurbine(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_WindTurbine/LargeBlockWindTurbine");
    recipe.addItem(FComponentItem.Computer(2 * amount));
    recipe.addItem(FComponentItem.Girder(24 * amount));
    recipe.addItem(FComponentItem.Construction(20 * amount));
    recipe.addItem(FComponentItem.Motor(8 * amount));
    recipe.addItem(FComponentItem.InteriorPlate(40 * amount));
    return recipe;
  }
  static public CRecipe LargeBattery(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock");
    recipe.addItem(FComponentItem.Computer(25 * amount));
    recipe.addItem(FComponentItem.PowerCell(80 * amount));
    recipe.addItem(FComponentItem.Construction(30 * amount));
    recipe.addItem(FComponentItem.SteelPlate(80 * amount));
    return recipe;
  }
  static public CRecipe LargeRadioAntenna(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna");
    recipe.addItem(FComponentItem.RadioCommunication(40 * amount));
    recipe.addItem(FComponentItem.Computer(8 * amount));
    recipe.addItem(FComponentItem.Construction(30 * amount));
    recipe.addItem(FComponentItem.SmallTube(60 * amount));
    recipe.addItem(FComponentItem.LargeTube(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(80 * amount));
    return recipe;
  }
  static public CRecipe LargeLargeContainer(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CargoContainer/LargeBlockLargeContainer");
    recipe.addItem(FComponentItem.Computer(8 * amount));
    recipe.addItem(FComponentItem.Display(1 * amount));
    recipe.addItem(FComponentItem.Motor(20 * amount));
    recipe.addItem(FComponentItem.SmallTube(60 * amount));
    recipe.addItem(FComponentItem.MetalGrid(24 * amount));
    recipe.addItem(FComponentItem.Construction(80 * amount));
    recipe.addItem(FComponentItem.InteriorPlate(360 * amount));
    return recipe;
  }
  static public CRecipe LargeSmallContainer(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CargoContainer/LargeBlockSmallContainer");
    recipe.addItem(FComponentItem.Computer(2 * amount));
    recipe.addItem(FComponentItem.Display(1 * amount));
    recipe.addItem(FComponentItem.Motor(4 * amount));
    recipe.addItem(FComponentItem.SmallTube(20 * amount));
    recipe.addItem(FComponentItem.MetalGrid(4 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.InteriorPlate(40 * amount));
    return recipe;
  }
  static public CRecipe HeavyArmorBlock(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorBlock");
    recipe.addItem(FComponentItem.MetalGrid(50 * amount));
    recipe.addItem(FComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe ArmorBlock(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeBlockArmorBlock");
    recipe.addItem(FComponentItem.SteelPlate(25 * amount));
    return recipe;
  }
  static public CRecipe ArmorCorner(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorCorner");
    recipe.addItem(FComponentItem.SteelPlate(135 * amount));
    return recipe;
  }
  static public CRecipe ArmorInvCorner(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorInvCorner");
    recipe.addItem(FComponentItem.SteelPlate(135 * amount));
    return recipe;
  }
  static public CRecipe ArmorSide(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorSide");
    recipe.addItem(FComponentItem.SteelPlate(130 * amount));
    return recipe;
  }
  static public CRecipe ArmorCenter(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorCenter");
    recipe.addItem(FComponentItem.SteelPlate(140 * amount));
    return recipe;
  }
  static public CRecipe LargeArmorRoundCorner(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner");
    recipe.addItem(FComponentItem.SteelPlate(13 * amount));
    return recipe;
  }
  static public CRecipe SmallLight(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_InteriorLight/SmallLight");
    recipe.addItem(FComponentItem.Construction(2 * amount));
    return recipe;
  }
  static public CRecipe ConveyorTube(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ConveyorConnector/ConveyorTube");
    recipe.addItem(FComponentItem.Motor(6 * amount));
    recipe.addItem(FComponentItem.SmallTube(12 * amount));
    recipe.addItem(FComponentItem.Construction(20 * amount));
    recipe.addItem(FComponentItem.InteriorPlate(14 * amount));
    return recipe;
  }
  static public CRecipe LargeShipMergeBlock(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_MergeBlock/LargeShipMergeBlock");
    recipe.addItem(FComponentItem.Computer(2 * amount));
    recipe.addItem(FComponentItem.LargeTube(6 * amount));
    recipe.addItem(FComponentItem.Motor(2 * amount));
    recipe.addItem(FComponentItem.Construction(15 * amount));
    recipe.addItem(FComponentItem.SteelPlate(12 * amount));
    return recipe;
  }
  static public CRecipe Connector(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ShipConnector/Connector");
    recipe.addItem(FComponentItem.Computer(20 * amount));
    recipe.addItem(FComponentItem.Motor(8 * amount));
    recipe.addItem(FComponentItem.SmallTube(12 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe LargeConveyor(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Conveyor/LargeBlockConveyor");
    recipe.addItem(FComponentItem.Motor(6 * amount));
    recipe.addItem(FComponentItem.SmallTube(20 * amount));
    recipe.addItem(FComponentItem.Construction(30 * amount));
    recipe.addItem(FComponentItem.InteriorPlate(20 * amount));
    return recipe;
  }
  static public CRecipe MedicalRoom(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_MedicalRoom/LargeMedicalRoom");
    recipe.addItem(FComponentItem.Medical(15 * amount));
    recipe.addItem(FComponentItem.Computer(10 * amount));
    recipe.addItem(FComponentItem.Display(10 * amount));
    recipe.addItem(FComponentItem.LargeTube(5 * amount));
    recipe.addItem(FComponentItem.SmallTube(20 * amount));
    recipe.addItem(FComponentItem.MetalGrid(60 * amount));
    recipe.addItem(FComponentItem.Construction(80 * amount));
    recipe.addItem(FComponentItem.InteriorPlate(240 * amount));
    return recipe;
  }
  static public CRecipe SolarPanel(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_SolarPanel/LargeBlockSolarPanel");
    recipe.addItem(FComponentItem.BulletproofGlass(4 * amount));
    recipe.addItem(FComponentItem.SolarCell(32 * amount));
    recipe.addItem(FComponentItem.Computer(4 * amount));
    recipe.addItem(FComponentItem.Girder(12 * amount));
    recipe.addItem(FComponentItem.Construction(14 * amount));
    recipe.addItem(FComponentItem.SteelPlate(4 * amount));
    return recipe;
  }
  static public CRecipe AtmosphericThrust(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Thrust/LargeBlockLargeAtmosphericThrust");
    recipe.addItem(FComponentItem.Motor(1100 * amount));
    recipe.addItem(FComponentItem.MetalGrid(40 * amount));
    recipe.addItem(FComponentItem.LargeTube(50 * amount));
    recipe.addItem(FComponentItem.Construction(60 * amount));
    recipe.addItem(FComponentItem.SteelPlate(230 * amount));
    return recipe;
  }
  static public CRecipe IonThrust(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Thrust/LargeBlockLargeThrust");
    recipe.addItem(FComponentItem.Thrust(960 * amount));
    recipe.addItem(FComponentItem.LargeTube(40 * amount));
    recipe.addItem(FComponentItem.Construction(100 * amount));
    recipe.addItem(FComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe IonThrustModule(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Thrust/LargeBlockSmallModularThruster");
    recipe.addItem(FComponentItem.Thrust(80 * amount));
    recipe.addItem(FComponentItem.LargeTube(8 * amount));
    recipe.addItem(FComponentItem.Construction(60 * amount));
    recipe.addItem(FComponentItem.SteelPlate(25 * amount));
    return recipe;
  }
  static public CRecipe HydrogenThrust(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Thrust/LargeBlockLargeHydrogenThrust");
    recipe.addItem(FComponentItem.LargeTube(40 * amount));
    recipe.addItem(FComponentItem.MetalGrid(250 * amount));
    recipe.addItem(FComponentItem.Construction(180 * amount));
    recipe.addItem(FComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe LargeWelder(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ShipWelder/LargeShipWelder");
    recipe.addItem(FComponentItem.Computer(2 * amount));
    recipe.addItem(FComponentItem.Motor(2 * amount));
    recipe.addItem(FComponentItem.LargeTube(1 * amount));
    recipe.addItem(FComponentItem.Construction(30 * amount));
    recipe.addItem(FComponentItem.SteelPlate(20 * amount));
    return recipe;
  }
  static public CRecipe LargeGrinder(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ShipGrinder/LargeShipGrinder");
    recipe.addItem(FComponentItem.Computer(2 * amount));
    recipe.addItem(FComponentItem.Motor(4 * amount));
    recipe.addItem(FComponentItem.LargeTube(1 * amount));
    recipe.addItem(FComponentItem.Construction(30 * amount));
    recipe.addItem(FComponentItem.SteelPlate(20 * amount));
    return recipe;
  }
  static public CRecipe LargeDrill(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Drill/LargeBlockDrill");
    recipe.addItem(FComponentItem.Computer(5 * amount));
    recipe.addItem(FComponentItem.Motor(5 * amount));
    recipe.addItem(FComponentItem.LargeTube(12 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(300 * amount));
    return recipe;
  }
  static public CRecipe LargeOreDetector(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_OreDetector/LargeOreDetector");
    recipe.addItem(FComponentItem.Detector(20 * amount));
    recipe.addItem(FComponentItem.Computer(25 * amount));
    recipe.addItem(FComponentItem.Motor(5 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(50 * amount));
    return recipe;
  }
  static public CRecipe LargeReactor(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Reactor/LargeBlockLargeGenerator");
    recipe.addItem(FComponentItem.Computer(75 * amount));
    recipe.addItem(FComponentItem.Motor(20 * amount));
    recipe.addItem(FComponentItem.Reactor(2000 * amount));
    recipe.addItem(FComponentItem.Superconductor(100 * amount));
    recipe.addItem(FComponentItem.LargeTube(40 * amount));
    recipe.addItem(FComponentItem.MetalGrid(40 * amount));
    recipe.addItem(FComponentItem.Construction(70 * amount));
    recipe.addItem(FComponentItem.SteelPlate(1000 * amount));
    return recipe;
  }
  static public CRecipe SmallReactor(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Reactor/LargeBlockSmallGenerator");
    recipe.addItem(FComponentItem.Computer(25 * amount));
    recipe.addItem(FComponentItem.Motor(6 * amount));
    recipe.addItem(FComponentItem.Reactor(100 * amount));
    recipe.addItem(FComponentItem.Superconductor(100 * amount));
    recipe.addItem(FComponentItem.LargeTube(8 * amount));
    recipe.addItem(FComponentItem.MetalGrid(4 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(80 * amount));
    return recipe;
  }
  static public CRecipe OxygenFarm(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_OxygenFarm/LargeBlockOxygenFarm");
    recipe.addItem(FComponentItem.Computer(20 * amount));
    recipe.addItem(FComponentItem.Construction(20 * amount));
    recipe.addItem(FComponentItem.SmallTube(10 * amount));
    recipe.addItem(FComponentItem.LargeTube(20 * amount));
    recipe.addItem(FComponentItem.BulletproofGlass(100 * amount));
    recipe.addItem(FComponentItem.SteelPlate(40 * amount));
    return recipe;
  }
  // static public CRecipe GravGenSphere(int amount = 1)
  // {
  //   CRecipe recipe = new CRecipe("MyObjectBuilder_GravityGeneratorSphere/");
  //   recipe.addItem(FComponentItem.Computer(40 * amount));
  //   recipe.addItem(FComponentItem.Motor(6 * amount));
  //   recipe.addItem(FComponentItem.LargeTube(4 * amount));
  //   recipe.addItem(FComponentItem.Construction(60 * amount));
  //   recipe.addItem(FComponentItem.GravityGenerator(6 * amount));
  //   recipe.addItem(FComponentItem.SteelPlate(150 * amount));
  //   return recipe;
  // }
  static public CRecipe GravGen(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_GravityGenerator/");
    recipe.addItem(FComponentItem.Computer(40 * amount));
    recipe.addItem(FComponentItem.Motor(6 * amount));
    recipe.addItem(FComponentItem.LargeTube(4 * amount));
    recipe.addItem(FComponentItem.Construction(60 * amount));
    recipe.addItem(FComponentItem.GravityGenerator(6 * amount));
    recipe.addItem(FComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe PowerEfficiencyModule(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_UpgradeModule/LargeEnergyModule");
    recipe.addItem(FComponentItem.Motor(4 * amount));
    recipe.addItem(FComponentItem.PowerCell(20 * amount));
    recipe.addItem(FComponentItem.SmallTube(20 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(100 * amount));
    return recipe;
  }
  static public CRecipe SpeedModule(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_UpgradeModule/LargeProductivityModule");
    recipe.addItem(FComponentItem.Motor(4 * amount));
    recipe.addItem(FComponentItem.Computer(60 * amount));
    recipe.addItem(FComponentItem.SmallTube(20 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(100 * amount));
    return recipe;
  }
  static public CRecipe YieldModule(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_UpgradeModule/LargeEffectivenessModule");
    recipe.addItem(FComponentItem.Motor(4 * amount));
    recipe.addItem(FComponentItem.Superconductor(20 * amount));
    recipe.addItem(FComponentItem.SmallTube(20 * amount));
    recipe.addItem(FComponentItem.Construction(50 * amount));
    recipe.addItem(FComponentItem.SteelPlate(100 * amount));
    return recipe;
  }
  static public CRecipe Assembler(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Assembler/LargeAssembler");
    recipe.addItem(FComponentItem.Computer(160 * amount));
    recipe.addItem(FComponentItem.MetalGrid(10 * amount));
    recipe.addItem(FComponentItem.Display(10 * amount));
    recipe.addItem(FComponentItem.Motor(20 * amount));
    recipe.addItem(FComponentItem.Construction(80 * amount));
    recipe.addItem(FComponentItem.SteelPlate(140 * amount));
    return recipe;
  }
  static public CRecipe Refinery(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Refinery/LargeRefinery");
    recipe.addItem(FComponentItem.Computer(20 * amount));
    recipe.addItem(FComponentItem.MetalGrid(20 * amount));
    recipe.addItem(FComponentItem.Motor(16 * amount));
    recipe.addItem(FComponentItem.LargeTube(20 * amount));
    recipe.addItem(FComponentItem.Construction(40 * amount));
    recipe.addItem(FComponentItem.SteelPlate(1200 * amount));
    return recipe;
  }
}

public class CRecipes
{
  public CRecipes() { m_recipes = new List<CRecipe>(); }
  public void add(CRecipe recipe) { m_recipes.Add(recipe); }
  public void add(CComponentItem item) { m_recipes.Add(FRecipe.SingleItem(item)); }
  public List<CRecipe> recipes() { return m_recipes; }
  public List<CComponentItem> sourceItems()
  {
    Dictionary<EItemType, CComponentItem> tmpDict = new Dictionary<EItemType, CComponentItem>();
    foreach (CRecipe recipe in m_recipes)
    {
      foreach(CComponentItem srcItem in recipe.sourceItems())
      {
        if(!tmpDict.ContainsKey(srcItem.itemType()))
        {
          tmpDict.Add(srcItem.itemType(), new CComponentItem(srcItem.itemType(), 0));
        }
        tmpDict[srcItem.itemType()].appendAmount(srcItem.amount());
      }
    }
    return tmpDict.Values.ToList();
  }

  private List<CRecipe> m_recipes;
}
