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
CDisplay lcd;
public string program() {
	lcd = new CDisplay();
	lcd.addDisplay("[Бур] Дисплей Статус 0", 0, 0);
	lcd.addDisplay("[Бур] Дисплей Статус 1", 1, 0);
	lcd.addDisplay("[Бур] Дисплей Статус 2", 2, 0);
	return "Тестирование дисплеев"; }
public void main(string argument, UpdateType updateSource) {
	for(int i = 0; i < 100; i++) {
		lcd.echo($"{i.ToString("0000")} 678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
		lcd.echo($"{i.ToString("0000")} 10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200"); } }
