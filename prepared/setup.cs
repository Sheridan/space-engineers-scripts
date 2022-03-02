static string structureName;
static string scriptName;
static Program self;
static float blockSize;
static CBO prbOptions;
public void applyDefaultMeDisplayTexsts() {
Me.GetSurface(0).WriteText(scriptName.Replace(" ", "\n"));
Me.GetSurface(1).WriteText(structureName); }
public void echoMe(string text, int surface) { Me.GetSurface(surface).WriteText(text, false); }
public void echoMeBig(string text) { echoMe(text, 0); }
public void echoMeSmall(string text) { echoMe(text, 1); }
public void sMe(string i_scriptName) {
scriptName = i_scriptName;
Me.CustomName = $"[{structureName}] ПрБ {scriptName}";
sMeSurface(0, 2f);
sMeSurface(1, 5f);
applyDefaultMeDisplayTexsts(); }
public void sMeSurface(int i, float fontSize) {
IMyTextSurface surface = Me.GetSurface(i);
surface.ContentType = ContentType.TEXT_AND_IMAGE;
surface.Font = "Monospace";
surface.FontColor = new Color(255, 255, 255);
surface.BackgroundColor = new Color(0, 0, 0);
surface.FontSize = fontSize;
surface.Alignment = TextAlignment.CENTER; }
public static void debug(string text) { self.Echo(text); }
public void init() {
structureName = Me.CubeGrid.CustomName;
blockSize = Me.CubeGrid.GridSize;
prbOptions = new CBO(Me);
sMe(program());
debug($"{Me.CustomName}: init done"); }
public Program() {
self = this;
init(); }
public void Main(string argument, UpdateType updateSource) {
if(argument == "init") {
UpdateFrequency uf = Runtime.UpdateFrequency;
Runtime.UpdateFrequency = UpdateFrequency.None;
init();
Runtime.UpdateFrequency = uf; }
else { main(argument, updateSource); } }
public class CBO {
public CBO(IMyTerminalBlock block) {
m_available = false;
m_block = block;
read(); }
private void read() {
if(m_block.CustomData.Length > 0) {
m_ini = new MyIni();
MyIniParseResult result;
m_available = m_ini.TryParse(m_block.CustomData, out result);
if(!m_available) { debug(result.ToString()); } } }
private void write() { m_block.CustomData = m_ini.ToString(); }
private bool exists(string section, string name) { return m_available && m_ini.ContainsKey(section, name); }
public string g(string section, string name, string defaultValue = "") {
if(exists(section, name)) { return m_ini.Get(section, name).ToString(); }
return defaultValue; }
public bool g(string section, string name, bool defaultValue = true) {
if(exists(section, name)) { return m_ini.Get(section, name).ToBoolean(); }
return defaultValue; }
public float g(string section, string name, float defaultValue = 0f) {
if(exists(section, name)) { return float.Parse(m_ini.Get(section, name).ToString()); }
return defaultValue; }
public int g(string section, string name, int defaultValue = 0) {
if(exists(section, name)) { return m_ini.Get(section, name).ToInt32(); }
return defaultValue; }
public Color g(string section, string name, Color defaultValue) {
if(exists(section, name)) {
string[] color = m_ini.Get(section, name).ToString().Split(';');
return new Color(float.Parse(color[0]), float.Parse(color[1]), float.Parse(color[2]), float.Parse(color[3])); }
return defaultValue; }
IMyTerminalBlock m_block;
private bool m_available;
private MyIni m_ini; }
public class CB<T> : CBB<T> where T : class, IMyTerminalBlock {
public CB(string purpose = "", bool loadOnlySameGrid = true) : base(purpose) {
refresh(loadOnlySameGrid); }
public void refresh(bool loadOnlySameGrid = true) {
clear();
if(loadOnlySameGrid) { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
else { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks) ; } } }
public class CBB<T> where T : class, IMyTerminalBlock {
public CBB(string purpose = "") {
m_blocks = new List<T>();
m_purpose = purpose; }
public void s(string name,
bool visibleInTerminal = false,
bool visibleInInventory = false,
bool visibleInToolBar = false) {
Dictionary<string, int> counetrs = new Dictionary<string, int>();
string zeros = new string('0', count().ToString().Length);
foreach(T block in m_blocks) {
string blockPurpose = "";
CBO o = new CBO(block);
if(isAssignable<IMyShipConnector>()) {
IMyShipConnector blk = block as IMyShipConnector;
blk.PullStrength = 1f;
blk.CollectAll = o.g("connector", "collectAll", false);
blk.ThrowOut = o.g("connector", "throwOut", false); }
else if(isAssignable<IMyInteriorLight>()) {
IMyInteriorLight blk = block as IMyInteriorLight;
blk.Radius = 10f;
blk.Intensity = 10f;
blk.Falloff = 3f;
blk.Color = o.g("lamp", "color", Color.White); }
else if(isAssignable<IMyConveyorSorter>()) {
IMyConveyorSorter blk = block as IMyConveyorSorter;
blk.DrainAll = o.g("sorter", "drainAll", false); }
else if(isAssignable<IMyLargeTurretBase>()) {
IMyLargeTurretBase blk = block as IMyLargeTurretBase;
blk.EnableIdleRotation = true;
blk.Elevation = 0f;
blk.Azimuth = 0f; }
else if(isAssignable<IMyAssembler>()) {
blockPurpose = "Master";
if(o.g("assembler", "is_slave", false)) {
IMyAssembler blk = block as IMyAssembler;
blk.CooperativeMode = true;
blockPurpose = "Slave"; } }
string realPurpose = $"{getPurpose(o).Trim()} {blockPurpose}";
if(!counetrs.ContainsKey(realPurpose)) { counetrs.Add(realPurpose, 0); }
string sZeros = count() > 1 ? counetrs[realPurpose].ToString(zeros).Trim() : "";
block.CustomName = TrimAllSpaces($"[{structureName}] {name} {realPurpose} {sZeros}");
counetrs[realPurpose]++;
sBlocksVisibility(block,
o.g("generic", "visibleInTerminal", visibleInTerminal),
o.g("generic", "visibleInInventory", visibleInInventory),
o.g("generic", "visibleInToolBar", visibleInToolBar)); } }
private string getPurpose(CBO o) {
string result = o.g("generic", "purpose", m_purpose);
return result != "" ? $" {result} " : " "; }
private void sBlocksVisibility(T block,
bool vTerminal,
bool vInventory,
bool vToolBar) {
IMySlimBlock sBlock = block.CubeGrid.GetCubeBlock(block.Position);
block.ShowInTerminal = vTerminal && sBlock.IsFullIntegrity && sBlock.BuildIntegrity < 1f;
block.ShowInToolbarConfig = vToolBar;
if(block.HasInventory) { block.ShowInInventory = vInventory; } }
public bool empty() { return m_blocks.Count == 0; }
public int count() { return m_blocks.Count; }
public string subtypeName() { return empty() ? "N/A" : m_blocks[0].DefinitionDisplayNameText; }
public bool isAssignable<U>() where U : class, IMyTerminalBlock {
if(empty()) { return false; }
return m_blocks[0] is U; }
public List<T> blocks() { return m_blocks; }
public string purpose() { return m_purpose; }
protected void clear() { m_blocks.Clear(); }
protected List<T> m_blocks;
private string m_purpose; }
public static string TrimAllSpaces(string value) {
var newString = new StringBuilder();
bool pIW = false;
for(int i = 0; i < value.Length; i++) {
if(Char.IsWhiteSpace(value[i])) {
if(pIW) {
continue; }
pIW = true; }
else {
pIW = false; }
newString.Append(value[i]); }
return newString.ToString(); }
public class CBG<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBG(string groupName,
string purpose = "",
bool loadOnlySameGrid = true) : base(purpose) {
m_groupName = groupName;
refresh(loadOnlySameGrid); }
public void refresh(bool loadOnlySameGrid = true) {
clear();
IMyBlockGroup group = self.GridTerminalSystem.GetBlockGroupWithName(m_groupName);
if(loadOnlySameGrid) { group.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
else { group.GetBlocksOfType<T>(m_blocks) ; } }
public string groupName() { return m_groupName; }
private string m_groupName; }
public class CBT<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBT(string subTypeName,
string purpose = "",
bool loadOnlySameGrid = true) : base(purpose) {
m_subTypeName = subTypeName;
refresh(loadOnlySameGrid); }
public void refresh(bool loadOnlySameGrid = true) {
clear();
if(loadOnlySameGrid) {
self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => (x.IsSameConstructAs(self.Me) &&
x.BlockDefinition.ToString().Contains(m_subTypeName))); }
else { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.BlockDefinition.ToString().Contains(m_subTypeName)); } }
public string subTypeName() { return m_subTypeName; }
private string m_subTypeName; }
public string program() { return "Настройка структуры"; }
public int gIndex;
public void main(string argument, UpdateType updateSource) {
if(Runtime.UpdateFrequency == UpdateFrequency.Update100) {
gIndex++;
step(gIndex); }
else if(argument == "start") {
gIndex = 0;
Runtime.UpdateFrequency = UpdateFrequency.Update100; } }
public void step(int index) {
echoMeBig(index.ToString());
switch(index) {
case 0 : (new CB<IMyPistonBase> ()).s("Поршень"); break;
case 1 : (new CB<IMyBatteryBlock> ()).s("Батарея"); break;
case 2 : (new CB<IMySolarPanel> ()).s("С.Батарея"); break;
case 3 : (new CB<IMyRemoteControl> ()).s("ДУ", true, false, true); break;
case 4 : (new CB<IMyOreDetector> ()).s("Детектор руды"); break;
case 5 : (new CB<IMyLandingGear> ()).s("Шасси"); break;
case 6 : (new CB<IMyShipDrill> ()).s("Бур"); break;
case 7 : (new CB<IMyShipGrinder> ()).s("Резак"); break;
case 8 : (new CB<IMyShipWelder> ()).s("Сварщик"); break;
case 9 : (new CB<IMyGyro> ()).s("Гироскоп"); break;
case 10: (new CB<IMyCollector> ()).s("Коллектор"); break;
case 11: (new CB<IMyGasGenerator> ()).s("H2:O2 Генератор"); break;
case 12: (new CB<IMyOxygenFarm> ()).s("Ферма O2"); break;
case 13: (new CB<IMyShipMergeBlock> ()).s("Соединитель"); break;
case 14: (new CB<IMyRefinery> ()).s("Очистительный завод"); break;
case 15: (new CB<IMyMedicalRoom> ()).s("Медпост"); break;
case 16: (new CB<IMySmallGatlingGun> ()).s("М.Пушка"); break;
case 17: (new CB<IMyLargeGatlingTurret> ()).s("Б.Пушка"); break;
case 18: (new CB<IMyLargeMissileTurret> ()).s("Б.Ракетница"); break;
case 19: (new CB<IMyDoor> ()).s("Дверь"); break;
case 20: (new CB<IMyAirVent> ()).s("Вентиляция", false, false, true); break;
case 21: (new CB<IMyCameraBlock> ()).s("Камера", false, false, true); break;
case 22: (new CB<IMyButtonPanel> ()).s("Кнопки"); break;
case 23: (new CB<IMyRadioAntenna> ()).s("Антенна"); break;
case 24: (new CB<IMyLaserAntenna> ()).s("Л.Антенна"); break;
case 25: (new CB<IMyTextPanel> ()).s("Дисплей"); break;
case 26: (new CB<IMyShipConnector> ()).s("Коннектор", false, false, true); break;
case 27: (new CB<IMyTimerBlock> ()).s("Таймер", true, false, true); break;
case 28: (new CB<IMySensorBlock> ()).s("Сенсор"); break;
case 29: (new CBT<IMyPowerProducer> ("HydrogenEngine")).s("H2 Электрогенератор"); break;
case 30: (new CBT<IMyPowerProducer> ("WindTurbine")).s("Ветрогенератор"); break;
case 31: (new CBT<IMyThrust> ("LargeAtmosphericThrust")).s("БАУ"); break;
case 32: (new CBT<IMyThrust> ("SmallAtmosphericThrust")).s("АУ"); break;
case 33: (new CBT<IMyThrust> ("LargeHydrogenThrust")).s("БВУ"); break;
case 34: (new CBT<IMyThrust> ("SmallHydrogenThrust")).s("ВУ"); break;
case 35: (new CBT<IMyThrust> ("LargeThrust")).s("БИУ"); break;
case 36: (new CBT<IMyThrust> ("SmallThrust")).s("ИУ"); break;
case 37: (new CBT<IMyGasTank> ("OxygenTankSmall")).s("Бак O2"); break;
case 38: (new CBT<IMyGasTank> ("OxygenTank/")).s("Б.Бак O2"); break;
case 39: (new CBT<IMyGasTank> ("/LargeHydrogenTank")).s("ОБ.Бак H2"); break;
case 40: (new CBT<IMyGasTank> ("/LargeHydrogenTankSmall")).s("Б.Бак H2"); break;
case 41: (new CBT<IMyGasTank> ("/SmallHydrogenTank")).s("Бак H2"); break;
case 42: (new CBT<IMyGasTank> ("/SmallHydrogenTankSmall")).s("Бак H2"); break;
case 43: (new CBT<IMyCargoContainer> ("SmallContainer")).s("МК", false, true); break;
case 44: (new CBT<IMyCargoContainer> ("MediumContainer")).s("СК", false, true); break;
case 45: (new CBT<IMyCargoContainer> ("LargeContainer")).s("БК", false, true); break;
case 46: (new CBT<IMyCargoContainer> ("LargeIndustrialContainer")).s("БК", false, true); break;
case 47: (new CB<IMyConveyorSorter> ()).s("Сортировщик", false, false, true); break;
case 48: (new CBT<IMyUpgradeModule> ("ProductivityModule")).s("М.Продуктивности"); break;
case 49: (new CBT<IMyUpgradeModule> ("EffectivenessModule")).s("М.Эффективности"); break;
case 50: (new CBT<IMyUpgradeModule> ("EnergyModule")).s("М.Энергоэффективности"); break;
case 51: (new CBT<IMyInteriorLight> ("SmallLight")).s("Лампа"); break;
case 52: (new CBT<IMyInteriorLight> ("Light_1corner")).s("Угл. Лампа"); break;
case 53: (new CBT<IMyInteriorLight> ("Light_2corner")).s("2хУгл. Лампа"); break;
case 54: (new CBT<IMyReflectorLight> ("FrontLight")).s("Прожектор"); break;
case 55: (new CBT<IMyReflectorLight> ("RotatingLight")).s("Вр. прожектор"); break;
case 56: (new CBT<IMyMotorSuspension>("Suspension1x1")).s("Колесо 1x1 правое"); break;
case 57: (new CBT<IMyMotorSuspension>("Suspension3x3")).s("Колесо 3x3 правое"); break;
case 58: (new CBT<IMyMotorSuspension>("Suspension5x5")).s("Колесо 5x5 правое"); break;
case 59: (new CBT<IMyMotorSuspension>("Suspension1x1mirrored")).s("Колесо 1x1 левое"); break;
case 60: (new CBT<IMyMotorSuspension>("Suspension3x3mirrored")).s("Колесо 3x3 левое"); break;
case 61: (new CBT<IMyMotorSuspension>("Suspension5x5mirrored")).s("Колесо 5x5 левое"); break;
case 62: (new CBT<IMyCockpit> ("LargeBlockCouch")).s("Диван"); break;
case 63: (new CBT<IMyCockpit> ("LargeBlockCouchCorner")).s("Угл. Диван"); break;
case 64: (new CBT<IMyAssembler> ("LargeAssembler")).s("Сборщик"); break;
default: {
Runtime.UpdateFrequency = UpdateFrequency.None;
applyDefaultMeDisplayTexsts(); } break; } }
