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
public class CRecipe {
	public CRecipe(string blueprint) { m_blueprint = blueprint; m_sourceItems = new List<CComponentItem>(); }
	public void addItem(CComponentItem item) { m_sourceItems.Add(item); }
	public string blueprint() { return m_blueprint; }
	public List<CComponentItem> sourceItems() { return m_sourceItems; }
	private string m_blueprint;
	private List<CComponentItem> m_sourceItems; }
public class FRecipe {
	static public CRecipe fromString(string itemString, int amount = 1) {
		switch(itemString) {
		case "MyObjectBuilder_CubeBlock/LargeBlockArmorBlock"         : return LargeBlockArmorBlock(amount);
		case "MyObjectBuilder_InteriorLight/SmallLight"               : return SmallLight(amount);
		case "MyObjectBuilder_ConveyorConnector/ConveyorTube"         : return ConveyorTube(amount);
		case "MyObjectBuilder_MergeBlock/LargeShipMergeBlock"         : return LargeShipMergeBlock(amount);
		case "MyObjectBuilder_ShipConnector/Connector"                : return Connector(amount);
		case "MyObjectBuilder_Conveyor/LargeBlockConveyor"            : return LargeBlockConveyor(amount);
		case "MyObjectBuilder_CubeBlock/ArmorCorner"                  : return ArmorCorner(amount);
		case "MyObjectBuilder_CubeBlock/ArmorInvCorner"               : return ArmorInvCorner(amount);
		case "MyObjectBuilder_CubeBlock/ArmorSide"                    : return ArmorSide(amount);
		case "MyObjectBuilder_CubeBlock/ArmorCenter"                  : return ArmorCenter(amount);
		case "MyObjectBuilder_CargoContainer/LargeBlockLargeContainer": return LargeBlockLargeContainer(amount);
		case "MyObjectBuilder_CargoContainer/LargeBlockSmallContainer": return LargeBlockSmallContainer(amount);
		case "MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna"    : return LargeBlockRadioAntenna(amount);
		case "MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock"    : return LargeBlockBatteryBlock(amount);
		case "MyObjectBuilder_WindTurbine/LargeBlockWindTurbine"      : return LargeBlockWindTurbine(amount);
		case "MyObjectBuilder_Gyro/LargeBlockGyro"                    : return LargeBlockGyro(amount);
		case "MyObjectBuilder_CubeBlock/Window3x3Flat"                : return Window3x3Flat(amount);
		case "MyObjectBuilder_Wheel/Wheel5x5"                         : return Wheel5x5(amount);
		case "MyObjectBuilder_MotorSuspension/Suspension5x5"          : return Suspension5x5(amount);
		case "MyObjectBuilder_ExtendedPistonBase/LargePistonBase"     : return LargePistonBase(amount);
		case "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner"   : return LargeBlockArmorRoundCorner(amount); }
		throw new System.ArgumentException("Не знаю такой строки", itemString); }
	static public CRecipe LargePistonBase(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_ExtendedPistonBase/LargePistonBase");
		recipe.addItem(FComponentItem.Computer(2 * amount));
		recipe.addItem(FComponentItem.Motor(4 * amount));
		recipe.addItem(FComponentItem.LargeTube(4 * amount));
		recipe.addItem(FComponentItem.Construction(10 * amount));
		recipe.addItem(FComponentItem.SteelPlate(15 * amount));
		return recipe; }
	static public CRecipe Wheel5x5(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_Wheel/Wheel5x5");
		recipe.addItem(FComponentItem.LargeTube(8 * amount));
		recipe.addItem(FComponentItem.Construction(30 * amount));
		recipe.addItem(FComponentItem.SteelPlate(16 * amount));
		return recipe; }
	static public CRecipe Suspension5x5(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_MotorSuspension/Suspension5x5");
		recipe.addItem(FComponentItem.Motor(20 * amount));
		recipe.addItem(FComponentItem.SmallTube(30 * amount));
		recipe.addItem(FComponentItem.LargeTube(20 * amount));
		recipe.addItem(FComponentItem.Construction(40 * amount));
		recipe.addItem(FComponentItem.SteelPlate(70 * amount));
		return recipe; }
	static public CRecipe Window3x3Flat(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/Window3x3Flat");
		recipe.addItem(FComponentItem.BulletproofGlass(196 * amount));
		recipe.addItem(FComponentItem.Girder(40 * amount));
		return recipe; }
	static public CRecipe LargeBlockGyro(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_Gyro/LargeBlockGyro");
		recipe.addItem(FComponentItem.Computer(5 * amount));
		recipe.addItem(FComponentItem.Motor(4 * amount));
		recipe.addItem(FComponentItem.MetalGrid(50 * amount));
		recipe.addItem(FComponentItem.LargeTube(4 * amount));
		recipe.addItem(FComponentItem.Construction(40 * amount));
		recipe.addItem(FComponentItem.SteelPlate(600 * amount));
		return recipe; }
	static public CRecipe LargeBlockWindTurbine(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_WindTurbine/LargeBlockWindTurbine");
		recipe.addItem(FComponentItem.Computer(2 * amount));
		recipe.addItem(FComponentItem.Girder(24 * amount));
		recipe.addItem(FComponentItem.Construction(20 * amount));
		recipe.addItem(FComponentItem.Motor(8 * amount));
		recipe.addItem(FComponentItem.InteriorPlate(40 * amount));
		return recipe; }
	static public CRecipe LargeBlockBatteryBlock(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock");
		recipe.addItem(FComponentItem.Computer(25 * amount));
		recipe.addItem(FComponentItem.PowerCell(80 * amount));
		recipe.addItem(FComponentItem.Construction(30 * amount));
		recipe.addItem(FComponentItem.SteelPlate(80 * amount));
		return recipe; }
	static public CRecipe LargeBlockRadioAntenna(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna");
		recipe.addItem(FComponentItem.RadioCommunication(40 * amount));
		recipe.addItem(FComponentItem.Computer(8 * amount));
		recipe.addItem(FComponentItem.Construction(30 * amount));
		recipe.addItem(FComponentItem.SmallTube(60 * amount));
		recipe.addItem(FComponentItem.LargeTube(40 * amount));
		recipe.addItem(FComponentItem.SteelPlate(80 * amount));
		return recipe; }
	static public CRecipe LargeBlockLargeContainer(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CargoContainer/LargeBlockLargeContainer");
		recipe.addItem(FComponentItem.Computer(8 * amount));
		recipe.addItem(FComponentItem.Display(1 * amount));
		recipe.addItem(FComponentItem.Motor(20 * amount));
		recipe.addItem(FComponentItem.SmallTube(60 * amount));
		recipe.addItem(FComponentItem.MetalGrid(24 * amount));
		recipe.addItem(FComponentItem.Construction(80 * amount));
		recipe.addItem(FComponentItem.InteriorPlate(360 * amount));
		return recipe; }
	static public CRecipe LargeBlockSmallContainer(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CargoContainer/LargeBlockSmallContainer");
		recipe.addItem(FComponentItem.Computer(2 * amount));
		recipe.addItem(FComponentItem.Display(1 * amount));
		recipe.addItem(FComponentItem.Motor(4 * amount));
		recipe.addItem(FComponentItem.SmallTube(20 * amount));
		recipe.addItem(FComponentItem.MetalGrid(4 * amount));
		recipe.addItem(FComponentItem.Construction(40 * amount));
		recipe.addItem(FComponentItem.InteriorPlate(40 * amount));
		return recipe; }
	static public CRecipe ArmorCorner(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorCorner");
		recipe.addItem(FComponentItem.SteelPlate(135 * amount));
		return recipe; }
	static public CRecipe ArmorInvCorner(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorInvCorner");
		recipe.addItem(FComponentItem.SteelPlate(135 * amount));
		return recipe; }
	static public CRecipe ArmorSide(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorSide");
		recipe.addItem(FComponentItem.SteelPlate(130 * amount));
		return recipe; }
	static public CRecipe ArmorCenter(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/ArmorCenter");
		recipe.addItem(FComponentItem.SteelPlate(140 * amount));
		return recipe; }
	static public CRecipe LargeBlockArmorBlock(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeBlockArmorBlock");
		recipe.addItem(FComponentItem.SteelPlate(25 * amount));
		return recipe; }
	static public CRecipe LargeBlockArmorRoundCorner(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner");
		recipe.addItem(FComponentItem.SteelPlate(13 * amount));
		return recipe; }
	static public CRecipe SmallLight(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_InteriorLight/SmallLight");
		recipe.addItem(FComponentItem.Construction(2 * amount));
		return recipe; }
	static public CRecipe ConveyorTube(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_ConveyorConnector/ConveyorTube");
		recipe.addItem(FComponentItem.Motor(6 * amount));
		recipe.addItem(FComponentItem.SmallTube(12 * amount));
		recipe.addItem(FComponentItem.Construction(20 * amount));
		recipe.addItem(FComponentItem.InteriorPlate(14 * amount));
		return recipe; }
	static public CRecipe LargeShipMergeBlock(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_MergeBlock/LargeShipMergeBlock");
		recipe.addItem(FComponentItem.Computer(2 * amount));
		recipe.addItem(FComponentItem.LargeTube(6 * amount));
		recipe.addItem(FComponentItem.Motor(2 * amount));
		recipe.addItem(FComponentItem.Construction(15 * amount));
		recipe.addItem(FComponentItem.SteelPlate(12 * amount));
		return recipe; }
	static public CRecipe Connector(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_ShipConnector/Connector");
		recipe.addItem(FComponentItem.Computer(20 * amount));
		recipe.addItem(FComponentItem.Motor(8 * amount));
		recipe.addItem(FComponentItem.SmallTube(12 * amount));
		recipe.addItem(FComponentItem.Construction(40 * amount));
		recipe.addItem(FComponentItem.SteelPlate(150 * amount));
		return recipe; }
	static public CRecipe LargeBlockConveyor(int amount = 1) {
		CRecipe recipe = new CRecipe("MyObjectBuilder_Conveyor/LargeBlockConveyor");
		recipe.addItem(FComponentItem.Motor(6 * amount));
		recipe.addItem(FComponentItem.SmallTube(20 * amount));
		recipe.addItem(FComponentItem.Construction(30 * amount));
		recipe.addItem(FComponentItem.InteriorPlate(20 * amount));
		return recipe; } }
