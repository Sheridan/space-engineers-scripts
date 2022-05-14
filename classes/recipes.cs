public class CComponentItem
{
  public CComponentItem(MyItemType itemType, int amount = 0) { m_itemType = itemType; m_amount = amount; }
  public  void       appendAmount(int delta) { m_amount += delta; }
  public  int        amount      (         ) { return m_amount; }
  public  MyItemType itemType    (         ) { return m_itemType; }
  private MyItemType m_itemType;
  private int        m_amount;

  public static CComponentItem BulletproofGlass   (int amount) { return new CComponentItem(MyItemType.MakeComponent("BulletproofGlass"),   amount); }
  public static CComponentItem Canvas             (int amount) { return new CComponentItem(MyItemType.MakeComponent("Canvas"),             amount); }
  public static CComponentItem Computer           (int amount) { return new CComponentItem(MyItemType.MakeComponent("Computer"),           amount); }
  public static CComponentItem Construction       (int amount) { return new CComponentItem(MyItemType.MakeComponent("Construction"),       amount); }
  public static CComponentItem Detector           (int amount) { return new CComponentItem(MyItemType.MakeComponent("Detector"),           amount); }
  public static CComponentItem Display            (int amount) { return new CComponentItem(MyItemType.MakeComponent("Display"),            amount); }
  public static CComponentItem Explosives         (int amount) { return new CComponentItem(MyItemType.MakeComponent("Explosives"),         amount); }
  public static CComponentItem Girder             (int amount) { return new CComponentItem(MyItemType.MakeComponent("Girder"),             amount); }
  public static CComponentItem GravityGenerator   (int amount) { return new CComponentItem(MyItemType.MakeComponent("GravityGenerator"),   amount); }
  public static CComponentItem InteriorPlate      (int amount) { return new CComponentItem(MyItemType.MakeComponent("InteriorPlate"),      amount); }
  public static CComponentItem LargeTube          (int amount) { return new CComponentItem(MyItemType.MakeComponent("LargeTube"),          amount); }
  public static CComponentItem Medical            (int amount) { return new CComponentItem(MyItemType.MakeComponent("Medical"),            amount); }
  public static CComponentItem MetalGrid          (int amount) { return new CComponentItem(MyItemType.MakeComponent("MetalGrid"),          amount); }
  public static CComponentItem Motor              (int amount) { return new CComponentItem(MyItemType.MakeComponent("Motor"),              amount); }
  public static CComponentItem PowerCell          (int amount) { return new CComponentItem(MyItemType.MakeComponent("PowerCell"),          amount); }
  public static CComponentItem RadioCommunication (int amount) { return new CComponentItem(MyItemType.MakeComponent("RadioCommunication"), amount); }
  public static CComponentItem Reactor            (int amount) { return new CComponentItem(MyItemType.MakeComponent("Reactor"),            amount); }
  public static CComponentItem SmallTube          (int amount) { return new CComponentItem(MyItemType.MakeComponent("SmallTube"),          amount); }
  public static CComponentItem SolarCell          (int amount) { return new CComponentItem(MyItemType.MakeComponent("SolarCell"),          amount); }
  public static CComponentItem SteelPlate         (int amount) { return new CComponentItem(MyItemType.MakeComponent("SteelPlate"),         amount); }
  public static CComponentItem Superconductor     (int amount) { return new CComponentItem(MyItemType.MakeComponent("Superconductor"),     amount); }
  public static CComponentItem Thrust             (int amount) { return new CComponentItem(MyItemType.MakeComponent("Thrust"),             amount); }
  public static CComponentItem ZoneChip           (int amount) { return new CComponentItem(MyItemType.MakeComponent("ZoneChip"),           amount); }

  public static CComponentItem NATO_5p56x45mm                      (int amount) { return new CComponentItem(MyItemType.MakeAmmo("NATO_5p56x45mm"),                      amount); }
  public static CComponentItem LargeCalibreAmmo                    (int amount) { return new CComponentItem(MyItemType.MakeAmmo("LargeCalibreAmmo"),                    amount); }
  public static CComponentItem MediumCalibreAmmo                   (int amount) { return new CComponentItem(MyItemType.MakeAmmo("MediumCalibreAmmo"),                   amount); }
  public static CComponentItem AutocannonClip                      (int amount) { return new CComponentItem(MyItemType.MakeAmmo("AutocannonClip"),                      amount); }
  public static CComponentItem NATO_25x184mm                       (int amount) { return new CComponentItem(MyItemType.MakeAmmo("NATO_25x184mm"),                       amount); }
  public static CComponentItem LargeRailgunAmmo                    (int amount) { return new CComponentItem(MyItemType.MakeAmmo("LargeRailgunAmmo"),                    amount); }
  public static CComponentItem Missile200mm                        (int amount) { return new CComponentItem(MyItemType.MakeAmmo("Missile200mm"),                        amount); }
  public static CComponentItem AutomaticRifleGun_Mag_20rd          (int amount) { return new CComponentItem(MyItemType.MakeAmmo("AutomaticRifleGun_Mag_20rd"),          amount); }
  public static CComponentItem UltimateAutomaticRifleGun_Mag_30rd  (int amount) { return new CComponentItem(MyItemType.MakeAmmo("UltimateAutomaticRifleGun_Mag_30rd"),  amount); }
  public static CComponentItem RapidFireAutomaticRifleGun_Mag_50rd (int amount) { return new CComponentItem(MyItemType.MakeAmmo("RapidFireAutomaticRifleGun_Mag_50rd"), amount); }
  public static CComponentItem PreciseAutomaticRifleGun_Mag_5rd    (int amount) { return new CComponentItem(MyItemType.MakeAmmo("PreciseAutomaticRifleGun_Mag_5rd"),    amount); }
  public static CComponentItem SemiAutoPistolMagazine              (int amount) { return new CComponentItem(MyItemType.MakeAmmo("SemiAutoPistolMagazine"),              amount); }
  public static CComponentItem ElitePistolMagazine                 (int amount) { return new CComponentItem(MyItemType.MakeAmmo("ElitePistolMagazine"),                 amount); }
  public static CComponentItem FullAutoPistolMagazine              (int amount) { return new CComponentItem(MyItemType.MakeAmmo("FullAutoPistolMagazine"),              amount); }
  public static CComponentItem SmallRailgunAmmo                    (int amount) { return new CComponentItem(MyItemType.MakeAmmo("SmallRailgunAmmo"),                    amount); }

  public static CComponentItem AngleGrinder4Item            (int amount) { return new CComponentItem(MyItemType.MakeTool("AngleGrinder4Item"),            amount); }
  public static CComponentItem HandDrill4Item               (int amount) { return new CComponentItem(MyItemType.MakeTool("HandDrill4Item"),               amount); }
  public static CComponentItem Welder4Item                  (int amount) { return new CComponentItem(MyItemType.MakeTool("Welder4Item"),                  amount); }
  public static CComponentItem AngleGrinder2Item            (int amount) { return new CComponentItem(MyItemType.MakeTool("AngleGrinder2Item"),            amount); }
  public static CComponentItem HandDrill2Item               (int amount) { return new CComponentItem(MyItemType.MakeTool("HandDrill2Item"),               amount); }
  public static CComponentItem Welder2Item                  (int amount) { return new CComponentItem(MyItemType.MakeTool("Welder2Item"),                  amount); }
  public static CComponentItem AngleGrinderItem             (int amount) { return new CComponentItem(MyItemType.MakeTool("AngleGrinderItem"),             amount); }
  public static CComponentItem HandDrillItem                (int amount) { return new CComponentItem(MyItemType.MakeTool("HandDrillItem"),                amount); }
  public static CComponentItem AutomaticRifleItem           (int amount) { return new CComponentItem(MyItemType.MakeTool("AutomaticRifleItem"),           amount); }
  public static CComponentItem UltimateAutomaticRifleItem   (int amount) { return new CComponentItem(MyItemType.MakeTool("UltimateAutomaticRifleItem"),   amount); }
  public static CComponentItem RapidFireAutomaticRifleItem  (int amount) { return new CComponentItem(MyItemType.MakeTool("RapidFireAutomaticRifleItem"),  amount); }
  public static CComponentItem PreciseAutomaticRifleItem    (int amount) { return new CComponentItem(MyItemType.MakeTool("PreciseAutomaticRifleItem"),    amount); }
  public static CComponentItem AdvancedHandHeldLauncherItem (int amount) { return new CComponentItem(MyItemType.MakeTool("AdvancedHandHeldLauncherItem"), amount); }
  public static CComponentItem AngleGrinder3Item            (int amount) { return new CComponentItem(MyItemType.MakeTool("AngleGrinder3Item"),            amount); }
  public static CComponentItem HandDrill3Item               (int amount) { return new CComponentItem(MyItemType.MakeTool("HandDrill3Item"),               amount); }
  public static CComponentItem Welder3Item                  (int amount) { return new CComponentItem(MyItemType.MakeTool("Welder3Item"),                  amount); }
  public static CComponentItem BasicHandHeldLauncherItem    (int amount) { return new CComponentItem(MyItemType.MakeTool("BasicHandHeldLauncherItem"),    amount); }
  public static CComponentItem SemiAutoPistolItem           (int amount) { return new CComponentItem(MyItemType.MakeTool("SemiAutoPistolItem"),           amount); }
  public static CComponentItem ElitePistolItem              (int amount) { return new CComponentItem(MyItemType.MakeTool("ElitePistolItem"),              amount); }
  public static CComponentItem FullAutoPistolItem           (int amount) { return new CComponentItem(MyItemType.MakeTool("FullAutoPistolItem"),           amount); }
  public static CComponentItem WelderItem                   (int amount) { return new CComponentItem(MyItemType.MakeTool("WelderItem"),                   amount); }
}

