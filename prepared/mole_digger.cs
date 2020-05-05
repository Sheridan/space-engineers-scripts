static string structureName;
static Program self;
static float blockSize;
static CBlockOptions prbOptions;
public void setupMe(string scriptName) {
	Me.CustomName = $"[{structureName}] ПрБ {scriptName}";
	setupMeSurface(0, 2f);
	setupMeSurface(1, 5f);
	Me.GetSurface(0).WriteText(scriptName.Replace(" ", "\n"));
	Me.GetSurface(1).WriteText(structureName); }
public void setupMeSurface(int i, float fontSize) {
	IMyTextSurface surface = Me.GetSurface(i);
	surface.ContentType = ContentType.TEXT_AND_IMAGE;
	surface.Font = "Monospace";
	surface.FontColor = new Color(255, 255, 255);
	surface.BackgroundColor = new Color(0, 0, 0);
	surface.FontSize = fontSize;
	surface.Alignment = TextAlignment.CENTER; }
public static void debug(string text) { self.Echo(text); }
public Program() {
	self = this;
	structureName = Me.CubeGrid.CustomName;
	blockSize = Me.CubeGrid.GridSize;
	prbOptions = new CBlockOptions(Me);
	setupMe(program()); }
public void Main(string argument, UpdateType updateSource) { main(argument, updateSource); }
public class CBlockOptions {
	public CBlockOptions(IMyTerminalBlock block) {
		m_available = false;
		m_block = block;
		read(); }
	private void read() {
		if(m_block.CustomData.Length > 0) {
			m_ini = new MyIni();
			MyIniParseResult result;
			m_available = m_ini.TryParse(m_block.CustomData, out result);
			if(!m_available) { debug(result.ToString()); } } }
	private void write() {
		m_block.CustomData = m_ini.ToString(); }
	private bool exists(string section, string name) {
		return m_available && m_ini.ContainsKey(section, name); }
	public string getValue(string section, string name, string defaultValue = "") {
		if(exists(section, name)) { return m_ini.Get(section, name).ToString(); }
		return defaultValue; }
	public bool getValue(string section, string name, bool defaultValue = true) {
		if(exists(section, name)) { return m_ini.Get(section, name).ToBoolean(); }
		return defaultValue; }
	public float getValue(string section, string name, float defaultValue = 0f) {
		if(exists(section, name)) { return float.Parse(m_ini.Get(section, name).ToString()); }
		return defaultValue; }
	public int getValue(string section, string name, int defaultValue = 0) {
		if(exists(section, name)) { return m_ini.Get(section, name).ToInt32(); }
		return defaultValue; }
	IMyTerminalBlock m_block;
	private bool m_available;
	private MyIni m_ini; }
public class CDisplay : CTextSurface {
	public CDisplay() : base() {
		m_initialized = false; }
	private void initSize(IMyTextPanel display) {
		if(!m_initialized) {
			switch(display.BlockDefinition.SubtypeName) {
			case "LargeLCDPanelWide": setup(0.602f, 28, 87, 0.35f); break;
			default: setup(1f, 0, 0, 0f); break; } } }
	public void addDisplay(string name, int x, int y) {
		IMyTextPanel display = self.GridTerminalSystem.GetBlockWithName(name) as IMyTextPanel;
		initSize(display);
		addSurface(display as IMyTextSurface, x, y); }
	private bool m_initialized; }