public class CRecipes {
	public CRecipes() { m_recipes = new List<CRecipe>(); }
	public void add(CRecipe recipe) { m_recipes.Add(recipe); }
	public List<CRecipe> recipes() { return m_recipes; }
	public List<CComponentItem> sourceItems() {
		Dictionary<EItemType, CComponentItem> tmpDict = new Dictionary<EItemType, CComponentItem>();
		foreach(CRecipe recipe in m_recipes) {
			foreach(CComponentItem srcItem in recipe.sourceItems()) {
				if(!tmpDict.ContainsKey(srcItem.itemType())) {
					tmpDict.Add(srcItem.itemType(), new CComponentItem(srcItem.itemType(), 0)); }
				tmpDict[srcItem.itemType()].appendAmount(srcItem.amount()); } }
		return tmpDict.Values.ToList(); }
	private List<CRecipe> m_recipes; }
public enum EItemType {
	BulletproofGlass,
	Canvas,
	Computer,
	Construction,
	Detector,
	Display,
	Explosives,
	Girder,
	GravityGenerator,
	InteriorPlate,
	LargeTube,
	Medical,
	MetalGrid,
	Motor,
	PowerCell,
	RadioCommunication,
	Reactor,
	SmallTube,
	SolarCell,
	SteelPlate,
	Superconductor,
	Thrust,
	ZoneChip }
