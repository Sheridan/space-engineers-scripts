static string structureName;
static string scriptName;
static Program _;
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
public static void debug(string text) { _.Echo(text); }
public void init() {
structureName = Me.CubeGrid.CustomName;
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
public CB(string name = "", string purpose = "", bool loadOnlySameGrid = true) : base(purpose) {
refresh(name, loadOnlySameGrid); }
public void refresh(string name = "", bool loadOnlySameGrid = true) {
clear();
if(loadOnlySameGrid) { _.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(_.Me)); }
else { _.GridTerminalSystem.GetBlocksOfType<T>(m_blocks) ; }
if(name != string.Empty) {
for(int i = count() - 1; i >= 0; i--) {
if(!m_blocks[i].CustomName.Contains(name)) { removeBlockAt(i); } } } } }
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
if(iA<IMyShipConnector>()) {
IMyShipConnector blk = block as IMyShipConnector;
blk.PullStrength = 1f;
blk.CollectAll = o.g("connector", "collectAll", false);
blk.ThrowOut = o.g("connector", "throwOut", false); }
else if(iA<IMyInteriorLight>()) {
IMyInteriorLight blk = block as IMyInteriorLight;
blk.Radius = 10f;
blk.Intensity = 10f;
blk.Falloff = 3f;
blk.Color = o.g("lamp", "color", Color.White); }
else if(iA<IMyConveyorSorter>()) {
IMyConveyorSorter blk = block as IMyConveyorSorter;
blk.DrainAll = o.g("sorter", "drainAll", false); }
else if(iA<IMyLargeTurretBase>()) {
IMyLargeTurretBase blk = block as IMyLargeTurretBase;
blk.EnableIdleRotation = true;
blk.Elevation = 0f;
blk.Azimuth = 0f; }
else if(iA<IMyAssembler>()) {
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
return o.g("generic", "purpose", m_purpose); }
private void sBlocksVisibility(T block,
bool vTerminal,
bool vInventory,
bool vToolBar) {
IMySlimBlock sBlock = block.CubeGrid.GetCubeBlock(block.Position);
block.ShowInTerminal = vTerminal && sBlock.IsFullIntegrity && sBlock.BuildIntegrity < 1f;
block.ShowInToolbarConfig = vToolBar;
if(block.HasInventory) { block.ShowInInventory = vInventory; } }
public bool empty() { return count() == 0; }
public int count() { return m_blocks.Count; }
public void removeBlock(T blk) { m_blocks.Remove(blk); }
public void removeBlockAt(int i) { m_blocks.RemoveAt(i); }
public string subtypeName() { return empty() ? "N/A" : m_blocks[0].DefinitionDisplayNameText; }
public bool iA<U>() where U : class, IMyTerminalBlock {
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
public class CRotor : CF<IMyMotorStator> {
public CRotor(CBB<IMyMotorStator> blocks) : base(blocks) {
m_reversed = false; }
public void rotate(float rpm = 1f) {
foreach(IMyMotorStator block in m_blocks.blocks()) {
block.TargetVelocityRPM = rpm; } }
public void stop() {
foreach(IMyMotorStator block in m_blocks.blocks()) {
block.TargetVelocityRPM = 0f; } }
public void reverse() {
foreach(IMyMotorStator block in m_blocks.blocks()) {
block.GetActionWithName("Reverse").Apply(block); }
m_reversed = !m_reversed; }
public float angle() {
float r = 0f;
foreach(IMyMotorStator block in m_blocks.blocks()) {
r += block.Angle; }
float agl = (r/m_blocks.count()) * (180f/3.1415926f);
return m_reversed ? 360f - agl : agl; }
public void setLimit(float lmin = float.MinValue, float lmax = float.MaxValue) {
foreach(IMyMotorStator block in m_blocks.blocks()) {
block.LowerLimitDeg = lmin;
block.UpperLimitRad = lmax; } }
private bool m_reversed; }
public class CF<T> : CT<T> where T : class, IMyTerminalBlock {
public CF(CBB<T> blocks) : base(blocks) {}
public bool enable(bool target = true) {
foreach(IMyFunctionalBlock block in m_blocks.blocks()) {
if(block.Enabled != target) { block.Enabled = target; } }
return enabled() == target; }
public bool disable() { return enable(false); }
public bool enabled() {
bool r = true;
foreach(IMyFunctionalBlock block in m_blocks.blocks()) {
r = r && block.Enabled; }
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
void showInTerminal(bool show = true) { foreach(T block in m_blocks.blocks()) { if(block.ShowInTerminal != show) { block.ShowInTerminal = show; }}}
void hideInTerminal() { showInTerminal(false); }
void showInToolbarConfig(bool show = true) { foreach(T block in m_blocks.blocks()) { if(block.ShowInToolbarConfig != show) { block.ShowInToolbarConfig = show; }}}
void hideInToolbarConfig() { showInToolbarConfig(false); }
void showInInventory(bool show = true) { foreach(T block in m_blocks.blocks()) { if(block.ShowInInventory != show) { block.ShowInInventory = show; }}}
void hideInInventory() { showInInventory(false); }
void showOnHUD(bool show = true) { foreach(T block in m_blocks.blocks()) { if(block.ShowOnHUD != show) { block.ShowOnHUD = show; }}}
void hideOnHUD() { showOnHUD(false); }
public bool empty() { return m_blocks.empty(); }
public int count() { return m_blocks.count(); }
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
public class CPiston : CF<IMyPistonBase> {
public CPiston(CBB<IMyPistonBase> blocks, int pistonsInStack = 1) : base(blocks) {
m_stackSize = pistonsInStack; }
private bool checkLength(float currentPos, float targetPos, float sensetivity = 0.2f) {
return currentPos <= targetPos + sensetivity && currentPos >= targetPos - sensetivity; }
public float currentLength() {
float l = 0;
foreach(IMyPistonBase piston in m_blocks.blocks()) {
l += piston.CurrentPosition; }
return l/m_blocks.count()/m_stackSize; }
public bool retract(float length, float velocity) {
bool r = true;
float realLength = length / m_stackSize;
float realVelocity = velocity / m_stackSize;
foreach(IMyPistonBase piston in m_blocks.blocks()) {
switch(piston.Status) {
case PistonStatus.Stopped:
case PistonStatus.Extended:
case PistonStatus.Extending:
case PistonStatus.Retracted: {
if(piston.CurrentPosition > realLength) {
piston.Velocity = realVelocity;
piston.MinLimit = realLength;
piston.MaxLimit = 10f;
piston.Retract(); } }
break; }
r = r && (piston.Status == PistonStatus.Retracted ||
(
piston.Status == PistonStatus.Retracting &&
checkLength(piston.CurrentPosition, realLength)
)); }
return r; }
public bool expand(float length, float velocity) {
bool r = true;
float realLength = length / m_stackSize;
float realVelocity = velocity / m_stackSize;
foreach(IMyPistonBase piston in m_blocks.blocks()) {
switch(piston.Status) {
case PistonStatus.Stopped:
case PistonStatus.Retracted:
case PistonStatus.Retracting:
case PistonStatus.Extended: {
if(piston.CurrentPosition < realLength) {
piston.Velocity = realVelocity;
piston.MinLimit = 0f;
piston.MaxLimit = realLength;
piston.Extend(); } }
break; }
r = r && (piston.Status == PistonStatus.Extended ||
(
piston.Status == PistonStatus.Extending &&
checkLength(piston.CurrentPosition, realLength)
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
foreach(IMyShipMergeBlock blk in m_blocks.blocks()) {
r = r && blk.IsConnected; }
return r; } }
public class CShipTool : CF<IMyShipToolBase> {
public CShipTool(CBB<IMyShipToolBase> blocks) : base(blocks) { }
public bool on(bool target = true) {
return enable(target) && checkActive(target); }
public bool off() { return on(false); }
public bool active() { return checkActive(true); }
private bool checkActive(bool target) {
bool r = true;
foreach(IMyShipToolBase tool in m_blocks.blocks()) {
r = r && tool.IsActivated == target; }
return r; } }
public class CProjector : CF<IMyProjector> {
public CProjector(CBB<IMyProjector> blocks) : base(blocks) { }
public bool projecting() {
bool r = true;
foreach(IMyProjector blk in m_blocks.blocks()) {
r = r && blk.IsProjecting; }
return r; }
public int totalBlocks() {
int r = 0;
foreach(IMyProjector blk in m_blocks.blocks()) {
r = r + blk.TotalBlocks; }
return r; }
public int remainingBlocks() {
int r = 0;
foreach(IMyProjector blk in m_blocks.blocks()) {
r = r + blk.RemainingBlocks; }
return r; }
public int weldedBlocks() {
return totalBlocks() - remainingBlocks(); } }
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
public CStateMachine(CTS lcd) {
m_lcd = lcd;
m_states = new List<CStateMachineState>();
m_currentStateIndex = 0;
waitCount = 0;
_Driven = false; }
public void addState(string name, Func<object, bool> method, object data = null) { m_states.Add(new CStateMachineState(name, method, data)); m_currentStateIndex++; }
private CStateMachineState state(int index) { return m_states[index]; }
public CStateMachineState currentState() { return state(m_currentStateIndex); }
private void switchToNextState() {
waitCount = 0;
m_currentStateIndex++;
if(active()) {
m_lcd.echo($"[{currentTime()}] Переключение состояния с {state(m_currentStateIndex-1).name()} на {state(m_currentStateIndex).name()}"); }
else {
m_lcd.echo($"[{currentTime()}] Алгоритм завершен");
if(_Driven) { _.Runtime.UpdateFrequency = UpdateFrequency.None; } } }
public void step() {
m_lcd.echo_at($"Текущее состояние: {currentState().name()} ({waitCount++})", 0);
if(currentState().callMethod()) { switchToNextState(); } }
public void start(bool _DrivenMachine = false) {
m_currentStateIndex = 0;
_Driven = _DrivenMachine;
m_lcd.echo($"[{currentTime()}] Алгоритм запущен");
if(_Driven) { _.Runtime.UpdateFrequency = UpdateFrequency.Update100; } }
public void listStates() {
foreach(CStateMachineState state in m_states) {
m_lcd.echo(state.name()); } }
public bool active() { return m_currentStateIndex < m_states.Count; }
private List<CStateMachineState> m_states;
private int m_currentStateIndex;
private CTS m_lcd;
private int waitCount;
private bool _Driven; }
public static string currentTime() {
return System.DateTime.Now.ToString("HH:mm:ss.FF"); }
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
public enum EBoolToString {
btsOnOff }
public string boolToString(bool val, EBoolToString bsType = EBoolToString.btsOnOff) {
switch(bsType) {
case EBoolToString.btsOnOff: return val ? "Вкл." : "Выкл."; }
return val.ToString(); }
float drillRotorsRPM = 0.5f;
float wallHeight = 4f * 2.5f;
float drillSteps = 20f;
float drillPistonsStep;
float drillPistonsSpeed = 1f;
float lastRotorsAngle = 0f;
float stepRotorsAngle = 190f;
float continuosRotorAngle = 0f;
int lastAngleFactor = 1;
CD logLcd;
CD statusLcd;
CRotor drillRotors0;
CRotor drillRotors45;
CPiston drillPistons;
CPiston welderPistons;
CPiston platformPistons;
CShipTool drills;
CShipTool welders;
CMerger topMergers;
CMerger bottomMergers;
CProjector wallProjector;
CStateMachine states;
public string program() {
drillPistonsStep = wallHeight / drillSteps;
logLcd = new CD();
logLcd.aD($"[{structureName}] Дисплей Лог 1", 0, 0);
logLcd.aD($"[{structureName}] Дисплей Лог 0", 1, 0);
statusLcd = new CD();
statusLcd.aD($"[{structureName}] Дисплей Статус 0", 0, 0);
drillRotors0 = new CRotor(new CB<IMyMotorStator >("Инструмент (0)"));
drillRotors45 = new CRotor(new CB<IMyMotorStator >("Инструмент (45)"));
drillPistons = new CPiston(new CB<IMyPistonBase >("Бур"));
welderPistons = new CPiston(new CB<IMyPistonBase >("Сварщик"));
platformPistons = new CPiston(new CB<IMyPistonBase >("Подвес"));
drills = new CShipTool(new CB<IMyShipToolBase >("Бур"));
welders = new CShipTool(new CB<IMyShipToolBase >("Сварщик"));
topMergers = new CMerger(new CB<IMyShipMergeBlock>("Верхний"));
bottomMergers = new CMerger(new CB<IMyShipMergeBlock>("Нижний"));
wallProjector = new CProjector(new CB<IMyProjector >("Тоннель"));
states = new CStateMachine(logLcd);
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
sS();
return "Управление кротом"; }
public void main(string argument, UpdateType updateSource) {
if(argument == "start") {
states.start(true); }
else if(argument == "to_zero") { toZero(); }
else if(argument == "to_zero_stop") { stopDrillRotors(null); }
else if(argument == "rotate") { prepareStart(null); startDrillRotors(null); }
else if(argument == "stats") { _.Runtime.UpdateFrequency = UpdateFrequency.Update100; }
else { if(states.active()) { states.step(); } sS(); } }
public float maxGradPerRun() {
return drillRotorsRPM * 6f * (float)Runtime.TimeSinceLastRun.TotalSeconds * lastAngleFactor + ((lastAngleFactor-1) * 0.5f); }
public float getRotors45Angle() {
float r45 = drillRotors45.angle()+45f;
return r45 >= 360f ? r45-360f : r45; }
public float getRotorsAngle() {
return (drillRotors0.angle() + getRotors45Angle())/2; }
public void sS() {
int i = 0;
statusLcd.echo_at($"Состояние системы: {boolToString(states.active())}", i++);
if(states.active()) { statusLcd.echo_at($"Текущий шаг: {states.currentState().name()}", i++); }
statusLcd.echo_at($"Состояние буров: {boolToString(drills.active())}", i++);
statusLcd.echo_at($"Углы роторов: [0:{drillRotors0.angle():f2},45:{getRotors45Angle():f2}] -> [avg:{getRotorsAngle():f2},cont:{continuosRotorAngle:f2},max:{maxGradPerRun():f2}]", i++);
statusLcd.echo_at($"Длинна буровых поршней: {drillPistons.currentLength():f2}", i++);
statusLcd.echo_at($"Состояние сварщиков: {boolToString(welders.active())}", i++);
statusLcd.echo_at($"Длинна сварочных поршней: {welderPistons.currentLength():f2}", i++);
statusLcd.echo_at($"Длинна подвесных поршней: {platformPistons.currentLength():f2}", i++);
statusLcd.echo_at($"Состояние верхних соединителей: {boolToString(topMergers.connected())}", i++);
statusLcd.echo_at($"Состояние нижних соединителей: {boolToString(bottomMergers.connected())}", i++);
statusLcd.echo_at(
$"Состояние проектора тоннеля: {boolToString(wallProjector.enabled())}; total:{wallProjector.totalBlocks()}; remaining:{wallProjector.remainingBlocks()}; welded:{wallProjector.weldedBlocks()}",
i++); }
public void toZero() {
drillRotors0.setLimit(0f, 0f);
drillRotors45.setLimit(45f, 45f);
startDrillRotors(null); }
public bool prepareStart(object data) {
statusLcd.clear();
logLcd.clear();
drillRotors0.setLimit();
drillRotors45.setLimit();
return true; }
public bool startDrillRotors(object data) {
drillRotors0.rotate(drillRotorsRPM);
drillRotors45.rotate(drillRotorsRPM);
drillRotors45.reverse();
return true; }
public bool stopDrillRotors(object data) {
drillRotors0.stop();
drillRotors45.stop();
return true; }
public bool startDrill(object data) { return drills.on(); }
public bool stopDrill(object data) { return drills.off(); }
public bool stepDrillPiston(object data) {
return drillPistons.expand((float)data, drillPistonsSpeed); }
public bool retractDrillPiston(object data) {
return drillPistons.retract(0f, drillPistonsSpeed); }
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
