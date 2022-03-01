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
public class CBlockStatusDisplay : CDisplay {
	public CBlockStatusDisplay() : base() {}
	private string getFunctionaBlocksStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyFunctionalBlock>()) { return ""; }
		string result = "";
		int pOn = 0;
		int fOn = 0;
		int wOn = 0;
		float powerConsumed = 0f;
		float powerMaxConsumed = 0f;
		foreach(IMyFunctionalBlock block in group.blocks()) {
			if(block.Enabled) {
				pOn++;
				CBlockPowerInfo pInfo = new CBlockPowerInfo(block);
				powerConsumed += pInfo.currentConsume();
				powerMaxConsumed += pInfo.maxConsume(); }
			if(block.IsFunctional) { fOn++; }
			if(block.IsWorking) { wOn++; } }
		result += $"PFW: {pOn}:{fOn}:{wOn} ";
		if(powerMaxConsumed > 0) {
			result += $"Consuming (now,max): {toHumanReadable(powerConsumed, EHRUnit.Power)}:{toHumanReadable(powerMaxConsumed, EHRUnit.Power)} "; }
		return result; }
	private string getRotorsStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyMotorStator>()) { return ""; }
		string result = "";
		List<string> rpm = new List<string>();
		List<string> angle = new List<string>();
		foreach(IMyMotorStator block in group.blocks()) {
			float angleGrad = block.Angle * 180 / (float)Math.PI;
			rpm.Add($"{block.TargetVelocityRPM:f2}");
			angle.Add($"{angleGrad:f2}°"); }
		result += $"Angle: {string.Join(":", angle)} "
				 + $"RPM: {string.Join(":", rpm)} ";
		return result; }
	private string getGasTanksStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyGasTank>()) { return ""; }
		string result = "";
		float capacity = 0;
		double filledRatio = 0;
		foreach(IMyGasTank block in group.blocks()) {
			capacity += block.Capacity;
			filledRatio += block.FilledRatio; }
		result += $"Capacity: {toHumanReadable(capacity, EHRUnit.Volume)} "
				 + $"Filled: {filledRatio/group.count()*100:f2}% ";
		return result; }
	private string getBatteryesStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyBatteryBlock>()) { return ""; }
		string result = "";
		float currentStored = 0;
		float maxStored = 0;
		foreach(IMyBatteryBlock block in group.blocks()) {
			currentStored += block.CurrentStoredPower;
			maxStored += block.MaxStoredPower; }
		currentStored *= 1000000;
		maxStored *= 1000000;
		result += $"Capacity: {toHumanReadable(currentStored, EHRUnit.PowerCapacity)}:{toHumanReadable(maxStored, EHRUnit.PowerCapacity)} ";
		return result; }
	private string getInvertoryesStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		long volume = 0;
		long volumeMax = 0;
		int mass = 0;
		int items = 0;
		int inventoryes = 0;
		foreach(IMyTerminalBlock block in group.blocks()) {
			if(block.HasInventory) {
				IMyInventory inventory;
				inventoryes = block.InventoryCount;
				for(int i = 0; i < inventoryes; i++) {
					inventory = block.GetInventory(i);
					volume += inventory.CurrentVolume.ToIntSafe();
					volumeMax += inventory.MaxVolume.ToIntSafe();
					mass += inventory.CurrentMass.ToIntSafe();
					items += inventory.ItemCount; } } }
		if(inventoryes > 0) {
			mass *= 1000;
			return $"VMI: ({toHumanReadable(volume, EHRUnit.Volume)}:{toHumanReadable(volumeMax, EHRUnit.Volume)}):{toHumanReadable(mass, EHRUnit.Mass)}:{toHumanReadable(items)} from {inventoryes} "; }
		return ""; }
	private string getPistonsStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyPistonBase>()) { return ""; }
		string result = "";
		List<string> positions = new List<string>();
		int statusStopped = 0;
		int statusExtending = 0;
		int statusExtended = 0;
		int statusRetracting = 0;
		int statusRetracted = 0;
		foreach(IMyPistonBase block in group.blocks()) {
			switch(block.Status) {
				case PistonStatus.Stopped: statusStopped++; break;
				case PistonStatus.Extending: statusExtending++; break;
				case PistonStatus.Extended: statusExtended++; break;
				case PistonStatus.Retracting: statusRetracting++; break;
				case PistonStatus.Retracted: statusRetracted++; break; }
			positions.Add($"{block.CurrentPosition:f2}"); }
		result += $"SeErR: {statusStopped}:{statusExtending}:{statusExtended}:{statusRetracting}:{statusRetracted} "
				 + $"Pos: {string.Join(":", positions)} ";
		return result; }
	private string getGyroStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyGyro>()) { return ""; }
		string result = "";
		float yaw = 0;
		float pitch = 0;
		float roll = 0;
		foreach(IMyGyro block in group.blocks()) {
			yaw += Math.Abs(block.Yaw);
			pitch += Math.Abs(block.Pitch);
			roll += Math.Abs(block.Roll); }
		result += $"YPR: {yaw/group.count():f4}:{pitch/group.count():f4}:{roll/group.count():f4} ";
		return result; }
	private string getMergersStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyShipMergeBlock>()) { return ""; }
		string result = "";
		int connected = 0;
		foreach(IMyShipMergeBlock block in group.blocks()) {
			if(block.IsConnected) { connected++; } }
		result += $"Connected: {connected} ";
		return result; }
	private string getConnectorsStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyShipConnector>()) { return ""; }
		string result = "";
		int statusUnconnected = 0;
		int statusConnectable = 0;
		int statusConnected = 0;
		foreach(IMyShipConnector block in group.blocks()) {
			switch(block.Status) {
				case MyShipConnectorStatus.Unconnected: statusUnconnected++; break;
				case MyShipConnectorStatus.Connectable: statusConnectable++; break;
				case MyShipConnectorStatus.Connected: statusConnected++; break; } }
		result += $"UcC: {statusUnconnected}:{statusConnectable}:{statusConnected} ";
		return result; }
	private string getProjectorsStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyProjector>()) { return ""; }
		string result = "";
		int projecting = 0;
		List<string> blocksTotal = new List<string>();
		List<string> blocksRemaining = new List<string>();
		List<string> blocksBuildable = new List<string>();
		foreach(IMyProjector block in group.blocks()) {
			if(block.IsProjecting) { projecting++; }
			blocksTotal.Add($"{block.TotalBlocks}");
			blocksRemaining.Add($"{block.RemainingBlocks}");
			blocksBuildable.Add($"{block.BuildableBlocksCount}"); }
		result += $"Pr: {projecting} "
				 + $"B: {string.Join(":", blocksBuildable)} "
				 + $"R: {string.Join(":", blocksRemaining)} "
				 + $"T: {string.Join(":", blocksTotal)} "
				 ;
		return result; }
	private string getPowerProducersStatus<T>(CBB<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyPowerProducer>()) { return ""; }
		string result = "";
		float currentOutput = 0f;
		float maxOutput = 0f;
		foreach(IMyPowerProducer block in group.blocks()) {
			CBlockPowerInfo pInfo = new CBlockPowerInfo(block);
			currentOutput += pInfo.currentProduce();
			maxOutput += pInfo.maxProduce(); }
		result += $"Ген. энергии (now:max): {toHumanReadable(currentOutput, EHRUnit.Power)}:{toHumanReadable(maxOutput, EHRUnit.Power)} ";
		return result; }
	public void showStatus<T>(CBB<T> group, int position) where T : class, IMyTerminalBlock {
		string result = $"[{group.subtypeName()}] {group.purpose()} ";
		if(!group.empty()) {
			result += $"({group.count()}) "
					 + getPistonsStatus<T>(group)
					 + getConnectorsStatus<T>(group)
					 + getMergersStatus<T>(group)
					 + getProjectorsStatus<T>(group)
					 + getRotorsStatus<T>(group)
					 + getGyroStatus<T>(group)
					 + getBatteryesStatus<T>(group)
					 + getGasTanksStatus<T>(group)
					 + getPowerProducersStatus<T>(group)
					 + getInvertoryesStatus<T>(group)
					 + getFunctionaBlocksStatus<T>(group)
					 ; }
		else {
			result += "Таких блоков нет"; }
		echo_at(result, position); } }
