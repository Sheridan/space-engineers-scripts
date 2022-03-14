static string sN;
static string scriptName;
static Program _;
static float blockSize;
static CBO prbOptions;
public void applyDefaultMeDisplayTexsts() {
Me.GetSurface(0).WriteText(scriptName.Replace(" ", "\n"));
Me.GetSurface(1).WriteText(sN); }
public void echoMe(string text, int surface) { Me.GetSurface(surface).WriteText(text, false); }
public void echoMeBig(string text) { echoMe(text, 0); }
public void echoMeSmall(string text) { echoMe(text, 1); }
public void sMe(string i_scriptName) {
scriptName = i_scriptName;
Me.CustomName = $"[{sN}] ПрБ {scriptName}";
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
public static void debug(string text) { _.Echo(text); }
public void init() {
sN = Me.CubeGrid.CustomName;
blockSize = Me.CubeGrid.GridSize;
prbOptions = new CBO(Me);
sMe(program());
debug($"{Me.CustomName}: init done"); }
public Program() {
_ = this;
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
MyIniParseResult r;
m_available = m_ini.TryParse(m_block.CustomData, out r);
if(!m_available) { debug(r.ToString()); } } }
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
public class CS<T> : CT<T> where T : class, IMyTerminalBlock {
public CS(CBB<T> blocks) : base(blocks) {
m_zeros = new string('0', count().ToString().Length);
m_counetrs = new Dictionary<string, int>(); }
public void s(string name,
bool visibleInTerminal = false,
bool visibleInInventory = false,
bool visibleInToolBar = false) {
foreach(T b in m_blocks.blocks()) {
string suffix = "";
CBO o = m_blocks.o(b);
if(m_blocks.iA<IMyShipConnector >()) { suffix = s(o, b as IMyShipConnector); }
else if(m_blocks.iA<IMyInteriorLight >()) { suffix = s(o, b as IMyInteriorLight); }
else if(m_blocks.iA<IMyConveyorSorter >()) { suffix = s(o, b as IMyConveyorSorter); }
else if(m_blocks.iA<IMyLargeTurretBase>()) { suffix = s(o, b as IMyLargeTurretBase); }
else if(m_blocks.iA<IMyAssembler >()) { suffix = s(o, b as IMyAssembler); }
b.CustomName = generateName(name, suffix, loadPurpose(o));
sBlocksVisibility(b,
o.g("generic", "visibleInTerminal", visibleInTerminal),
o.g("generic", "visibleInInventory", visibleInInventory),
o.g("generic", "visibleInToolBar", visibleInToolBar)); } }
private string generateName(string name, string suffix, string purpose) {
string baseName = TrimAllSpaces($"{name} {purpose} {suffix}");
if(!m_counetrs.ContainsKey(baseName)) { m_counetrs.Add(baseName, 0); }
string order = count() > 1 ? m_counetrs[baseName].ToString(m_zeros).Trim() : "";
m_counetrs[baseName]++;
return TrimAllSpaces($"[{sN}] {baseName} {order}"); }
private string s(CBO o, IMyShipConnector b) {
b.PullStrength = 1f;
b.CollectAll = o.g("connector", "collectAll", false);
b.ThrowOut = o.g("connector", "throwOut", false);
return string.Empty; }
private string s(CBO o, IMyInteriorLight b) {
b.Radius = 10f;
b.Intensity = 10f;
b.Falloff = 3f;
b.Color = o.g("lamp", "color", Color.White);
return string.Empty; }
private string s(CBO o, IMyConveyorSorter b) {
b.DrainAll = o.g("sorter", "drainAll", false);
return string.Empty; }
private string s(CBO o, IMyLargeTurretBase b) {
b.EnableIdleRotation = true;
b.Elevation = 0f;
b.Azimuth = 0f;
return string.Empty; }
private string s(CBO o, IMyAssembler b) {
if(o.g("assembler", "is_slave", false)) {
b.CooperativeMode = true;
return "Slave"; }
return "Master"; }
private string loadPurpose(CBO o) { return o.g("generic", "purpose", "").Trim(); }
private void sBlocksVisibility(T b,
bool vTerminal,
bool vInventory,
bool vToolBar) {
IMySlimBlock sB = b.CubeGrid.GetCubeBlock(b.Position);
b.ShowInTerminal = vTerminal && sB.IsFullIntegrity && sB.BuildIntegrity < 1f;
b.ShowInToolbarConfig = vToolBar;
if(b.HasInventory) { b.ShowInInventory = vInventory; } }
private string m_zeros;
private Dictionary<string, int> m_counetrs; }
public class CT<T> where T : class, IMyTerminalBlock {
public CT(CBB<T> blocks) { m_blocks = blocks; }
public void listProperties(CTS lcd) {
if(m_blocks.count() == 0) { return; }
List<ITerminalProperty> properties = new List<ITerminalProperty>();
m_blocks.blocks()[0].GetProperties(properties);
foreach(var property in properties) {
lcd.echo($"id: {property.Id}, type: {property.TypeName}"); } }
public void listActions(CTS lcd) {
if(m_blocks.count() == 0) { return; }
List<ITerminalAction> actions = new List<ITerminalAction>();
m_blocks.blocks()[0].GetActions(actions);
foreach(var action in actions) {
lcd.echo($"id: {action.Id}, name: {action.Name}"); } }
void showInTerminal(bool show = true) { foreach(T b in m_blocks.blocks()) { if(b.ShowInTerminal != show) { b.ShowInTerminal = show; }}}
void hideInTerminal() { showInTerminal(false); }
void showInToolbarConfig(bool show = true) { foreach(T b in m_blocks.blocks()) { if(b.ShowInToolbarConfig != show) { b.ShowInToolbarConfig = show; }}}
void hideInToolbarConfig() { showInToolbarConfig(false); }
void showInInventory(bool show = true) { foreach(T b in m_blocks.blocks()) { if(b.ShowInInventory != show) { b.ShowInInventory = show; }}}
void hideInInventory() { showInInventory(false); }
void showOnHUD(bool show = true) { foreach(T b in m_blocks.blocks()) { if(b.ShowOnHUD != show) { b.ShowOnHUD = show; }}}
void hideOnHUD() { showOnHUD(false); }
public bool empty() { return m_blocks.empty(); }
public int count() { return m_blocks.count(); }
protected CBB<T> m_blocks; }
public class CBB<T> where T : class, IMyTerminalBlock {
public CBB(bool loadOnlySameGrid = true) { m_blocks = new List<T>(); m_loadOnlySameGrid = loadOnlySameGrid; }
public bool empty() { return count() == 0; }
public int count() { return m_blocks.Count; }
public List<T> blocks() { return m_blocks; }
protected void clear() { m_blocks.Clear(); }
public void removeBlock(T b) { m_blocks.Remove(b); }
public void removeBlockAt(int i) { m_blocks.RemoveAt(i); }
public string subtypeName() { return empty() ? "N/A" : m_blocks[0].DefinitionDisplayNameText; }
public CBO o(T b) { return new CBO(b); }
public bool iA<U>() where U : class, IMyTerminalBlock {
if(empty()) { return false; }
return m_blocks[0] is U; }
protected virtual void load() { _.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => checkBlock(x)); }
protected virtual bool checkBlock(T b) { return m_loadOnlySameGrid ? b.IsSameConstructAs(_.Me) : true; }
protected List<T> m_blocks;
protected bool m_loadOnlySameGrid; }
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
public class CTS {
public CTS() {
m_text = new List<string>();
m_s = new CXYCollection<IMyTextSurface>(); }
public void setSurface(IMyTextSurface surface, float fontSize, int maxLines, int maxColumns, float padding = 0) {
s(fontSize, maxLines, maxColumns, padding);
addSurface(surface, 0, 0); }
public void addSurface(IMyTextSurface surface, int x, int y) {
m_s.set(x, y, s(surface)); }
protected void s(float fontSize, int maxLines, int maxColumns, float padding) {
m_fontSize = fontSize;
m_maxLines = maxLines;
m_maxColumns = maxColumns;
m_padding = padding; }
private IMyTextSurface s(IMyTextSurface surface) {
surface.ContentType = ContentType.TEXT_AND_IMAGE;
surface.Font = "Monospace";
surface.FontColor = new Color(255, 255, 255);
surface.BackgroundColor = new Color(0, 0, 0);
surface.FontSize = m_fontSize;
surface.Alignment = TextAlignment.LEFT;
surface.TextPadding = m_padding;
return surface; }
public void clear() {
clearSurfaces();
m_text.Clear(); }
private void clearSurfaces() {
foreach(IMyTextSurface surface in m_s) { surface.WriteText("", false); } }
private bool unknownTypeEcho(string text) {
if(m_maxLines == 0 && m_s.exists(0, 0)) { m_s.get(0, 0).WriteText(text + '\n', true); return true; }
return false; }
public void echo(string text) {
if(!unknownTypeEcho(text)) {
if(m_text.Count > m_maxLines * m_s.countY()) { m_text.RemoveAt(0); }
m_text.Add(text); }
echoText(); }
public void echo_last(string text) {
if(!unknownTypeEcho(text)) {
m_text[m_text.Count - 1] = text;
echoText(); } }
public void echo_at(string text, int lineNum) {
if(!unknownTypeEcho(text)) {
if(lineNum >= m_text.Count) {
for(int i = m_text.Count; i <= lineNum; i++) { m_text.Add("\n"); } }
m_text[lineNum] = text;
echoText(); } }
private void updateSurface(int x, int y) {
int minColumn = x * m_maxColumns;
int maxColumn = minColumn + m_maxColumns;
int minLine = y * m_maxLines + y;
int maxLine = minLine + m_maxLines;
for(int lineNum = minLine; lineNum <= maxLine; lineNum++) {
if(m_text.Count <= lineNum) { break; }
string line = m_text[lineNum];
int substringLength = line.Length > maxColumn ? m_maxColumns : line.Length - minColumn;
if(substringLength > 0) {
m_s.get(x, y).WriteText(line.Substring(minColumn, substringLength) + '\n', true); }
else {
m_s.get(x, y).WriteText("\n", true); } } }
private void echoText() {
clearSurfaces();
for(int x = 0; x < m_s.countX(); x++) {
for(int y = 0; y < m_s.countY(); y++) {
updateSurface(x, y); } } }
private int m_maxLines;
private int m_maxColumns;
private float m_fontSize;
private float m_padding;
private List<string> m_text;
CXYCollection<IMyTextSurface> m_s; }
public class CXYCollection<T> : IEnumerable {
public CXYCollection() {
m_data = new Dictionary<int, Dictionary<int, T>>(); }
public T get(int x, int y) {
if(exists(x, y)) {
return m_data[x][y]; }
return default(T); }
public void set(int x, int y, T data) {
debug($"set {x}:{y}");
if(!m_data.ContainsKey(x)) { m_data[x] = new Dictionary<int, T>(); }
m_data[x][y] = data; }
public bool exists(int x, int y) {
return m_data.ContainsKey(x) && m_data[x].ContainsKey(y); }
public int count() {
int r = 0;
foreach(KeyValuePair<int, Dictionary<int, T>> i in m_data) {
r += i.Value.Count; }
return r; }
public int countX() { return m_data.Count; }
public int countY() { return empty() ? 0 : m_data[0].Count; }
public bool empty() { return countX() == 0; }
public IEnumerator GetEnumerator() {
foreach(KeyValuePair<int, Dictionary<int, T>> i in m_data) {
foreach(KeyValuePair<int, T> j in i.Value) {
yield return j.Value; } } }
private Dictionary<int, Dictionary<int, T>> m_data; }
public class CB<T> : CBB<T> where T : class, IMyTerminalBlock {
public CB(bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { load(); } }
public class CBT<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBT(string subTypeName, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_subTypeName = subTypeName; load(); }
protected override bool checkBlock(T b) {
return (m_loadOnlySameGrid ? b.IsSameConstructAs(_.Me) : true) && b.BlockDefinition.ToString().Contains(m_subTypeName); }
public string subTypeName() { return m_subTypeName; }
private string m_subTypeName; }
public string program() { return "Настройка структуры"; }
private int gIndex;
public void main(string argument, UpdateType updateSource) {
if(argument.Length == 0) { step(gIndex++); }
else if(argument == "start") { gIndex = 0; Runtime.UpdateFrequency = UpdateFrequency.Update10; }
else if(argument == "stop") { stop(); } }
public void step(int index) {
echoMeBig(index.ToString());
switch(index) {
case 0 : (new CS<IMyPistonBase> (new CB <IMyPistonBase> ())).s("Поршень") ; break;
case 1 : (new CS<IMyBatteryBlock> (new CB <IMyBatteryBlock> ())).s("Батарея") ; break;
case 2 : (new CS<IMySolarPanel> (new CB <IMySolarPanel> ())).s("С.Батарея") ; break;
case 3 : (new CS<IMyRemoteControl> (new CB <IMyRemoteControl> ())).s("ДУ", true, false, true) ; break;
case 4 : (new CS<IMyOreDetector> (new CB <IMyOreDetector> ())).s("Детектор руды") ; break;
case 5 : (new CS<IMyLandingGear> (new CB <IMyLandingGear> ())).s("Шасси") ; break;
case 6 : (new CS<IMyShipDrill> (new CB <IMyShipDrill> ())).s("Бур") ; break;
case 7 : (new CS<IMyShipGrinder> (new CB <IMyShipGrinder> ())).s("Резак") ; break;
case 8 : (new CS<IMyShipWelder> (new CB <IMyShipWelder> ())).s("Сварщик") ; break;
case 9 : (new CS<IMyGyro> (new CB <IMyGyro> ())).s("Гироскоп") ; break;
case 10: (new CS<IMyCollector> (new CB <IMyCollector> ())).s("Коллектор") ; break;
case 11: (new CS<IMyGasGenerator> (new CB <IMyGasGenerator> ())).s("H2:O2 Генератор") ; break;
case 12: (new CS<IMyOxygenFarm> (new CB <IMyOxygenFarm> ())).s("Ферма O2") ; break;
case 13: (new CS<IMyShipMergeBlock> (new CB <IMyShipMergeBlock> ())).s("Соединитель") ; break;
case 14: (new CS<IMyRefinery> (new CB <IMyRefinery> ())).s("Очистительный завод") ; break;
case 15: (new CS<IMyMedicalRoom> (new CB <IMyMedicalRoom> ())).s("Медпост") ; break;
case 16: (new CS<IMySmallGatlingGun> (new CB <IMySmallGatlingGun> ())).s("М.Пушка") ; break;
case 17: (new CS<IMyLargeGatlingTurret> (new CB <IMyLargeGatlingTurret> ())).s("Б.Пушка") ; break;
case 18: (new CS<IMyLargeMissileTurret> (new CB <IMyLargeMissileTurret> ())).s("Б.Ракетница") ; break;
case 19: (new CS<IMyDoor> (new CB <IMyDoor> ())).s("Дверь") ; break;
case 20: (new CS<IMyAirVent> (new CB <IMyAirVent> ())).s("Вентиляция", false, false, true) ; break;
case 21: (new CS<IMyCameraBlock> (new CB <IMyCameraBlock> ())).s("Камера", false, false, true) ; break;
case 22: (new CS<IMyButtonPanel> (new CB <IMyButtonPanel> ())).s("Кнопки") ; break;
case 23: (new CS<IMyRadioAntenna> (new CB <IMyRadioAntenna> ())).s("Антенна") ; break;
case 24: (new CS<IMyLaserAntenna> (new CB <IMyLaserAntenna> ())).s("Л.Антенна") ; break;
case 25: (new CS<IMyTextPanel> (new CB <IMyTextPanel> ())).s("Дисплей") ; break;
case 26: (new CS<IMyShipConnector> (new CB <IMyShipConnector> ())).s("Коннектор", false, false, true) ; break;
case 27: (new CS<IMyTimerBlock> (new CB <IMyTimerBlock> ())).s("Таймер", true, false, true) ; break;
case 28: (new CS<IMySensorBlock> (new CB <IMySensorBlock> ())).s("Сенсор") ; break;
case 29: (new CS<IMyPowerProducer> (new CBT<IMyPowerProducer> ("HydrogenEngine"))).s("H2 Электрогенератор") ; break;
case 30: (new CS<IMyPowerProducer> (new CBT<IMyPowerProducer> ("WindTurbine"))).s("Ветрогенератор") ; break;
case 31: (new CS<IMyThrust> (new CBT<IMyThrust> ("LargeAtmosphericThrust"))).s("БАУ") ; break;
case 32: (new CS<IMyThrust> (new CBT<IMyThrust> ("SmallAtmosphericThrust"))).s("АУ") ; break;
case 33: (new CS<IMyThrust> (new CBT<IMyThrust> ("LargeHydrogenThrust"))).s("БВУ") ; break;
case 34: (new CS<IMyThrust> (new CBT<IMyThrust> ("SmallHydrogenThrust"))).s("ВУ") ; break;
case 35: (new CS<IMyThrust> (new CBT<IMyThrust> ("LargeThrust"))).s("БИУ") ; break;
case 36: (new CS<IMyThrust> (new CBT<IMyThrust> ("SmallThrust"))).s("ИУ") ; break;
case 37: (new CS<IMyGasTank> (new CBT<IMyGasTank> ("OxygenTankSmall"))).s("Бак O2") ; break;
case 38: (new CS<IMyGasTank> (new CBT<IMyGasTank> ("OxygenTank/"))).s("Б.Бак O2") ; break;
case 39: (new CS<IMyGasTank> (new CBT<IMyGasTank> ("/LargeHydrogenTank"))).s("ОБ.Бак H2") ; break;
case 40: (new CS<IMyGasTank> (new CBT<IMyGasTank> ("/LargeHydrogenTankSmall"))).s("Б.Бак H2") ; break;
case 41: (new CS<IMyGasTank> (new CBT<IMyGasTank> ("/SmallHydrogenTank"))).s("Бак H2") ; break;
case 42: (new CS<IMyGasTank> (new CBT<IMyGasTank> ("/SmallHydrogenTankSmall"))).s("Бак H2") ; break;
case 43: (new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("SmallContainer"))).s("МК", false, true) ; break;
case 44: (new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("MediumContainer"))).s("СК", false, true) ; break;
case 45: (new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("LargeContainer"))).s("БК", false, true) ; break;
case 46: (new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("LargeIndustrialContainer"))).s("БК", false, true) ; break;
case 47: (new CS<IMyConveyorSorter> (new CB <IMyConveyorSorter> ())).s("Сортировщик", false, false, true); break;
case 48: (new CS<IMyUpgradeModule> (new CBT<IMyUpgradeModule> ("ProductivityModule"))).s("М.Продуктивности") ; break;
case 49: (new CS<IMyUpgradeModule> (new CBT<IMyUpgradeModule> ("EffectivenessModule"))).s("М.Эффективности") ; break;
case 50: (new CS<IMyUpgradeModule> (new CBT<IMyUpgradeModule> ("EnergyModule"))).s("М.Энергоэффективности") ; break;
case 51: (new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("SmallLight"))).s("Лампа") ; break;
case 52: (new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("Light_1corner"))).s("Угл. Лампа") ; break;
case 53: (new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("Light_2corner"))).s("2хУгл. Лампа") ; break;
case 54: (new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("LightPanel"))).s("Светопанель") ; break;
case 55: (new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("OffsetLight"))).s("Диодная фара") ; break;
case 56: (new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("PassageSciFiLight"))).s("SciFi свет") ; break;
case 57: (new CS<IMyReflectorLight> (new CBT<IMyReflectorLight> ("FrontLight"))).s("Прожектор") ; break;
case 58: (new CS<IMyReflectorLight> (new CBT<IMyReflectorLight> ("RotatingLight"))).s("Вр. прожектор") ; break;
case 59: (new CS<IMyReflectorLight> (new CBT<IMyReflectorLight> ("Spotlight"))).s("Фара") ; break;
case 60: (new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension1x1"))).s("Колесо 1x1 правое") ; break;
case 61: (new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension3x3"))).s("Колесо 3x3 правое") ; break;
case 62: (new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension5x5"))).s("Колесо 5x5 правое") ; break;
case 63: (new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension1x1mirrored"))).s("Колесо 1x1 левое") ; break;
case 64: (new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension3x3mirrored"))).s("Колесо 3x3 левое") ; break;
case 65: (new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension5x5mirrored"))).s("Колесо 5x5 левое") ; break;
case 66: (new CS<IMyCockpit> (new CBT<IMyCockpit> ("LargeBlockCouch"))).s("Диван") ; break;
case 67: (new CS<IMyCockpit> (new CBT<IMyCockpit> ("LargeBlockCouchCorner"))).s("Угл. Диван") ; break;
case 68: (new CS<IMyAssembler> (new CBT<IMyAssembler> ("LargeAssembler"))).s("Сборщик") ; break;
case 69: (new CS<IMyProjector> (new CB <IMyProjector> ())).s("Проектор", false, false, true) ; break;
case 70: (new CS<IMyMotorStator> (new CB <IMyMotorStator> ())).s("Ротор") ; break;
case 71: (new CS<IMyMotorAdvancedStator>(new CB <IMyMotorAdvancedStator>())).s("Ул. Ротор") ; break;
case 72: (new CS<IMyControlPanel> (new CB <IMyControlPanel> ())).s("Панель упр.") ; break;
case 73: (new CS<IMySoundBlock> (new CB <IMySoundBlock> ())).s("Динамик") ; break;
case 74: (new CS<IMyCryoChamber> (new CB <IMyCryoChamber> ())).s("Криокамера") ; break;
default: { stop(); } break; } }
public void stop() { Runtime.UpdateFrequency = UpdateFrequency.None; applyDefaultMeDisplayTexsts(); }
