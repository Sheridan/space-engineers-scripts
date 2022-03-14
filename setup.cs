// #include classes/main.cs
// #include classes/setup.cs
// #include classes/blocks/base/blocks.cs
// #include classes/blocks/base/blocks_typed.cs

public string program() { return "Настройка структуры"; }

private int gIndex;

public void main(string argument, UpdateType updateSource)
{
       if (argument.Length == 0) { step(gIndex++); }
  else if (argument == "start")  { gIndex = 0; Runtime.UpdateFrequency = UpdateFrequency.Update10; }
  else if (argument == "stop")   { stop(); }
}

public void step(int index)
{
  echoMeBig(index.ToString());
  switch (index)
  {
    case 0 : (new CSetup<IMyPistonBase>         (new CBlocks     <IMyPistonBase>         (                          ))).setup("Поршень")                        ; break;
    case 1 : (new CSetup<IMyBatteryBlock>       (new CBlocks     <IMyBatteryBlock>       (                          ))).setup("Батарея")                        ; break;
    case 2 : (new CSetup<IMySolarPanel>         (new CBlocks     <IMySolarPanel>         (                          ))).setup("С.Батарея")                      ; break;
    case 3 : (new CSetup<IMyRemoteControl>      (new CBlocks     <IMyRemoteControl>      (                          ))).setup("ДУ", true, false, true)          ; break;
    case 4 : (new CSetup<IMyOreDetector>        (new CBlocks     <IMyOreDetector>        (                          ))).setup("Детектор руды")                  ; break;
    case 5 : (new CSetup<IMyLandingGear>        (new CBlocks     <IMyLandingGear>        (                          ))).setup("Шасси")                          ; break;
    case 6 : (new CSetup<IMyShipDrill>          (new CBlocks     <IMyShipDrill>          (                          ))).setup("Бур")                            ; break;
    case 7 : (new CSetup<IMyShipGrinder>        (new CBlocks     <IMyShipGrinder>        (                          ))).setup("Резак")                          ; break;
    case 8 : (new CSetup<IMyShipWelder>         (new CBlocks     <IMyShipWelder>         (                          ))).setup("Сварщик")                        ; break;
    case 9 : (new CSetup<IMyGyro>               (new CBlocks     <IMyGyro>               (                          ))).setup("Гироскоп")                       ; break;
    case 10: (new CSetup<IMyCollector>          (new CBlocks     <IMyCollector>          (                          ))).setup("Коллектор")                      ; break;
    case 11: (new CSetup<IMyGasGenerator>       (new CBlocks     <IMyGasGenerator>       (                          ))).setup("H2:O2 Генератор")                ; break;
    case 12: (new CSetup<IMyOxygenFarm>         (new CBlocks     <IMyOxygenFarm>         (                          ))).setup("Ферма O2")                       ; break;
    case 13: (new CSetup<IMyShipMergeBlock>     (new CBlocks     <IMyShipMergeBlock>     (                          ))).setup("Соединитель")                    ; break;
    case 14: (new CSetup<IMyRefinery>           (new CBlocks     <IMyRefinery>           (                          ))).setup("Очистительный завод")            ; break;
    case 15: (new CSetup<IMyMedicalRoom>        (new CBlocks     <IMyMedicalRoom>        (                          ))).setup("Медпост")                        ; break;
    case 16: (new CSetup<IMySmallGatlingGun>    (new CBlocks     <IMySmallGatlingGun>    (                          ))).setup("М.Пушка")                        ; break;
    case 17: (new CSetup<IMyLargeGatlingTurret> (new CBlocks     <IMyLargeGatlingTurret> (                          ))).setup("Б.Пушка")                        ; break;
    case 18: (new CSetup<IMyLargeMissileTurret> (new CBlocks     <IMyLargeMissileTurret> (                          ))).setup("Б.Ракетница")                    ; break;
    case 19: (new CSetup<IMyDoor>               (new CBlocks     <IMyDoor>               (                          ))).setup("Дверь")                          ; break;
    case 20: (new CSetup<IMyAirVent>            (new CBlocks     <IMyAirVent>            (                          ))).setup("Вентиляция", false, false, true) ; break;
    case 21: (new CSetup<IMyCameraBlock>        (new CBlocks     <IMyCameraBlock>        (                          ))).setup("Камера", false, false, true)     ; break;
    case 22: (new CSetup<IMyButtonPanel>        (new CBlocks     <IMyButtonPanel>        (                          ))).setup("Кнопки")                         ; break;
    case 23: (new CSetup<IMyRadioAntenna>       (new CBlocks     <IMyRadioAntenna>       (                          ))).setup("Антенна")                        ; break;
    case 24: (new CSetup<IMyLaserAntenna>       (new CBlocks     <IMyLaserAntenna>       (                          ))).setup("Л.Антенна")                      ; break;
    case 25: (new CSetup<IMyTextPanel>          (new CBlocks     <IMyTextPanel>          (                          ))).setup("Дисплей")                        ; break;
    case 26: (new CSetup<IMyShipConnector>      (new CBlocks     <IMyShipConnector>      (                          ))).setup("Коннектор", false, false, true)  ; break;
    case 27: (new CSetup<IMyTimerBlock>         (new CBlocks     <IMyTimerBlock>         (                          ))).setup("Таймер", true, false, true)      ; break;
    case 28: (new CSetup<IMySensorBlock>        (new CBlocks     <IMySensorBlock>        (                          ))).setup("Сенсор")                         ; break;
    case 29: (new CSetup<IMyPowerProducer>      (new CBlocksTyped<IMyPowerProducer>      ("HydrogenEngine"          ))).setup("H2 Электрогенератор")            ; break;
    case 30: (new CSetup<IMyPowerProducer>      (new CBlocksTyped<IMyPowerProducer>      ("WindTurbine"             ))).setup("Ветрогенератор")                 ; break;
    case 31: (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("LargeAtmosphericThrust"  ))).setup("БАУ")                            ; break;
    case 32: (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("SmallAtmosphericThrust"  ))).setup("АУ")                             ; break;
    case 33: (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("LargeHydrogenThrust"     ))).setup("БВУ")                            ; break;
    case 34: (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("SmallHydrogenThrust"     ))).setup("ВУ")                             ; break;
    case 35: (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("LargeThrust"             ))).setup("БИУ")                            ; break;
    case 36: (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("SmallThrust"             ))).setup("ИУ")                             ; break;
    case 37: (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("OxygenTankSmall"         ))).setup("Бак O2")                         ; break;
    case 38: (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("OxygenTank/"             ))).setup("Б.Бак O2")                       ; break;
    case 39: (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("/LargeHydrogenTank"      ))).setup("ОБ.Бак H2")                      ; break;
    case 40: (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("/LargeHydrogenTankSmall" ))).setup("Б.Бак H2")                       ; break;
    case 41: (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("/SmallHydrogenTank"      ))).setup("Бак H2")                         ; break;
    case 42: (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("/SmallHydrogenTankSmall" ))).setup("Бак H2")                         ; break;
    case 43: (new CSetup<IMyCargoContainer>     (new CBlocksTyped<IMyCargoContainer>     ("SmallContainer"          ))).setup("МК", false, true)                ; break;
    case 44: (new CSetup<IMyCargoContainer>     (new CBlocksTyped<IMyCargoContainer>     ("MediumContainer"         ))).setup("СК", false, true)                ; break;
    case 45: (new CSetup<IMyCargoContainer>     (new CBlocksTyped<IMyCargoContainer>     ("LargeContainer"          ))).setup("БК", false, true)                ; break;
    case 46: (new CSetup<IMyCargoContainer>     (new CBlocksTyped<IMyCargoContainer>     ("LargeIndustrialContainer"))).setup("БК", false, true)                ; break;
    case 47: (new CSetup<IMyConveyorSorter>     (new CBlocks     <IMyConveyorSorter>     (                          ))).setup("Сортировщик", false, false, true); break;
    case 48: (new CSetup<IMyUpgradeModule>      (new CBlocksTyped<IMyUpgradeModule>      ("ProductivityModule"      ))).setup("М.Продуктивности")               ; break;
    case 49: (new CSetup<IMyUpgradeModule>      (new CBlocksTyped<IMyUpgradeModule>      ("EffectivenessModule"     ))).setup("М.Эффективности")                ; break;
    case 50: (new CSetup<IMyUpgradeModule>      (new CBlocksTyped<IMyUpgradeModule>      ("EnergyModule"            ))).setup("М.Энергоэффективности")          ; break;
    case 51: (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("SmallLight"              ))).setup("Лампа")                          ; break;
    case 52: (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("Light_1corner"           ))).setup("Угл. Лампа")                     ; break;
    case 53: (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("Light_2corner"           ))).setup("2хУгл. Лампа")                   ; break;
    case 54: (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("LightPanel"              ))).setup("Светопанель")                    ; break;
    case 55: (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("OffsetLight"             ))).setup("Диодная фара")                   ; break;
    case 56: (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("PassageSciFiLight"       ))).setup("SciFi свет")                     ; break;
    case 57: (new CSetup<IMyReflectorLight>     (new CBlocksTyped<IMyReflectorLight>     ("FrontLight"              ))).setup("Прожектор")                      ; break;
    case 58: (new CSetup<IMyReflectorLight>     (new CBlocksTyped<IMyReflectorLight>     ("RotatingLight"           ))).setup("Вр. прожектор")                  ; break;
    case 59: (new CSetup<IMyReflectorLight>     (new CBlocksTyped<IMyReflectorLight>     ("Spotlight"               ))).setup("Фара")                           ; break;
    case 60: (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension1x1"           ))).setup("Колесо 1x1 правое")              ; break;
    case 61: (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension3x3"           ))).setup("Колесо 3x3 правое")              ; break;
    case 62: (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension5x5"           ))).setup("Колесо 5x5 правое")              ; break;
    case 63: (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension1x1mirrored"   ))).setup("Колесо 1x1 левое")               ; break;
    case 64: (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension3x3mirrored"   ))).setup("Колесо 3x3 левое")               ; break;
    case 65: (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension5x5mirrored"   ))).setup("Колесо 5x5 левое")               ; break;
    case 66: (new CSetup<IMyCockpit>            (new CBlocksTyped<IMyCockpit>            ("LargeBlockCouch"         ))).setup("Диван")                          ; break;
    case 67: (new CSetup<IMyCockpit>            (new CBlocksTyped<IMyCockpit>            ("LargeBlockCouchCorner"   ))).setup("Угл. Диван")                     ; break;
    case 68: (new CSetup<IMyAssembler>          (new CBlocksTyped<IMyAssembler>          ("LargeAssembler"          ))).setup("Сборщик")                        ; break;
    case 69: (new CSetup<IMyProjector>          (new CBlocks     <IMyProjector>          (                          ))).setup("Проектор", false, false, true)   ; break;
    case 70: (new CSetup<IMyMotorStator>        (new CBlocks     <IMyMotorStator>        (                          ))).setup("Ротор")                          ; break;
    case 71: (new CSetup<IMyMotorAdvancedStator>(new CBlocks     <IMyMotorAdvancedStator>(                          ))).setup("Ул. Ротор")                      ; break;
    case 72: (new CSetup<IMyControlPanel>       (new CBlocks     <IMyControlPanel>       (                          ))).setup("Панель упр.")                    ; break;
    case 73: (new CSetup<IMySoundBlock>         (new CBlocks     <IMySoundBlock>         (                          ))).setup("Динамик")                        ; break;
    case 74: (new CSetup<IMyCryoChamber>        (new CBlocks     <IMyCryoChamber>        (                          ))).setup("Криокамера")                     ; break;
    default: { stop(); } break;
  }
}

public void stop() { Runtime.UpdateFrequency = UpdateFrequency.None; applyDefaultMeDisplayTexsts(); }
