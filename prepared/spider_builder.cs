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
public class CStateMachineState {
	public CStateMachineState(string name, Func<bool> method) {
		m_name = name;
		m_method = method; }
	public bool callMethod() { return m_method(); }
	public string name() { return m_name; }
	string m_name;
	Func<bool> m_method; }
public class CStateMachine {
	public CStateMachine(CTextSurface lcd, int defaultState = 0) {
		m_lcd = lcd;
		m_states = new List<CStateMachineState>();
		m_currentStateIndex = defaultState-1; }
	public void addState(string name, Func<bool> method) { m_states.Add(new CStateMachineState(name, method)); }
	private CStateMachineState state(int index) { return m_states[index]; }
	public CStateMachineState currentState() { return state(m_currentStateIndex); }
	private void switchToNextState() {
		if(m_currentStateIndex < m_states.Count-1) {
			m_lcd.echo($"Переключение состояния с {state(m_currentStateIndex).name()} на {state(m_currentStateIndex+1).name()}");
			m_currentStateIndex++; } }
	public void step() {
		m_lcd.echo_at($"Текущее состояние: {currentState().name()}", 0);
		if(currentState().callMethod()) { switchToNextState(); } }
	public void start() { m_currentStateIndex = 0; }
	public void listStates() {
		foreach(CStateMachineState state in m_states) {
			m_lcd.echo(state.name()); } }
	private List<CStateMachineState> m_states;
	private int m_currentStateIndex;
	private CTextSurface m_lcd; }
public class CPiston : CFunctional<IMyPistonBase> {
	public CPiston(CBlocksBase<IMyPistonBase> blocks, int pistonsInStack = 1) : base(blocks) {
		m_stackSize = pistonsInStack; }
	private bool checkLength(float currentPos, float targetPos, float sensetivity = 0.2f) {
		return currentPos <= targetPos + sensetivity && currentPos >= targetPos - sensetivity; }
	public bool retract(float length, float velocity) {
		bool result = true;
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
			result = result && (piston.Status == PistonStatus.Retracted ||
								(
									piston.Status == PistonStatus.Retracting &&
									checkLength(piston.CurrentPosition, realLength)
								)); }
		return result; }
	public bool expand(float length, float velocity) {
		bool result = true;
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
			result = result && (piston.Status == PistonStatus.Extended ||
								(
									piston.Status == PistonStatus.Extending &&
									checkLength(piston.CurrentPosition, realLength)
								)); }
		return result; }
	private int m_stackSize; }
public class CFunctional<T> : CTerminal<T> where T : class, IMyTerminalBlock {
	public CFunctional(CBlocksBase<T> blocks) : base(blocks) {}
	public void enable(bool enabled = true) { foreach(IMyFunctionalBlock block in m_blocks.blocks()) { if(block.Enabled != enabled) { block.Enabled = enabled; }}}
	public void disable() { enable(false); } }
