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
public class CBSD : CD {
public CBSD() : base() {}
private string gFBS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyFunctionalBlock>()) { return ""; }
string r = "";
int pOn = 0;
int fOn = 0;
int wOn = 0;
float powerConsumed = 0f;
float powerMaxConsumed = 0f;
foreach(IMyFunctionalBlock block in group.blocks()) {
if(block.Enabled) {
pOn++;
CBPI pInfo = new CBPI(block);
powerConsumed += pInfo.currentConsume();
powerMaxConsumed += pInfo.maxConsume(); }
if(block.IsFunctional) { fOn++; }
if(block.IsWorking) { wOn++; } }
r += $"PFW: {pOn}:{fOn}:{wOn} ";
if(powerMaxConsumed > 0) {
r += $"Consuming (now,max): {tHR(powerConsumed, EHRU.Power)}:{tHR(powerMaxConsumed, EHRU.Power)} "; }
return r; }
private string gRS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyMotorStator>()) { return ""; }
string r = "";
List<string> rpm = new List<string>();
List<string> angle = new List<string>();
foreach(IMyMotorStator block in group.blocks()) {
float angleGrad = block.Angle * 180 / (float)Math.PI;
rpm.Add($"{block.TargetVelocityRPM:f2}");
angle.Add($"{angleGrad:f2}°"); }
r += $"Angle: {string.Join(":", angle)} "
+ $"RPM: {string.Join(":", rpm)} ";
return r; }
private string gGTS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyGasTank>()) { return ""; }
string r = "";
float capacity = 0;
double filledRatio = 0;
foreach(IMyGasTank block in group.blocks()) {
capacity += block.Capacity;
filledRatio += block.FilledRatio; }
r += $"Capacity: {tHR(capacity, EHRU.Volume)} "
+ $"Filled: {filledRatio/group.count()*100:f2}% ";
return r; }
private string gBS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyBatteryBlock>()) { return ""; }
string r = "";
float currentStored = 0;
float maxStored = 0;
foreach(IMyBatteryBlock block in group.blocks()) {
currentStored += block.CurrentStoredPower;
maxStored += block.MaxStoredPower; }
currentStored *= 1000000;
maxStored *= 1000000;
r += $"Capacity: {tHR(currentStored, EHRU.PowerCapacity)}:{tHR(maxStored, EHRU.PowerCapacity)} ";
return r; }
private string gIS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
long volume = 0;
long volumeMax = 0;
int mass = 0;
int items = 0;
int inventoryes = 0;
foreach(IMyTerminalBlock block in group.blocks()) {
if(block.HasInventory) {
IMyInventory inventory;
inventoryes = block.InventoryCount;
for(int i = 0; i < inventoryes; i++) {
inventory = block.GetInventory(i);
volume += inventory.CurrentVolume.ToIntSafe();
volumeMax += inventory.MaxVolume.ToIntSafe();
mass += inventory.CurrentMass.ToIntSafe();
items += inventory.ItemCount; } } }
if(inventoryes > 0) {
mass *= 1000;
return $"VMI: ({tHR(volume, EHRU.Volume)}:{tHR(volumeMax, EHRU.Volume)}):{tHR(mass, EHRU.Mass)}:{tHR(items)} from {inventoryes} "; }
return ""; }
private string gPsS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyPistonBase>()) { return ""; }
string r = "";
List<string> positions = new List<string>();
int statusStopped = 0;
int statusExtending = 0;
int statusExtended = 0;
int statusRetracting = 0;
int statusRetracted = 0;
foreach(IMyPistonBase block in group.blocks()) {
switch(block.Status) {
case PistonStatus.Stopped: statusStopped++; break;
case PistonStatus.Extending: statusExtending++; break;
case PistonStatus.Extended: statusExtended++; break;
case PistonStatus.Retracting: statusRetracting++; break;
case PistonStatus.Retracted: statusRetracted++; break; }
positions.Add($"{block.CurrentPosition:f2}"); }
r += $"SeErR: {statusStopped}:{statusExtending}:{statusExtended}:{statusRetracting}:{statusRetracted} "
+ $"Pos: {string.Join(":", positions)} ";
return r; }
private string gGS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyGyro>()) { return ""; }
string r = "";
float yaw = 0;
float pitch = 0;
float roll = 0;
foreach(IMyGyro block in group.blocks()) {
yaw += Math.Abs(block.Yaw);
pitch += Math.Abs(block.Pitch);
roll += Math.Abs(block.Roll); }
r += $"YPR: {yaw/group.count():f4}:{pitch/group.count():f4}:{roll/group.count():f4} ";
return r; }
private string gMS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyShipMergeBlock>()) { return ""; }
string r = "";
int connected = 0;
foreach(IMyShipMergeBlock block in group.blocks()) {
if(block.IsConnected) { connected++; } }
r += $"Connected: {connected} ";
return r; }
private string gCS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyShipConnector>()) { return ""; }
string r = "";
int statusUnconnected = 0;
int statusConnectable = 0;
int statusConnected = 0;
foreach(IMyShipConnector block in group.blocks()) {
switch(block.Status) {
case MyShipConnectorStatus.Unconnected: statusUnconnected++; break;
case MyShipConnectorStatus.Connectable: statusConnectable++; break;
case MyShipConnectorStatus.Connected: statusConnected++; break; } }
r += $"UcC: {statusUnconnected}:{statusConnectable}:{statusConnected} ";
return r; }
private string gPS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyProjector>()) { return ""; }
string r = "";
int projecting = 0;
List<string> blocksTotal = new List<string>();
List<string> blocksRemaining = new List<string>();
List<string> blocksBuildable = new List<string>();
foreach(IMyProjector block in group.blocks()) {
if(block.IsProjecting) { projecting++; }
blocksTotal.Add($"{block.TotalBlocks}");
blocksRemaining.Add($"{block.RemainingBlocks}");
blocksBuildable.Add($"{block.BuildableBlocksCount}"); }
r += $"Pr: {projecting} "
+ $"B: {string.Join(":", blocksBuildable)} "
+ $"R: {string.Join(":", blocksRemaining)} "
+ $"T: {string.Join(":", blocksTotal)} "
;
return r; }
private string gPPS<T>(CBB<T> group) where T : class, IMyTerminalBlock {
if(!group.iA<IMyPowerProducer>()) { return ""; }
string r = "";
float currentOutput = 0f;
float maxOutput = 0f;
foreach(IMyPowerProducer block in group.blocks()) {
CBPI pInfo = new CBPI(block);
currentOutput += pInfo.currentProduce();
maxOutput += pInfo.maxProduce(); }
r += $"Ген. энергии (now:max): {tHR(currentOutput, EHRU.Power)}:{tHR(maxOutput, EHRU.Power)} ";
return r; }
public void sS<T>(CBB<T> group, int position) where T : class, IMyTerminalBlock {
string r = $"[{group.subtypeName()}] ";
if(!group.empty()) {
r += $"({group.count()}) "
+ gPsS<T>(group)
+ gCS<T>(group)
+ gMS<T>(group)
+ gPS<T>(group)
+ gRS<T>(group)
+ gGS<T>(group)
+ gBS<T>(group)
+ gGTS<T>(group)
+ gPPS<T>(group)
+ gIS<T>(group)
+ gFBS<T>(group)
; }
else {
r += "Таких блоков нет"; }
echo_at(r, position); } }
public class CD : CTS {
public CD() : base()
{}
private void mineDimensions(IMyTextPanel display) {
debug($"{display.BlockDefinition.SubtypeName}");
switch(display.BlockDefinition.SubtypeName) {
case "LargeLCDPanelWide" : s(0.602f, 28, 87, 0.35f); break;
case "LargeLCDPanel" : s(0.602f, 28, 44, 0.35f); break;
case "TransparentLCDLarge": s(0.602f, 28, 44, 0.35f); break;
case "TransparentLCDSmall": s(0.602f, 26, 40, 4f); break;
default: s(1f, 1, 1, 1f); break; } }
public void aDs(string name) {
CBNamed<IMyTextPanel> displays = new CBNamed<IMyTextPanel>(name);
if(displays.empty()) { throw new System.ArgumentException("Не найдены дисплеи", name); }
mineDimensions(displays.blocks()[0]);
foreach(IMyTextPanel display in displays.blocks()) {
CBO o = displays.o(display);
int x = o.g("display", "x", -1);
int y = o.g("display", "y", -1);
if(x<0 || y<0) { throw new System.ArgumentException("Не указаны координаты дисплея", display.CustomName); }
addSurface(display as IMyTextSurface, x, y); }
clear(); } }
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
public class CBNamed<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBNamed(string name, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_name = name; load(); }
protected override bool checkBlock(T b) {
return (m_loadOnlySameGrid ? b.IsSameConstructAs(_.Me) : true) && b.CustomName.Contains(m_name); }
public string name() { return m_name; }
private string m_name; }
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
class CBU {
public CBU(IMyUpgradableBlock upBlock) {
Dictionary<string, float> upgrades = new Dictionary<string, float>();
upBlock.GetUpgrades(out upgrades);
Effectiveness = upgrades.ContainsKey("Effectiveness") ? upgrades["Effectiveness" ] : 0;
Productivity = upgrades.ContainsKey("Productivity") ? upgrades["Productivity" ] : 0;
PowerEfficiency = upgrades.ContainsKey("PowerEfficiency") ? upgrades["PowerEfficiency"] : 0; }
public float calcPowerUse(float power) {
return (power+((power/2)*(Productivity*2)))/(PowerEfficiency > 1 ? (float)Math.Pow(1.2228445f, PowerEfficiency) : 1); }
private float Effectiveness;
private float Productivity;
private float PowerEfficiency; }
public class CBPI {
public CBPI(IMyTerminalBlock block) {
m_block = block;
m_blockSinkComponent = m_block.Components.Get<MyResourceSinkComponent>(); }
public bool canProduce() { return m_block is IMyPowerProducer; }
public bool canConsume() {
return m_blockSinkComponent != null && m_blockSinkComponent.IsPoweredByType(Electricity); }
public float currentProduce() {
if(canProduce()) { return (m_block as IMyPowerProducer).CurrentOutput*1000000; }
return 0f; }
public float maxProduce() {
if(canProduce()) { return (m_block as IMyPowerProducer).MaxOutput*1000000; }
return 0f; }
public float currentConsume() {
if(canConsume()) {
float r = m_blockSinkComponent.CurrentInputByType(Electricity);
return r * 1000000; }
return 0f; }
public float maxConsume() {
if(canConsume()) {
float r = m_blockSinkComponent.MaxRequiredInputByType(Electricity);
if(m_block is IMyAssembler || m_block is IMyRefinery) {
CBU upgrades = new CBU(m_block as IMyUpgradableBlock);
upgrades.calcPowerUse(r); }
return r * 1000000; }
return 0f; }
MyResourceSinkComponent m_blockSinkComponent;
IMyTerminalBlock m_block;
private static readonly MyDefinitionId Electricity = MyDefinitionId.Parse("MyObjectBuilder_GasProperties/Electricity"); }
public enum EHRU {
None,
Mass,
Volume,
Power,
PowerCapacity }
public static string hrSuffix(EHRU unit) {
switch(unit) {
case EHRU.None : return "шт.";
case EHRU.Mass : return "г.";
case EHRU.Volume : return "м³";
case EHRU.Power : return "Вт.";
case EHRU.PowerCapacity : return "ВтЧ."; }
return ""; }
public static string tHR(float value, EHRU unit = EHRU.None) {
int divider = unit == EHRU.Volume ? 1000000000 : 1000;
string suffix = hrSuffix(unit);
if(unit == EHRU.Mass) {
if(value >= 1000) {
suffix = "Т.";
value = value/1000; } }
if(value < divider) { return $"{value}{suffix}"; }
int exp = (int)(Math.Log(value) / Math.Log(divider));
return $"{value / Math.Pow(divider, exp):f2}{("кМГТПЭ")[exp - 1]}{suffix}"; }
public class CB<T> : CBB<T> where T : class, IMyTerminalBlock {
public CB(bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { load(); } }
public class CBT<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBT(string subTypeName, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_subTypeName = subTypeName; load(); }
protected override bool checkBlock(T b) {
return (m_loadOnlySameGrid ? b.IsSameConstructAs(_.Me) : true) && b.BlockDefinition.ToString().Contains(m_subTypeName); }
public string subTypeName() { return m_subTypeName; }
private string m_subTypeName; }
CBSD lcd;
public CB<IMyShipDrill> drills;
public CB<IMyShipConnector> connectors;
public CB<IMyCargoContainer> storage;
public CB<IMyThrust> thrusters;
public CB<IMyGyro> gyroscopes;
public CB<IMyBatteryBlock> battaryes;
public void initGroups() {
drills = new CB<IMyShipDrill>();
connectors = new CB<IMyShipConnector>();
storage = new CB<IMyCargoContainer>();
thrusters = new CB<IMyThrust>();
gyroscopes = new CB<IMyGyro>();
battaryes = new CB<IMyBatteryBlock>(); }
public string program() {
Runtime.UpdateFrequency = UpdateFrequency.Update100;
lcd = new CBSD();
lcd.aDs("Статус");
initGroups();
return "Отображение статуса"; }
public void main(string argument, UpdateType updateSource) {
int i = 0;
lcd.sS<IMyShipDrill>(drills, i++);
lcd.sS<IMyCargoContainer>(storage, i++);
lcd.sS<IMyShipConnector>(connectors, i++);
lcd.sS<IMyThrust>(thrusters, i++);
lcd.sS<IMyGyro>(gyroscopes, i++);
lcd.sS<IMyBatteryBlock>(battaryes, i++); }
