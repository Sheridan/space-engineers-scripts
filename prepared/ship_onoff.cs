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
public class CBlocks<T> : CBlocksBase<T> where T : class, IMyTerminalBlock {
	public CBlocks(string purpose = "", bool loadOnlySameGrid = true) : base(purpose) {
		refresh(loadOnlySameGrid); }
	public void refresh(bool loadOnlySameGrid = true) {
		clear();
		if(loadOnlySameGrid) { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
		else { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks) ; } } }
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
public class CBattery : CFunctional<IMyBatteryBlock> {
	public CBattery(CBlocksBase<IMyBatteryBlock> blocks) : base(blocks) { }
	public bool setChargeMode(ChargeMode mode) {
		bool result = true;
		foreach(IMyBatteryBlock battery in m_blocks.blocks()) {
			if(battery.ChargeMode != mode) { battery.ChargeMode = mode; }
			result = result && battery.ChargeMode == mode; }
		return result; }
	public bool recharge() { return setChargeMode(ChargeMode.Recharge); }
	public bool discharge() { return setChargeMode(ChargeMode.Discharge); }
	public bool autocharge() { return setChargeMode(ChargeMode.Auto); } }
public class CConnector : CFunctional<IMyShipConnector> {
	public CConnector(CBlocksBase<IMyShipConnector> blocks) : base(blocks) { }
	public bool connect(bool enabled = true) {
		bool result = true;
		foreach(IMyShipConnector connector in m_blocks.blocks()) {
			if(enabled) { connector.Connect(); }
			else { connector.Disconnect(); }
			result = result &&
					 (
						 enabled ? connector.Status == MyShipConnectorStatus.Connected
						 : connector.Status == MyShipConnectorStatus.Unconnected
					 ); }
		return result; }
	public bool disconnect() { return connect(false); } }
public class CTank : CFunctional<IMyGasTank> {
	public CTank(CBlocksBase<IMyGasTank> blocks) : base(blocks) { }
	public bool enableStockpile(bool enabled = true) {
		bool result = true;
		foreach(IMyGasTank tank in m_blocks.blocks()) {
			if(tank.Stockpile != enabled) { tank.Stockpile = enabled; }
			result = result && tank.Stockpile == enabled; }
		return result; }
	public bool disableStockpile() { return enableStockpile(false); } }
public CFunctional<IMyGyro> gyroscopes;
public CFunctional<IMyThrust> thrusters;
public CFunctional<IMyLightingBlock> lamps;
public CBattery battaryes;
public CConnector connectors;
public CTank tanks;
IMyProgrammableBlock pbAutoHorizont;
bool connected;
public string program() {
	pbAutoHorizont = self.GridTerminalSystem.GetBlockWithName($"[{structureName}] ПрБ Атоматический горизонт") as IMyProgrammableBlock;
	gyroscopes = new CFunctional<IMyGyro>(new CBlocks<IMyGyro>());
	thrusters = new CFunctional<IMyThrust>(new CBlocks<IMyThrust>());
	battaryes = new CBattery(new CBlocks<IMyBatteryBlock>());
	connectors = new CConnector(new CBlocks<IMyShipConnector>());
	lamps = new CFunctional<IMyLightingBlock>(new CBlocks<IMyLightingBlock>());
	tanks = new CTank(new CBlocks<IMyGasTank>());
	connected = true;
	return "Управление стыковкой корабля"; }
public void main(string argument, UpdateType updateSource) {
	if(argument == "start") {
		if(connected) { turnOff(); }
		else { turnOn(); } } }
public void turnOn() {
	battaryes.autocharge();
	tanks.disableStockpile();
	thrusters.enable();
	gyroscopes.enable();
	if(pbAutoHorizont != null) { pbAutoHorizont.TryRun("start"); }
	lamps.enable();
	connectors.disconnect();
	connected = true; }
public void turnOff() {
	connectors.connect();
	lamps.disable();
	if(pbAutoHorizont != null) { pbAutoHorizont.TryRun("stop"); }
	gyroscopes.disable();
	thrusters.disable();
	tanks.enableStockpile();
	battaryes.recharge();
	connected = false; }
