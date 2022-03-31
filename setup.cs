// #include classes/main.cs
// #include classes/setup.cs
// #include classes/blocks/base/blocks.cs
// #include classes/blocks/base/blocks_typed.cs

public string program() { buildActions(); return "Настройка структуры"; }

private int gIndex;
private List<Action> actions;

public void main(string argument, UpdateType updateSource)
{
       if (argument.Length == 0) { step(gIndex++); }
  else if (argument == "start")  { gIndex = 0; Runtime.UpdateFrequency = UpdateFrequency.Update10; }
  else if (argument == "start slow")  { gIndex = 0; Runtime.UpdateFrequency = UpdateFrequency.Update100; }
  else if (argument == "stop")   { stop(); }
}

private void buildActions()
{
  actions = new List<Action>();
  actions.Add(() => { (new CSetup<IMyRemoteControl>      (new CBlocks     <IMyRemoteControl>      (                          ))).setup("ДУ", true, false, true)          ; });
  actions.Add(() => { (new CSetup<IMyMedicalRoom>        (new CBlocks     <IMyMedicalRoom>        (                          ))).setup("Медпост")                        ; });
  actions.Add(() => { (new CSetup<IMyAirVent>            (new CBlocks     <IMyAirVent>            (                          ))).setup("Вентиляция", false, false, true) ; });
  actions.Add(() => { (new CSetup<IMyCameraBlock>        (new CBlocks     <IMyCameraBlock>        (                          ))).setup("Камера", false, false, true)     ; });
  actions.Add(() => { (new CSetup<IMyProjector>          (new CBlocks     <IMyProjector>          (                          ))).setup("Проектор", false, false, true)   ; });
  actions.Add(() => { (new CSetup<IMyCryoChamber>        (new CBlocks     <IMyCryoChamber>        (                          ))).setup("Криокамера")                     ; });
  actions.Add(() => { (new CSetup<IMyGyro>               (new CBlocks     <IMyGyro>               (                          ))).setup("Гироскоп")                       ; });

  // automatisation && access
  actions.Add(() => { (new CSetup<IMyControlPanel>       (new CBlocks     <IMyControlPanel>       (                          ))).setup("Панель упр.")                    ; });
  actions.Add(() => { (new CSetup<IMySoundBlock>         (new CBlocks     <IMySoundBlock>         (                          ))).setup("Динамик")                        ; });
  actions.Add(() => { (new CSetup<IMyButtonPanel>        (new CBlocks     <IMyButtonPanel>        (                          ))).setup("Кнопки")                         ; });
  actions.Add(() => { (new CSetup<IMyTextPanel>          (new CBlocks     <IMyTextPanel>          (                          ))).setup("Дисплей")                        ; });
  actions.Add(() => { (new CSetup<IMyTimerBlock>         (new CBlocks     <IMyTimerBlock>         (                          ))).setup("Таймер", true, false, true)      ; });
  actions.Add(() => { (new CSetup<IMySensorBlock>        (new CBlocks     <IMySensorBlock>        (                          ))).setup("Сенсор")                         ; });
  // connection
  actions.Add(() => { (new CSetup<IMyLandingGear>        (new CBlocks     <IMyLandingGear>        (                          ))).setup("Шасси")                          ; });
  actions.Add(() => { (new CSetup<IMyShipConnector>      (new CBlocks     <IMyShipConnector>      (                          ))).setup("Коннектор", false, false, true)  ; });
  actions.Add(() => { (new CSetup<IMyShipMergeBlock>     (new CBlocks     <IMyShipMergeBlock>     (                          ))).setup("Соединитель")                    ; });
  // effectors
  actions.Add(() => { (new CSetup<IMyPistonBase>         (new CBlocks     <IMyPistonBase>         (                          ))).setup("Поршень")                        ; });
  actions.Add(() => { (new CSetup<IMyMotorStator>        (new CBlocks     <IMyMotorStator>        (                          ))).setup("Ротор")                          ; });
  actions.Add(() => { (new CSetup<IMyMotorAdvancedStator>(new CBlocks     <IMyMotorAdvancedStator>(                          ))).setup("Ул. Ротор")                      ; });
  // tools
  actions.Add(() => { (new CSetup<IMyShipDrill>          (new CBlocks     <IMyShipDrill>          (                          ))).setup("Бур")                            ; });
  actions.Add(() => { (new CSetup<IMyShipGrinder>        (new CBlocks     <IMyShipGrinder>        (                          ))).setup("Резак")                          ; });
  actions.Add(() => { (new CSetup<IMyShipWelder>         (new CBlocks     <IMyShipWelder>         (                          ))).setup("Сварщик")                        ; });
  actions.Add(() => { (new CSetup<IMyCollector>          (new CBlocks     <IMyCollector>          (                          ))).setup("Коллектор")                      ; });
  actions.Add(() => { (new CSetup<IMyOreDetector>        (new CBlocks     <IMyOreDetector>        (                          ))).setup("Детектор руды")                  ; });
  actions.Add(() => { (new CSetup<IMyRadioAntenna>       (new CBlocks     <IMyRadioAntenna>       (                          ))).setup("Антенна")                        ; });
  actions.Add(() => { (new CSetup<IMyLaserAntenna>       (new CBlocks     <IMyLaserAntenna>       (                          ))).setup("Л.Антенна")                      ; });
  // cockpits
  actions.Add(() => { (new CSetup<IMyCockpit>            (new CBlocksTyped<IMyCockpit>            ("LargeBlockCouch"         ))).setup("Диван")                          ; });
  actions.Add(() => { (new CSetup<IMyCockpit>            (new CBlocksTyped<IMyCockpit>            ("LargeBlockCouchCorner"   ))).setup("Угл. Диван")                     ; });
  // manufacturing
  actions.Add(() => { (new CSetup<IMyRefinery>           (new CBlocks     <IMyRefinery>           (                          ))).setup("Очистительный завод")            ; });
  actions.Add(() => { (new CSetup<IMyAssembler>          (new CBlocksTyped<IMyAssembler>          ("LargeAssembler"          ))).setup("Сборщик")                        ; });
  actions.Add(() => { (new CSetup<IMyGasGenerator>       (new CBlocks     <IMyGasGenerator>       (                          ))).setup("H2:O2 Генератор")                ; });
  actions.Add(() => { (new CSetup<IMyOxygenFarm>         (new CBlocks     <IMyOxygenFarm>         (                          ))).setup("Ферма O2")                       ; });
  // weapon
  actions.Add(() => { (new CSetup<IMySmallGatlingGun>    (new CBlocks     <IMySmallGatlingGun>    (                          ))).setup("М.Пушка")                        ; });
  actions.Add(() => { (new CSetup<IMyLargeGatlingTurret> (new CBlocks     <IMyLargeGatlingTurret> (                          ))).setup("Б.Пушка")                        ; });
  actions.Add(() => { (new CSetup<IMyLargeMissileTurret> (new CBlocks     <IMyLargeMissileTurret> (                          ))).setup("Б.Ракетница")                    ; });
  // power
  actions.Add(() => { (new CSetup<IMyPowerProducer>      (new CBlocksTyped<IMyPowerProducer>      ("HydrogenEngine"          ))).setup("H2 Электрогенератор")            ; });
  actions.Add(() => { (new CSetup<IMyPowerProducer>      (new CBlocksTyped<IMyPowerProducer>      ("WindTurbine"             ))).setup("Ветрогенератор")                 ; });
  actions.Add(() => { (new CSetup<IMyBatteryBlock>       (new CBlocks     <IMyBatteryBlock>       (                          ))).setup("Батарея")                        ; });
  actions.Add(() => { (new CSetup<IMySolarPanel>         (new CBlocks     <IMySolarPanel>         (                          ))).setup("С.Батарея")                      ; });
  // modules
  actions.Add(() => { (new CSetup<IMyUpgradeModule>      (new CBlocksTyped<IMyUpgradeModule>      ("ProductivityModule"      ))).setup("М.Продуктивности")               ; });
  actions.Add(() => { (new CSetup<IMyUpgradeModule>      (new CBlocksTyped<IMyUpgradeModule>      ("EffectivenessModule"     ))).setup("М.Эффективности")                ; });
  actions.Add(() => { (new CSetup<IMyUpgradeModule>      (new CBlocksTyped<IMyUpgradeModule>      ("EnergyModule"            ))).setup("М.Энергоэффективности")          ; });
  // conveyors
  actions.Add(() => { (new CSetup<IMyConveyorSorter>     (new CBlocks     <IMyConveyorSorter>     (                          ))).setup("Сортировщик", false, false, true); });
  // containers
  actions.Add(() => { (new CSetup<IMyCargoContainer>     (new CBlocksTyped<IMyCargoContainer>     ("SmallContainer"          ))).setup("МК", false, true)                ; });
  actions.Add(() => { (new CSetup<IMyCargoContainer>     (new CBlocksTyped<IMyCargoContainer>     ("MediumContainer"         ))).setup("СК", false, true)                ; });
  actions.Add(() => { (new CSetup<IMyCargoContainer>     (new CBlocksTyped<IMyCargoContainer>     ("LargeContainer"          ))).setup("БК", false, true)                ; });
  actions.Add(() => { (new CSetup<IMyCargoContainer>     (new CBlocksTyped<IMyCargoContainer>     ("LargeIndustrialContainer"))).setup("БК", false, true)                ; });
  // tanks
  actions.Add(() => { (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("OxygenTankSmall"         ))).setup("Бак O2")                         ; });
  actions.Add(() => { (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("OxygenTank/"             ))).setup("Б.Бак O2")                       ; });
  actions.Add(() => { (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("/LargeHydrogenTank"      ))).setup("ОБ.Бак H2")                      ; });
  actions.Add(() => { (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("/LargeHydrogenTankSmall" ))).setup("Б.Бак H2")                       ; });
  actions.Add(() => { (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("/SmallHydrogenTank"      ))).setup("Бак H2")                         ; });
  actions.Add(() => { (new CSetup<IMyGasTank>            (new CBlocksTyped<IMyGasTank>            ("/SmallHydrogenTankSmall" ))).setup("Бак H2")                         ; });
  // lamps
  actions.Add(() => { (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("SmallLight"              ))).setup("Лампа")                          ; });
  actions.Add(() => { (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("Light_1corner"           ))).setup("Угл. Лампа")                     ; });
  actions.Add(() => { (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("Light_2corner"           ))).setup("2хУгл. Лампа")                   ; });
  actions.Add(() => { (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("LightPanel"              ))).setup("Светопанель")                    ; });
  actions.Add(() => { (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("OffsetLight"             ))).setup("Диодная фара")                   ; });
  actions.Add(() => { (new CSetup<IMyInteriorLight>      (new CBlocksTyped<IMyInteriorLight>      ("PassageSciFiLight"       ))).setup("SciFi свет")                     ; });
  actions.Add(() => { (new CSetup<IMyReflectorLight>     (new CBlocksTyped<IMyReflectorLight>     ("FrontLight"              ))).setup("Прожектор")                      ; });
  actions.Add(() => { (new CSetup<IMyReflectorLight>     (new CBlocksTyped<IMyReflectorLight>     ("RotatingLight"           ))).setup("Вр. прожектор")                  ; });
  actions.Add(() => { (new CSetup<IMyReflectorLight>     (new CBlocksTyped<IMyReflectorLight>     ("Spotlight"               ))).setup("Фара")                           ; });
  // suspensions
  actions.Add(() => { (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension1x1"           ))).setup("Колесо 1x1 правое")              ; });
  actions.Add(() => { (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension3x3"           ))).setup("Колесо 3x3 правое")              ; });
  actions.Add(() => { (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension5x5"           ))).setup("Колесо 5x5 правое")              ; });
  actions.Add(() => { (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension1x1mirrored"   ))).setup("Колесо 1x1 левое")               ; });
  actions.Add(() => { (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension3x3mirrored"   ))).setup("Колесо 3x3 левое")               ; });
  actions.Add(() => { (new CSetup<IMyMotorSuspension>    (new CBlocksTyped<IMyMotorSuspension>    ("Suspension5x5mirrored"   ))).setup("Колесо 5x5 левое")               ; });
  // thrusts
  actions.Add(() => { (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("LargeAtmosphericThrust"  ))).setup("БАУ")                            ; });
  actions.Add(() => { (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("SmallAtmosphericThrust"  ))).setup("АУ")                             ; });
  actions.Add(() => { (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("LargeHydrogenThrust"     ))).setup("БВУ")                            ; });
  actions.Add(() => { (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("SmallHydrogenThrust"     ))).setup("ВУ")                             ; });
  actions.Add(() => { (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("LargeThrust"             ))).setup("БИУ")                            ; });
  actions.Add(() => { (new CSetup<IMyThrust>             (new CBlocksTyped<IMyThrust>             ("SmallThrust"             ))).setup("ИУ")                             ; });
  // doors
  actions.Add(() => { (new CSetup<IMyAirtightHangarDoor> (new CBlocks     <IMyAirtightHangarDoor> (                          ))).setup("Ангарслайд")                     ; });
  actions.Add(() => { (new CSetup<IMyDoor>               (new CBlocks     <IMyDoor>               (                          ))).setup("Дверь")                          ; });

}

public void step(int index)
{
  if(index >= actions.Count) { stop(); }
  echoMeSmall(index.ToString());
  actions[index]();
}

public void stop() { Runtime.UpdateFrequency = UpdateFrequency.None; applyDefaultMeDisplayTexsts(); }
