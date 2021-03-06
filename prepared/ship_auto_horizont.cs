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
public class CBlockStatusDisplay : CDisplay {
	public CBlockStatusDisplay() : base() {}
	private string getFunctionaBlocksStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getRotorsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getGasTanksStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getBatteryesStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getInvertoryesStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getPistonsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getGyroStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getMergersStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
		if(!group.isAssignable<IMyShipMergeBlock>()) { return ""; }
		string result = "";
		int connected = 0;
		foreach(IMyShipMergeBlock block in group.blocks()) {
			if(block.IsConnected) { connected++; } }
		result += $"Connected: {connected} ";
		return result; }
	private string getConnectorsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getProjectorsStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	private string getPowerProducersStatus<T>(CBlocksBase<T> group) where T : class, IMyTerminalBlock {
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
	public void showStatus<T>(CBlocksBase<T> group, int position) where T : class, IMyTerminalBlock {
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
public enum EAHForwardDirection {
	Forward,
	Up }
public class CShipController {
	public CShipController(IMyShipController controller, EAHForwardDirection forwardDirection = EAHForwardDirection.Forward) {
		m_controller = controller;
		m_autoHorizontGyroscopes = null;
		m_forwardDirection = forwardDirection; }
	public CShipController(string name) {
		m_controller = self.GridTerminalSystem.GetBlockWithName(name) as IMyShipController;
		m_autoHorizontGyroscopes = null; }
	public void enableAutoHorizont(CBlockGroup<IMyGyro> gyroscopes) {
		if(autoHorizontIsEnabled()) { return; }
		m_autoHorizontGyroscopes = gyroscopes;
		foreach(IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks()) {
			gyroscope.GyroOverride = true; } }
	public void disableAutoHorizont() {
		if(!autoHorizontIsEnabled()) { return; }
		foreach(IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks()) {
			gyroscope.Yaw = 0;
			gyroscope.Roll = 0;
			gyroscope.Pitch = 0;
			gyroscope.GyroOverride = false; }
		m_autoHorizontGyroscopes = null; }
	private void applyGyroOverride(double yaw, double roll, double pitch) {
		Vector3D relRotVector = Vector3D.TransformNormal(new Vector3D(-pitch, yaw, roll),
								m_controller.WorldMatrix);
		foreach(IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks()) {
			Vector3D transRotVector = Vector3D.TransformNormal(relRotVector, Matrix.Transpose(gyroscope.WorldMatrix));
			switch(m_forwardDirection) {
			case EAHForwardDirection.Forward: {
				gyroscope.Yaw = (float)transRotVector.Y;
				gyroscope.Roll = (float)transRotVector.Z;
				gyroscope.Pitch = (float)transRotVector.X; } break;
			case EAHForwardDirection.Up: {
				gyroscope.Yaw = (float)transRotVector.Y;
				gyroscope.Roll = (float)transRotVector.X;
				gyroscope.Pitch = (float)transRotVector.Z; } break; } } }
	private Vector3D getForwardDirection() {
		switch(m_forwardDirection) {
		case EAHForwardDirection.Forward: return m_controller.WorldMatrix.Forward;
		case EAHForwardDirection.Up: return m_controller.WorldMatrix.Down; }
		return m_controller.WorldMatrix.Forward; }
	public void autoHorizont(float yaw) {
		if(!autoHorizontIsEnabled()) { return; }
		Vector3D normGravity = Vector3D.Normalize(m_controller.GetNaturalGravity());
		applyGyroOverride(yaw,
						 (float)normGravity.Dot(m_controller.WorldMatrix.Left),
						 (float)normGravity.Dot(getForwardDirection())); }
	public bool autoHorizontIsEnabled() { return m_autoHorizontGyroscopes != null; }
	public void checkAutoHorizont() {
		Vector3D normGravity = Vector3D.Normalize(m_controller.GetNaturalGravity());
		debug($"Ctrl: {m_controller.DisplayNameText}");
		debug($"Left: {normGravity.Dot(m_controller.WorldMatrix.Left)}");
		debug($"Forward: {normGravity.Dot(getForwardDirection())}"); }
	private IMyShipController m_controller;
	private CBlockGroup<IMyGyro> m_autoHorizontGyroscopes;
	private EAHForwardDirection m_forwardDirection; }
public static double angleBetweenVectors(Vector3D a, Vector3D b) {
	return MathHelper.ToDegrees(Math.Acos(a.Dot(b) / (a.Length() * b.Length()))); }
public enum EBoolToString {
	btsOnOff }
public string boolToString(bool val, EBoolToString bsType = EBoolToString.btsOnOff) {
	switch(bsType) {
	case EBoolToString.btsOnOff: return val ? "Вкл." : "Выкл."; }
	return val.ToString(); }
CShipController controller;
CBlockGroup<IMyGyro> gyroscopes;
CTextSurface lcd;
IMyCockpit cockpit;
float autoRotateYawRPM;
bool autorotate;
public EAHForwardDirection loadForwardDirection() {
	switch(prbOptions.getValue("script", "forwardDirection", "forward")) {
	case "forward": return EAHForwardDirection.Forward;
	case "up": return EAHForwardDirection.Up; }
	return EAHForwardDirection.Forward; }
public string program() {
	autorotate = false;
	string cockpitName = prbOptions.getValue("script", "cockpitName", "");
	cockpit = self.GridTerminalSystem.GetBlockWithName(cockpitName) as IMyCockpit;
	lcd = new CTextSurface();
	int cockpitSurface = prbOptions.getValue("script", "cockpitSurface", 0);
	if(cockpitSurface >= 0) { lcd.setSurface(cockpit.GetSurface(cockpitSurface), 2f, 7, 30); }
	else { lcd.setSurface(Me.GetSurface(0), 2f, 7, 14); }
	controller = new CShipController(
		self.GridTerminalSystem.GetBlockWithName(prbOptions.getValue("script", "controllerName", cockpitName)) as IMyShipController,
		loadForwardDirection());
	gyroscopes = new CBlockGroup<IMyGyro>(prbOptions.getValue("script", "gyrosGroupName", ""), "Гироскопы");
	return "Атоматический горизонт"; }
public void main(string argument, UpdateType updateSource) {
	if(argument.Length == 0) {
		controller.autoHorizont(autorotate ? autoRotateYawRPM : cockpit.RotationIndicator.Y); }
	else {
		if(argument == "check") { controller.checkAutoHorizont(); }
		else if(argument == "start") { if(controller.autoHorizontIsEnabled()) { disableAH(); } else { enableAH(); } }
		else if(argument == "stop") { disableAH(); }
		else if(argument == "restart") { disableAH(); enableAH(); }
		else if(argument == "autorotate") { autorotate = !autorotate; }
		lcd.echo_at($"АГ: {boolToString(controller.autoHorizontIsEnabled())}", 0);
		lcd.echo_at($"Авто: {boolToString(autorotate)}", 1);
		lcd.echo_at($"Гир: {gyroscopes.count()}", 2); } }
public void enableAH() {
	autoRotateYawRPM = prbOptions.getValue("script", "autorotateRPM", 0f);
	controller.enableAutoHorizont(gyroscopes);
	Runtime.UpdateFrequency = UpdateFrequency.Update1; }
public void disableAH() {
	controller.disableAutoHorizont();
	Runtime.UpdateFrequency = UpdateFrequency.None; }
