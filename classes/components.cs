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
  ZoneChip,
  // ammo
  NATO_5p56x45mm,
  LargeCalibreAmmo,
  MediumCalibreAmmo,
  AutocannonClip,
  NATO_25x184mm,
  LargeRailgunAmmo,
  Missile200mm,
  AutomaticRifleGun_Mag_20rd,
  UltimateAutomaticRifleGun_Mag_30rd,
  RapidFireAutomaticRifleGun_Mag_50rd,
  PreciseAutomaticRifleGun_Mag_5rd,
  SemiAutoPistolMagazine,
  ElitePistolMagazine,
  FullAutoPistolMagazine,
  SmallRailgunAmmo
}

public class CComponentItem
{
  public CComponentItem(string    itemType, int amount = 0) { m_itemType = fromString(itemType); m_amount = amount; }
  public CComponentItem(EItemType itemType, int amount = 0) { m_itemType = itemType            ; m_amount = amount; }

  public static EItemType fromString(string itemType)
  {
         if(itemType.Contains("BulletproofGlass"))                    { return EItemType.BulletproofGlass; }
    else if(itemType.Contains("Canvas"))                              { return EItemType.Canvas; }
    else if(itemType.Contains("Computer"))                            { return EItemType.Computer; }
    else if(itemType.Contains("Construction"))                        { return EItemType.Construction; }
    else if(itemType.Contains("Detector"))                            { return EItemType.Detector; }
    else if(itemType.Contains("Display"))                             { return EItemType.Display; }
    else if(itemType.Contains("Explosives"))                          { return EItemType.Explosives; }
    else if(itemType.Contains("Girder"))                              { return EItemType.Girder; }
    else if(itemType.Contains("GravityGenerator"))                    { return EItemType.GravityGenerator; }
    else if(itemType.Contains("InteriorPlate"))                       { return EItemType.InteriorPlate; }
    else if(itemType.Contains("LargeTube"))                           { return EItemType.LargeTube; }
    else if(itemType.Contains("Medical"))                             { return EItemType.Medical; }
    else if(itemType.Contains("MetalGrid"))                           { return EItemType.MetalGrid; }
    else if(itemType.Contains("Motor"))                               { return EItemType.Motor; }
    else if(itemType.Contains("PowerCell"))                           { return EItemType.PowerCell; }
    else if(itemType.Contains("RadioCommunication"))                  { return EItemType.RadioCommunication; }
    else if(itemType.Contains("Reactor"))                             { return EItemType.Reactor; }
    else if(itemType.Contains("SmallTube"))                           { return EItemType.SmallTube; }
    else if(itemType.Contains("SolarCell"))                           { return EItemType.SolarCell; }
    else if(itemType.Contains("SteelPlate"))                          { return EItemType.SteelPlate; }
    else if(itemType.Contains("Superconductor"))                      { return EItemType.Superconductor; }
    else if(itemType.Contains("Thrust"))                              { return EItemType.Thrust; }
    else if(itemType.Contains("ZoneChip"))                            { return EItemType.ZoneChip; }
    // ammo
    else if(itemType.Contains("NATO_5p56x45mm"))                      { return EItemType.NATO_5p56x45mm;                      }
    else if(itemType.Contains("LargeCalibreAmmo"))                    { return EItemType.LargeCalibreAmmo;                    }
    else if(itemType.Contains("MediumCalibreAmmo"))                   { return EItemType.MediumCalibreAmmo;                   }
    else if(itemType.Contains("AutocannonClip"))                      { return EItemType.AutocannonClip;                      }
    else if(itemType.Contains("NATO_25x184mm"))                       { return EItemType.NATO_25x184mm;                       }
    else if(itemType.Contains("LargeRailgunAmmo"))                    { return EItemType.LargeRailgunAmmo;                    }
    else if(itemType.Contains("Missile200mm"))                        { return EItemType.Missile200mm;                        }
    else if(itemType.Contains("AutomaticRifleGun_Mag_20rd"))          { return EItemType.AutomaticRifleGun_Mag_20rd;          }
    else if(itemType.Contains("UltimateAutomaticRifleGun_Mag_30rd"))  { return EItemType.UltimateAutomaticRifleGun_Mag_30rd;  }
    else if(itemType.Contains("RapidFireAutomaticRifleGun_Mag_50rd")) { return EItemType.RapidFireAutomaticRifleGun_Mag_50rd; }
    else if(itemType.Contains("PreciseAutomaticRifleGun_Mag_5rd"))    { return EItemType.PreciseAutomaticRifleGun_Mag_5rd;    }
    else if(itemType.Contains("SemiAutoPistolMagazine"))              { return EItemType.SemiAutoPistolMagazine;              }
    else if(itemType.Contains("ElitePistolMagazine"))                 { return EItemType.ElitePistolMagazine;                 }
    else if(itemType.Contains("FullAutoPistolMagazine"))              { return EItemType.FullAutoPistolMagazine;              }
    else if(itemType.Contains("SmallRailgunAmmo"))                    { return EItemType.SmallRailgunAmmo;                    }
    throw new System.ArgumentException("Не знаю такой строки", itemType);
  }

