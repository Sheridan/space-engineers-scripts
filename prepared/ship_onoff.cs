static string sN;
static string scriptName;
static Program _;
static float blockSize;
static CBO prbOptions;
public void applyDefaultMeDisplayTexsts() {
Me.GetSurface(0).WriteText(scriptName.Replace(" ", "\n"));
Me.GetSurface(1).WriteText(sN); }
public static void echoMe(string text, int surface) { _.Me.GetSurface(surface).WriteText(text, false); }
public static void echoMeBig(string text) { echoMe(text, 0); }
public static void echoMeSmall(string text) { echoMe(text, 1); }
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
public class CB<T> : CBB<T> where T : class, IMyTerminalBlock {
public CB(bool lSG = true) : base(lSG) { load(); } }
public class CBB<T> where T : class, IMyEntity {
public CBB(bool lSG = true) { m_blocks = new List<T>(); m_lSG = lSG; }
public bool empty() { return count() == 0; }
public int count() { return m_blocks.Count; }
public List<T> blocks() { return m_blocks; }
protected void clear() { m_blocks.Clear(); }
public void removeBlock(T b) { m_blocks.Remove(b); }
public void removeBlockAt(int i) { m_blocks.RemoveAt(i); }
public T first() { return m_blocks[0]; }
public bool iA<U>() where U : class, IMyEntity {
if(empty()) { return false; }
return m_blocks[0] is U; }
public IEnumerator GetEnumerator() {
foreach(T i in m_blocks) {
yield return i; } }
protected virtual void load() { _.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => checkBlock(x)); }
protected virtual bool checkBlock(T b) {
return m_lSG ? _.Me.IsSameConstructAs(b as IMyTerminalBlock) : true; }
protected List<T> m_blocks;
protected bool m_lSG; }
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
public class CBNamed<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBNamed(string name, bool lSG = true) : base(lSG) { m_name = name; load(); }
protected override bool checkBlock(T b) {
return (m_lSG ? b.IsSameConstructAs(_.Me) : true) && b.CustomName.Contains(m_name); }
public string name() { return m_name; }
private string m_name; }
public class CF<T> : CT<T> where T : class, IMyFunctionalBlock {
public CF(CBB<T> blocks) : base(blocks) {}
public bool enable(bool target = true) {
foreach(IMyFunctionalBlock b in m_blocks) {
if(b.Enabled != target) { b.Enabled = target; } }
return enabled() == target; }
public bool disable() { return enable(false); }
public bool enabled() {
bool r = true;
foreach(IMyFunctionalBlock b in m_blocks) {
r = r && b.Enabled; }
return r; } }
public class CT<T> : CCube<T> where T : class, IMyTerminalBlock {
public CT(CBB<T> blocks) : base(blocks) {}
public void listProperties(CTS lcd) {
if(empty()) { return; }
List<ITerminalProperty> properties = new List<ITerminalProperty>();
first().GetProperties(properties);
foreach(var property in properties) {
lcd.echo($"id: {property.Id}, type: {property.TypeName}"); } }
public void listActions(CTS lcd) {
if(empty()) { return; }
List<ITerminalAction> actions = new List<ITerminalAction>();
first().GetActions(actions);
foreach(var action in actions) {
lcd.echo($"id: {action.Id}, name: {action.Name}"); } }
void showInTerminal(bool show = true) { foreach(T b in m_blocks) { if(b.ShowInTerminal != show) { b.ShowInTerminal = show; }}}
void hideInTerminal() { showInTerminal(false); }
void showInToolbarConfig(bool show = true) { foreach(T b in m_blocks) { if(b.ShowInToolbarConfig != show) { b.ShowInToolbarConfig = show; }}}
void hideInToolbarConfig() { showInToolbarConfig(false); }
void showInInventory(bool show = true) { foreach(T b in m_blocks) { if(b.ShowInInventory != show) { b.ShowInInventory = show; }}}
void hideInInventory() { showInInventory(false); }
void showOnHUD(bool show = true) { foreach(T b in m_blocks) { if(b.ShowOnHUD != show) { b.ShowOnHUD = show; }}}
void hideOnHUD() { showOnHUD(false); } }
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
public class CCube<T> : CEntity<T> where T : class, IMyCubeBlock {
public CCube(CBB<T> blocks) : base(blocks) {} }
public class CEntity<T> where T : class, IMyEntity {
public CEntity(CBB<T> blocks) { m_blocks = blocks; }
public bool empty() { return m_blocks.empty(); }
public int count() { return m_blocks.count(); }
public T first() { return m_blocks.first(); }
public IEnumerator GetEnumerator() {
foreach(T i in m_blocks) {
yield return i; } }
public List<IMyInventory> invertoryes() {
List<IMyInventory> r = new List<IMyInventory>();
foreach(T i in m_blocks) {
for(int j = 0; j < i.InventoryCount; j++) {
r.Add(i.GetInventory(j)); } }
return r; }
protected CBB<T> m_blocks; }
public class CBt : CF<IMyBatteryBlock> {
public CBt(CBB<IMyBatteryBlock> blocks) : base(blocks) { }
public bool setChargeMode(ChargeMode mode) {
bool r = true;
foreach(IMyBatteryBlock b in m_blocks) {
if(b.ChargeMode != mode) { b.ChargeMode = mode; }
r = r && b.ChargeMode == mode; }
return r; }
public bool recharge() { return setChargeMode(ChargeMode.Recharge); }
public bool discharge() { return setChargeMode(ChargeMode.Discharge); }
public bool autocharge() { return setChargeMode(ChargeMode.Auto); } }
public class CC : CF<IMyShipConnector> {
public CC(CBB<IMyShipConnector> blocks) : base(blocks) { }
public bool connect(bool target = true) {
foreach(IMyShipConnector b in m_blocks) {
if(target) { b.Connect(); }
else { b.Disconnect(); } }
return checkConnected(target); }
public bool disconnect() { return connect(false); }
public bool connected() { return checkConnected(true); }
private bool checkConnected(bool target) {
bool r = true;
foreach(IMyShipConnector b in m_blocks) {
r = r &&
(
target ? b.Status == MyShipConnectorStatus.Connected
: b.Status == MyShipConnectorStatus.Unconnected || b.Status == MyShipConnectorStatus.Connectable
); }
return r; }
public bool connectable() {
bool r = true;
foreach(IMyShipConnector b in m_blocks) {
r = r && b.Status == MyShipConnectorStatus.Connectable; }
return r; } }
public class CLG : CF<IMyLandingGear> {
public CLG(CBB<IMyLandingGear> blocks) : base(blocks) { }
public bool lockGear(bool enabled = true) {
foreach(IMyLandingGear b in m_blocks) {
if(enabled) { b.Lock(); }
else { b.Unlock(); } }
return checkLocked(enabled); }
public bool unlockGear() { return lockGear(false); }
public bool locked() { return checkLocked(); }
private bool checkLocked(bool target = true) {
bool r = true;
foreach(IMyLandingGear b in m_blocks) {
r = r && b.IsLocked; }
return r == target; } }
public class CTank : CF<IMyGasTank> {
public CTank(CBB<IMyGasTank> blocks) : base(blocks) { }
public bool enableStockpile(bool enabled = true) {
bool r = true;
foreach(IMyGasTank b in m_blocks) {
if(b.Stockpile != enabled) { b.Stockpile = enabled; }
r = r && b.Stockpile == enabled; }
return r; }
public bool disableStockpile() { return enableStockpile(false); } }
public CF<IMyGyro> gyroscopes;
public CF<IMyThrust> thrusters;
public CF<IMyLightingBlock> lamps;
public CF<IMyRadioAntenna> antennas;
public CF<IMyOreDetector> oreDetectors;
public CBt battaryes;
public CC connectors;
public CLG landGears;
public CTank tanks;
IMyProgrammableBlock pbAutoHorizont;
IMyProgrammableBlock pbShipTools;
public string program() {
pbAutoHorizont = _.GridTerminalSystem.GetBlockWithName($"[{sN}] ПрБ Атоматический горизонт") as IMyProgrammableBlock;
pbShipTools = _.GridTerminalSystem.GetBlockWithName($"[{sN}] ПрБ Управление инструментом") as IMyProgrammableBlock;
gyroscopes = new CF<IMyGyro>(new CB<IMyGyro>());
thrusters = new CF<IMyThrust>(new CB<IMyThrust>());
battaryes = new CBt(new CB<IMyBatteryBlock>());
connectors = new CC(new CBNamed<IMyShipConnector>("Главный"));
landGears = new CLG(new CB<IMyLandingGear>());
lamps = new CF<IMyLightingBlock>(new CB<IMyLightingBlock>());
antennas = new CF<IMyRadioAntenna>(new CB<IMyRadioAntenna>());
oreDetectors = new CF<IMyOreDetector>(new CB<IMyOreDetector>());
tanks = new CTank(new CB<IMyGasTank>());
debug($"{connected()}:{connectors.count()}:{landGears.count()}");
return "Управление стыковкой корабля"; }
public void main(string argument, UpdateType updateSource) {
if(argument == "start") {
if(connected()) { turnOn(); }
else { turnOff(); } } }
public bool connected() { return connectors.connected(); }
public void turnOn() {
debug("On");
battaryes.autocharge();
tanks.disableStockpile();
thrusters.enable();
gyroscopes.enable();
antennas.enable();
oreDetectors.enable();
if(pbAutoHorizont != null) { pbAutoHorizont.TryRun("init_restart"); }
lamps.enable();
connectors.disconnect();
landGears.unlockGear();
if(pbShipTools != null) { pbShipTools.TryRun("stop"); } }
public void turnOff() {
debug("Off");
if(connectors.connectable()) {
connectors.connect();
landGears.lockGear();
lamps.disable();
if(pbAutoHorizont != null) { pbAutoHorizont.TryRun("stop"); }
if(pbShipTools != null) { pbShipTools .TryRun("stop"); }
gyroscopes.disable();
thrusters.disable();
oreDetectors.disable();
antennas.disable();
tanks.enableStockpile();
battaryes.recharge(); } }