public class CTerminal<T> where T : class, IMyTerminalBlock {
	public CTerminal(CBlocksBase<T> blocks) { m_blocks = blocks; }
	public void listProperties(CTextSurface lcd) {
		if(m_blocks.count() == 0) { return; }
		List<ITerminalProperty> properties = new List<ITerminalProperty>();
		m_blocks.blocks()[0].GetProperties(properties);
		foreach(var property in properties) {
			lcd.echo($"id: {property.Id}, type: {property.TypeName}"); } }
	public void listActions(CTextSurface lcd) {
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
	protected CBlocksBase<T> m_blocks; }
public class CMerger : CFunctional<IMyShipMergeBlock> {
	public CMerger(CBlocksBase<IMyShipMergeBlock> blocks) : base(blocks) { }
	public bool connect(bool enabled = true) {
		enable(enabled);
		bool result = true;
		foreach(IMyShipMergeBlock merger in m_blocks.blocks()) {
			result = result && merger.Enabled == enabled; }
		return result; }
	public bool disconnect() { return connect(false); } }
public CBlockGroup<IMyShipMergeBlock> weldersMergers;
public CBlockGroup<IMyPistonBase> weldersMergersPistons;
public CBlockGroup<IMyShipMergeBlock> supportMergers;
public CBlockGroup<IMyPistonBase> supportMergersPistons;
public CBlockGroup<IMyShipConnector> mainConnectors;
public CBlockGroup<IMyPistonBase> mainPistons;
public CBlockGroup<IMyShipWelder> welders;
public CBlockGroup<IMyProjector> projectors;
public void initGroups() {
	weldersMergers = new CBlockGroup<IMyShipMergeBlock>("[Паук] Соединители верхней опоры", "СВО");
	weldersMergersPistons = new CBlockGroup<IMyPistonBase>("[Паук] Поршни соединителей верхней опоры", "Поршни СВО");
	supportMergers = new CBlockGroup<IMyShipMergeBlock>("[Паук] Соединители нижней опоры", "СНО");
	supportMergersPistons = new CBlockGroup<IMyPistonBase>("[Паук] Поршни соединителей нижней опоры", "Поршни СНО");
	mainPistons = new CBlockGroup<IMyPistonBase>("[Паук] Поршни хода", "Поршни хода");
	welders = new CBlockGroup<IMyShipWelder>("[Паук] Сварщики", "Сварщики");
	projectors = new CBlockGroup<IMyProjector>("[Паук] Проекторы", "Проекторы"); }
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
CDisplay lcd;
CStateMachine states;
CPiston weldersMergersPistonsWorker;
CMerger weldersMergersWorker;
CPiston supportMergersPistonsWorker;
CMerger supportMergersWorker;
float foundationPistonSpeed = 2.5f;
float foundationPistonLength = 2.3f;
public string program() {
	Runtime.UpdateFrequency = UpdateFrequency.Update100;
	lcd = new CDisplay();
	lcd.addDisplay("[Паук] Дисплей логов строительства 0", 0, 0);
	lcd.addDisplay("[Паук] Дисплей логов строительства 1", 1, 0);
	initGroups();
	weldersMergersPistonsWorker = new CPiston(weldersMergersPistons);
	weldersMergersWorker = new CMerger(weldersMergers);
	supportMergersPistonsWorker = new CPiston(supportMergersPistons);
	supportMergersWorker = new CMerger(supportMergers);
	states = new CStateMachine(lcd, 10);
	states.addState("Wakeup", wakeup);
	states.addState("Disconnect Welder Foundation", disconnectWelderFoundation);
	states.addState("Prepare Welding", prepareWelding);
	states.addState("Welding", welding);
	states.addState("Stop Welding", stopWelding);
	states.addState("Connect Welder Foundation", connectWelderFoundation);
	states.addState("Disconnect Support Foundation", disconnectSupportFoundation);
	states.addState("Move Base", moveBase);
	states.addState("Connect Support Foundation", connectSupportFoundation);
	states.addState("Sleep", sleep);
	return "Управление строительством"; }
public void main(string argument, UpdateType updateSource) {
	if(argument == "start") { states.start(); }
	else if(argument == "test")
	{ disconnectWelderFoundation(); }
	else { states.step(); } }
public bool wakeup() {
	return true; }
public bool disconnectWelderFoundation() {
	return weldersMergersWorker.disconnect() &&
			weldersMergersPistonsWorker.retract(0f, foundationPistonSpeed); }
public bool prepareWelding() {
	return true; }
public bool welding() {
	return true; }
public bool stopWelding() {
	return true; }
public bool connectWelderFoundation() {
	return weldersMergersPistonsWorker.expand(foundationPistonLength, foundationPistonSpeed) &&
			weldersMergersWorker.connect(); }
public bool disconnectSupportFoundation() {
	return supportMergersWorker.disconnect() &&
			supportMergersPistonsWorker.retract(0f, foundationPistonSpeed); }
public bool moveBase() {
	return true; }
public bool connectSupportFoundation() {
	return supportMergersPistonsWorker.expand(foundationPistonLength, foundationPistonSpeed) &&
			supportMergersWorker.connect(); }
public bool sleep() {
	return true; }
