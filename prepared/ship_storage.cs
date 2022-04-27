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
public class StorageInfo {
public StorageInfo(string storageName, string lcdName) {
m_lastItemsTypes = 0;
m_lcd = new CD();
m_lcd.aDs(lcdName);
m_storage = new CContainer(new CBNamed<IMyCargoContainer>(storageName)); }
public void update() {
Dictionary<MyItemType, float> data = m_storage.items();
if(m_lastItemsTypes > data.Count) { m_lcd.clear(); }
m_lastItemsTypes = data.Count;
int idx = 0;
float maxVolume = m_storage.maxVolume();
float volume = m_storage.volume();
m_lcd.echo_at($"Использовано {tHR(volume, EHRU.Volume)} из {tHR(maxVolume, EHRU.Volume)}: {volume/(maxVolume/100):f2}%", idx++);
m_lcd.echo_at($"Общая масса: {tHR(m_storage.mass(), EHRU.Mass)}", idx++);
foreach(KeyValuePair<MyItemType, float> i in data) {
MyItemInfo inf = i.Key.GetItemInfo();
string cnt = inf.UsesFractions ? $"Mass: {tHR(i.Value, EHRU.Mass)}" : $"{i.Value:f0} шт., Mass: {tHR(i.Value*inf.Mass, EHRU.Mass)}";
m_lcd.echo_at($"{MyDefinitionId.Parse(i.Key.ToString()).SubtypeName}: {cnt}, Volume: {tHR(inf.Volume*i.Value, EHRU.Volume)}", idx++); } }
private CD m_lcd;
private CContainer m_storage;
private int m_lastItemsTypes; }
public class CContainer : CT<IMyCargoContainer> {
public CContainer(CBB<IMyCargoContainer> blocks) : base(blocks) { }
public int items(EIT itemType) {
CCI r = new CCI(itemType);
MyItemType miType = r.asMyItemType();
foreach(IMyCargoContainer b in m_blocks) {
r.appendAmount(b.GetInventory().GetItemAmount(miType).ToIntSafe()); }
return r.a(); }
public Dictionary<MyItemType, float> items() {
Dictionary<MyItemType, float> r = new Dictionary<MyItemType, float>();
foreach(IMyCargoContainer b in m_blocks) {
List<MyInventoryItem> ci = new List<MyInventoryItem>();
b.GetInventory().GetItems(ci, x => true);
foreach(MyInventoryItem i in ci) {
if(!r.ContainsKey(i.Type)) { r.Add(i.Type, (float)i.Amount); }
else { r[i.Type] += (float)i.Amount; } } }
return r; }
public float maxVolume() {
float r = 0;
foreach(IMyCargoContainer b in m_blocks) {
r += (float)b.GetInventory().MaxVolume; }
return r; }
public float volume() {
float r = 0;
foreach(IMyCargoContainer b in m_blocks) {
r += (float)b.GetInventory().CurrentVolume; }
return r; }
public float mass() {
float r = 0;
foreach(IMyCargoContainer b in m_blocks) {
r += (float)b.GetInventory().CurrentMass; }
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
ZoneChip,
NATO_5p56x45mm,
LargeCalibreAmmo,
MediumCalibreAmmo,
AutocannonClip,
NATO_25x184mm,
LargeRailgunAmmo,
Missile200mm,
AutomaticRifleGun_Mag_20rd,
UltimateAutomaticRifleGun_Mag_30rd,
RapidFireAutomaticRifleGun_Mag_50rd,
PreciseAutomaticRifleGun_Mag_5rd,
SemiAutoPistolMagazine,
ElitePistolMagazine,
FullAutoPistolMagazine,
SmallRailgunAmmo }
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
else if(itemType.Contains("NATO_5p56x45mm")) { return EIT.NATO_5p56x45mm; }
else if(itemType.Contains("LargeCalibreAmmo")) { return EIT.LargeCalibreAmmo; }
else if(itemType.Contains("MediumCalibreAmmo")) { return EIT.MediumCalibreAmmo; }
else if(itemType.Contains("AutocannonClip")) { return EIT.AutocannonClip; }
else if(itemType.Contains("NATO_25x184mm")) { return EIT.NATO_25x184mm; }
else if(itemType.Contains("LargeRailgunAmmo")) { return EIT.LargeRailgunAmmo; }
else if(itemType.Contains("Missile200mm")) { return EIT.Missile200mm; }
else if(itemType.Contains("AutomaticRifleGun_Mag_20rd")) { return EIT.AutomaticRifleGun_Mag_20rd; }
else if(itemType.Contains("UltimateAutomaticRifleGun_Mag_30rd")) { return EIT.UltimateAutomaticRifleGun_Mag_30rd; }
else if(itemType.Contains("RapidFireAutomaticRifleGun_Mag_50rd")) { return EIT.RapidFireAutomaticRifleGun_Mag_50rd; }
else if(itemType.Contains("PreciseAutomaticRifleGun_Mag_5rd")) { return EIT.PreciseAutomaticRifleGun_Mag_5rd; }
else if(itemType.Contains("SemiAutoPistolMagazine")) { return EIT.SemiAutoPistolMagazine; }
else if(itemType.Contains("ElitePistolMagazine")) { return EIT.ElitePistolMagazine; }
else if(itemType.Contains("FullAutoPistolMagazine")) { return EIT.FullAutoPistolMagazine; }
else if(itemType.Contains("SmallRailgunAmmo")) { return EIT.SmallRailgunAmmo; }
throw new System.ArgumentException("Не знаю такой строки", itemType); }
public int a() { return m_a; }
public void appendAmount(int aDelta) { m_a += aDelta; }
public EIT itemType() { return m_itemType; }
public string asComponent() {
string name = "";
string iSType = "";
switch(m_itemType) {
case EIT.BulletproofGlass: { iSType = "Component"; name = "BulletproofGlass"; } break;
case EIT.Canvas: { iSType = "Component"; name = "Canvas"; } break;
case EIT.Computer: { iSType = "Component"; name = "Computer"; } break;
case EIT.Construction: { iSType = "Component"; name = "Construction"; } break;
case EIT.Detector: { iSType = "Component"; name = "Detector"; } break;
case EIT.Display: { iSType = "Component"; name = "Display"; } break;
case EIT.Explosives: { iSType = "Component"; name = "Explosives"; } break;
case EIT.Girder: { iSType = "Component"; name = "Girder"; } break;
case EIT.GravityGenerator: { iSType = "Component"; name = "GravityGenerator"; } break;
case EIT.InteriorPlate: { iSType = "Component"; name = "InteriorPlate"; } break;
case EIT.LargeTube: { iSType = "Component"; name = "LargeTube"; } break;
case EIT.Medical: { iSType = "Component"; name = "Medical"; } break;
case EIT.MetalGrid: { iSType = "Component"; name = "MetalGrid"; } break;
case EIT.Motor: { iSType = "Component"; name = "Motor"; } break;
case EIT.PowerCell: { iSType = "Component"; name = "PowerCell"; } break;
case EIT.RadioCommunication: { iSType = "Component"; name = "RadioCommunication"; } break;
case EIT.Reactor: { iSType = "Component"; name = "Reactor"; } break;
case EIT.SmallTube: { iSType = "Component"; name = "SmallTube"; } break;
case EIT.SolarCell: { iSType = "Component"; name = "SolarCell"; } break;
case EIT.SteelPlate: { iSType = "Component"; name = "SteelPlate"; } break;
case EIT.Superconductor: { iSType = "Component"; name = "Superconductor"; } break;
case EIT.Thrust: { iSType = "Component"; name = "Thrust"; } break;
case EIT.ZoneChip: { iSType = "Component"; name = "ZoneChip"; } break;
case EIT.NATO_5p56x45mm: { iSType = "AmmoMagazine"; name = "NATO_5p56x45mm"; } break;
case EIT.LargeCalibreAmmo: { iSType = "AmmoMagazine"; name = "LargeCalibreAmmo"; } break;
case EIT.MediumCalibreAmmo: { iSType = "AmmoMagazine"; name = "MediumCalibreAmmo"; } break;
case EIT.AutocannonClip: { iSType = "AmmoMagazine"; name = "AutocannonClip"; } break;
case EIT.NATO_25x184mm: { iSType = "AmmoMagazine"; name = "NATO_25x184mm"; } break;
case EIT.LargeRailgunAmmo: { iSType = "AmmoMagazine"; name = "LargeRailgunAmmo"; } break;
case EIT.Missile200mm: { iSType = "AmmoMagazine"; name = "Missile200mm"; } break;
case EIT.AutomaticRifleGun_Mag_20rd: { iSType = "AmmoMagazine"; name = "AutomaticRifleGun_Mag_20rd"; } break;
case EIT.UltimateAutomaticRifleGun_Mag_30rd: { iSType = "AmmoMagazine"; name = "UltimateAutomaticRifleGun_Mag_30rd"; } break;
case EIT.RapidFireAutomaticRifleGun_Mag_50rd: { iSType = "AmmoMagazine"; name = "RapidFireAutomaticRifleGun_Mag_50rd"; } break;
case EIT.PreciseAutomaticRifleGun_Mag_5rd: { iSType = "AmmoMagazine"; name = "PreciseAutomaticRifleGun_Mag_5rd"; } break;
case EIT.SemiAutoPistolMagazine: { iSType = "AmmoMagazine"; name = "SemiAutoPistolMagazine"; } break;
case EIT.ElitePistolMagazine: { iSType = "AmmoMagazine"; name = "ElitePistolMagazine"; } break;
case EIT.FullAutoPistolMagazine: { iSType = "AmmoMagazine"; name = "FullAutoPistolMagazine"; } break;
case EIT.SmallRailgunAmmo: { iSType = "AmmoMagazine"; name = "SmallRailgunAmmo"; } break; }
return $"MyObjectBuilder_{iSType}/{name}"; }
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
case EIT.ZoneChip: name = "ZoneChip"; break;
case EIT.NATO_5p56x45mm: name = "NATO_5p56x45mmMagazine"; break;
case EIT.LargeCalibreAmmo: name = "LargeCalibreAmmo"; break;
case EIT.MediumCalibreAmmo: name = "MediumCalibreAmmo"; break;
case EIT.AutocannonClip: name = "AutocannonClip"; break;
case EIT.NATO_25x184mm: name = "NATO_25x184mmMagazine"; break;
case EIT.LargeRailgunAmmo: name = "LargeRailgunAmmo"; break;
case EIT.Missile200mm: name = "Missile200mm"; break;
case EIT.AutomaticRifleGun_Mag_20rd: name = "AutomaticRifleGun_Mag_20rd"; break;
case EIT.UltimateAutomaticRifleGun_Mag_30rd: name = "UltimateAutomaticRifleGun_Mag_30rd"; break;
case EIT.RapidFireAutomaticRifleGun_Mag_50rd: name = "RapidFireAutomaticRifleGun_Mag_50rd"; break;
case EIT.PreciseAutomaticRifleGun_Mag_5rd: name = "PreciseAutomaticRifleGun_Mag_5rd"; break;
case EIT.SemiAutoPistolMagazine: name = "SemiAutoPistolMagazine"; break;
case EIT.ElitePistolMagazine: name = "ElitePistolMagazine"; break;
case EIT.FullAutoPistolMagazine: name = "FullAutoPistolMagazine"; break;
case EIT.SmallRailgunAmmo: name = "SmallRailgunAmmo"; break; }
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
static public CCI ZoneChip(int a = 0) { return new CCI(EIT.ZoneChip, a); }
static public CCI NATO_5p56x45mm(int a = 0) { return new CCI(EIT.NATO_5p56x45mm, a); }
static public CCI LargeCalibreAmmo(int a = 0) { return new CCI(EIT.LargeCalibreAmmo, a); }
static public CCI MediumCalibreAmmo(int a = 0) { return new CCI(EIT.MediumCalibreAmmo, a); }
static public CCI AutocannonClip(int a = 0) { return new CCI(EIT.AutocannonClip, a); }
static public CCI NATO_25x184mm(int a = 0) { return new CCI(EIT.NATO_25x184mm, a); }
static public CCI LargeRailgunAmmo(int a = 0) { return new CCI(EIT.LargeRailgunAmmo, a); }
static public CCI Missile200mm(int a = 0) { return new CCI(EIT.Missile200mm, a); }
static public CCI AutomaticRifleGun_Mag_20rd(int a = 0) { return new CCI(EIT.AutomaticRifleGun_Mag_20rd, a); }
static public CCI UltimateAutomaticRifleGun_Mag_30rd(int a = 0) { return new CCI(EIT.UltimateAutomaticRifleGun_Mag_30rd, a); }
static public CCI RapidFireAutomaticRifleGun_Mag_50rd(int a = 0) { return new CCI(EIT.RapidFireAutomaticRifleGun_Mag_50rd, a); }
static public CCI PreciseAutomaticRifleGun_Mag_5rd(int a = 0) { return new CCI(EIT.PreciseAutomaticRifleGun_Mag_5rd, a); }
static public CCI SemiAutoPistolMagazine(int a = 0) { return new CCI(EIT.SemiAutoPistolMagazine, a); }
static public CCI ElitePistolMagazine(int a = 0) { return new CCI(EIT.ElitePistolMagazine, a); }
static public CCI FullAutoPistolMagazine(int a = 0) { return new CCI(EIT.FullAutoPistolMagazine, a); }
static public CCI SmallRailgunAmmo(int a = 0) { return new CCI(EIT.SmallRailgunAmmo, a); } }
public class CBNamed<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBNamed(string name, bool lSG = true) : base(lSG) { m_name = name; load(); }
protected override bool checkBlock(T b) {
return (m_lSG ? b.IsSameConstructAs(_.Me) : true) && b.CustomName.Contains(m_name); }
public string name() { return m_name; }
private string m_name; }
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
StorageInfo sto;
public string program() {
Runtime.UpdateFrequency = UpdateFrequency.Update100;
sto = new StorageInfo(sN, "Хранилище");
return "Статус контейнеров"; }
public void main(string argument, UpdateType updateSource) {
sto.update(); }