public class CComponentItem {
	public CComponentItem(string    itemType)                 { m_itemType = fromString(itemType); m_amount =      0; }
	public CComponentItem(EItemType itemType, int amount = 0) { m_itemType = itemType            ; m_amount = amount; }
	public static EItemType fromString(string itemType) {
		if(itemType.Contains("BulletproofGlass"))   { return EItemType.BulletproofGlass; }
		else if(itemType.Contains("Canvas"))             { return EItemType.Canvas; }
		else if(itemType.Contains("Computer"))           { return EItemType.Computer; }
		else if(itemType.Contains("Construction"))       { return EItemType.Construction; }
		else if(itemType.Contains("Detector"))           { return EItemType.Detector; }
		else if(itemType.Contains("Display"))            { return EItemType.Display; }
		else if(itemType.Contains("Explosives"))         { return EItemType.Explosives; }
		else if(itemType.Contains("Girder"))             { return EItemType.Girder; }
		else if(itemType.Contains("GravityGenerator"))   { return EItemType.GravityGenerator; }
		else if(itemType.Contains("InteriorPlate"))      { return EItemType.InteriorPlate; }
		else if(itemType.Contains("LargeTube"))          { return EItemType.LargeTube; }
		else if(itemType.Contains("Medical"))            { return EItemType.Medical; }
		else if(itemType.Contains("MetalGrid"))          { return EItemType.MetalGrid; }
		else if(itemType.Contains("Motor"))              { return EItemType.Motor; }
		else if(itemType.Contains("PowerCell"))          { return EItemType.PowerCell; }
		else if(itemType.Contains("RadioCommunication")) { return EItemType.RadioCommunication; }
		else if(itemType.Contains("Reactor"))            { return EItemType.Reactor; }
		else if(itemType.Contains("SmallTube"))          { return EItemType.SmallTube; }
		else if(itemType.Contains("SolarCell"))          { return EItemType.SolarCell; }
		else if(itemType.Contains("SteelPlate"))         { return EItemType.SteelPlate; }
		else if(itemType.Contains("Superconductor"))     { return EItemType.Superconductor; }
		else if(itemType.Contains("Thrust"))             { return EItemType.Thrust; }
		else if(itemType.Contains("ZoneChip"))           { return EItemType.ZoneChip; }
		throw new System.ArgumentException("Не знаю такой строки", itemType); }
	public int amount() { return m_amount; }
	public void appendAmount(int amountDelta) { m_amount += amountDelta; }
	public EItemType itemType() { return m_itemType; }
	public string asComponent() {
		string name = "";
		switch(m_itemType) {
		case EItemType.BulletproofGlass:   name = "BulletproofGlass";   break;
		case EItemType.Canvas:             name = "Canvas";             break;
		case EItemType.Computer:           name = "Computer";           break;
		case EItemType.Construction:       name = "Construction";       break;
		case EItemType.Detector:           name = "Detector";           break;
		case EItemType.Display:            name = "Display";            break;
		case EItemType.Explosives:         name = "Explosives";         break;
		case EItemType.Girder:             name = "Girder";             break;
		case EItemType.GravityGenerator:   name = "GravityGenerator";   break;
		case EItemType.InteriorPlate:      name = "InteriorPlate";      break;
		case EItemType.LargeTube:          name = "LargeTube";          break;
		case EItemType.Medical:            name = "Medical";            break;
		case EItemType.MetalGrid:          name = "MetalGrid";          break;
		case EItemType.Motor:              name = "Motor";              break;
		case EItemType.PowerCell:          name = "PowerCell";          break;
		case EItemType.RadioCommunication: name = "RadioCommunication"; break;
		case EItemType.Reactor:            name = "Reactor";            break;
		case EItemType.SmallTube:          name = "SmallTube";          break;
		case EItemType.SolarCell:          name = "SolarCell";          break;
		case EItemType.SteelPlate:         name = "SteelPlate";         break;
		case EItemType.Superconductor:     name = "Superconductor";     break;
		case EItemType.Thrust:             name = "Thrust";             break;
		case EItemType.ZoneChip:           name = "ZoneChip";           break; }
		return $"MyObjectBuilder_Component/{name}"; }
	public string asBlueprintDefinition() {
		string name = "";
		switch(m_itemType) {
		case EItemType.BulletproofGlass:   name = "BulletproofGlass";            break;
		case EItemType.Canvas:             name = "Canvas";                      break;
		case EItemType.Computer:           name = "ComputerComponent";           break;
		case EItemType.Construction:       name = "ConstructionComponent";       break;
		case EItemType.Detector:           name = "DetectorComponent";           break;
		case EItemType.Display:            name = "Display";                     break;
		case EItemType.Explosives:         name = "ExplosivesComponent";         break;
		case EItemType.Girder:             name = "GirderComponent";             break;
		case EItemType.GravityGenerator:   name = "GravityGeneratorComponent";   break;
		case EItemType.InteriorPlate:      name = "InteriorPlate";               break;
		case EItemType.LargeTube:          name = "LargeTube";                   break;
		case EItemType.Medical:            name = "MedicalComponent";            break;
		case EItemType.MetalGrid:          name = "MetalGrid";                   break;
		case EItemType.Motor:              name = "MotorComponent";              break;
		case EItemType.PowerCell:          name = "PowerCell";                   break;
		case EItemType.RadioCommunication: name = "RadioCommunicationComponent"; break;
		case EItemType.Reactor:            name = "ReactorComponent";            break;
		case EItemType.SmallTube:          name = "SmallTube";                   break;
		case EItemType.SolarCell:          name = "SolarCell";                   break;
		case EItemType.SteelPlate:         name = "SteelPlate";                  break;
		case EItemType.Superconductor:     name = "Superconductor";              break;
		case EItemType.Thrust:             name = "ThrustComponent";             break;
		case EItemType.ZoneChip:           name = "ZoneChip";                    break; }
		return $"MyObjectBuilder_BlueprintDefinition/{name}"; }
	public MyItemType asMyItemType() { return MyItemType.Parse(asComponent()); }
	private EItemType m_itemType;
	private int m_amount; }
