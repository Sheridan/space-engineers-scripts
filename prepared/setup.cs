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
		else                  { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks)                                   ; } } }
public class CBlocksBase<T> where T : class, IMyTerminalBlock {
	public CBlocksBase(string purpose = "") {
		m_blocks = new List<T>();
		m_purpose = purpose; }
	public void setup(string name,
					  bool visibleInTerminal  = false,
					  bool visibleInInventory = false,
					  bool visibleInToolBar   = false) {
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
		else                  { group.GetBlocksOfType<T>(m_blocks)                                   ; } }
	public string  groupName() { return m_groupName; }
	private string m_groupName; }
public class CBlocksTyped<T> : CBlocksBase<T> where T : class, IMyTerminalBlock {
	public CBlocksTyped(string subTypeName,
						string purpose = "",
						bool loadOnlySameGrid = true) : base(purpose) {
		m_subTypeName = subTypeName;
		refresh(loadOnlySameGrid); }
	public void refresh(bool loadOnlySameGrid = true) {
		clear();
		if(loadOnlySameGrid) {
			self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => (x.IsSameConstructAs(self.Me) &&
					x.BlockDefinition.SubtypeId.Contains(m_subTypeName))); }
		else { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x =>  x.BlockDefinition.SubtypeId.Contains(m_subTypeName)); } }
	public string subTypeName() { return m_subTypeName; }
	private string m_subTypeName; }
public string program() {
	return "Настройка"; }
public void main(string argument, UpdateType updateSource) {
	(new CBlocks<IMyBatteryBlock> ()).setup("Батарея");
	(new CBlocks<IMyRemoteControl> ()).setup("Д.У.");
	(new CBlocks<IMyOreDetector> ()).setup("Детектор руды");
	(new CBlocks<IMyLandingGear> ()).setup("Шасси");
	(new CBlocks<IMyShipDrill> ()).setup("Бур");
	(new CBlocks<IMyShipGrinder> ()).setup("Резак");
	(new CBlocks<IMyShipWelder> ()).setup("Сварщик");
	(new CBlocks<IMyGyro> ()).setup("Гироскоп");
	(new CBlocks<IMyCollector> ()).setup("Коллектор");
	(new CBlocks<IMyGasGenerator> ()).setup("H2:O2 Генератор");
	(new CBlocks<IMyShipMergeBlock>()).setup("Соединитель");
	(new CBlocks<IMyRefinery> ()).setup("Очистительный завод");
	(new CBlocksTyped<IMyPowerProducer> ("HydrogenEngine"))        .setup("H2 Электрогенератор");
	(new CBlocksTyped<IMyPowerProducer> ("WindTurbine"))           .setup("Ветрогенератор");
	(new CBlocksTyped<IMyReflectorLight>("FrontLight"))            .setup("Прожектор");
	(new CBlocksTyped<IMyThrust> ("LargeAtmosphericThrust")).setup("Б.А.У.");
	(new CBlocksTyped<IMyThrust> ("SmallAtmosphericThrust")).setup("А.У.");
	(new CBlocksTyped<IMyThrust> ("LargeHydrogenThrust"))   .setup("Б.В.У.");
	(new CBlocksTyped<IMyThrust> ("SmallHydrogenThrust"))   .setup("В.У.");
	(new CBlocksTyped<IMyGasTank> ("OxygenTankSmall"))       .setup("Бак O2", false, true);
	(new CBlocksTyped<IMyGasTank> ("HydrogenTank"))          .setup("Б.Бак H2", false, true);
	(new CBlocksTyped<IMyGasTank> ("HydrogenTankSmall"))     .setup("Бак H2", false, true);
	(new CBlocksTyped<IMyCargoContainer>("SmallContainer"))        .setup("МК", false, true);
	(new CBlocksTyped<IMyCargoContainer>("MediumContainer"))       .setup("СК", false, true);
	(new CBlocksTyped<IMyCargoContainer>("LargeContainer"))        .setup("БК", false, true);
	(new CBlocksTyped<IMyUpgradeModule> ("ProductivityModule"))    .setup("Модуль Продуктивности");
	(new CBlocksTyped<IMyUpgradeModule> ("EffectivenessModule"))   .setup("Модуль Эффективности");
	(new CBlocksTyped<IMyUpgradeModule> ("EnergyModule"))          .setup("Модуль Энергоэффективности"); }
