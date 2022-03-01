static string structureName;
static string scriptName;
static Program self;
static float blockSize;
static CBO prbOptions;
public void applyDefaultMeDisplayTexsts() {
	Me.GetSurface(0).WriteText(scriptName.Replace(" ", "\n"));
	Me.GetSurface(1).WriteText(structureName); }
public void echoMe(string text, int surface) { Me.GetSurface(surface).WriteText(text, false); }
public void echoMeBig(string text) { echoMe(text, 0); }
public void echoMeSmall(string text) { echoMe(text, 1); }
public void setupMe(string i_scriptName) {
	scriptName = i_scriptName;
	Me.CustomName = $"[{structureName}] ПрБ {scriptName}";
	setupMeSurface(0, 2f);
	setupMeSurface(1, 5f);
	applyDefaultMeDisplayTexsts(); }
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
	prbOptions = new CBO(Me);
	setupMe(program()); }
public void Main(string argument, UpdateType updateSource) { main(argument, updateSource); }
public class CBO {
	public CBO(IMyTerminalBlock block) {
		m_available = false;
		m_block = block;
		read(); }
	private void read() {
		if(m_block.CustomData.Length > 0) {
			m_ini = new MyIni();
			MyIniParseResult result;
			m_available = m_ini.TryParse(m_block.CustomData, out result);
			if(!m_available) { debug(result.ToString()); } } }
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
	public CB(string purpose = "", bool loadOnlySameGrid = true) : base(purpose) {
		refresh(loadOnlySameGrid); }
	public void refresh(bool loadOnlySameGrid = true) {
		clear();
		if(loadOnlySameGrid) { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
		else { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks) ; } } }
public class CBB<T> where T : class, IMyTerminalBlock {
	public CBB(string purpose = "") {
		m_blocks = new List<T>();
		m_purpose = purpose; }
	public void setup(string name,
					 bool visibleInTerminal = false,
					 bool visibleInInventory = false,
					 bool visibleInToolBar = false) {
		Dictionary<string, int> counetrs = new Dictionary<string, int>();
		string zeros = new string('0', count().ToString().Length);
		foreach(T block in m_blocks) {
			string blockPurpose = "";
			CBO o = new CBO(block);
			if(isAssignable<IMyShipConnector>()) {
				IMyShipConnector blk = block as IMyShipConnector;
				blk.PullStrength = 1f;
				blk.CollectAll = o.g("connector", "collectAll", false);
				blk.ThrowOut = o.g("connector", "throwOut", false); }
			else if(isAssignable<IMyInteriorLight>()) {
				IMyInteriorLight blk = block as IMyInteriorLight;
				blk.Radius = 10f;
				blk.Intensity = 10f;
				blk.Falloff = 3f;
				blk.Color = o.g("lamp", "color", Color.White); }
			else if(isAssignable<IMyConveyorSorter>()) {
				IMyConveyorSorter blk = block as IMyConveyorSorter;
				blk.DrainAll = o.g("sorter", "drainAll", false); }
			else if(isAssignable<IMyLargeTurretBase>()) {
				IMyLargeTurretBase blk = block as IMyLargeTurretBase;
				blk.EnableIdleRotation = true;
				blk.Elevation = 0f;
				blk.Azimuth = 0f; }
			else if(isAssignable<IMyAssembler>()) {
				blockPurpose = "Master";
				if(o.g("assembler", "is_slave", false)) {
					IMyAssembler blk = block as IMyAssembler;
					blk.CooperativeMode = true;
					blockPurpose = "Slave"; } }
			string realPurpose = $"{getPurpose(o).Trim()} {blockPurpose}";
			if(!counetrs.ContainsKey(realPurpose)) { counetrs.Add(realPurpose, 0); }
			block.CustomName = TrimAllSpaces($"[{structureName}] {name} {realPurpose} {counetrs[realPurpose].ToString(zeros).Trim()}");
			counetrs[realPurpose]++;
			setupBlocksVisibility(block,
								 o.g("generic", "visibleInTerminal", visibleInTerminal),
								 o.g("generic", "visibleInInventory", visibleInInventory),
								 o.g("generic", "visibleInToolBar", visibleInToolBar)); } }
	private string getPurpose(CBO o) {
		string result = o.g("generic", "purpose", m_purpose);
		return result != "" ? $" {result} " : " "; }
	private void setupBlocksVisibility(T block,
									 bool vTerminal,
									 bool vInventory,
									 bool vToolBar) {
		IMySlimBlock sBlock = block.CubeGrid.GetCubeBlock(block.Position);
		block.ShowInTerminal = vTerminal && sBlock.IsFullIntegrity && sBlock.BuildIntegrity < 1f;
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
public static string TrimAllSpaces(string value) {
	var newString = new StringBuilder();
	bool previousIsWhitespace = false;
	for(int i = 0; i < value.Length; i++) {
		if(Char.IsWhiteSpace(value[i])) {
			if(previousIsWhitespace) {
				continue; }
			previousIsWhitespace = true; }
		else {
			previousIsWhitespace = false; }
		newString.Append(value[i]); }
	return newString.ToString(); }
public class CBT<T> : CBB<T> where T : class, IMyTerminalBlock {
	public CBT(string subTypeName,
			 string purpose = "",
			 bool loadOnlySameGrid = true) : base(purpose) {
		m_subTypeName = subTypeName;
		refresh(loadOnlySameGrid); }
	public void refresh(bool loadOnlySameGrid = true) {
		clear();
		if(loadOnlySameGrid) {
			self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => (x.IsSameConstructAs(self.Me) &&
																	 x.BlockDefinition.ToString().Contains(m_subTypeName))); }
		else { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.BlockDefinition.ToString().Contains(m_subTypeName)); } }
	public string subTypeName() { return m_subTypeName; }
	private string m_subTypeName; }
public class CF<T> : CTerminal<T> where T : class, IMyTerminalBlock {
	public CF(CBB<T> blocks) : base(blocks) {}
	public void enable(bool enabled = true) { foreach(IMyFunctionalBlock block in m_blocks.blocks()) { if(block.Enabled != enabled) { block.Enabled = enabled; }}}
	public void disable() { enable(false); } }
public class CTerminal<T> where T : class, IMyTerminalBlock {
	public CTerminal(CBB<T> blocks) { m_blocks = blocks; }
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
	protected CBB<T> m_blocks; }
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
public CF<IMyPowerProducer> h2Engines;
bool generationOn;
public string program() {
	h2Engines = new CF<IMyPowerProducer>(new CBT<IMyPowerProducer>("HydrogenEngine"));
	generationOn = false;
	switchGenerate();
	return "Управление водородными генераторами"; }
public void switchGenerate() {
	h2Engines.enable(generationOn);
	generationOn = !generationOn; }
public void main(string argument, UpdateType updateSource) {
	if(argument == "generate") {
		switchGenerate(); } }