public class FComponentItem {
	static public CComponentItem BulletproofGlass(int amount = 0) { return new CComponentItem(EItemType.BulletproofGlass,   amount); }
	static public CComponentItem Canvas(int amount = 0) { return new CComponentItem(EItemType.Canvas,             amount); }
	static public CComponentItem Computer(int amount = 0) { return new CComponentItem(EItemType.Computer,           amount); }
	static public CComponentItem Construction(int amount = 0) { return new CComponentItem(EItemType.Construction,       amount); }
	static public CComponentItem Detector(int amount = 0) { return new CComponentItem(EItemType.Detector,           amount); }
	static public CComponentItem Display(int amount = 0) { return new CComponentItem(EItemType.Display,            amount); }
	static public CComponentItem Explosives(int amount = 0) { return new CComponentItem(EItemType.Explosives,         amount); }
	static public CComponentItem Girder(int amount = 0) { return new CComponentItem(EItemType.Girder,             amount); }
	static public CComponentItem GravityGenerator(int amount = 0) { return new CComponentItem(EItemType.GravityGenerator,   amount); }
	static public CComponentItem InteriorPlate(int amount = 0) { return new CComponentItem(EItemType.InteriorPlate,      amount); }
	static public CComponentItem LargeTube(int amount = 0) { return new CComponentItem(EItemType.LargeTube,          amount); }
	static public CComponentItem Medical(int amount = 0) { return new CComponentItem(EItemType.Medical,            amount); }
	static public CComponentItem MetalGrid(int amount = 0) { return new CComponentItem(EItemType.MetalGrid,          amount); }
	static public CComponentItem Motor(int amount = 0) { return new CComponentItem(EItemType.Motor,              amount); }
	static public CComponentItem PowerCell(int amount = 0) { return new CComponentItem(EItemType.PowerCell,          amount); }
	static public CComponentItem RadioCommunication(int amount = 0) { return new CComponentItem(EItemType.RadioCommunication, amount); }
	static public CComponentItem Reactor(int amount = 0) { return new CComponentItem(EItemType.Reactor,            amount); }
	static public CComponentItem SmallTube(int amount = 0) { return new CComponentItem(EItemType.SmallTube,          amount); }
	static public CComponentItem SolarCell(int amount = 0) { return new CComponentItem(EItemType.SolarCell,          amount); }
	static public CComponentItem SteelPlate(int amount = 0) { return new CComponentItem(EItemType.SteelPlate,         amount); }
	static public CComponentItem Superconductor(int amount = 0) { return new CComponentItem(EItemType.Superconductor,     amount); }
	static public CComponentItem Thrust(int amount = 0) { return new CComponentItem(EItemType.Thrust,             amount); }
	static public CComponentItem ZoneChip(int amount = 0) { return new CComponentItem(EItemType.ZoneChip,           amount); } }
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
public class CStoragesGroup : CBlockGroup<IMyCargoContainer> {
	public CStoragesGroup(string groupName,
						  string purpose = "") : base(groupName, purpose)
	{}
	public int countItems(EItemType itemType) {
		CComponentItem result = new CComponentItem(itemType);
		MyItemType miType = result.asMyItemType();
		foreach(IMyCargoContainer container in blocks()) {
			result.appendAmount(container.GetInventory().GetItemAmount(miType).ToIntSafe()); }
		return result.amount(); } }
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
CRecipes recipes;
CDisplay lcd;
CStoragesGroup storage;
IMyAssembler targetAssembler;
public string program() {
	lcd = new CDisplay();
	lcd.addDisplay("[Земля] Дисплей производства", 0, 0);
	targetAssembler = GridTerminalSystem.GetBlockWithName("[Земля] Master Сборщик 0") as IMyAssembler;
	storage = new CStoragesGroup("[Земля] БК Компоненты", "Компоненты");
	recipes = new CRecipes();
	recipes.add(FRecipe.LargeBlockArmorBlock(1000 * 4));
	recipes.add(FRecipe.Window3x3Flat(8));
	recipes.add(FRecipe.ArmorSide(64 * 16));
	recipes.add(FRecipe.ArmorCenter(512 * 16));
	recipes.add(FRecipe.LargeBlockArmorRoundCorner(32 * 16));
	recipes.add(FRecipe.LargeBlockRadioAntenna(4));
	recipes.add(FRecipe.SmallLight(16 + 2*4));
	recipes.add(FRecipe.LargeShipMergeBlock(4 + 16*2 + 4*2));
	recipes.add(FRecipe.LargePistonBase(8));
	recipes.add(FRecipe.LargeBlockGyro(16));
	recipes.add(FRecipe.LargeBlockWindTurbine(32));
	recipes.add(FRecipe.LargeBlockBatteryBlock(32));
	recipes.add(FRecipe.LargeBlockSmallContainer(16));
	recipes.add(FRecipe.LargeBlockLargeContainer(16));
	recipes.add(FRecipe.Connector(8));
	recipes.add(FRecipe.ConveyorTube(32));
	recipes.add(FRecipe.LargeBlockConveyor(32));
	recipes.add(FRecipe.Wheel5x5(4));
	recipes.add(FRecipe.Suspension5x5(4));
	return "Планирование производства"; }
public void main(string argument, UpdateType updateSource) {
	bool assemblerProducing = targetAssembler.IsProducing;
	string state = assemblerProducing ? "Producing" : "Stopped";
	lcd.echo_at($"Assemblesr state: {state}", 0);
	lcd.echo_at("---", 1);
	int lcdIndex = 2;
	foreach(var component in recipes.sourceItems()) {
		int inStorageAmount = storage.countItems(component.itemType());
		int needAmount = component.amount();
		int amount = needAmount - inStorageAmount;
		lcd.echo_at($"{component.itemType().ToString()}: {inStorageAmount} of {needAmount}", lcdIndex); lcdIndex++;
		if(amount > 0 && !assemblerProducing) {
			targetAssembler.AddQueueItem(
				MyDefinitionId.Parse(component.asBlueprintDefinition()),
				(double)amount); } } }