public class CTextSurface {
	public CTextSurface() {
		m_text = new List<string>();
		m_surfaces = new List<List<IMyTextSurface>>(); }
	public void setSurface(IMyTextSurface surface, float fontSize, int maxLines, int maxColumns, float padding = 0) {
		setup(fontSize, maxLines, maxColumns, padding);
		addSurface(surface, 0, 0); }
	public void addSurface(IMyTextSurface surface, int x, int y) {
		if(countSurfacesX() <= x) { m_surfaces.Add(new List<IMyTextSurface>()); }
		if(countSurfacesY(x) <= y) { m_surfaces[x].Add(surface); }
		else { m_surfaces[x][y] = surface; }
		setup(); }
	public void setup(float fontSize, int maxLines, int maxColumns, float padding) {
		m_fontSize = fontSize;
		m_maxLines = maxLines;
		m_maxColumns = maxColumns;
		m_padding = padding;
		setup(); }
	private void setup() {
		foreach(List<IMyTextSurface> sfList in m_surfaces) {
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
		foreach(List<IMyTextSurface> sfList in m_surfaces) {
			foreach(IMyTextSurface surface in sfList) {
				surface.WriteText("", false); } } }
	private bool surfaceExists(int x, int y) {
		return y < countSurfacesY(x); }
	private bool unknownTypeEcho(string text) {
		if(m_maxLines == 0 && surfaceExists(0, 0)) { m_surfaces[0][0].WriteText(text + '\n', true); return true; }
		return false; }
	private int countSurfacesX() { return m_surfaces.Count; }
	private int countSurfacesY(int x) { return x < countSurfacesX() ? m_surfaces[x].Count : 0; }
	public void echo(string text) {
		if(!unknownTypeEcho(text)) {
			if(m_text.Count > m_maxLines * countSurfacesY(0)) { m_text.RemoveAt(0); }
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
				m_surfaces[x][y].WriteText(line.Substring(minColumn, substringLength) + '\n', true); }
			else {
				m_surfaces[x][y].WriteText("\n", true); } } }
	private void echoText() {
		clear();
		for(int x = 0; x < countSurfacesX(); x++) {
			for(int y = 0; y < countSurfacesY(x); y++) {
				updateSurface(x, y); } } }
	private int m_maxLines;
	private int m_maxColumns;
	private float m_fontSize;
	private float m_padding;
	private List<string> m_text;
	private List<List<IMyTextSurface>> m_surfaces; }
public enum EBoolToString {
	btsOnOff }
public string boolToString(bool val, EBoolToString bsType = EBoolToString.btsOnOff) {
	switch(bsType) {
	case EBoolToString.btsOnOff: return val ? "Вкл." : "Выкл."; }
	return val.ToString(); }
public CBlockGroup<IMyShipDrill> drills;
public CBlockGroup<IMyPistonBase> pistons;
public CBlockGroup<IMyMotorStator> rotors;
public CBlockGroup<IMyGyro> gyroscopes;
const float drillRPM = 0.005f;
public void initGroups() {
	rotors = new CBlockGroup<IMyMotorStator>("[Крот] Роторы буров", "Роторы");
	drills = new CBlockGroup<IMyShipDrill>("[Крот] Буры", "Буры");
	pistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни буров", "Поршни");
	gyroscopes = new CBlockGroup<IMyGyro>("[Крот] Гироскопы бура", "Гироскопы автогоризонта"); }
