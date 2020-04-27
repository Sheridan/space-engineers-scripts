public enum EItemType
{
  BulletproofGlass,
  Canvas,
  Computer,
  Construction,
  Detector,
  Display,
  Explosives,
  Girder,
  GravityGenerator,
  InteriorPlate,
  LargeTube,
  Medical,
  MetalGrid,
  Motor,
  PowerCell,
  RadioCommunication,
  Reactor,
  SmallTube,
  SolarCell,
  SteelPlate,
  Superconductor,
  Thrust,
  ZoneChip
}

public class CRecipeSourceItem
{
  public CRecipeSourceItem(EItemType itemType, int amount) { m_itemType = itemType; m_amount = amount; }
  public int amount() { return m_amount; }
  public void appendAmount(int amountDelta) { m_amount += amountDelta; }
  public EItemType itemType() { return m_itemType; }
  public string asComponent()
  {
    string name = "";
    switch (m_itemType)
    {
      case EItemType.BulletproofGlass:   name = "BulletproofGlass";   break;
      case EItemType.Canvas:             name = "Canvas";             break;
      case EItemType.Computer:           name = "Computer";           break;
      case EItemType.Construction:       name = "Construction";       break;
      case EItemType.Detector:           name = "Detector";           break;
      case EItemType.Display:            name = "Display";            break;
      case EItemType.Explosives:         name = "Explosives";         break;
      case EItemType.Girder:             name = "Girder";             break;
      case EItemType.GravityGenerator:   name = "GravityGenerator";   break;
      case EItemType.InteriorPlate:      name = "InteriorPlate";      break;
      case EItemType.LargeTube:          name = "LargeTube";          break;
      case EItemType.Medical:            name = "Medical";            break;
      case EItemType.MetalGrid:          name = "MetalGrid";          break;
      case EItemType.Motor:              name = "Motor";              break;
      case EItemType.PowerCell:          name = "PowerCell";          break;
      case EItemType.RadioCommunication: name = "RadioCommunication"; break;
      case EItemType.Reactor:            name = "Reactor";            break;
      case EItemType.SmallTube:          name = "SmallTube";          break;
      case EItemType.SolarCell:          name = "SolarCell";          break;
      case EItemType.SteelPlate:         name = "SteelPlate";         break;
      case EItemType.Superconductor:     name = "Superconductor";     break;
      case EItemType.Thrust:             name = "Thrust";             break;
      case EItemType.ZoneChip:           name = "ZoneChip";           break;
    }
    return $"MyObjectBuilder_Component/{name}";
  }
  public string asBlueprintDefinition()
  {
    string name = "";
    switch (m_itemType)
    {
      case EItemType.BulletproofGlass:   name = "BulletproofGlass";            break;
      case EItemType.Canvas:             name = "Canvas";                      break;
      case EItemType.Computer:           name = "ComputerComponent";           break;
      case EItemType.Construction:       name = "ConstructionComponent";       break;
      case EItemType.Detector:           name = "DetectorComponent";           break;
      case EItemType.Display:            name = "Display";                     break;
      case EItemType.Explosives:         name = "ExplosivesComponent";         break;
      case EItemType.Girder:             name = "GirderComponent";             break;
      case EItemType.GravityGenerator:   name = "GravityGeneratorComponent";   break;
      case EItemType.InteriorPlate:      name = "InteriorPlate";               break;
      case EItemType.LargeTube:          name = "LargeTube";                   break;
      case EItemType.Medical:            name = "MedicalComponent";            break;
      case EItemType.MetalGrid:          name = "MetalGrid";                   break;
      case EItemType.Motor:              name = "MotorComponent";              break;
      case EItemType.PowerCell:          name = "PowerCell";                   break;
      case EItemType.RadioCommunication: name = "RadioCommunicationComponent"; break;
      case EItemType.Reactor:            name = "ReactorComponent";            break;
      case EItemType.SmallTube:          name = "SmallTube";                   break;
      case EItemType.SolarCell:          name = "SolarCell";                   break;
      case EItemType.SteelPlate:         name = "SteelPlate";                  break;
      case EItemType.Superconductor:     name = "Superconductor";              break;
      case EItemType.Thrust:             name = "ThrustComponent";             break;
      case EItemType.ZoneChip:           name = "ZoneChip";                    break;
    }
    return $"MyObjectBuilder_BlueprintDefinition/{name}";
  }
  public MyItemType asMyItemType() { return MyItemType.Parse(asComponent()); }
  private EItemType m_itemType;
  private int m_amount;
}

public class FRecipeSourceItem
{
  static public CRecipeSourceItem SteelPlate   (int amount) { return new CRecipeSourceItem(EItemType.SteelPlate   , amount); }
  static public CRecipeSourceItem Motor        (int amount) { return new CRecipeSourceItem(EItemType.Motor        , amount); }
  static public CRecipeSourceItem Computer     (int amount) { return new CRecipeSourceItem(EItemType.Computer     , amount); }
  static public CRecipeSourceItem SmallTube    (int amount) { return new CRecipeSourceItem(EItemType.SmallTube    , amount); }
  static public CRecipeSourceItem LargeTube    (int amount) { return new CRecipeSourceItem(EItemType.LargeTube    , amount); }
  static public CRecipeSourceItem Construction (int amount) { return new CRecipeSourceItem(EItemType.Construction , amount); }
  static public CRecipeSourceItem InteriorPlate(int amount) { return new CRecipeSourceItem(EItemType.InteriorPlate, amount); }
}

