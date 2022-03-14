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
m_lcd.echo($"[{currentTime()}] [{formatTimeSpan(ts)}] Задача {state(m_currentStateIndex-1).name()} завершена за время {formatTimeSpan(ts)}");
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
public void play() { foreach(IMySoundBlock b in m_blocks.blocks()) { b.Play(); } }
public void stop() { foreach(IMySoundBlock b in m_blocks.blocks()) { b.Stop(); } } }
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
public void removeBlock(T blk) { m_blocks.Remove(blk); }
public void removeBlockAt(int i) { m_blocks.RemoveAt(i); }
public string subtypeName() { return empty() ? "N/A" : m_blocks[0].DefinitionDisplayNameText; }
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
public class CBNamed<T> : CBB<T> where T : class, IMyTerminalBlock {
public CBNamed(string name, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_name = name; load(); }
protected override bool checkBlock(T b) {
return (m_loadOnlySameGrid ? b.IsSameConstructAs(_.Me) : true) && b.CustomName.Contains(m_name); }
public string name() { return m_name; }
private string m_name; }
public class CB<T> : CBB<T> where T : class, IMyTerminalBlock {
public CB(bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { load(); } }
public class CPiston : CF<IMyPistonBase> {
public CPiston(CBB<IMyPistonBase> blocks, int pistonsInStack = 1) : base(blocks) {
m_stackSize = pistonsInStack; }
private bool checkLength(float currentPos, float targetPos, float sensetivity = 0.2f) {
return currentPos <= targetPos + sensetivity && currentPos >= targetPos - sensetivity; }
public float currentLength() {
float l = 0;
foreach(IMyPistonBase b in m_blocks.blocks()) {
l += b.CurrentPosition; }
return l/m_blocks.count()/m_stackSize; }
public bool retract(float length, float velocity) {
bool r = true;
float realLength = length / m_stackSize;
float realVelocity = velocity / m_stackSize;
foreach(IMyPistonBase b in m_blocks.blocks()) {
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
foreach(IMyPistonBase b in m_blocks.blocks()) {
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
foreach(IMyShipMergeBlock b in m_blocks.blocks()) {
r = r && b.IsConnected; }
return r; } }
public class CC : CF<IMyShipConnector> {
public CC(CBB<IMyShipConnector> blocks) : base(blocks) { }
public bool connect(bool target = true) {
foreach(IMyShipConnector b in m_blocks.blocks()) {
if(target) { b.Connect(); }
else { b.Disconnect(); } }
return checkConnected(target); }
public bool disconnect() { return connect(false); }
public bool connected() { return checkConnected(true); }
private bool checkConnected(bool target) {
bool r = true;
foreach(IMyShipConnector b in m_blocks.blocks()) {
r = r &&
(
target ? b.Status == MyShipConnectorStatus.Connected
: b.Status == MyShipConnectorStatus.Unconnected || b.Status == MyShipConnectorStatus.Connectable
); }
return r; }
public bool connectable() {
bool r = true;
foreach(IMyShipConnector b in m_blocks.blocks()) {
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
foreach(IMyShipToolBase b in m_blocks.blocks()) {
r = r && b.IsActivated == target; }
return r; } }
public class CSensor : CF<IMySensorBlock> {
public CSensor(CBB<IMySensorBlock> blocks) : base(blocks) { } }
public class CLamp : CF<IMyLightingBlock> {
public CLamp(CBB<IMyLightingBlock> blocks) : base(blocks) { } }
public enum EBoolToString {
btsOnOff }
public string boolToString(bool val, EBoolToString bsType = EBoolToString.btsOnOff) {
switch(bsType) {
case EBoolToString.btsOnOff: return val ? "Вкл." : "Выкл."; }
return val.ToString(); }
CShipTool tools;
CC frontShipConnector;
CC bottomShipConnector;
CC activeShipConnector;
CC toolConnector;
CC parkingConnector;
CMerger frontShipMerger;
CMerger bottomShipMerger;
CMerger activeShipMerger;
CMerger toolMerger;
CPiston toolExtender;
CStateMachine connectStates;
CStateMachine parkStates;
CD statusLcd;
CSensor safetySensor;
CLamp safetyLamp;
private bool toolsConnected;
private bool toolsActive;
public string program() {
frontShipMerger = new CMerger(new CBNamed<IMyShipMergeBlock>("Фронтальный"));
bottomShipMerger = new CMerger(new CBNamed<IMyShipMergeBlock>("Нижний"));
frontShipConnector = new CC(new CBNamed<IMyShipConnector>("Фронтальный"));
bottomShipConnector = new CC(new CBNamed<IMyShipConnector>("Нижний"));
statusLcd = new CD();
statusLcd.aD("[Универсал] Дисплей Лог 1", 0, 0);
statusLcd.aD("[Универсал] Дисплей Лог 0", 1, 0);
statusLcd.aD("[Универсал] Дисплей Лог 2", 2, 0);
connectStates = new CStateMachine(statusLcd);
connectStates.addState("Слияние структур", merge);
connectStates.addState("Соединение коннекторов", connect);
connectStates.addState("Переименование", renameGrid);
connectStates.addState("Инициализация инструмента", toolsInit);
connectStates.addState("Отключение парковочного коннектора", unpark);
connectStates.addState("Ожидание отхода", unparkBack);
connectStates.addState("Отключение инструмента", toolsOff);
parkStates = new CStateMachine(statusLcd);
parkStates.addState("Отключение инструмента", toolsOff);
parkStates.addState("Соединение парковочного коннектора", park);
parkStates.addState("Отключение корабля", disconnect);
parkStates.addState("Разделение структур", unmerge);
parkStates.addState("Переименование", renameGrid);
parkStates.addState("Ожидание отхода", parkBack);
parkStates.addState("Отмена инициализации", toolsInit);
toolsInit(null);
searchActiveShipConnector();
return "Управление инструментом"; }
public void main(string argument, UpdateType updateSource) {
if(argument.Length > 0) {
if(argument == "catch") {
statusLcd.echo("Стыковка инструмента");
if(!toolsConnected) {
statusLcd.echo("Запрос стыковки с инструментом");
searchActiveShipConnector();
if(activeShipConnector != null && activeShipConnector.connectable()) {
connectStates.start(true); }
else { statusLcd.echo($"Корабельный коннектор не в состоянии ожидания"); } }
else {
statusLcd.echo("Запрос расстыковки с инструментом");
if(parkingConnector.connectable()) {
parkStates.start(true); }
else { statusLcd.echo($"Парковочный коннектор не в состоянии ожидания ({boolToString(parkingConnector.connectable())})"); } } }
else if(argument == "onoff") {
if(toolsConnected) { if(!toolsActive) { toolsOn(null); } else { toolsOff(null); } } }
else if(argument == "expand") { if(toolsConnected) { toolExtender.expandRelative(1f, 2f); } }
else if(argument == "retract") { if(toolsConnected) { toolExtender.retractRelative(1f, 2f); } }
else if(argument == "expand_full") { if(toolsConnected) { toolExtender.expand(10f, 2f); } }
else if(argument == "retract_full") { if(toolsConnected) { toolExtender.retract(0f, 2f); } } }
else {
if(connectStates.active()) { connectStates.step(); }
if(parkStates .active()) { parkStates .step(); } } }
void searchActiveShipConnector() {
string activeConnectorPos = "";
if(toolsConnected) {
if(frontShipConnector.connected()) {
activeShipConnector = frontShipConnector;
activeShipMerger = frontShipMerger;
activeConnectorPos = "Фронтальный"; }
else if(bottomShipConnector.connected()) {
activeShipConnector = bottomShipConnector;
activeShipMerger = bottomShipMerger;
activeConnectorPos = "Нижний"; } }
else {
if(frontShipConnector.connectable()) {
activeShipConnector = frontShipConnector;
activeShipMerger = frontShipMerger;
activeConnectorPos = "Фронтальный"; }
else if(bottomShipConnector.connectable()) {
activeShipConnector = bottomShipConnector;
activeShipMerger = bottomShipMerger;
activeConnectorPos = "Нижний"; } }
if(activeShipConnector != null) {
statusLcd.echo(
$"Активный коннектор: {activeConnectorPos} ({activeShipConnector.count()}:{activeShipMerger.count()}), ({boolToString(activeShipConnector.connected())},{boolToString(activeShipMerger.connected())})"); }
else {
statusLcd.echo("Инструменты не подключены и не найдены в зоне доступа"); } }
public bool renameGrid(object data) { Me.CubeGrid.CustomName = sN; return Me.CubeGrid.CustomName == sN; }
public bool connect(object data) { return activeShipConnector.connect(); }
public bool park(object data) { return parkingConnector .connect(); }
public bool parkBack(object data) { return !activeShipConnector.connectable(); }
public bool merge(object data) { return activeShipMerger .connect(); }
public bool disconnect(object data) { return activeShipConnector.disconnect(); }
public bool unpark(object data) { return parkingConnector .disconnect(); }
public bool unparkBack(object data) { return !parkingConnector .connectable(); }
public bool unmerge(object data) { return activeShipMerger .disconnect(); }
public bool toolsInit(object data) {
tools = new CShipTool(new CB<IMyShipToolBase>());
toolConnector = new CC(new CBNamed<IMyShipConnector >("Инструментальный"));
parkingConnector = new CC(new CBNamed<IMyShipConnector >("Парковка"));
toolMerger = new CMerger(new CBNamed<IMyShipMergeBlock>("Инструментальный"));
safetySensor = new CSensor(new CBNamed<IMySensorBlock >("Безопасность"));
safetyLamp = new CLamp(new CBNamed<IMyLightingBlock >("Безопасность"));
toolExtender = new CPiston(new CBNamed<IMyPistonBase >("Удлиннитель"));
toolsConnected = toolMerger != null && !toolMerger.empty() &&
parkingConnector != null && !parkingConnector.empty() &&
toolConnector != null && !toolConnector.empty() &&
toolExtender != null && !toolExtender.empty() &&
tools != null && !tools.empty();
sS();
return (connectStates.active() && toolsConnected) || (parkStates.active() && !toolsConnected); }
public void sS() {
statusLcd.echo($"Статус фронтального коннектора ({frontShipConnector.count()}): {boolToString(frontShipConnector.connected())}");
statusLcd.echo($"Статус фронтального мержера ({frontShipMerger.count()}): {boolToString(frontShipMerger.connected())}");
statusLcd.echo($"Статус нижнего коннектора ({bottomShipConnector.count()}): {boolToString(bottomShipConnector.connected())}");
statusLcd.echo($"Статус нижнего мержера ({bottomShipMerger.count()}): {boolToString(bottomShipMerger.connected())}");
statusLcd.echo($"Статус подключения: {boolToString(toolsConnected)}");
if(toolsConnected) {
statusLcd.echo($"Статус коннектора инструментов ({toolConnector.count()}): {boolToString(toolConnector.connected())}");
statusLcd.echo($"Статус коннектора парковки ({parkingConnector.count()}): {boolToString(parkingConnector.connected())}");
statusLcd.echo($"Статус мержера ({toolMerger.count()}): {boolToString(toolMerger.connected())}");
statusLcd.echo($"Статус лампы ({safetyLamp.count()}): {boolToString(toolMerger.enabled())}");
statusLcd.echo($"Статус сенсора ({safetySensor.count()}): {boolToString(safetySensor.enabled())}");
statusLcd.echo($"Статус инструментов ({tools.count()}): {boolToString(tools.enabled())}"); } }
public bool toolsOn(object data) {
safetyLamp.enable();
safetySensor.enable();
toolsActive = safetyLamp.enabled() && safetySensor.enabled() && tools.on();
statusLcd.echo($"Статус: {boolToString(toolsActive)}");
return toolsActive; }
public bool toolsOff(object data) {
safetyLamp.disable();
safetySensor.disable();
toolsActive = !(!safetyLamp.enabled() && !safetySensor.enabled() && tools.off());
statusLcd.echo($"Статус: {boolToString(toolsActive)}");
return !toolsActive; }