public class CRecipe
{
  public CRecipe()                 { m_blueprint = "none"   ; m_sourceItems = new List<CComponentItem>(); }
  public CRecipe(string blueprint) { m_blueprint = blueprint; m_sourceItems = new List<CComponentItem>(); }
  public void addItem(CComponentItem item) { m_sourceItems.Add(item); }
  public string blueprint() { return m_blueprint; }
  public List<CComponentItem> sourceItems() { return m_sourceItems; }
  private string m_blueprint;
  private List<CComponentItem> m_sourceItems;
  public IEnumerator GetEnumerator() { foreach(CComponentItem i in m_sourceItems) { yield return i; } }
}

public class FRecipe
{

  static public CRecipe SingleItem(CComponentItem item, int amount = 1)
  {
    CRecipe recipe = new CRecipe(item.asBlueprintDefinition());
    recipe.addItem(item);
    return recipe;
  }

  static public CRecipe LargePistonBase(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ExtendedPistonBase/LargePistonBase");
    recipe.addItem(CComponentItem.Computer(2 * amount));
    recipe.addItem(CComponentItem.Motor(4 * amount));
    recipe.addItem(CComponentItem.LargeTube((8+4) * amount)); // + top
    recipe.addItem(CComponentItem.Construction(10 * amount));
    recipe.addItem(CComponentItem.SteelPlate((10+15) * amount)); // + top
    return recipe;
  }
  static public CRecipe Wheel5x5(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Wheel/Wheel5x5");
    recipe.addItem(CComponentItem.LargeTube(8 * amount));
    recipe.addItem(CComponentItem.Construction(30 * amount));
    recipe.addItem(CComponentItem.SteelPlate(16 * amount));
    return recipe;
  }
  static public CRecipe Suspension5x5(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_MotorSuspension/Suspension5x5");
    recipe.addItem(CComponentItem.Motor(20 * amount));
    recipe.addItem(CComponentItem.SmallTube(30 * amount));
    recipe.addItem(CComponentItem.LargeTube(20 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(70 * amount));
    return recipe;
  }
  static public CRecipe Window3x3Flat(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/Window3x3Flat");
    recipe.addItem(CComponentItem.BulletproofGlass(196 * amount));
    recipe.addItem(CComponentItem.Girder(40 * amount));
    return recipe;
  }
  static public CRecipe LargeGyro(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Gyro/LargeBlockGyro");
    recipe.addItem(CComponentItem.Computer(5 * amount));
    recipe.addItem(CComponentItem.Motor(4 * amount));
    recipe.addItem(CComponentItem.MetalGrid(50 * amount));
    recipe.addItem(CComponentItem.LargeTube(4 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(600 * amount));
    return recipe;
  }
  static public CRecipe LargeWindTurbine(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_WindTurbine/LargeBlockWindTurbine");
    recipe.addItem(CComponentItem.Computer(2 * amount));
    recipe.addItem(CComponentItem.Girder(24 * amount));
    recipe.addItem(CComponentItem.Construction(20 * amount));
    recipe.addItem(CComponentItem.Motor(8 * amount));
    recipe.addItem(CComponentItem.InteriorPlate(40 * amount));
    return recipe;
  }
  static public CRecipe LargeBattery(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock");
    recipe.addItem(CComponentItem.Computer(25 * amount));
    recipe.addItem(CComponentItem.PowerCell(80 * amount));
    recipe.addItem(CComponentItem.Construction(30 * amount));
    recipe.addItem(CComponentItem.SteelPlate(80 * amount));
    return recipe;
  }
  static public CRecipe LargeRadioAntenna(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna");
    recipe.addItem(CComponentItem.RadioCommunication(40 * amount));
    recipe.addItem(CComponentItem.Computer(8 * amount));
    recipe.addItem(CComponentItem.Construction(30 * amount));
    recipe.addItem(CComponentItem.SmallTube(60 * amount));
    recipe.addItem(CComponentItem.LargeTube(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(80 * amount));
    return recipe;
  }
  static public CRecipe LargeLargeContainer(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CargoContainer/LargeBlockLargeContainer");
    recipe.addItem(CComponentItem.Computer(8 * amount));
    recipe.addItem(CComponentItem.Display(1 * amount));
    recipe.addItem(CComponentItem.Motor(20 * amount));
    recipe.addItem(CComponentItem.SmallTube(60 * amount));
    recipe.addItem(CComponentItem.MetalGrid(24 * amount));
    recipe.addItem(CComponentItem.Construction(80 * amount));
    recipe.addItem(CComponentItem.InteriorPlate(360 * amount));
    return recipe;
  }
  static public CRecipe LargeSmallContainer(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CargoContainer/LargeBlockSmallContainer");
    recipe.addItem(CComponentItem.Computer(2 * amount));
    recipe.addItem(CComponentItem.Display(1 * amount));
    recipe.addItem(CComponentItem.Motor(4 * amount));
    recipe.addItem(CComponentItem.SmallTube(20 * amount));
    recipe.addItem(CComponentItem.MetalGrid(4 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.InteriorPlate(40 * amount));
    return recipe;
  }
  static public CRecipe HeavyArmorBlock(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorBlock");
    recipe.addItem(CComponentItem.MetalGrid(50 * amount));
    recipe.addItem(CComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe ArmorBlock(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeBlockArmorBlock");
    recipe.addItem(CComponentItem.SteelPlate(25 * amount));
    return recipe;
  }
  static public CRecipe ArmorCorner(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorCorner");
    recipe.addItem(CComponentItem.SteelPlate(135 * amount));
    return recipe;
  }
  static public CRecipe ArmorInvCorner(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorInvCorner");
    recipe.addItem(CComponentItem.SteelPlate(135 * amount));
    return recipe;
  }
  static public CRecipe ArmorSide(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorSide");
    recipe.addItem(CComponentItem.SteelPlate(130 * amount));
    return recipe;
  }
  static public CRecipe ArmorCenter(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorCenter");
    recipe.addItem(CComponentItem.SteelPlate(140 * amount));
    return recipe;
  }
  static public CRecipe LargeArmorRoundCorner(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner");
    recipe.addItem(CComponentItem.SteelPlate(13 * amount));
    return recipe;
  }
  static public CRecipe SmallLight(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_InteriorLight/SmallLight");
    recipe.addItem(CComponentItem.Construction(2 * amount));
    return recipe;
  }
  static public CRecipe ConveyorTube(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ConveyorConnector/ConveyorTube");
    recipe.addItem(CComponentItem.Motor(6 * amount));
    recipe.addItem(CComponentItem.SmallTube(12 * amount));
    recipe.addItem(CComponentItem.Construction(20 * amount));
    recipe.addItem(CComponentItem.InteriorPlate(14 * amount));
    return recipe;
  }
  static public CRecipe LargeShipMergeBlock(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_MergeBlock/LargeShipMergeBlock");
    recipe.addItem(CComponentItem.Computer(2 * amount));
    recipe.addItem(CComponentItem.LargeTube(6 * amount));
    recipe.addItem(CComponentItem.Motor(2 * amount));
    recipe.addItem(CComponentItem.Construction(15 * amount));
    recipe.addItem(CComponentItem.SteelPlate(12 * amount));
    return recipe;
  }
  static public CRecipe Connector(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ShipConnector/Connector");
    recipe.addItem(CComponentItem.Computer(20 * amount));
    recipe.addItem(CComponentItem.Motor(8 * amount));
    recipe.addItem(CComponentItem.SmallTube(12 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe LargeConveyor(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Conveyor/LargeBlockConveyor");
    recipe.addItem(CComponentItem.Motor(6 * amount));
    recipe.addItem(CComponentItem.SmallTube(20 * amount));
    recipe.addItem(CComponentItem.Construction(30 * amount));
    recipe.addItem(CComponentItem.InteriorPlate(20 * amount));
    return recipe;
  }
  static public CRecipe MedicalRoom(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_MedicalRoom/LargeMedicalRoom");
    recipe.addItem(CComponentItem.Medical(15 * amount));
    recipe.addItem(CComponentItem.Computer(10 * amount));
    recipe.addItem(CComponentItem.Display(10 * amount));
    recipe.addItem(CComponentItem.LargeTube(5 * amount));
    recipe.addItem(CComponentItem.SmallTube(20 * amount));
    recipe.addItem(CComponentItem.MetalGrid(60 * amount));
    recipe.addItem(CComponentItem.Construction(80 * amount));
    recipe.addItem(CComponentItem.InteriorPlate(240 * amount));
    return recipe;
  }
  static public CRecipe SolarPanel(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_SolarPanel/LargeBlockSolarPanel");
    recipe.addItem(CComponentItem.BulletproofGlass(4 * amount));
    recipe.addItem(CComponentItem.SolarCell(32 * amount));
    recipe.addItem(CComponentItem.Computer(4 * amount));
    recipe.addItem(CComponentItem.Girder(12 * amount));
    recipe.addItem(CComponentItem.Construction(14 * amount));
    recipe.addItem(CComponentItem.SteelPlate(4 * amount));
    return recipe;
  }
  static public CRecipe AtmosphericThrust(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Thrust/LargeBlockLargeAtmosphericThrust");
    recipe.addItem(CComponentItem.Motor(1100 * amount));
    recipe.addItem(CComponentItem.MetalGrid(40 * amount));
    recipe.addItem(CComponentItem.LargeTube(50 * amount));
    recipe.addItem(CComponentItem.Construction(60 * amount));
    recipe.addItem(CComponentItem.SteelPlate(230 * amount));
    return recipe;
  }
  static public CRecipe IonThrust(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Thrust/LargeBlockLargeThrust");
    recipe.addItem(CComponentItem.Thrust(960 * amount));
    recipe.addItem(CComponentItem.LargeTube(40 * amount));
    recipe.addItem(CComponentItem.Construction(100 * amount));
    recipe.addItem(CComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe IonThrustModule(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Thrust/LargeBlockSmallModularThruster");
    recipe.addItem(CComponentItem.Thrust(80 * amount));
    recipe.addItem(CComponentItem.LargeTube(8 * amount));
    recipe.addItem(CComponentItem.Construction(60 * amount));
    recipe.addItem(CComponentItem.SteelPlate(25 * amount));
    return recipe;
  }
  static public CRecipe HydrogenThrust(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Thrust/LargeBlockLargeHydrogenThrust");
    recipe.addItem(CComponentItem.LargeTube(40 * amount));
    recipe.addItem(CComponentItem.MetalGrid(250 * amount));
    recipe.addItem(CComponentItem.Construction(180 * amount));
    recipe.addItem(CComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe LargeWelder(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ShipWelder/LargeShipWelder");
    recipe.addItem(CComponentItem.Computer(2 * amount));
    recipe.addItem(CComponentItem.Motor(2 * amount));
    recipe.addItem(CComponentItem.LargeTube(1 * amount));
    recipe.addItem(CComponentItem.Construction(30 * amount));
    recipe.addItem(CComponentItem.SteelPlate(20 * amount));
    return recipe;
  }
  static public CRecipe LargeGrinder(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ShipGrinder/LargeShipGrinder");
    recipe.addItem(CComponentItem.Computer(2 * amount));
    recipe.addItem(CComponentItem.Motor(4 * amount));
    recipe.addItem(CComponentItem.LargeTube(1 * amount));
    recipe.addItem(CComponentItem.Construction(30 * amount));
    recipe.addItem(CComponentItem.SteelPlate(20 * amount));
    return recipe;
  }
  static public CRecipe LargeDrill(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Drill/LargeBlockDrill");
    recipe.addItem(CComponentItem.Computer(5 * amount));
    recipe.addItem(CComponentItem.Motor(5 * amount));
    recipe.addItem(CComponentItem.LargeTube(12 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(300 * amount));
    return recipe;
  }
  static public CRecipe LargeOreDetector(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_OreDetector/LargeOreDetector");
    recipe.addItem(CComponentItem.Detector(20 * amount));
    recipe.addItem(CComponentItem.Computer(25 * amount));
    recipe.addItem(CComponentItem.Motor(5 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(50 * amount));
    return recipe;
  }
  static public CRecipe LargeReactor(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Reactor/LargeBlockLargeGenerator");
    recipe.addItem(CComponentItem.Computer(75 * amount));
    recipe.addItem(CComponentItem.Motor(20 * amount));
    recipe.addItem(CComponentItem.Reactor(2000 * amount));
    recipe.addItem(CComponentItem.Superconductor(100 * amount));
    recipe.addItem(CComponentItem.LargeTube(40 * amount));
    recipe.addItem(CComponentItem.MetalGrid(40 * amount));
    recipe.addItem(CComponentItem.Construction(70 * amount));
    recipe.addItem(CComponentItem.SteelPlate(1000 * amount));
    return recipe;
  }
  static public CRecipe SmallReactor(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Reactor/LargeBlockSmallGenerator");
    recipe.addItem(CComponentItem.Computer(25 * amount));
    recipe.addItem(CComponentItem.Motor(6 * amount));
    recipe.addItem(CComponentItem.Reactor(100 * amount));
    recipe.addItem(CComponentItem.Superconductor(100 * amount));
    recipe.addItem(CComponentItem.LargeTube(8 * amount));
    recipe.addItem(CComponentItem.MetalGrid(4 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(80 * amount));
    return recipe;
  }
  static public CRecipe OxygenFarm(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_OxygenFarm/LargeBlockOxygenFarm");
    recipe.addItem(CComponentItem.Computer(20 * amount));
    recipe.addItem(CComponentItem.Construction(20 * amount));
    recipe.addItem(CComponentItem.SmallTube(10 * amount));
    recipe.addItem(CComponentItem.LargeTube(20 * amount));
    recipe.addItem(CComponentItem.BulletproofGlass(100 * amount));
    recipe.addItem(CComponentItem.SteelPlate(40 * amount));
    return recipe;
  }
  // static public CRecipe GravGenSphere(int amount = 1)
  // {
  //   CRecipe recipe = new CRecipe("MyObjectBuilder_GravityGeneratorSphere/");
  //   recipe.addItem(CComponentItem.Computer(40 * amount));
  //   recipe.addItem(CComponentItem.Motor(6 * amount));
  //   recipe.addItem(CComponentItem.LargeTube(4 * amount));
  //   recipe.addItem(CComponentItem.Construction(60 * amount));
  //   recipe.addItem(CComponentItem.GravityGenerator(6 * amount));
  //   recipe.addItem(CComponentItem.SteelPlate(150 * amount));
  //   return recipe;
  // }
  static public CRecipe GravGen(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_GravityGenerator/");
    recipe.addItem(CComponentItem.Computer(40 * amount));
    recipe.addItem(CComponentItem.Motor(6 * amount));
    recipe.addItem(CComponentItem.LargeTube(4 * amount));
    recipe.addItem(CComponentItem.Construction(60 * amount));
    recipe.addItem(CComponentItem.GravityGenerator(6 * amount));
    recipe.addItem(CComponentItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe PowerEfficiencyModule(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_UpgradeModule/LargeEnergyModule");
    recipe.addItem(CComponentItem.Motor(4 * amount));
    recipe.addItem(CComponentItem.PowerCell(20 * amount));
    recipe.addItem(CComponentItem.SmallTube(20 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(100 * amount));
    return recipe;
  }
  static public CRecipe SpeedModule(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_UpgradeModule/LargeProductivityModule");
    recipe.addItem(CComponentItem.Motor(4 * amount));
    recipe.addItem(CComponentItem.Computer(60 * amount));
    recipe.addItem(CComponentItem.SmallTube(20 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(100 * amount));
    return recipe;
  }
  static public CRecipe YieldModule(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_UpgradeModule/LargeEffectivenessModule");
    recipe.addItem(CComponentItem.Motor(4 * amount));
    recipe.addItem(CComponentItem.Superconductor(20 * amount));
    recipe.addItem(CComponentItem.SmallTube(20 * amount));
    recipe.addItem(CComponentItem.Construction(50 * amount));
    recipe.addItem(CComponentItem.SteelPlate(100 * amount));
    return recipe;
  }
  static public CRecipe Assembler(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Assembler/LargeAssembler");
    recipe.addItem(CComponentItem.Computer(160 * amount));
    recipe.addItem(CComponentItem.MetalGrid(10 * amount));
    recipe.addItem(CComponentItem.Display(10 * amount));
    recipe.addItem(CComponentItem.Motor(20 * amount));
    recipe.addItem(CComponentItem.Construction(80 * amount));
    recipe.addItem(CComponentItem.SteelPlate(140 * amount));
    return recipe;
  }
  static public CRecipe Refinery(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Refinery/LargeRefinery");
    recipe.addItem(CComponentItem.Computer(20 * amount));
    recipe.addItem(CComponentItem.MetalGrid(20 * amount));
    recipe.addItem(CComponentItem.Motor(16 * amount));
    recipe.addItem(CComponentItem.LargeTube(20 * amount));
    recipe.addItem(CComponentItem.Construction(40 * amount));
    recipe.addItem(CComponentItem.SteelPlate(1200 * amount));
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
    CRecipe result = new CRecipe();
    foreach (CRecipe recipe in m_recipes)
    {
      foreach(CComponentItem srcItem in recipe)
      {

      }
    }


    Dictionary<MyItemType, CComponentItem> tmpDict = new Dictionary<MyItemType, CComponentItem>();
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
