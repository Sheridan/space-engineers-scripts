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
if(exists(section, name)) { return m_ini.Get(section, name).ToString().Trim(); }
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
public CS(CBB<T> blocks,
string name,
bool visibleInTerminal,
bool visibleInInventory,
bool visibleInToolBar) : base(blocks) {
m_name = name;
m_visibleInTerminal = visibleInTerminal;
m_visibleInInventory = visibleInInventory;
m_visibleInToolBar = visibleInToolBar;
m_zeros = new string('0', count().ToString().Length);
m_counetrs = new Dictionary<string, int>();
echoMeBig(String.Join(Environment.NewLine, m_name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) + $"\n{count()}"); }
public bool s(int index) {
if(index >= count()) { return true; }
echoMeSmall(index.ToString());
T b = m_blocks[index];
string suffix = "";
CBO o = new CBO(b);
if(m_blocks.iA<IMyShipConnector >()) { suffix = s(o, b as IMyShipConnector); }
else if(m_blocks.iA<IMyInteriorLight >()) { suffix = s(o, b as IMyInteriorLight); }
else if(m_blocks.iA<IMyConveyorSorter >()) { suffix = s(o, b as IMyConveyorSorter); }
else if(m_blocks.iA<IMyLargeTurretBase>()) { suffix = s(o, b as IMyLargeTurretBase); }
else if(m_blocks.iA<IMyAssembler >()) { suffix = s(o, b as IMyAssembler); }
else if(m_blocks.iA<IMyReflectorLight >()) { suffix = s(o, b as IMyReflectorLight); }
b.CustomName = generateName(suffix, o);
sBlocksVisibility(b,
o.g("generic", "visibleInTerminal", m_visibleInTerminal),
o.g("generic", "visibleInInventory", m_visibleInInventory),
o.g("generic", "visibleInToolBar", m_visibleInToolBar));
return false; }
private string generateName(string suffix, CBO o) {
string purpose = loadPurpose(o);
string module = loadModuleName(o);
string baseName = TrimAllSpaces($"{module} {m_name} {purpose} {suffix}");
if(count() > 0) {
if(!m_counetrs.ContainsKey(baseName)) { m_counetrs.Add(baseName, 0); }
string order = m_counetrs[baseName].ToString(m_zeros);
m_counetrs[baseName]++;
return $"[{sN}] {baseName} {order}"; }
return $"[{sN}] {baseName}"; }
private string s(CBO o, IMyShipConnector b) {
b.PullStrength = o.g("connector", "strength", 0.5f);;
b.CollectAll = o.g("connector", "collectAll", false);
b.ThrowOut = o.g("connector", "throwOut", false);
return string.Empty; }
private string s(CBO o, IMyInteriorLight b) {
b.Radius = 100f;
b.Intensity = 10f;
b.Falloff = 3f;
b.Color = o.g("lamp", "color", Color.White);
return string.Empty; }
private string s(CBO o, IMyReflectorLight b) {
b.Radius = 160f;
b.Intensity = 100f;
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
private string loadPurpose(CBO o) { return o.g("generic", "purpose", ""); }
private string loadModuleName(CBO o) {
string r = o.g("generic", "module", "");
return string.IsNullOrEmpty(r) ? "" : $"<{r}>"; }
private void sBlocksVisibility(T b,
bool vTerminal,
bool vInventory,
bool vToolBar) {
b.ShowInTerminal = vTerminal;
b.ShowInToolbarConfig = vToolBar;
if(b.HasInventory) { b.ShowInInventory = vInventory; } }
private string m_zeros;
private Dictionary<string, int> m_counetrs;
string m_name;
bool m_visibleInTerminal;
bool m_visibleInInventory;
bool m_visibleInToolBar; }
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
public class CBB<T> where T : class, IMyEntity {
public CBB(bool lSG = true) { m_blocks = new List<T>(); m_lSG = lSG; }
public bool empty() { return count() == 0; }
public int count() { return m_blocks.Count; }
public List<T> blocks() { return m_blocks; }
protected void clear() { m_blocks.Clear(); }
public void removeBlock(T b) { m_blocks.Remove(b); }
public void removeBlockAt(int i) { m_blocks.RemoveAt(i); }
public T first() { return m_blocks[0]; }
public T this[int i] { get { return m_blocks[i]; } }
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
public class CB<T> : CBB<T> where T : class, IMyEntity {
public CB(bool lSG = true) : base(lSG) { load(); } }
public class CBT<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBT(string subTypeName, bool lSG = true) : base(lSG) { m_subTypeName = subTypeName; load(); }
protected override bool checkBlock(T b) {
return (m_lSG ? _.Me.IsSameConstructAs(b) : true) && b.BlockDefinition.ToString().Contains(m_subTypeName); }
public string subTypeName() { return m_subTypeName; }
private string m_subTypeName; }
public string program() { buildActions(); return "Настройка структуры"; }
private int gIndex;
private List<Action> actions;
public void main(string argument, UpdateType updateSource) {
if(argument.Length == 0) { step(gIndex); }
else if(argument == "start") { gIndex = 0; Runtime.UpdateFrequency = UpdateFrequency.Update1; }
else if(argument == "start slow") { gIndex = 0; Runtime.UpdateFrequency = UpdateFrequency.Update10; }
else if(argument == "start very slow") { gIndex = 0; Runtime.UpdateFrequency = UpdateFrequency.Update100; }
else if(argument == "stop") { stop(); } }
private object c_s = null;
private int bIndex;
private void buildActions() {
actions = new List<Action>();
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("SmallLight"), "Лампа", false, false, false); bIndex = 0; } if(((CS<IMyInteriorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("Light_1corner"), "Угл. Лампа", false, false, false); bIndex = 0; } if(((CS<IMyInteriorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("Light_2corner"), "2хУгл. Лампа", false, false, false); bIndex = 0; } if(((CS<IMyInteriorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("LightPanel"), "Светопанель", false, false, false); bIndex = 0; } if(((CS<IMyInteriorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("OffsetLight"), "Диодная фара", false, false, false); bIndex = 0; } if(((CS<IMyInteriorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyInteriorLight> (new CBT<IMyInteriorLight> ("PassageSciFiLight"), "SciFi свет", false, false, false); bIndex = 0; } if(((CS<IMyInteriorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyReflectorLight> (new CBT<IMyReflectorLight> ("FrontLight"), "Прожектор", false, false, false); bIndex = 0; } if(((CS<IMyReflectorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyReflectorLight> (new CBT<IMyReflectorLight> ("RotatingLight"), "Вр. прожектор", false, false, false); bIndex = 0; } if(((CS<IMyReflectorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyReflectorLight> (new CBT<IMyReflectorLight> ("Spotlight"), "Фара", false, false, false); bIndex = 0; } if(((CS<IMyReflectorLight>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCryoChamber> (new CBT<IMyCryoChamber> ("Bed"), "Кровать", false, false, false); bIndex = 0; } if(((CS<IMyCryoChamber>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCryoChamber> (new CBT<IMyCryoChamber> ("Cryo"), "Криокамера", false, false, false); bIndex = 0; } if(((CS<IMyCryoChamber>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyControlPanel> (new CB <IMyControlPanel> (), "Панель упр.", false, false, false); bIndex = 0; } if(((CS<IMyControlPanel>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMySoundBlock> (new CB <IMySoundBlock> (), "Динамик", false, false, false); bIndex = 0; } if(((CS<IMySoundBlock>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyButtonPanel> (new CB <IMyButtonPanel> (), "Кнопки", false, false, false); bIndex = 0; } if(((CS<IMyButtonPanel>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyTimerBlock> (new CB <IMyTimerBlock> (), "Таймер", true, false, true); bIndex = 0; } if(((CS<IMyTimerBlock>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMySensorBlock> (new CB <IMySensorBlock> (), "Сенсор", false, false, false); bIndex = 0; } if(((CS<IMySensorBlock>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyTextPanel> (new CB <IMyTextPanel> (), "Дисплей", false, false, false); bIndex = 0; } if(((CS<IMyTextPanel>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyLandingGear> (new CBT<IMyLandingGear> ("LandingGear"), "Шасси", false, false, false); bIndex = 0; } if(((CS<IMyLandingGear>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyLandingGear> (new CBT<IMyLandingGear> ("MagneticPlate"), "Магнитоплита", false, false, false); bIndex = 0; } if(((CS<IMyLandingGear>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyLandingGear> (new CBT<IMyLandingGear> ("SmallMagneticPlate"), "Магнитоплита малая", false, false, false); bIndex = 0; } if(((CS<IMyLandingGear>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyShipConnector> (new CB <IMyShipConnector> (), "Коннектор", false, false, true); bIndex = 0; } if(((CS<IMyShipConnector>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyShipMergeBlock> (new CB <IMyShipMergeBlock> (), "Соединитель", false, false, false); bIndex = 0; } if(((CS<IMyShipMergeBlock>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyPistonBase> (new CB <IMyPistonBase> (), "Поршень", false, false, false); bIndex = 0; } if(((CS<IMyPistonBase>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMotorStator> (new CB <IMyMotorStator> (), "Ротор", false, false, false); bIndex = 0; } if(((CS<IMyMotorStator>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMotorAdvancedStator>(new CB <IMyMotorAdvancedStator> (), "Ул. Ротор", false, false, false); bIndex = 0; } if(((CS<IMyMotorAdvancedStator>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyShipDrill> (new CB <IMyShipDrill> (), "Бур", false, false, false); bIndex = 0; } if(((CS<IMyShipDrill>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyShipGrinder> (new CB <IMyShipGrinder> (), "Резак", false, false, false); bIndex = 0; } if(((CS<IMyShipGrinder>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyShipWelder> (new CB <IMyShipWelder> (), "Сварщик", false, false, false); bIndex = 0; } if(((CS<IMyShipWelder>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCollector> (new CB <IMyCollector> (), "Коллектор", false, false, false); bIndex = 0; } if(((CS<IMyCollector>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyOreDetector> (new CB <IMyOreDetector> (), "Детектор руды", false, false, false); bIndex = 0; } if(((CS<IMyOreDetector>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyRadioAntenna> (new CB <IMyRadioAntenna> (), "Антенна", false, false, false); bIndex = 0; } if(((CS<IMyRadioAntenna>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyLaserAntenna> (new CB <IMyLaserAntenna> (), "Л.Антенна", false, false, false); bIndex = 0; } if(((CS<IMyLaserAntenna>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("Couch"), "Диван", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("CouchCorner"), "Угл. диван", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("Bathroom"), "Ванная", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("BathroomOpen"), "Откр. ванная", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("Toilet"), "Туалет", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("Desk"), "Стол", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("ВeskCorner"), "Угл. стол", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("PassengerBench"), "Пасс. скамья", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("PassengerSeat"), "Пасс. сиденье", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("CockpitSeat"), "Сиденье", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("BuggyCockpit"), "Багги-руль", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("RoverCockpit"), "Ровер-руль", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("CockpitOpen"), "Кокпит", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("OpenCockpit"), "Кресло управления", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("BlockCockpit"), "Кокпит", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("FighterCockpit"), "Истр. кокпит", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("CockpitIndustrial"), "Подвес. кокпит", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCockpit> (new CBT<IMyCockpit> ("StandingCockpit"), "Штурвал", false, false, false); bIndex = 0; } if(((CS<IMyCockpit>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyRefinery> (new CB <IMyRefinery> (), "Очиститель", false, false, false); bIndex = 0; } if(((CS<IMyRefinery>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyAssembler> (new CBT<IMyAssembler> ("LargeAssembler"), "Сборщик", false, false, false); bIndex = 0; } if(((CS<IMyAssembler>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyGasGenerator> (new CB <IMyGasGenerator> (), "H2:O2 Генератор", false, false, false); bIndex = 0; } if(((CS<IMyGasGenerator>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyOxygenFarm> (new CB <IMyOxygenFarm> (), "Ферма O2", false, false, false); bIndex = 0; } if(((CS<IMyOxygenFarm>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMySmallGatlingGun> (new CB <IMySmallGatlingGun> (), "М.Пушка", false, false, false); bIndex = 0; } if(((CS<IMySmallGatlingGun>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyLargeGatlingTurret> (new CB <IMyLargeGatlingTurret> (), "Б.Пушка", false, false, false); bIndex = 0; } if(((CS<IMyLargeGatlingTurret>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyLargeMissileTurret> (new CB <IMyLargeMissileTurret> (), "Б.Ракетница", false, false, false); bIndex = 0; } if(((CS<IMyLargeMissileTurret>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyLargeInteriorTurret>(new CB <IMyLargeInteriorTurret> (), "Б.Инт.Пушка", false, false, false); bIndex = 0; } if(((CS<IMyLargeInteriorTurret>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyPowerProducer> (new CBT<IMyPowerProducer> ("HydrogenEngine"), "H2 Электрогенератор", false, false, false); bIndex = 0; } if(((CS<IMyPowerProducer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyPowerProducer> (new CBT<IMyPowerProducer> ("WindTurbine"), "Ветрогенератор", false, false, false); bIndex = 0; } if(((CS<IMyPowerProducer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyBatteryBlock> (new CB <IMyBatteryBlock> (), "Батарея", false, false, false); bIndex = 0; } if(((CS<IMyBatteryBlock>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMySolarPanel> (new CB <IMySolarPanel> (), "С.Батарея", false, false, false); bIndex = 0; } if(((CS<IMySolarPanel>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyUpgradeModule> (new CBT<IMyUpgradeModule> ("ProductivityModule"), "М.Продуктивности", false, false, false); bIndex = 0; } if(((CS<IMyUpgradeModule>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyUpgradeModule> (new CBT<IMyUpgradeModule> ("EffectivenessModule"), "М.Эффективности", false, false, false); bIndex = 0; } if(((CS<IMyUpgradeModule>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyUpgradeModule> (new CBT<IMyUpgradeModule> ("EnergyModule"), "М.Энергоэффективности", false, false, false); bIndex = 0; } if(((CS<IMyUpgradeModule>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyConveyorSorter> (new CB <IMyConveyorSorter> (), "Сортировщик", false, false, true); bIndex = 0; } if(((CS<IMyConveyorSorter>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("SmallContainer"), "МК", false, true, false); bIndex = 0; } if(((CS<IMyCargoContainer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("MediumContainer"), "СК", false, true, false); bIndex = 0; } if(((CS<IMyCargoContainer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("LargeContainer"), "БК", false, true, false); bIndex = 0; } if(((CS<IMyCargoContainer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("LargeIndustrialContainer"), "БК", false, true, false); bIndex = 0; } if(((CS<IMyCargoContainer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("LockerRoom"), "Кам. хранения", false, true, false); bIndex = 0; } if(((CS<IMyCargoContainer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("Lockers"), "Шкафы", false, true, false); bIndex = 0; } if(((CS<IMyCargoContainer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCargoContainer> (new CBT<IMyCargoContainer> ("WeaponRack"), "Оруж. шкаф", false, true, false); bIndex = 0; } if(((CS<IMyCargoContainer>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyGasTank> (new CBT<IMyGasTank> ("OxygenTankSmall"), "Бак O2", false, false, false); bIndex = 0; } if(((CS<IMyGasTank>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyGasTank> (new CBT<IMyGasTank> ("OxygenTank/"), "Б.Бак O2", false, false, false); bIndex = 0; } if(((CS<IMyGasTank>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyGasTank> (new CBT<IMyGasTank> ("/LargeHydrogenTank"), "ОБ.Бак H2", false, false, false); bIndex = 0; } if(((CS<IMyGasTank>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyGasTank> (new CBT<IMyGasTank> ("/LargeHydrogenTankSmall"), "Б.Бак H2", false, false, false); bIndex = 0; } if(((CS<IMyGasTank>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyGasTank> (new CBT<IMyGasTank> ("/SmallHydrogenTank"), "Бак H2", false, false, false); bIndex = 0; } if(((CS<IMyGasTank>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyGasTank> (new CBT<IMyGasTank> ("/SmallHydrogenTankSmall"), "Бак H2", false, false, false); bIndex = 0; } if(((CS<IMyGasTank>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension1x1"), "Колесо 1x1 правое", false, false, false); bIndex = 0; } if(((CS<IMyMotorSuspension>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension3x3"), "Колесо 3x3 правое", false, false, false); bIndex = 0; } if(((CS<IMyMotorSuspension>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension5x5"), "Колесо 5x5 правое", false, false, false); bIndex = 0; } if(((CS<IMyMotorSuspension>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension1x1mirrored"), "Колесо 1x1 левое", false, false, false); bIndex = 0; } if(((CS<IMyMotorSuspension>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension3x3mirrored"), "Колесо 3x3 левое", false, false, false); bIndex = 0; } if(((CS<IMyMotorSuspension>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMotorSuspension> (new CBT<IMyMotorSuspension> ("Suspension5x5mirrored"), "Колесо 5x5 левое", false, false, false); bIndex = 0; } if(((CS<IMyMotorSuspension>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyThrust> (new CBT<IMyThrust> ("LargeAtmosphericThrust"), "БАУ", false, false, false); bIndex = 0; } if(((CS<IMyThrust>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyThrust> (new CBT<IMyThrust> ("SmallAtmosphericThrust"), "АУ", false, false, false); bIndex = 0; } if(((CS<IMyThrust>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyThrust> (new CBT<IMyThrust> ("LargeHydrogenThrust"), "БВУ", false, false, false); bIndex = 0; } if(((CS<IMyThrust>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyThrust> (new CBT<IMyThrust> ("SmallHydrogenThrust"), "ВУ", false, false, false); bIndex = 0; } if(((CS<IMyThrust>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyThrust> (new CBT<IMyThrust> ("LargeThrust"), "БИУ", false, false, false); bIndex = 0; } if(((CS<IMyThrust>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyThrust> (new CBT<IMyThrust> ("SmallThrust"), "ИУ", false, false, false); bIndex = 0; } if(((CS<IMyThrust>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyThrust> (new CBT<IMyThrust> ("LargeModularThruster"), "БМИУ", false, false, false); bIndex = 0; } if(((CS<IMyThrust>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyThrust> (new CBT<IMyThrust> ("SmallModularThruster"), "ММИУ", false, false, false); bIndex = 0; } if(((CS<IMyThrust>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyDoor> (new CB <IMyDoor> (), "Дверь", false, false, false); bIndex = 0; } if(((CS<IMyDoor>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyAirtightHangarDoor> (new CB <IMyAirtightHangarDoor> (), "Ангарслайд", false, false, false); bIndex = 0; } if(((CS<IMyAirtightHangarDoor>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyRemoteControl> (new CB <IMyRemoteControl> (), "ДУ", true, false, true); bIndex = 0; } if(((CS<IMyRemoteControl>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyMedicalRoom> (new CB <IMyMedicalRoom> (), "Медпост", false, false, false); bIndex = 0; } if(((CS<IMyMedicalRoom>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyFunctionalBlock> (new CBT<IMyFunctionalBlock> ("MedicalStation"), "Медстанция", false, false, false); bIndex = 0; } if(((CS<IMyFunctionalBlock>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyAirVent> (new CB <IMyAirVent> (), "Вентиляция", false, false, true); bIndex = 0; } if(((CS<IMyAirVent>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyCameraBlock> (new CB <IMyCameraBlock> (), "Камера", false, false, true); bIndex = 0; } if(((CS<IMyCameraBlock>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyProjector> (new CB <IMyProjector> (), "Проектор", false, false, true); bIndex = 0; } if(((CS<IMyProjector>)c_s).s(bIndex++)) { c_s = null; gIndex++; } });
actions.Add(() => { if(c_s == null) { c_s = new CS<IMyGyro> (new CB <IMyGyro> (), "Гироскоп", false, false, false); bIndex = 0; } if(((CS<IMyGyro>)c_s).s(bIndex++)) { c_s = null; gIndex++; } }); }
public void step(int index) {
if(index >= actions.Count) { stop(); return; }
actions[index](); }
public void stop() { Runtime.UpdateFrequency = UpdateFrequency.None; applyDefaultMeDisplayTexsts(); }