  public int amount() { return m_amount; }
  public void appendAmount(int amountDelta) { m_amount += amountDelta; }
  public EItemType itemType() { return m_itemType; }
  public string asComponent()
  {
    string name = "";
    string iSType = "";
    switch (m_itemType)
    {
      case EItemType.BulletproofGlass:   { iSType = "Component"; name = "BulletproofGlass";   } break;
      case EItemType.Canvas:             { iSType = "Component"; name = "Canvas";             } break;
      case EItemType.Computer:           { iSType = "Component"; name = "Computer";           } break;
      case EItemType.Construction:       { iSType = "Component"; name = "Construction";       } break;
      case EItemType.Detector:           { iSType = "Component"; name = "Detector";           } break;
      case EItemType.Display:            { iSType = "Component"; name = "Display";            } break;
      case EItemType.Explosives:         { iSType = "Component"; name = "Explosives";         } break;
      case EItemType.Girder:             { iSType = "Component"; name = "Girder";             } break;
      case EItemType.GravityGenerator:   { iSType = "Component"; name = "GravityGenerator";   } break;
      case EItemType.InteriorPlate:      { iSType = "Component"; name = "InteriorPlate";      } break;
      case EItemType.LargeTube:          { iSType = "Component"; name = "LargeTube";          } break;
      case EItemType.Medical:            { iSType = "Component"; name = "Medical";            } break;
      case EItemType.MetalGrid:          { iSType = "Component"; name = "MetalGrid";          } break;
      case EItemType.Motor:              { iSType = "Component"; name = "Motor";              } break;
      case EItemType.PowerCell:          { iSType = "Component"; name = "PowerCell";          } break;
      case EItemType.RadioCommunication: { iSType = "Component"; name = "RadioCommunication"; } break;
      case EItemType.Reactor:            { iSType = "Component"; name = "Reactor";            } break;
      case EItemType.SmallTube:          { iSType = "Component"; name = "SmallTube";          } break;
      case EItemType.SolarCell:          { iSType = "Component"; name = "SolarCell";          } break;
      case EItemType.SteelPlate:         { iSType = "Component"; name = "SteelPlate";         } break;
      case EItemType.Superconductor:     { iSType = "Component"; name = "Superconductor";     } break;
      case EItemType.Thrust:             { iSType = "Component"; name = "Thrust";             } break;
      case EItemType.ZoneChip:           { iSType = "Component"; name = "ZoneChip";           } break;
      // ammo
      case EItemType.NATO_5p56x45mm:                      { iSType = "AmmoMagazine"; name = "NATO_5p56x45mm";                      } break;
      case EItemType.LargeCalibreAmmo:                    { iSType = "AmmoMagazine"; name = "LargeCalibreAmmo";                    } break;
      case EItemType.MediumCalibreAmmo:                   { iSType = "AmmoMagazine"; name = "MediumCalibreAmmo";                   } break;
      case EItemType.AutocannonClip:                      { iSType = "AmmoMagazine"; name = "AutocannonClip";                      } break;
      case EItemType.NATO_25x184mm:                       { iSType = "AmmoMagazine"; name = "NATO_25x184mm";                       } break;
      case EItemType.LargeRailgunAmmo:                    { iSType = "AmmoMagazine"; name = "LargeRailgunAmmo";                    } break;
      case EItemType.Missile200mm:                        { iSType = "AmmoMagazine"; name = "Missile200mm";                        } break;
      case EItemType.AutomaticRifleGun_Mag_20rd:          { iSType = "AmmoMagazine"; name = "AutomaticRifleGun_Mag_20rd";          } break;
      case EItemType.UltimateAutomaticRifleGun_Mag_30rd:  { iSType = "AmmoMagazine"; name = "UltimateAutomaticRifleGun_Mag_30rd";  } break;
      case EItemType.RapidFireAutomaticRifleGun_Mag_50rd: { iSType = "AmmoMagazine"; name = "RapidFireAutomaticRifleGun_Mag_50rd"; } break;
      case EItemType.PreciseAutomaticRifleGun_Mag_5rd:    { iSType = "AmmoMagazine"; name = "PreciseAutomaticRifleGun_Mag_5rd";    } break;
      case EItemType.SemiAutoPistolMagazine:              { iSType = "AmmoMagazine"; name = "SemiAutoPistolMagazine";              } break;
      case EItemType.ElitePistolMagazine:                 { iSType = "AmmoMagazine"; name = "ElitePistolMagazine";                 } break;
      case EItemType.FullAutoPistolMagazine:              { iSType = "AmmoMagazine"; name = "FullAutoPistolMagazine";              } break;
      case EItemType.SmallRailgunAmmo:                    { iSType = "AmmoMagazine"; name = "SmallRailgunAmmo";                    } break;
    }
    return $"MyObjectBuilder_{iSType}/{name}";
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
      // ammo
      case EItemType.NATO_5p56x45mm:                      name = "NATO_5p56x45mmMagazine";              break;
      case EItemType.LargeCalibreAmmo:                    name = "LargeCalibreAmmo";                    break;
      case EItemType.MediumCalibreAmmo:                   name = "MediumCalibreAmmo";                   break;
      case EItemType.AutocannonClip:                      name = "AutocannonClip";                      break;
      case EItemType.NATO_25x184mm:                       name = "NATO_25x184mmMagazine";               break;
      case EItemType.LargeRailgunAmmo:                    name = "LargeRailgunAmmo";                    break;
      case EItemType.Missile200mm:                        name = "Missile200mm";                        break;
      case EItemType.AutomaticRifleGun_Mag_20rd:          name = "AutomaticRifleGun_Mag_20rd";          break;
      case EItemType.UltimateAutomaticRifleGun_Mag_30rd:  name = "UltimateAutomaticRifleGun_Mag_30rd";  break;
      case EItemType.RapidFireAutomaticRifleGun_Mag_50rd: name = "RapidFireAutomaticRifleGun_Mag_50rd"; break;
      case EItemType.PreciseAutomaticRifleGun_Mag_5rd:    name = "PreciseAutomaticRifleGun_Mag_5rd";    break;
      case EItemType.SemiAutoPistolMagazine:              name = "SemiAutoPistolMagazine";              break;
      case EItemType.ElitePistolMagazine:                 name = "ElitePistolMagazine";                 break;
      case EItemType.FullAutoPistolMagazine:              name = "FullAutoPistolMagazine";              break;
      case EItemType.SmallRailgunAmmo:                    name = "SmallRailgunAmmo";                    break;
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
  // ammo
  static public CComponentItem NATO_5p56x45mm                      (int amount = 0) { return new CComponentItem(EItemType.NATO_5p56x45mm,                      amount); }
  static public CComponentItem LargeCalibreAmmo                    (int amount = 0) { return new CComponentItem(EItemType.LargeCalibreAmmo,                    amount); }
  static public CComponentItem MediumCalibreAmmo                   (int amount = 0) { return new CComponentItem(EItemType.MediumCalibreAmmo,                   amount); }
  static public CComponentItem AutocannonClip                      (int amount = 0) { return new CComponentItem(EItemType.AutocannonClip,                      amount); }
  static public CComponentItem NATO_25x184mm                       (int amount = 0) { return new CComponentItem(EItemType.NATO_25x184mm,                       amount); }
  static public CComponentItem LargeRailgunAmmo                    (int amount = 0) { return new CComponentItem(EItemType.LargeRailgunAmmo,                    amount); }
  static public CComponentItem Missile200mm                        (int amount = 0) { return new CComponentItem(EItemType.Missile200mm,                        amount); }
  static public CComponentItem AutomaticRifleGun_Mag_20rd          (int amount = 0) { return new CComponentItem(EItemType.AutomaticRifleGun_Mag_20rd,          amount); }
  static public CComponentItem UltimateAutomaticRifleGun_Mag_30rd  (int amount = 0) { return new CComponentItem(EItemType.UltimateAutomaticRifleGun_Mag_30rd,  amount); }
  static public CComponentItem RapidFireAutomaticRifleGun_Mag_50rd (int amount = 0) { return new CComponentItem(EItemType.RapidFireAutomaticRifleGun_Mag_50rd, amount); }
  static public CComponentItem PreciseAutomaticRifleGun_Mag_5rd    (int amount = 0) { return new CComponentItem(EItemType.PreciseAutomaticRifleGun_Mag_5rd,    amount); }
  static public CComponentItem SemiAutoPistolMagazine              (int amount = 0) { return new CComponentItem(EItemType.SemiAutoPistolMagazine,              amount); }
  static public CComponentItem ElitePistolMagazine                 (int amount = 0) { return new CComponentItem(EItemType.ElitePistolMagazine,                 amount); }
  static public CComponentItem FullAutoPistolMagazine              (int amount = 0) { return new CComponentItem(EItemType.FullAutoPistolMagazine,              amount); }
  static public CComponentItem SmallRailgunAmmo                    (int amount = 0) { return new CComponentItem(EItemType.SmallRailgunAmmo,                    amount); }


}
