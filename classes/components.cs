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

public class CComponentItem
{
  public CComponentItem(string    itemType, int amount = 0) { m_itemType = fromString(itemType); m_amount = amount; }
  public CComponentItem(EItemType itemType, int amount = 0) { m_itemType = itemType            ; m_amount = amount; }

  public static EItemType fromString(string itemType)
  {
         if(itemType.Contains("BulletproofGlass"))   { return EItemType.BulletproofGlass; }
    else if(itemType.Contains("Canvas"))             { return EItemType.Canvas; }
    else if(itemType.Contains("Computer"))           { return EItemType.Computer; }
    else if(itemType.Contains("Construction"))       { return EItemType.Construction; }
    else if(itemType.Contains("Detector"))           { return EItemType.Detector; }
    else if(itemType.Contains("Display"))            { return EItemType.Display; }
    else if(itemType.Contains("Explosives"))         { return EItemType.Explosives; }
    else if(itemType.Contains("Girder"))             { return EItemType.Girder; }
    else if(itemType.Contains("GravityGenerator"))   { return EItemType.GravityGenerator; }
    else if(itemType.Contains("InteriorPlate"))      { return EItemType.InteriorPlate; }
    else if(itemType.Contains("LargeTube"))          { return EItemType.LargeTube; }
    else if(itemType.Contains("Medical"))            { return EItemType.Medical; }
    else if(itemType.Contains("MetalGrid"))          { return EItemType.MetalGrid; }
    else if(itemType.Contains("Motor"))              { return EItemType.Motor; }
    else if(itemType.Contains("PowerCell"))          { return EItemType.PowerCell; }
    else if(itemType.Contains("RadioCommunication")) { return EItemType.RadioCommunication; }
    else if(itemType.Contains("Reactor"))            { return EItemType.Reactor; }
    else if(itemType.Contains("SmallTube"))          { return EItemType.SmallTube; }
    else if(itemType.Contains("SolarCell"))          { return EItemType.SolarCell; }
    else if(itemType.Contains("SteelPlate"))         { return EItemType.SteelPlate; }
    else if(itemType.Contains("Superconductor"))     { return EItemType.Superconductor; }
    else if(itemType.Contains("Thrust"))             { return EItemType.Thrust; }
    else if(itemType.Contains("ZoneChip"))           { return EItemType.ZoneChip; }
    throw new System.ArgumentException("Не знаю такой строки", itemType);
  }

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

public class FComponentItem
{
  static public CComponentItem BulletproofGlass   (int amount = 0) { return new CComponentItem(EItemType.BulletproofGlass,   amount); }
  static public CComponentItem Canvas             (int amount = 0) { return new CComponentItem(EItemType.Canvas,             amount); }
  static public CComponentItem Computer           (int amount = 0) { return new CComponentItem(EItemType.Computer,           amount); }
  static public CComponentItem Construction       (int amount = 0) { return new CComponentItem(EItemType.Construction,       amount); }
  static public CComponentItem Detector           (int amount = 0) { return new CComponentItem(EItemType.Detector,           amount); }
  static public CComponentItem Display            (int amount = 0) { return new CComponentItem(EItemType.Display,            amount); }
  static public CComponentItem Explosives         (int amount = 0) { return new CComponentItem(EItemType.Explosives,         amount); }
  static public CComponentItem Girder             (int amount = 0) { return new CComponentItem(EItemType.Girder,             amount); }
  static public CComponentItem GravityGenerator   (int amount = 0) { return new CComponentItem(EItemType.GravityGenerator,   amount); }
  static public CComponentItem InteriorPlate      (int amount = 0) { return new CComponentItem(EItemType.InteriorPlate,      amount); }
  static public CComponentItem LargeTube          (int amount = 0) { return new CComponentItem(EItemType.LargeTube,          amount); }
  static public CComponentItem Medical            (int amount = 0) { return new CComponentItem(EItemType.Medical,            amount); }
  static public CComponentItem MetalGrid          (int amount = 0) { return new CComponentItem(EItemType.MetalGrid,          amount); }
  static public CComponentItem Motor              (int amount = 0) { return new CComponentItem(EItemType.Motor,              amount); }
  static public CComponentItem PowerCell          (int amount = 0) { return new CComponentItem(EItemType.PowerCell,          amount); }
  static public CComponentItem RadioCommunication (int amount = 0) { return new CComponentItem(EItemType.RadioCommunication, amount); }
  static public CComponentItem Reactor            (int amount = 0) { return new CComponentItem(EItemType.Reactor,            amount); }
  static public CComponentItem SmallTube          (int amount = 0) { return new CComponentItem(EItemType.SmallTube,          amount); }
  static public CComponentItem SolarCell          (int amount = 0) { return new CComponentItem(EItemType.SolarCell,          amount); }
  static public CComponentItem SteelPlate         (int amount = 0) { return new CComponentItem(EItemType.SteelPlate,         amount); }
  static public CComponentItem Superconductor     (int amount = 0) { return new CComponentItem(EItemType.Superconductor,     amount); }
  static public CComponentItem Thrust             (int amount = 0) { return new CComponentItem(EItemType.Thrust,             amount); }
  static public CComponentItem ZoneChip           (int amount = 0) { return new CComponentItem(EItemType.ZoneChip,           amount); }
}