public class CBlockGroup<T> : CBlocksBase<T> where T : class, IMyTerminalBlock {
	public CBlockGroup(string groupName,
					 string purpose = "",
					 bool loadOnlySameGrid = true) : base(purpose) {
		m_groupName = groupName;
		refresh(loadOnlySameGrid); }
	public void refresh(bool loadOnlySameGrid = true) {
		clear();
		IMyBlockGroup group = self.GridTerminalSystem.GetBlockGroupWithName(m_groupName);
		if(loadOnlySameGrid) { group.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
		else { group.GetBlocksOfType<T>(m_blocks) ; } }
	public string groupName() { return m_groupName; }
	private string m_groupName; }
public class CBlocksBase<T> where T : class, IMyTerminalBlock {
	public CBlocksBase(string purpose = "") {
		m_blocks = new List<T>();
		m_purpose = purpose; }
	public void setup(string name,
					 bool visibleInTerminal = false,
					 bool visibleInInventory = false,
					 bool visibleInToolBar = false) {
		Dictionary<string, int> counetrs = new Dictionary<string, int>();
		string zeros = new string('0', count().ToString().Length);
		foreach(T block in m_blocks) {
			CBlockOptions options = new CBlockOptions(block);
			string realPurpose = options.getValue("generic", "purpose", m_purpose);
			if(realPurpose != "") { realPurpose = $" {realPurpose} "; }
			else { realPurpose = " "; }
			if(!counetrs.ContainsKey(realPurpose)) { counetrs.Add(realPurpose, 0); }
			block.CustomName = $"[{structureName}] {name}{realPurpose}{counetrs[realPurpose].ToString(zeros)}";
			counetrs[realPurpose]++;
			setupBlocksVisibility(block,
								 options.getValue("generic", "visibleInTerminal", visibleInTerminal),
								 options.getValue("generic", "visibleInInventory", visibleInInventory),
								 options.getValue("generic", "visibleInToolBar", visibleInToolBar)); } }
	private void setupBlocksVisibility(T block,
									 bool vTerminal,
									 bool vInventory,
									 bool vToolBar) {
		block.ShowInTerminal = vTerminal;
		block.ShowInToolbarConfig = vToolBar;
		if(block.HasInventory) { block.ShowInInventory = vInventory; } }
	public bool empty() { return m_blocks.Count == 0; }
	public int count() { return m_blocks.Count; }
	public string subtypeName() { return empty() ? "N/A" : m_blocks[0].DefinitionDisplayNameText; }
	public bool isAssignable<U>() where U : class, IMyTerminalBlock {
		if(empty()) { return false; }
		return m_blocks[0] is U; }
	public List<T> blocks() { return m_blocks; }
	public string purpose() { return m_purpose; }
	protected void clear() { m_blocks.Clear(); }
	protected List<T> m_blocks;
	private string m_purpose; }
public class CAngle {
	public CAngle(float angle) { m_angle = checkBoards(angle); }
	public float angle() { return m_angle; }
	private static float checkBoards(float angle) {
		if(angle == 360) { return 0; }
		if(angle > 360 || angle < 0) { return Math.Abs(angle % 360); }
		return angle; }
	public static CAngle fromRad(float rad) { return new CAngle(rad * 180 / (float)Math.PI); }
	public float toRad() { return m_angle * (float)Math.PI/180; }
	public static CAngle operator +(CAngle a, CAngle b) { return new CAngle(a.angle() + b.angle()); }
	public static CAngle operator -(CAngle a, CAngle b) { return new CAngle(a.angle() - b.angle()); }
	public static CAngle operator *(CAngle a, CAngle b) { return new CAngle(a.angle() * b.angle()); }
	public static CAngle operator /(CAngle a, CAngle b) { return new CAngle(a.angle() / b.angle()); }
	public static CAngle operator +(CAngle a, float b) { return new CAngle(a.angle() + checkBoards(b)); }
	public static CAngle operator -(CAngle a, float b) { return new CAngle(a.angle() - checkBoards(b)); }
	public static CAngle operator *(CAngle a, float b) { return new CAngle(a.angle() * checkBoards(b)); }
	public static CAngle operator /(CAngle a, float b) { return new CAngle(a.angle() / checkBoards(b)); }
	public static CAngle operator ++(CAngle a) { return new CAngle(a.angle()+1f); }
	public static CAngle operator --(CAngle a) { return new CAngle(a.angle()-1f); }
	public static bool operator false(CAngle a) { return a.angle() == 0f; }
	public static bool operator true(CAngle a) { return a.angle() > 0f; }
	public static bool operator ==(CAngle a, CAngle b) { return a.angle() == b.angle(); }
	public static bool operator !=(CAngle a, CAngle b) { return a.angle() != b.angle(); }
	public static bool operator >=(CAngle a, CAngle b) { return a.angle() >= b.angle(); }
	public static bool operator <=(CAngle a, CAngle b) { return a.angle() <= b.angle(); }
	public static bool operator >(CAngle a, CAngle b) { return a.angle() > b.angle(); }
	public static bool operator <(CAngle a, CAngle b) { return a.angle() < b.angle(); }
	public static bool operator ==(CAngle a, float b) { return a.angle() == checkBoards(b); }
	public static bool operator !=(CAngle a, float b) { return a.angle() != checkBoards(b); }
	public static bool operator >=(CAngle a, float b) { return a.angle() >= checkBoards(b); }
	public static bool operator <=(CAngle a, float b) { return a.angle() <= checkBoards(b); }
	public static bool operator >(CAngle a, float b) { return a.angle() > checkBoards(b); }
	public static bool operator <(CAngle a, float b) { return a.angle() < checkBoards(b); }
	public override bool Equals(object obj) {
		if(obj == null) { return false; }
		CAngle a = obj as CAngle;
		if(a as CAngle == null) { return false; }
		return a.angle() == m_angle; }
	public override int GetHashCode() {
		return (int)m_angle *100000; }
	public override string ToString() {
		return $"{m_angle:f4}°"; }
	private float m_angle; }
public void expandPistons(CBlockGroup<IMyPistonBase> pistons,
						 float length,
						 float velocity,
						 float force,
						 int stackSize = 1) {
	float realLength = length / stackSize;
	float realVelocity = velocity / stackSize;
	float currentPosition = 0;
	foreach(IMyPistonBase piston in pistons.blocks()) {
		switch(piston.Status) {
		case PistonStatus.Stopped:
		case PistonStatus.Retracted:
		case PistonStatus.Retracting:
		case PistonStatus.Extended: {
			if(piston.CurrentPosition < realLength) {
				piston.Velocity = realVelocity;
				piston.MinLimit = 0f;
				piston.MaxLimit = realLength;
				piston.SetValue<float>("MaxImpulseAxis", force);
				piston.SetValue<float>("MaxImpulseNonAxis", force);
				piston.Extend(); } }
		break; }
		currentPosition += piston.CurrentPosition; }
	currentPosition = currentPosition / pistons.count();
	lcd.echo($"[{pistons.purpose()}] Выдвигаются до {currentPosition:f2}->{realLength:f2}"); }
public void retractPistons(CBlockGroup<IMyPistonBase> pistons,
						 float minLength,
						 float velocity,
						 float force,
						 int stackSize = 1) {
	float realLength = minLength / stackSize;
	float realVelocity = velocity / stackSize;
	float currentPosition = 0;
	foreach(IMyPistonBase piston in pistons.blocks()) {
		switch(piston.Status) {
		case PistonStatus.Stopped:
		case PistonStatus.Extended:
		case PistonStatus.Extending:
		case PistonStatus.Retracted: {
			if(piston.CurrentPosition > realLength) {
				piston.Velocity = realVelocity;
				piston.MinLimit = realLength;
				piston.MaxLimit = 10f;
				piston.SetValue<float>("MaxImpulseAxis", force);
				piston.SetValue<float>("MaxImpulseNonAxis", force);
				piston.Retract(); } }
		break; }
		currentPosition += piston.CurrentPosition; }
	currentPosition = currentPosition / pistons.count();
	lcd.echo($"[{pistons.purpose()}] Задвигаются до {currentPosition:f2}->{realLength:f2}"); }
public void playSound(string name) {
	soundBlock.SelectedSound = name;
	soundBlock.Play(); }
CAngle nextStepAngle;
const float stepAtEveryDegree = 100f;
public void incrementNextStepAngle(CAngle currentAngle) {
	nextStepAngle = currentAngle + stepAtEveryDegree;
	lcd.echo($"Следующий угол шага: {currentAngle.ToString()} -> {nextStepAngle.ToString()}"); }
public bool canStep(CAngle currentAngle, float delta = 1f) {
	lcd.echo_at($"Осталось до следующего шага: {(nextStepAngle - currentAngle).ToString()}", 0);
	if((nextStepAngle - currentAngle) <= delta) {
		incrementNextStepAngle(currentAngle + delta);
		return true; }
	return false; }
CDisplay lcd;
IMySoundBlock soundBlock;
const float pistonDrillVelocity = 0.1f;
const float pistonUpVelocity = 0.5f;
const float pistonDrillForce = 1000000f;
const float pistonUpForce = 500000f;
const int pistonsInStack = 3;
const float gyroscopeMaxPR = 0.01f;
float maxDrillLength;
float pistonStep;
IMyProgrammableBlock autoHorizont;
public string program() {
	Runtime.UpdateFrequency = UpdateFrequency.Update100;
	autoHorizont = GridTerminalSystem.GetBlockWithName("[Крот] ПрБ Атоматический горизонт") as IMyProgrammableBlock;
	pistonStep = blockSize;
	maxDrillLength = pistonStep*10;
	lcd = new CDisplay();
	lcd.addDisplay("[Крот] Дисплей логов бурения 0", 0, 0);
	lcd.addDisplay("[Крот] Дисплей логов бурения 1", 1, 0);
	initGroups();
	soundBlock = GridTerminalSystem.GetBlockWithName("[Крот] Динамик") as IMySoundBlock;
	nextStepAngle = new CAngle(0);
	return "Управление бурением"; }
public void main(string argument, UpdateType updateSource) {
	if(!parseArgumets(argument)) {
		IMyMotorStator rotor = rotors.blocks()[0];
		if(rotor.TargetVelocityRPM > 0f) {
			float pitch = 0;
			float roll = 0;
			foreach(IMyGyro gyroscope in gyroscopes.blocks()) {
				pitch += Math.Abs(gyroscope.Pitch);
				roll += Math.Abs(gyroscope.Roll); }
			if(!pauseWork(pitch/gyroscopes.count() > gyroscopeMaxPR ||
						 roll /gyroscopes.count() > gyroscopeMaxPR) &&
					canStep(CAngle.fromRad(rotor.Angle))) {
				bool pistonsAtMaxLength = true;
				foreach(IMyPistonBase piston in pistons.blocks()) {
					pistonsAtMaxLength = pistonsAtMaxLength && piston.CurrentPosition >= maxDrillLength/pistonsInStack; }
				if(!pistonsAtMaxLength) { pistonsStep(); }
				else { stopWork(); } } } } }
float pistonPosition = 0f;
public bool parseArgumets(string argument) {
	if(argument.Length == 0) { return false; }
	if(argument == "go") { startWork(); }
	else if(argument == "stop") { stopWork(); }
	else if(argument.Contains("power")) { turnDrills(argument.Contains("_on")); }
	else if(argument.Contains("rotor")) { turnRotors(argument.Contains("_start")); }
	else if(argument.Contains("piston")) {
		if(argument.Contains("_up")) { pistonsUp() ; }
		else if(argument.Contains("_step")) { pistonsStep(); } }
	return true; }
public void pistonsStep(int steps = 1) {
	playSound("Operation Alarm");
	pistonPosition += pistonStep*steps;
	lcd.echo($"Шаг {pistonPosition/pistonStep:f0} на позицию {pistonPosition:f2}");
	expandPistons(pistons, pistonPosition, pistonDrillVelocity, pistonDrillForce, pistonsInStack); }
public void pistonsUp() {
	retractPistons(pistons, 0f, pistonUpVelocity, pistonUpForce, pistonsInStack);
	pistonPosition = 0f; }
public void turnRotors(bool enable) {
	lcd.echo($"Вращение ({rotors.count()} шт.): {boolToString(enable, EBoolToString.btsOnOff)}");
	foreach(IMyMotorStator rotor in rotors.blocks())
	{ rotor.TargetVelocityRPM = enable ? drillRPM : 0f; } }
public void turnDrills(bool enable) {
	lcd.echo($"Питание буров ({drills.count()} шт.): {boolToString(enable, EBoolToString.btsOnOff)}");
	foreach(IMyShipDrill drill in drills.blocks()) { drill.Enabled = enable; } }
public void stopWork() {
	turnDrills(false);
	turnRotors(false);
	pistonsUp();
	autoHorizont.TryRun("stop"); }
public void startWork() {
	workPaused = false;
	incrementNextStepAngle(CAngle.fromRad(rotors.blocks()[0].Angle));
	turnDrills(true);
	turnRotors(true);
	autoHorizont.TryRun("start"); }
bool workPaused;
public bool pauseWork(bool wait) {
	if(workPaused != wait) {
		turnDrills(!wait);
		turnRotors(!wait);
		workPaused = wait; }
	return wait; }
