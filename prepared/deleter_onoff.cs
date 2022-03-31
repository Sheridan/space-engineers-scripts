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
public class CBB<T> where T : class, IMyEntity {
public CBB(bool lSG = true) { m_blocks = new List<T>(); m_lSG = lSG; }
public bool empty() { return count() == 0; }
public int count() { return m_blocks.Count; }
public List<T> blocks() { return m_blocks; }
protected void clear() { m_blocks.Clear(); }
public void removeBlock(T b) { m_blocks.Remove(b); }
public void removeBlockAt(int i) { m_blocks.RemoveAt(i); }
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
foreach(IMyShipToolBase b in m_blocks) {
r = r && b.IsActivated == target; }
return r; } }
public class CB<T> : CBB<T> where T : class, IMyTerminalBlock {
public CB(bool lSG = true) : base(lSG) { load(); } }
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
