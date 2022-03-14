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
public class CCollector : CF<IMyCollector> {
public CCollector(CBB<IMyCollector> blocks) : base(blocks) { } }
public class CF<T> : CT<T> where T : class, IMyTerminalBlock {
public CF(CBB<T> blocks) : base(blocks) {}
public bool enable(bool target = true) {
foreach(IMyFunctionalBlock b in m_blocks.blocks()) {
if(b.Enabled != target) { b.Enabled = target; } }
return enabled() == target; }
public bool disable() { return enable(false); }
public bool enabled() {
bool r = true;
foreach(IMyFunctionalBlock b in m_blocks.blocks()) {
r = r && b.Enabled; }
return r; } }
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
public class CLamp : CF<IMyLightingBlock> {
public CLamp(CBB<IMyLightingBlock> blocks) : base(blocks) { } }
public class CShipTool : CF<IMyShipToolBase> {
public CShipTool(CBB<IMyShipToolBase> blocks) : base(blocks) { }
public bool on(bool target = true) {
return enable(target) && activated(target); }
public bool off() { return on(false); }
public bool active() { return activated(true); }
private bool activated(bool target) {
bool r = true;
foreach(IMyShipToolBase b in m_blocks.blocks()) {
r = r && b.IsActivated == target; }
return r; } }
public class CB<T> : CBB<T> where T : class, IMyTerminalBlock {
public CB(bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { load(); } }
CLamp lamps;
CShipTool tools;
CCollector collectors;
public string program() {
lamps = new CLamp(new CB<IMyLightingBlock>());
tools = new CShipTool(new CB<IMyShipToolBase>());
collectors = new CCollector(new CB<IMyCollector>());
return "Управление статусом стирателя"; }
public void main(string argument, UpdateType updateSource) {
switch(argument) {
case "on" : { on(); } break;
case "off": { off(); } break; } }
public void on() {
lamps.enable();
tools.enable();
collectors.enable(); }
public void off() {
lamps.disable();
tools.disable();
collectors.disable(); }