public class CRecipe
{
  public CRecipe(string blueprint) { m_blueprint = blueprint; m_sourceItems = new List<CRecipeSourceItem>(); }
  public void addItem(CRecipeSourceItem item) { m_sourceItems.Add(item); }
  public string blueprint() { return m_blueprint; }
  public List<CRecipeSourceItem> sourceItems() { return m_sourceItems; }
  private string m_blueprint;
  private List<CRecipeSourceItem> m_sourceItems;
}

public class FRecipe
{
  static public CRecipe fromString(string itemString, int amount = 1)
  {
    switch (itemString)
    {
      case "MyObjectBuilder_CubeBlock/LargeBlockArmorBlock": return LargeBlockArmorBlock (amount);
      case "MyObjectBuilder_InteriorLight/SmallLight":       return SmallLight           (amount);
      case "MyObjectBuilder_ConveyorConnector/ConveyorTube": return ConveyorTube         (amount);
      case "MyObjectBuilder_MergeBlock/LargeShipMergeBlock": return LargeShipMergeBlock  (amount);
      case "MyObjectBuilder_ShipConnector/Connector":        return Connector            (amount);
      case "MyObjectBuilder_Conveyor/LargeBlockConveyor":    return LargeBlockConveyor   (amount);
    }
    throw new System.ArgumentException("Не знаю такой строки", itemString);
  }
  static public CRecipe LargeBlockArmorBlock(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeBlockArmorBlock");
    recipe.addItem(FRecipeSourceItem.SteelPlate(25 * amount));
    return recipe;
  }
  static public CRecipe SmallLight(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_InteriorLight/SmallLight");
    recipe.addItem(FRecipeSourceItem.Construction(2 * amount));
    return recipe;
  }
  static public CRecipe ConveyorTube(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ConveyorConnector/ConveyorTube");
    recipe.addItem(FRecipeSourceItem.Motor(6 * amount));
    recipe.addItem(FRecipeSourceItem.SmallTube(12 * amount));
    recipe.addItem(FRecipeSourceItem.Construction(20 * amount));
    recipe.addItem(FRecipeSourceItem.InteriorPlate(14 * amount));
    return recipe;
  }
  static public CRecipe LargeShipMergeBlock(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_MergeBlock/LargeShipMergeBlock");
    recipe.addItem(FRecipeSourceItem.Computer(2 * amount));
    recipe.addItem(FRecipeSourceItem.LargeTube(6 * amount));
    recipe.addItem(FRecipeSourceItem.Motor(2 * amount));
    recipe.addItem(FRecipeSourceItem.Construction(15 * amount));
    recipe.addItem(FRecipeSourceItem.SteelPlate(12 * amount));
    return recipe;
  }
  static public CRecipe Connector(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_ShipConnector/Connector");
    recipe.addItem(FRecipeSourceItem.Computer(20 * amount));
    recipe.addItem(FRecipeSourceItem.Motor(8 * amount));
    recipe.addItem(FRecipeSourceItem.SmallTube(12 * amount));
    recipe.addItem(FRecipeSourceItem.Construction(40 * amount));
    recipe.addItem(FRecipeSourceItem.SteelPlate(150 * amount));
    return recipe;
  }
  static public CRecipe LargeBlockConveyor(int amount = 1)
  {
    CRecipe recipe = new CRecipe("MyObjectBuilder_Conveyor/LargeBlockConveyor");
    recipe.addItem(FRecipeSourceItem.Motor(6 * amount));
    recipe.addItem(FRecipeSourceItem.SmallTube(20 * amount));
    recipe.addItem(FRecipeSourceItem.Construction(30 * amount));
    recipe.addItem(FRecipeSourceItem.InteriorPlate(20 * amount));
    return recipe;
  }
}

public class CRecipes
{
  public CRecipes() { m_recipes = new List<CRecipe>(); }
  public void add(CRecipe recipe) { m_recipes.Add(recipe); }
  public List<CRecipe> recipes() { return m_recipes; }
  public List<CRecipeSourceItem> sourceItems()
  {
    Dictionary<EItemType, CRecipeSourceItem> tmpDict = new Dictionary<EItemType, CRecipeSourceItem>();
    foreach (CRecipe recipe in m_recipes)
    {
      foreach(CRecipeSourceItem srcItem in recipe.sourceItems())
      {
        if(!tmpDict.ContainsKey(srcItem.itemType()))
        {
          tmpDict.Add(srcItem.itemType(), new CRecipeSourceItem(srcItem.itemType(), 0));
        }
        tmpDict[srcItem.itemType()].appendAmount(srcItem.amount());
      }
    }
    return tmpDict.Values.ToList();
  }
  private List<CRecipe> m_recipes;
}
