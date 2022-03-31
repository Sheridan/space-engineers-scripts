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
public class CRotor : CF<IMyMotorStator> {
public CRotor(CBB<IMyMotorStator> blocks) : base(blocks) {
m_reversed = false; }
public void rotate(float rpm = 1f) {
foreach(IMyMotorStator b in m_blocks) {
b.TargetVelocityRPM = rpm; } }
public void stop() {
foreach(IMyMotorStator b in m_blocks) {
b.TargetVelocityRPM = 0f; } }
public void reverse() {
foreach(IMyMotorStator b in m_blocks) {
b.GetActionWithName("Reverse").Apply(b); }
m_reversed = !m_reversed; }
public bool reversed() { return m_reversed; }
public float angle() {
float r = 0f;
foreach(IMyMotorStator b in m_blocks) {
r += b.Angle; }
float agl = (r/m_blocks.count()) * (180f/3.1415926f);
return m_reversed ? 360f - agl : agl; }
public void setLimit(float lmin = float.MinValue, float lmax = float.MaxValue) {
foreach(IMyMotorStator b in m_blocks) {
b.LowerLimitDeg = lmin;
b.UpperLimitDeg = lmax; } }
private bool m_reversed; }
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
public class CPiston : CF<IMyPistonBase> {
public CPiston(CBB<IMyPistonBase> blocks, int pistonsInStack = 1) : base(blocks) {
m_stackSize = pistonsInStack; }
private bool checkLength(float currentPos, float targetPos, float sensetivity = 0.2f) {
return currentPos <= targetPos + sensetivity && currentPos >= targetPos - sensetivity; }
public float currentLength() {
float l = 0;
foreach(IMyPistonBase b in m_blocks) {
l += b.CurrentPosition; }
return l/m_blocks.count()/m_stackSize; }
public bool retract(float length, float velocity) {
bool r = true;
float realLength = length / m_stackSize;
float realVelocity = velocity / m_stackSize;
foreach(IMyPistonBase b in m_blocks) {
switch(b.Status) {
case PistonStatus.Stopped:
case PistonStatus.Extended:
case PistonStatus.Extending:
case PistonStatus.Retracted: {
if(b.CurrentPosition > realLength) {
b.Velocity = realVelocity;
b.MinLimit = realLength;
b.MaxLimit = 10f;
b.Retract(); } }
break; }
r = r && (b.Status == PistonStatus.Retracted ||
(
b.Status == PistonStatus.Retracting &&
checkLength(b.CurrentPosition, realLength)
)); }
return r; }
public bool expand(float length, float velocity) {
bool r = true;
float realLength = length / m_stackSize;
float realVelocity = velocity / m_stackSize;
foreach(IMyPistonBase b in m_blocks) {
switch(b.Status) {
case PistonStatus.Stopped:
case PistonStatus.Retracted:
case PistonStatus.Retracting:
case PistonStatus.Extended: {
if(b.CurrentPosition < realLength) {
b.Velocity = realVelocity;
b.MinLimit = 0f;
b.MaxLimit = realLength;
b.Extend(); } }
break; }
r = r && (b.Status == PistonStatus.Extended ||
(
b.Status == PistonStatus.Extending &&
checkLength(b.CurrentPosition, realLength)
)); }
return r; }
public bool expandRelative(float length, float velocity) { return expand(currentLength() + length, velocity); }
public bool retractRelative(float length, float velocity) { return retract(currentLength() - length, velocity); }
private int m_stackSize; }
public class CMerger : CF<IMyShipMergeBlock> {
public CMerger(CBB<IMyShipMergeBlock> blocks) : base(blocks) { }
public bool connect(bool target = true) {
enable(target);
return connected() == target; }
public bool disconnect() { return connect(false); }
public bool connected() {
if(!enabled()) { return false; }
bool r = true;
foreach(IMyShipMergeBlock b in m_blocks) {
r = r && b.IsConnected; }
return r; } }
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
public class CProjector : CF<IMyProjector> {
public CProjector(CBB<IMyProjector> blocks) : base(blocks) { }
public bool projecting() {
bool r = true;
foreach(IMyProjector b in m_blocks) {
r = r && b.IsProjecting; }
return r; }
public int totalBlocks() {
int r = 0;
foreach(IMyProjector b in m_blocks) {
r = r + b.TotalBlocks; }
return r; }
public int remainingBlocks() {
int r = 0;
foreach(IMyProjector b in m_blocks) {
r = r + b.RemainingBlocks; }
return r; }
public int buildableBlocks() {
int r = 0;
foreach(IMyProjector b in m_blocks) {
r = r + b.BuildableBlocksCount; }
return r; }
public int weldedBlocks() { return totalBlocks() - remainingBlocks(); } }
public class CStateMachineState {
public CStateMachineState(string name, Func<object, bool> method, object data) {
m_name = name;
m_method = method;
m_data = data; }
public bool callMethod() { return m_method(m_data); }
public string name() { return m_name; }
string m_name;
Func<object, bool> m_method;
object m_data; }
public class CStateMachine {
public CStateMachine(CTS lcd, CSpeaker speaker = null) {
m_lcd = lcd;
m_states = new List<CStateMachineState>();
m_currentStateIndex = 0;
m_waitCount = 0;
m__Driven = false;
m_speaker = speaker; }
public void addState(string name, Func<object, bool> method, object data = null) { m_states.Add(new CStateMachineState(name, method, data)); m_currentStateIndex++; }
private CStateMachineState state(int index) { return m_states[index]; }
public CStateMachineState currentState() { return state(m_currentStateIndex); }
private void switchToNextState() {
playSound();
m_waitCount = 0;
m_currentStateIndex++;
TimeSpan ts = m_startDT - System.DateTime.Now;
TimeSpan tts = m_taskStartDT - System.DateTime.Now;
m_lcd.echo($"[{currentTime()}] [{formatTimeSpan(ts)}] Задача {state(m_currentStateIndex-1).name()} завершена за время {formatTimeSpan(tts)}");
if(active()) {
m_lcd.echo($"[{currentTime()}] [{formatTimeSpan(ts)}] Старт задачи '{state(m_currentStateIndex).name()}'"); }
else {
m_lcd.echo($"[{currentTime()}] Алгоритм завершен. Длительность: {formatTimeSpan(ts)}");
if(m__Driven) { _.Runtime.UpdateFrequency = UpdateFrequency.None; } }
m_taskStartDT = System.DateTime.Now; }
public void step() {
TimeSpan ts = m_startDT - System.DateTime.Now;
m_lcd.echo_at($"Текущее состояние: {currentState().name()} ({m_waitCount++}). Прошло времени: {formatTimeSpan(ts)}", 0);
if(currentState().callMethod()) { switchToNextState(); } }
public void start(bool _DrivenMachine = false) {
m_startDT = System.DateTime.Now;
m_taskStartDT = System.DateTime.Now;
m_currentStateIndex = 0;
m__Driven = _DrivenMachine;
m_lcd.echo($"[{currentTime()}] Алгоритм запущен");
if(m__Driven) { _.Runtime.UpdateFrequency = UpdateFrequency.Update100; } }
public void listStates() {
foreach(CStateMachineState state in m_states) {
m_lcd.echo(state.name()); } }
public bool active() { return m_currentStateIndex < m_states.Count; }
private void playSound() {
if(m_speaker != null) {
m_speaker.play(); } }
private List<CStateMachineState> m_states;
private int m_currentStateIndex;
private CTS m_lcd;
private int m_waitCount;
private bool m__Driven;
private DateTime m_startDT;
private DateTime m_taskStartDT;
private CSpeaker m_speaker; }
public static string currentTime() {
return System.DateTime.Now.ToString("HH:mm:ss"); }
public static string formatTimeSpan(TimeSpan ts) {
return ts.ToString(@"hh\:mm\:ss"); }
public class CSpeaker : CF<IMySoundBlock> {
public CSpeaker(CBB<IMySoundBlock> blocks) : base(blocks) { }
public void play() { foreach(IMySoundBlock b in m_blocks) { b.Play(); } }
public void stop() { foreach(IMySoundBlock b in m_blocks) { b.Stop(); } } }
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
public enum EBoolToString {
btsOnOff }
public string boolToString(bool val, EBoolToString bsType = EBoolToString.btsOnOff) {
switch(bsType) {
case EBoolToString.btsOnOff: return val ? "Вкл." : "Выкл."; }
return val.ToString(); }
float drillRotorsRPM = 0.5f;
float wallHeight = 4f * 2.5f;
float drillSteps = 8f;
float drillPistonsStep;
float welderSteps = 4f;
float welderPistonsStep;
float pistonsSpeed = 1f;
float lastRotorsAngle = 0f;
float stepRotorsAngle = 190f;
float continuosRotorAngle = 0f;
int lastAngleFactor = 1;
CD logLcd;
CD statusLcd;
CRotor drillRotors0;
CRotor drillRotors45;
CRotor beltRotor;
CPiston drillPistons;
CPiston welderPistons;
CPiston platformPistons;
CShipTool drills;
CShipTool welders;
CShipTool beltGrinder;
CShipTool beltWelder;
CMerger topMergers;
CMerger bottomMergers;
CPiston topPistons;
CPiston bottomPistons;
CPiston connectorPiston;
CProjector wallProjector;
CProjector beltProjector;
CC platformConnector;
CSpeaker speaker;
CStateMachine states;
public string program() {
drillPistonsStep = wallHeight / drillSteps;
welderPistonsStep = wallHeight / welderSteps;
logLcd = new CD();
logLcd.aDs("Лог");
statusLcd = new CD();
statusLcd.aDs("Статус");
drillRotors0 = new CRotor(new CBNamed<IMyMotorStator >("Инструмент (0)"));
drillRotors45 = new CRotor(new CBNamed<IMyMotorStator >("Инструмент (45)"));
drillPistons = new CPiston(new CBNamed<IMyPistonBase >("Бур"));
welderPistons = new CPiston(new CBNamed<IMyPistonBase >("Сварщик"));
platformPistons = new CPiston(new CBNamed<IMyPistonBase >("Подвес"));
drills = new CShipTool(new CBNamed<IMyShipToolBase >("Бур"));
welders = new CShipTool(new CBNamed<IMyShipToolBase >("Сварщик Стена"));
topMergers = new CMerger(new CBNamed<IMyShipMergeBlock>("Верхний"));
bottomMergers = new CMerger(new CBNamed<IMyShipMergeBlock>("Нижний"));
topPistons = new CPiston(new CBNamed<IMyPistonBase >("Верхний"));
bottomPistons = new CPiston(new CBNamed<IMyPistonBase >("Нижний"));
wallProjector = new CProjector(new CBNamed<IMyProjector >("Стена"));
beltProjector = new CProjector(new CBNamed<IMyProjector >("Конвейер"));
platformConnector = new CC(new CBNamed<IMyShipConnector >("Конвейер"));
connectorPiston = new CPiston(new CBNamed<IMyPistonBase >("Конвейер"));
speaker = new CSpeaker(new CBNamed<IMySoundBlock >("Динамик"));
beltRotor = new CRotor(new CBNamed<IMyMotorStator >("Конвейер"));
beltGrinder = new CShipTool(new CBNamed<IMyShipToolBase >("Резак Конвейер"));;
beltWelder = new CShipTool(new CBNamed<IMyShipToolBase >("Сварщик Конвейер"));;
states = new CStateMachine(logLcd, speaker);
states.addState("Подготовка к запуску", prepareStart);
states.addState("Запуск вращения буров", startDrillRotors);
states.addState("Запуск буров", startDrill);
for(int i = 1; i<=drillSteps; i++) {
states.addState($"Опускание буров (шаг {i})", stepDrillPiston, i*drillPistonsStep);
states.addState($"Получение текущего угла роторов (шаг {i})", prepareRotorsAngleCalc);
states.addState($"Ожидание поворота роторов (шаг {i})", waitRotorsTurn); }
states.addState("Остановка буров", stopDrill);
states.addState("Остановка вращения буров", stopDrillRotors);
states.addState("Поднятие поршней буров", retractDrillPiston);
states.addState("Установка буров в начальную позицию", toZero);
states.addState("Запуск сварщиков", startWelder);
states.addState("Запуск проектора стен", tunnelProjectorOn);
for(int i = 1; i<=welderSteps; i++) {
states.addState($"Опускание сварщиков (шаг {i})", stepWelderPiston, i*welderPistonsStep);
states.addState($"Ожидание сварщиков (шаг {i})", waitWallWelders, 600+i*120); }
states.addState("Остановка сварщиков", stopWelder);
states.addState("Остановка проектора стен", tunnelProjectorOff);
states.addState("Расстыковка нижних соединителей", disconnectBottomMergers);
states.addState("Сворачивание нижних поршней", retractBottomPistons);
states.addState("Расстыковка коннектора", disconnectPlatformConnector);
states.addState("Сворачивание поршня коннектора", retractConnectorPiston);
states.addState("Включение проектора трубы", beltProjectorOn);
states.addState("Запуск срезания коннектора", startBeltGrinder);
states.addState("Ожидание срезания коннектора", waitBeltGrinder);
states.addState("Остановка срезания коннектора", stopBeltGrinder);
states.addState("Смена инструмента на сварщик", rotateToWelder);
states.addState("Ожидание смены инструмента", waitToolRotateToWelder);
states.addState("Остановка ротора", stopToolRotate);
states.addState("Запуск сварки трубы", startBeltWelder);
for(int i = 1; i<=welderSteps; i++) {
states.addState($"Спуск платформы (шаг {i})", platformDown, i*welderPistonsStep);
states.addState($"Ожидание сварщика трубы (шаг {i})", waitBeltWelder, i+1); }
states.addState("Остановка сварки трубы", stopBeltWelder);
states.addState("Отключение проектора трубы", beltProjectorOff);
states.addState("Смена инструмента на резак", rotateToGrinder);
states.addState("Ожидание смены инструмента", waitToolRotateToGrinder);
states.addState("Остановка ротора", stopToolRotate);
states.addState("Разворачивание поршня коннектора", expandConnectorPiston);
states.addState("Стыковка коннектора", connectPlatformConnector);
states.addState("Включение нижних соединителей", connectBottomMergers);
states.addState("Разворачивание нижних поршней", expandBottomPistons);
states.addState("Расстыковка верхних соединителей", disconnectTopMergers);
states.addState("Сворачивание верхних поршней", retractTopPistons);
states.addState("Спуск верхней поддержки", capDown);
states.addState("Включение верхних соединителей", connectTopMergers);
states.addState("Разворачивание верхних поршней", expandTopPistons);
states.addState("Завершение", finishWork);
sS();
speaker.play();
return "Управление кротом"; }
public void main(string argument, UpdateType updateSource) {
if(argument == "start") {
states.start(true); }
else if(argument == "to_zero") { toZero(null); }
else if(argument == "to_zero_stop") { stopDrillRotors(null); }
else if(argument == "stats") { _.Runtime.UpdateFrequency = UpdateFrequency.Update100; }
else { if(states.active()) { states.step(); } sS(); } }
public float getRotors45Angle() {
float r45 = drillRotors45.angle()+45f;
return r45 >= 360f ? r45-360f : r45; }
public float maxGradPerRun() { return drillRotorsRPM * 6f * (float)Runtime.TimeSinceLastRun.TotalSeconds * lastAngleFactor + ((lastAngleFactor-1) * 1.2f); }
public float getRotorsAngle() { return (drillRotors0.angle() + getRotors45Angle())/2; }
public void sS() {
int i = 0;
statusLcd.echo_at($"Состояние системы: {boolToString(states.active())}", i++);
if(states.active()) { statusLcd.echo_at($"Текущий шаг: {states.currentState().name()}", i++); }
statusLcd.echo_at(
$"Углы роторов буров ({drillRotors0.count() + drillRotors45.count()}шт.): [0:{drillRotors0.angle():f2},45:{getRotors45Angle():f2}] -> [avg:{getRotorsAngle():f2},cont:{continuosRotorAngle:f2},max:{maxGradPerRun():f2}]",
i++);
statusLcd.echo_at($"Угол ротора инструмента({beltRotor.count()}шт.): {beltRotor.angle():f2}", i++);
statusLcd.echo_at($"Длинна верхних поршней ({topPistons.count()}шт.): {topPistons.currentLength():f2}м.", i++);
statusLcd.echo_at($"Длинна подвесных поршней ({platformPistons.count()}шт.): {platformPistons.currentLength():f2}м.", i++);
statusLcd.echo_at($"Длинна нижних поршней ({bottomPistons.count()}шт.): {bottomPistons.currentLength():f2}м.", i++);
statusLcd.echo_at($"Длинна сварочных поршней ({welderPistons.count()}шт.): {welderPistons.currentLength():f2}м.", i++);
statusLcd.echo_at($"Длинна буровых поршней ({drillPistons.count()}шт.): {drillPistons.currentLength():f2}м.", i++);
statusLcd.echo_at($"Длинна поршня коннектора ({connectorPiston.count()}шт.): {connectorPiston.currentLength():f2}м.", i++);
statusLcd.echo_at($"Состояние верхних соединителей ({topMergers.count()}шт.): {boolToString(topMergers.connected())}", i++);
statusLcd.echo_at($"Состояние нижних соединителей ({bottomMergers.count()}шт.): {boolToString(bottomMergers.connected())}", i++);
statusLcd.echo_at($"Проектора тоннеля ({wallProjector.count()}шт.): {boolToString(wallProjector.enabled())}" + (wallProjector.enabled() ?
$"; ttl:{wallProjector.totalBlocks()}; rmng:{wallProjector.remainingBlocks()}; bldbl:{wallProjector.buildableBlocks()}; wldd:{wallProjector.weldedBlocks()}" : ""), i++);
statusLcd.echo_at($"Проектора конвейера ({beltProjector.count()}шт.): {boolToString(beltProjector.enabled())}" + (beltProjector.enabled() ?
$"; ttl:{beltProjector.totalBlocks()}; rmng:{beltProjector.remainingBlocks()}; bldbl:{beltProjector.buildableBlocks()}; wldd:{beltProjector.weldedBlocks()}" : ""), i++);
statusLcd.echo_at($"Состояние коннектора ({platformConnector.count()}шт.): {boolToString(platformConnector.connected())}", i++);
statusLcd.echo_at($"Состояние сварщиков стен ({welders.count()}шт.): {boolToString(welders.active())}", i++);
statusLcd.echo_at($"Состояние сварщика конвейера ({beltWelder.count()}шт.): {boolToString(beltWelder.active())}", i++);
statusLcd.echo_at($"Состояние резчика конвейера ({beltGrinder.count()}шт.): {boolToString(beltGrinder.active())}", i++);
statusLcd.echo_at($"Состояние буров ({drills.count()}шт.): {boolToString(drills.active())}", i++); }
public bool toZero(object data) {
drillRotors0.setLimit(0f, 0f);
drillRotors45.setLimit(45f, 45f);
startDrillRotors(null);
return true; }
public bool prepareStart(object data) {
statusLcd.clear();
logLcd.clear();
drillRotors0.setLimit();
drillRotors45.setLimit();
return true; }
public bool finishWork(object data) {
return stopDrillRotors(null); }
public bool startDrillRotors(object data) {
drillRotors0.rotate(drillRotorsRPM);
drillRotors45.rotate(drillRotorsRPM);
if(!drillRotors45.reversed()) { drillRotors45.reverse(); }
return true; }
public bool stopDrillRotors(object data) {
drillRotors0.stop();
drillRotors45.stop();
return true; }
public bool startDrill(object data) { return drills.on(); }
public bool stopDrill(object data) { return drills.off(); }
public bool startWelder(object data) { return welders.on(); }
public bool stopWelder(object data) { return welders.off(); }
public bool startBeltWelder(object data) { return beltWelder.on(); }
public bool stopBeltWelder(object data) { return beltWelder.off(); }
public bool startBeltGrinder(object data) { return beltGrinder.on(); }
public bool stopBeltGrinder(object data) { return beltGrinder.off(); }
public bool stepDrillPiston(object data) { return drillPistons.expand((float)data, pistonsSpeed); }
public bool retractDrillPiston(object data) { return drillPistons.retract(0f, pistonsSpeed); }
public bool stepWelderPiston(object data) { return welderPistons.expand((float)data, pistonsSpeed); }
public bool retractWelderPiston(object data) { return welderPistons.retract(0f, pistonsSpeed); }
public bool tunnelProjectorOn(object data) { return wallProjector.enable(); }
public bool tunnelProjectorOff(object data) { return wallProjector.disable(); }
public bool beltProjectorOn(object data) { return beltProjector.enable(); }
public bool beltProjectorOff(object data) { return beltProjector.disable(); }
public bool connectTopMergers(object data) { return topMergers.enable(); }
public bool disconnectTopMergers(object data) { return topMergers.disable(); }
public bool expandTopPistons(object data) { return topPistons.expand(2.4f, pistonsSpeed); }
public bool retractTopPistons(object data) { return topPistons.retract(0f, pistonsSpeed); }
public bool connectBottomMergers(object data) { return bottomMergers.enable(); }
public bool disconnectBottomMergers(object data) { return bottomMergers.disable(); }
public bool expandBottomPistons(object data) { return bottomPistons.expand(2.4f, pistonsSpeed); }
public bool retractBottomPistons(object data) { return bottomPistons.retract(0f, pistonsSpeed); }
public bool connectPlatformConnector(object data) { return platformConnector.enable() && platformConnector.connect(); }
public bool disconnectPlatformConnector(object data) { return platformConnector.disconnect() && platformConnector.disable(); }
public bool expandConnectorPiston(object data) { return connectorPiston.expand(2.4f, pistonsSpeed); }
public bool retractConnectorPiston(object data) { return connectorPiston.retract(0f, pistonsSpeed); }
public bool waitWallWelders(object data) { return wallProjector.weldedBlocks() >= (int)data; }
public bool waitBeltGrinder(object data) { return beltProjector.buildableBlocks() > 0; }
public bool waitBeltWelder(object data) { return beltProjector.weldedBlocks() >= (int)data; }
public bool platformDown(object data) {
float l = (float)data;
bool pp = platformPistons.expand(l, pistonsSpeed);
bool wp = welderPistons.retract(10f - l, pistonsSpeed);
return pp && wp; }
public bool capDown(object data) { return platformPistons.retract(0f, pistonsSpeed); }
public bool rotateToWelder(object data) { beltRotor.setLimit(180f, 180f); beltRotor.rotate(5f); return true; }
public bool rotateToGrinder(object data) { beltRotor.setLimit(0f, 0f); beltRotor.rotate(5f); return true; }
public bool stopToolRotate(object data) { beltRotor.stop(); return true; }
public bool waitToolRotateToWelder(object data) { int agl = (int)beltRotor.angle(); return agl == 180; }
public bool waitToolRotateToGrinder(object data) { int agl = (int)beltRotor.angle(); return agl == 0 || agl == 360; }
public bool prepareRotorsAngleCalc(object data) {
lastRotorsAngle = getRotorsAngle();
continuosRotorAngle = 0f;
return true; }
public bool waitRotorsTurn(object data) {
float currAngle = getRotorsAngle();
float angleDiff = currAngle >= lastRotorsAngle ? currAngle - lastRotorsAngle : (360f-lastRotorsAngle) + currAngle;
if(angleDiff > maxGradPerRun()) { lastAngleFactor++; return false; }
lastAngleFactor = 1;
lastRotorsAngle = currAngle;
continuosRotorAngle += angleDiff;
return continuosRotorAngle >= stepRotorsAngle; }