public class CDisplay : CTextSurface {
	public CDisplay() : base() {
		m_initialized = false; }
	private void initSize(IMyTextPanel display) {
		if(!m_initialized) {
			debug($"{display.BlockDefinition.SubtypeName}");
			switch(display.BlockDefinition.SubtypeName) {
				case "LargeLCDPanelWide": setup(0.602f, 28, 87, 0.35f); break;
				case "LargeLCDPanel" : setup(0.602f, 28, 44, 0.35f); break;
				default: setup(1f, 1, 1, 1f); break; } } }
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
class CBlockUpgrades {
	public CBlockUpgrades(IMyUpgradableBlock upBlock) {
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
public class CBlockPowerInfo {
	public CBlockPowerInfo(IMyTerminalBlock block) {
		m_block = block;
		m_blockSinkComponent = m_block.Components.Get<MyResourceSinkComponent>(); }
	public bool canProduce() { return m_block is IMyPowerProducer; }
	public bool canConsume() {
		return m_blockSinkComponent != null && m_blockSinkComponent.IsPoweredByType(Electricity); }
	public float currentProduce() {
		if(canProduce()) { return (m_block as IMyPowerProducer).CurrentOutput*1000000; }
		return 0f; }
	public float maxProduce() {
		if(canProduce()) { return (m_block as IMyPowerProducer).MaxOutput*1000000; }
		return 0f; }
	public float currentConsume() {
		if(canConsume()) {
			float result = m_blockSinkComponent.CurrentInputByType(Electricity);
			return result * 1000000; }
		return 0f; }
	public float maxConsume() {
		if(canConsume()) {
			float result = m_blockSinkComponent.MaxRequiredInputByType(Electricity);
			if(m_block is IMyAssembler || m_block is IMyRefinery) {
				CBlockUpgrades upgrades = new CBlockUpgrades(m_block as IMyUpgradableBlock);
				upgrades.calcPowerUse(result); }
			return result * 1000000; }
		return 0f; }
	MyResourceSinkComponent m_blockSinkComponent;
	IMyTerminalBlock m_block;
	private static readonly MyDefinitionId Electricity = MyDefinitionId.Parse("MyObjectBuilder_GasProperties/Electricity"); }
public enum EHRUnit {
	None,
	Mass,
	Volume,
	Power,
	PowerCapacity }
public static string hrSuffix(EHRUnit unit) {
	switch(unit) {
		case EHRUnit.None : return "шт.";
		case EHRUnit.Mass : return "г.";
		case EHRUnit.Volume : return "м³";
		case EHRUnit.Power : return "Вт.";
		case EHRUnit.PowerCapacity : return "ВтЧ."; }
	return ""; }
public static string toHumanReadable(float value, EHRUnit unit = EHRUnit.None) {
	string suffix = hrSuffix(unit);
	if(value < 1000) { return $"{value}{suffix}"; }
	int exp = (int)(Math.Log(value) / Math.Log(1000));
	return $"{value / Math.Pow(1000, exp):f2}{("кМГТПЭ")[exp - 1]}{suffix}"; // "kMGTPE" "кМГТПЭ"
}
public class CB<T> : CBB<T> where T : class, IMyTerminalBlock {
	public CB(string purpose = "", bool loadOnlySameGrid = true) : base(purpose) {
		refresh(loadOnlySameGrid); }
	public void refresh(bool loadOnlySameGrid = true) {
		clear();
		if(loadOnlySameGrid) { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
		else { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks) ; } } }
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
CBlockStatusDisplay lcd;
public CB<IMyShipDrill> drills;
public CB<IMyShipConnector> connectors;
public CB<IMyCargoContainer> storage;
public CB<IMyThrust> thrusters;
public CB<IMyGyro> gyroscopes;
public CB<IMyBatteryBlock> battaryes;
public void initGroups() {
	drills = new CB<IMyShipDrill>();
	connectors = new CB<IMyShipConnector>();
	storage = new CB<IMyCargoContainer>();
	thrusters = new CB<IMyThrust>();
	gyroscopes = new CB<IMyGyro>();
	battaryes = new CB<IMyBatteryBlock>(); }
public string program() {
	Runtime.UpdateFrequency = UpdateFrequency.Update100;
	lcd = new CBlockStatusDisplay();
	lcd.addDisplay("[Бур] Дисплей Статус 5", 0, 0);
	lcd.addDisplay("[Бур] Дисплей Статус 4", 1, 0);
	lcd.addDisplay("[Бур] Дисплей Статус 0", 2, 0);
	lcd.addDisplay("[Бур] Дисплей Статус 1", 3, 0);
	lcd.addDisplay("[Бур] Дисплей Статус 2", 4, 0);
	lcd.addDisplay("[Бур] Дисплей Статус 3", 5, 0);
	lcd.addDisplay("[Бур] Дисплей Статус 6", 6, 0);
	initGroups();
	return "Отображение статуса"; }
public void main(string argument, UpdateType updateSource) {
	int i = 0;
	lcd.showStatus<IMyShipDrill>(drills, i++);
	lcd.showStatus<IMyCargoContainer>(storage, i++);
	lcd.showStatus<IMyShipConnector>(connectors, i++);
	lcd.showStatus<IMyThrust>(thrusters, i++);
	lcd.showStatus<IMyGyro>(gyroscopes, i++);
	lcd.showStatus<IMyBatteryBlock>(battaryes, i++); }
