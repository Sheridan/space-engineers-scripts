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
public class StorageInfo {
public StorageInfo(string storageName, string lcdName) {
m_lcd = new CD();
m_lcd.aD($"[{sN}] Дисплей {lcdName} 0", 0, 0);
m_storage = new CContainer(new CBNamed<IMyCargoContainer>(storageName)); }
public void update() {
int idx = 0;
float maxVolume = m_storage.maxVolume();
float volume = m_storage.volume();
m_lcd.echo_at($"Использовано {tHR(volume, EHRU.Volume)} из {tHR(maxVolume, EHRU.Volume)}: {volume/(maxVolume/100):f2}%", idx++);
m_lcd.echo_at($"Общая масса: {tHR(m_storage.mass(), EHRU.Mass)}", idx++);
foreach(KeyValuePair<MyItemType, float> i in m_storage.items()) {
MyItemInfo inf = i.Key.GetItemInfo();
string cnt = inf.UsesFractions ? tHR(i.Value, EHRU.Mass) : $"{i.Value:f0} шт.";
m_lcd.echo_at($"{MyDefinitionId.Parse(i.Key.ToString()).SubtypeName}: {cnt}", idx++); } }
private CD m_lcd;
private CContainer m_storage; }
public class CContainer : CT<IMyCargoContainer> {
public CContainer(CBB<IMyCargoContainer> blocks) : base(blocks) { }
public int items(EIT itemType) {
CCI r = new CCI(itemType);
MyItemType miType = r.asMyItemType();
foreach(IMyCargoContainer b in m_blocks.blocks()) {
r.appendAmount(b.GetInventory().GetItemAmount(miType).ToIntSafe()); }
return r.a(); }
public Dictionary<MyItemType, float> items() {
Dictionary<MyItemType, float> r = new Dictionary<MyItemType, float>();
foreach(IMyCargoContainer b in m_blocks.blocks()) {
List<MyInventoryItem> ci = new List<MyInventoryItem>();
b.GetInventory().GetItems(ci, x => true);
foreach(MyInventoryItem i in ci) {
if(!r.ContainsKey(i.Type)) { r.Add(i.Type, (float)i.Amount); }
else { r[i.Type] += (float)i.Amount; } } }
return r; }
public float maxVolume() {
float r = 0;
foreach(IMyCargoContainer b in m_blocks.blocks()) {
r += (float)b.GetInventory().MaxVolume; }
return r; }
public float volume() {
float r = 0;
foreach(IMyCargoContainer b in m_blocks.blocks()) {
r += (float)b.GetInventory().CurrentVolume; }
return r; }
public float mass() {
float r = 0;
foreach(IMyCargoContainer b in m_blocks.blocks()) {
r += (float)b.GetInventory().CurrentMass; }
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
public enum EIT {
BulletproofGlass,
Canvas,
Computer,
Construction,
Detector,
Display,
Explosives,
Girder,
GravityGenerator,
InteriorPlate,
LargeTube,
Medical,
MetalGrid,
Motor,
PowerCell,
RadioCommunication,
Reactor,
SmallTube,
SolarCell,
SteelPlate,
Superconductor,
Thrust,
ZoneChip }
public class CCI {
public CCI(string itemType, int a = 0) { m_itemType = fromString(itemType); m_a = a; }
public CCI(EIT itemType, int a = 0) { m_itemType = itemType ; m_a = a; }
public static EIT fromString(string itemType) {
if(itemType.Contains("BulletproofGlass")) { return EIT.BulletproofGlass; }
else if(itemType.Contains("Canvas")) { return EIT.Canvas; }
else if(itemType.Contains("Computer")) { return EIT.Computer; }
else if(itemType.Contains("Construction")) { return EIT.Construction; }
else if(itemType.Contains("Detector")) { return EIT.Detector; }
else if(itemType.Contains("Display")) { return EIT.Display; }
else if(itemType.Contains("Explosives")) { return EIT.Explosives; }
else if(itemType.Contains("Girder")) { return EIT.Girder; }
else if(itemType.Contains("GravityGenerator")) { return EIT.GravityGenerator; }
else if(itemType.Contains("InteriorPlate")) { return EIT.InteriorPlate; }
else if(itemType.Contains("LargeTube")) { return EIT.LargeTube; }
else if(itemType.Contains("Medical")) { return EIT.Medical; }
else if(itemType.Contains("MetalGrid")) { return EIT.MetalGrid; }
else if(itemType.Contains("Motor")) { return EIT.Motor; }
else if(itemType.Contains("PowerCell")) { return EIT.PowerCell; }
else if(itemType.Contains("RadioCommunication")) { return EIT.RadioCommunication; }
else if(itemType.Contains("Reactor")) { return EIT.Reactor; }
else if(itemType.Contains("SmallTube")) { return EIT.SmallTube; }
else if(itemType.Contains("SolarCell")) { return EIT.SolarCell; }
else if(itemType.Contains("SteelPlate")) { return EIT.SteelPlate; }
else if(itemType.Contains("Superconductor")) { return EIT.Superconductor; }
else if(itemType.Contains("Thrust")) { return EIT.Thrust; }
else if(itemType.Contains("ZoneChip")) { return EIT.ZoneChip; }
throw new System.ArgumentException("Не знаю такой строки", itemType); }
public int a() { return m_a; }
public void appendAmount(int aDelta) { m_a += aDelta; }
public EIT itemType() { return m_itemType; }
public string asComponent() {
string name = "";
switch(m_itemType) {
case EIT.BulletproofGlass: name = "BulletproofGlass"; break;
case EIT.Canvas: name = "Canvas"; break;
case EIT.Computer: name = "Computer"; break;
case EIT.Construction: name = "Construction"; break;
case EIT.Detector: name = "Detector"; break;
case EIT.Display: name = "Display"; break;
case EIT.Explosives: name = "Explosives"; break;
case EIT.Girder: name = "Girder"; break;
case EIT.GravityGenerator: name = "GravityGenerator"; break;
case EIT.InteriorPlate: name = "InteriorPlate"; break;
case EIT.LargeTube: name = "LargeTube"; break;
case EIT.Medical: name = "Medical"; break;
case EIT.MetalGrid: name = "MetalGrid"; break;
case EIT.Motor: name = "Motor"; break;
case EIT.PowerCell: name = "PowerCell"; break;
case EIT.RadioCommunication: name = "RadioCommunication"; break;
case EIT.Reactor: name = "Reactor"; break;
case EIT.SmallTube: name = "SmallTube"; break;
case EIT.SolarCell: name = "SolarCell"; break;
case EIT.SteelPlate: name = "SteelPlate"; break;
case EIT.Superconductor: name = "Superconductor"; break;
case EIT.Thrust: name = "Thrust"; break;
case EIT.ZoneChip: name = "ZoneChip"; break; }
return $"MyObjectBuilder_Component/{name}"; }
public string asBlueprintDefinition() {
string name = "";
switch(m_itemType) {
case EIT.BulletproofGlass: name = "BulletproofGlass"; break;
case EIT.Canvas: name = "Canvas"; break;
case EIT.Computer: name = "ComputerComponent"; break;
case EIT.Construction: name = "ConstructionComponent"; break;
case EIT.Detector: name = "DetectorComponent"; break;
case EIT.Display: name = "Display"; break;
case EIT.Explosives: name = "ExplosivesComponent"; break;
case EIT.Girder: name = "GirderComponent"; break;
case EIT.GravityGenerator: name = "GravityGeneratorComponent"; break;
case EIT.InteriorPlate: name = "InteriorPlate"; break;
case EIT.LargeTube: name = "LargeTube"; break;
case EIT.Medical: name = "MedicalComponent"; break;
case EIT.MetalGrid: name = "MetalGrid"; break;
case EIT.Motor: name = "MotorComponent"; break;
case EIT.PowerCell: name = "PowerCell"; break;
case EIT.RadioCommunication: name = "RadioCommunicationComponent"; break;
case EIT.Reactor: name = "ReactorComponent"; break;
case EIT.SmallTube: name = "SmallTube"; break;
case EIT.SolarCell: name = "SolarCell"; break;
case EIT.SteelPlate: name = "SteelPlate"; break;
case EIT.Superconductor: name = "Superconductor"; break;
case EIT.Thrust: name = "ThrustComponent"; break;
case EIT.ZoneChip: name = "ZoneChip"; break; }
return $"MyObjectBuilder_BlueprintDefinition/{name}"; }
public MyItemType asMyItemType() { return MyItemType.Parse(asComponent()); }
private EIT m_itemType;
private int m_a; }
public class FCI {
static public CCI BulletproofGlass(int a = 0) { return new CCI(EIT.BulletproofGlass, a); }
static public CCI Canvas(int a = 0) { return new CCI(EIT.Canvas, a); }
static public CCI Computer(int a = 0) { return new CCI(EIT.Computer, a); }
static public CCI Construction(int a = 0) { return new CCI(EIT.Construction, a); }
static public CCI Detector(int a = 0) { return new CCI(EIT.Detector, a); }
static public CCI Display(int a = 0) { return new CCI(EIT.Display, a); }
static public CCI Explosives(int a = 0) { return new CCI(EIT.Explosives, a); }
static public CCI Girder(int a = 0) { return new CCI(EIT.Girder, a); }
static public CCI GravityGenerator(int a = 0) { return new CCI(EIT.GravityGenerator, a); }
static public CCI InteriorPlate(int a = 0) { return new CCI(EIT.InteriorPlate, a); }
static public CCI LargeTube(int a = 0) { return new CCI(EIT.LargeTube, a); }
static public CCI Medical(int a = 0) { return new CCI(EIT.Medical, a); }
static public CCI MetalGrid(int a = 0) { return new CCI(EIT.MetalGrid, a); }
static public CCI Motor(int a = 0) { return new CCI(EIT.Motor, a); }
static public CCI PowerCell(int a = 0) { return new CCI(EIT.PowerCell, a); }
static public CCI RadioCommunication(int a = 0) { return new CCI(EIT.RadioCommunication, a); }
static public CCI Reactor(int a = 0) { return new CCI(EIT.Reactor, a); }
static public CCI SmallTube(int a = 0) { return new CCI(EIT.SmallTube, a); }
static public CCI SolarCell(int a = 0) { return new CCI(EIT.SolarCell, a); }
static public CCI SteelPlate(int a = 0) { return new CCI(EIT.SteelPlate, a); }
static public CCI Superconductor(int a = 0) { return new CCI(EIT.Superconductor, a); }
static public CCI Thrust(int a = 0) { return new CCI(EIT.Thrust, a); }
static public CCI ZoneChip(int a = 0) { return new CCI(EIT.ZoneChip, a); } }
public class CBNamed<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBNamed(string name, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_name = name; load(); }
protected override bool checkBlock(T b) {
return (m_loadOnlySameGrid ? b.IsSameConstructAs(_.Me) : true) && b.CustomName.Contains(m_name); }
public string name() { return m_name; }
private string m_name; }
public class CD : CTS {
public CD() : base() {
m_initialized = false; }
private void initSize(IMyTextPanel display) {
if(!m_initialized) {
debug($"{display.BlockDefinition.SubtypeName}");
switch(display.BlockDefinition.SubtypeName) {
case "LargeLCDPanelWide" : s(0.602f, 28, 87, 0.35f); break;
case "LargeLCDPanel" : s(0.602f, 28, 44, 0.35f); break;
case "TransparentLCDLarge": s(0.602f, 28, 44, 0.35f); break;
default: s(1f, 1, 1, 1f); break; } } }
public void aD(string name, int x, int y) {
IMyTextPanel display = _.GridTerminalSystem.GetBlockWithName(name) as IMyTextPanel;
initSize(display);
addSurface(display as IMyTextSurface, x, y); }
private bool m_initialized; }
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
string suffix = hrSuffix(unit);
if(value < 1000) { return $"{value}{suffix}"; }
int exp = (int)(Math.Log(value) / Math.Log(1000));
return $"{value / Math.Pow(1000, exp):f2}{("кМГТПЭ")[exp - 1]}{suffix}"; }
StorageInfo storage;
public string program() {
Runtime.UpdateFrequency = UpdateFrequency.Update100;
storage = new StorageInfo("МК", "Хранилище");
return "Статус хранилищ"; }
public void main(string argument, UpdateType updateSource) {
storage .update(); }
