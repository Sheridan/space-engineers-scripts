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
public class CF<T> : CT<T> where T : class, IMyTerminalBlock {
public CF(CBB<T> blocks) : base(blocks) {}
public void enable(bool enabled = true) { foreach(IMyFunctionalBlock block in m_blocks.blocks()) { if(block.Enabled != enabled) { block.Enabled = enabled; }}}
public void disable() { enable(false); } }
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
void showInTerminal(bool show = true) { foreach(T block in m_blocks.blocks()) { if(block.ShowInTerminal != show) { block.ShowInTerminal = show; }}}
void hideInTerminal() { showInTerminal(false); }
void showInToolbarConfig(bool show = true) { foreach(T block in m_blocks.blocks()) { if(block.ShowInToolbarConfig != show) { block.ShowInToolbarConfig = show; }}}
void hideInToolbarConfig() { showInToolbarConfig(false); }
void showInInventory(bool show = true) { foreach(T block in m_blocks.blocks()) { if(block.ShowInInventory != show) { block.ShowInInventory = show; }}}
void hideInInventory() { showInInventory(false); }
void showOnHUD(bool show = true) { foreach(T block in m_blocks.blocks()) { if(block.ShowOnHUD != show) { block.ShowOnHUD = show; }}}
void hideOnHUD() { showOnHUD(false); }
protected CBB<T> m_blocks; }
public class CTS {
public CTS() {
m_text = new List<string>();
m_s = new List<List<IMyTextSurface>>(); }
public void setSurface(IMyTextSurface surface, float fontSize, int maxLines, int maxColumns, float padding = 0) {
s(fontSize, maxLines, maxColumns, padding);
addSurface(surface, 0, 0); }
public void addSurface(IMyTextSurface surface, int x, int y) {
if(csX() <= x) { m_s.Add(new List<IMyTextSurface>()); }
if(csY(x) <= y) { m_s[x].Add(surface); }
else { m_s[x][y] = surface; }
s(); }
public void s(float fontSize, int maxLines, int maxColumns, float padding) {
m_fontSize = fontSize;
m_maxLines = maxLines;
m_maxColumns = maxColumns;
m_padding = padding;
s(); }
private void s() {
foreach(List<IMyTextSurface> sfList in m_s) {
foreach(IMyTextSurface surface in sfList) {
surface.ContentType = ContentType.TEXT_AND_IMAGE;
surface.Font = "Monospace";
surface.FontColor = new Color(255, 255, 255);
surface.BackgroundColor = new Color(0, 0, 0);
surface.FontSize = m_fontSize;
surface.Alignment = TextAlignment.LEFT;
surface.TextPadding = m_padding; } }
clear(); }
public void clear() {
foreach(List<IMyTextSurface> sfList in m_s) {
foreach(IMyTextSurface surface in sfList) {
surface.WriteText("", false); } } }
private bool surfaceExists(int x, int y) {
return y < csY(x); }
private bool unknownTypeEcho(string text) {
if(m_maxLines == 0 && surfaceExists(0, 0)) { m_s[0][0].WriteText(text + '\n', true); return true; }
return false; }
private int csX() { return m_s.Count; }
private int csY(int x) { return x < csX() ? m_s[x].Count : 0; }
public void echo(string text) {
if(!unknownTypeEcho(text)) {
if(m_text.Count > m_maxLines * csY(0)) { m_text.RemoveAt(0); }
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
m_s[x][y].WriteText(line.Substring(minColumn, substringLength) + '\n', true); }
else {
m_s[x][y].WriteText("\n", true); } } }
private void echoText() {
clear();
for(int x = 0; x < csX(); x++) {
for(int y = 0; y < csY(x); y++) {
updateSurface(x, y); } } }
private int m_maxLines;
private int m_maxColumns;
private float m_fontSize;
private float m_padding;
private List<string> m_text;
private List<List<IMyTextSurface>> m_s; }
public class CBattery : CF<IMyBatteryBlock> {
public CBattery(CBB<IMyBatteryBlock> blocks) : base(blocks) { }
public bool setChargeMode(ChargeMode mode) {
bool result = true;
foreach(IMyBatteryBlock battery in m_blocks.blocks()) {
if(battery.ChargeMode != mode) { battery.ChargeMode = mode; }
result = result && battery.ChargeMode == mode; }
return result; }
public bool recharge() { return setChargeMode(ChargeMode.Recharge); }
public bool discharge() { return setChargeMode(ChargeMode.Discharge); }
public bool autocharge() { return setChargeMode(ChargeMode.Auto); } }
public class CConnector : CF<IMyShipConnector> {
public CConnector(CBB<IMyShipConnector> blocks) : base(blocks) { }
public bool connect(bool enabled = true) {
bool result = true;
foreach(IMyShipConnector connector in m_blocks.blocks()) {
if(enabled) { connector.Connect(); }
else { connector.Disconnect(); }
result = result &&
(
enabled ? connector.Status == MyShipConnectorStatus.Connected
: connector.Status == MyShipConnectorStatus.Unconnected
); }
return result; }
public bool disconnect() { return connect(false); } }
public class CLandingGear : CF<IMyLandingGear> {
public CLandingGear(CBB<IMyLandingGear> blocks) : base(blocks) { }
public bool lockGear(bool enabled = true) {
bool result = true;
foreach(IMyLandingGear lg in m_blocks.blocks()) {
if(enabled) { lg.Lock(); }
else { lg.Unlock(); }
result = result && lg.IsLocked; }
return result; }
public bool unlockGear() { return lockGear(false); } }
public class CTank : CF<IMyGasTank> {
public CTank(CBB<IMyGasTank> blocks) : base(blocks) { }
public bool enableStockpile(bool enabled = true) {
bool result = true;
foreach(IMyGasTank tank in m_blocks.blocks()) {
if(tank.Stockpile != enabled) { tank.Stockpile = enabled; }
result = result && tank.Stockpile == enabled; }
return result; }
public bool disableStockpile() { return enableStockpile(false); } }
public CF<IMyGyro> gyroscopes;
public CF<IMyThrust> thrusters;
public CF<IMyLightingBlock> lamps;
public CF<IMyRadioAntenna> antennas;
public CF<IMyOreDetector> oreDetectors;
public CF<IMyShipToolBase> tools;
public CBattery battaryes;
public CConnector connectors;
public CLandingGear landGears;
public CTank tanks;
IMyProgrammableBlock pbAutoHorizont;
bool connected;
public string program() {
pbAutoHorizont = self.GridTerminalSystem.GetBlockWithName($"[{structureName}] ПрБ Атоматический горизонт") as IMyProgrammableBlock;
gyroscopes = new CF<IMyGyro>(new CB<IMyGyro>());
thrusters = new CF<IMyThrust>(new CB<IMyThrust>());
battaryes = new CBattery(new CB<IMyBatteryBlock>());
connectors = new CConnector(new CB<IMyShipConnector>());
landGears = new CLandingGear(new CB<IMyLandingGear>());
lamps = new CF<IMyLightingBlock>(new CB<IMyLightingBlock>());
antennas = new CF<IMyRadioAntenna>(new CB<IMyRadioAntenna>());
oreDetectors = new CF<IMyOreDetector>(new CB<IMyOreDetector>());
tools = new CF<IMyShipToolBase>(new CB<IMyShipToolBase>());
tanks = new CTank(new CB<IMyGasTank>());
connected = true;
return "Управление стыковкой корабля"; }
public void main(string argument, UpdateType updateSource) {
if(argument == "start") {
if(connected) { turnOff(); }
else { turnOn(); } } }
public void turnOn() {
debug("On");
battaryes.autocharge();
tanks.disableStockpile();
thrusters.enable();
gyroscopes.enable();
antennas.enable();
oreDetectors.enable();
if(pbAutoHorizont != null) { pbAutoHorizont.TryRun("start"); }
lamps.enable();
connectors.disconnect();
landGears.unlockGear();
connected = true; }
public void turnOff() {
debug("Off");
if(connectors.connect()) {
landGears.lockGear();
tools.disable();
lamps.disable();
if(pbAutoHorizont != null) { pbAutoHorizont.TryRun("stop"); }
gyroscopes.disable();
thrusters.disable();
oreDetectors.disable();
antennas.disable();
tanks.enableStockpile();
battaryes.recharge();
connected = false; } }
