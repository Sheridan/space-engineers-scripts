static string sN;
static string scriptName;
static Program _;
static float blockSize;
static CBO prbOptions;
public void applyDefaultMeDisplayTexsts() {
Me.GetSurface(0).WriteText(scriptName.Replace(" ", "\n"));
Me.GetSurface(1).WriteText(sN); }
public void echoMe(string text, int surface) { Me.GetSurface(surface).WriteText(text, false); }
public void echoMeBig(string text) { echoMe(text, 0); }
public void echoMeSmall(string text) { echoMe(text, 1); }
public void sMe(string i_scriptName) {
scriptName = i_scriptName;
Me.CustomName = $"[{sN}] ПрБ {scriptName}";
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
sN = Me.CubeGrid.CustomName;
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
IMyTextPanel display;
IMyTextSurface _drawingSurface;
RectangleF _viewport;
List<string> sprts;
public string program() {
sprts = new List<string>();
Runtime.UpdateFrequency = UpdateFrequency.Update100;
display = GridTerminalSystem.GetBlockWithName("[Земля] Дисплей Test 03") as IMyTextPanel;
_drawingSurface = display as IMyTextSurface;
_drawingSurface.GetSprites(sprts);
foreach(string spr in sprts) { debug(spr); }
_viewport = new RectangleF(
(_drawingSurface.TextureSize - _drawingSurface.SurfaceSize) / 2f,
_drawingSurface.SurfaceSize
);
return "Тест спрайтов"; }
public void main(string argument, UpdateType updateSource) {
var frame = _drawingSurface.DrawFrame();
DrawSprites(ref frame);
frame.Dispose(); }
public void DrawSprites(ref MySpriteDrawFrame frame) {
var sprite = new MySprite() {
Type = SpriteType.TEXTURE,
Data = "Grid",
Position = _viewport.Center,
Size = _viewport.Size,
Color = Color.White.Alpha(0.66f),
Alignment = TextAlignment.CENTER };
frame.Add(sprite);
var position = new Vector2(256, 20) + _viewport.Position;
sprite = new MySprite() {
Type = SpriteType.TEXT,
Data = "Line 1",
Position = position,
RotationOrScale = 0.8f /* 80 % of the font's default size */,
Color = Color.Red,
Alignment = TextAlignment.CENTER /* Center the text on the position */,
FontId = "White" };
frame.Add(sprite);
position += new Vector2(0, 20);
sprite = new MySprite() {
Type = SpriteType.TEXT,
Data = "Line 2",
Position = position,
RotationOrScale = 0.8f,
Color = Color.Blue,
Alignment = TextAlignment.CENTER,
FontId = "White" };
frame.Add(sprite); }
