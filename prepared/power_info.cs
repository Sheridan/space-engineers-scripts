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
public class CPowerInfo {
public CPowerInfo(string lcdNameProducing, string lcdNameConsuming) {
m_blocks = new CCube<IMyCubeBlock>(new CB<IMyCubeBlock>());
m_lcdProducers = new CD();
m_lcdProducers.aDs(lcdNameProducing);
m_lcdConsumers = new CD();
m_lcdConsumers.aDs(lcdNameConsuming); }
private void updateBlocksInfo() {
reset();
foreach(IMyCubeBlock b in m_blocks) {
string name = b.DefinitionDisplayNameText;
CBPI pi = new CBPI(b);
if(pi.canProduce()) { addPowerBlock(m_producers, name, pi.currentProduce(), pi.maxProduce()); }
if(pi.canConsume()) { addPowerBlock(m_consumers, name, pi.currentConsume(), pi.maxConsume()); } }
if(m_producers.Count < m_lastProdCout) { m_lcdProducers.clear(); } m_lastProdCout = m_producers.Count;
if(m_consumers.Count < m_lastConsCout) { m_lcdConsumers.clear(); } m_lastConsCout = m_consumers.Count; }
public void update() {
updateBlocksInfo();
drawInfo(m_lcdProducers, m_producers, "Генерация");
drawInfo(m_lcdConsumers, m_consumers, "Потребление"); }
private void drawInfo(CD to, Dictionary<string, SMinCurrentMax<float>> from, string title) {
int j = 0;
float curr = 0f; float maxx = 0f;
to.echo_at(title, j++);
foreach(KeyValuePair<string, SMinCurrentMax<float>> i in from) {
to.echo_at($"[{i.Key}:{i.Value.count}] {tHR(i.Value.current, EHRU.Power)} of {tHR(i.Value.max, EHRU.Power)}", j++);
curr += i.Value.current;
maxx += i.Value.max; }
to.echo_at($"Total: {tHR(curr, EHRU.Power)} of {tHR(maxx, EHRU.Power)}", j++); }
private void addPowerBlock(Dictionary<string, SMinCurrentMax<float>> to, string name, float cr, float mx) {
if(!to.ContainsKey(name)) { to[name] = new SMinCurrentMax<float>(0f, cr, mx); to[name].count++; }
else {
SMinCurrentMax<float> mcm = to[name];
mcm.current += cr;
mcm.max += mx;
mcm.count ++; } }
private void reset() {
m_producers = new Dictionary<string, SMinCurrentMax<float>>();
m_consumers = new Dictionary<string, SMinCurrentMax<float>>(); }
private CCube<IMyCubeBlock> m_blocks;
private Dictionary<string, SMinCurrentMax<float>> m_producers;
private Dictionary<string, SMinCurrentMax<float>> m_consumers;
private CD m_lcdConsumers;
private CD m_lcdProducers;
private int m_lastConsCout;
private int m_lastProdCout; }
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
case "SmallTextPanel" : s(0.602f, 48, 48, 0.35f); break;
default: s(1f, 1, 1, 1f); break; } }
public void aDs(string name) {
CBNamed<IMyTextPanel> displays = new CBNamed<IMyTextPanel>(name);
if(displays.empty()) { throw new System.ArgumentException("Не найдены дисплеи", name); }
mineDimensions(displays.blocks()[0]);
foreach(IMyTextPanel display in displays) {
CBO o = new CBO(display);
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
public CBNamed(string name, bool lSG = true) : base(lSG) { m_name = name; load(); }
protected override bool checkBlock(T b) {
return (m_lSG ? b.IsSameConstructAs(_.Me) : true) && b.CustomName.Contains(m_name); }
public string name() { return m_name; }
private string m_name; }
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
value *= 1000;
if(value >= 1000000) {
suffix = "Т.";
value = value/1000000; } }
if(value < divider) { return $"{value:f2}{suffix}"; }
int exp = (int)(Math.Log(value) / Math.Log(divider));
return $"{value / (float)Math.Pow(divider, exp):f2}{("кМГТПЭ")[exp - 1]}{suffix}"; }
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
public class CB<T> : CBB<T> where T : class, IMyEntity {
public CB(bool lSG = true) : base(lSG) { load(); } }
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
public CBPI(IMyCubeBlock block) {
m_block = block;
m_blockSinkComponent = m_block.Components.Get<MyResourceSinkComponent>(); }
public bool canProduce() { return m_block is IMyPowerProducer; }
public bool canConsume() {
return m_blockSinkComponent != null && m_blockSinkComponent.IsPoweredByType(Electricity); }
public float currentProduce() {
if(canProduce()) { return (m_block as IMyPowerProducer).CurrentOutput*1000000f; }
return 0f; }
public float maxProduce() {
if(canProduce()) { return (m_block as IMyPowerProducer).MaxOutput*1000000f; }
return 0f; }
public float currentConsume() {
if(canConsume()) {
float r = m_blockSinkComponent.CurrentInputByType(Electricity);
return r * 1000000f; }
return 0f; }
public float maxConsume() {
if(canConsume()) {
float r = m_blockSinkComponent.MaxRequiredInputByType(Electricity);
if(m_block is IMyAssembler || m_block is IMyRefinery) {
CBU upgrades = new CBU(m_block as IMyUpgradableBlock);
upgrades.calcPowerUse(r); }
return r * 1000000f; }
return 0f; }
MyResourceSinkComponent m_blockSinkComponent;
IMyCubeBlock m_block;
private static readonly MyDefinitionId Electricity = MyDefinitionId.Parse("MyObjectBuilder_GasProperties/Electricity"); }
public class SMinCurrentMax<T> {
public SMinCurrentMax(T mn, T cr, T mx) { min = mn; current = cr; max = mx; count = 0; }
public T min;
public T current;
public T max;
public int count; }
CPowerInfo pi;
public string program() {
Runtime.UpdateFrequency = UpdateFrequency.Update100;
pi = new CPowerInfo("Генерация", "Потребление");
return "Статус энергосети"; }
public void main(string argument, UpdateType updateSource) {
pi.update(); }
