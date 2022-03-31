// #include classes/main.cs
IMyTextPanel display;
IMyTextSurface _drawingSurface;
RectangleF _viewport;
List<string> sprts;

public string program()
{
  sprts = new List<string>();
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
      // Me is the programmable block which is running this script.
    // Retrieve the Large Display, which is the first surface
    display = GridTerminalSystem.GetBlockWithName("[Земля] Дисплей Test 03") as IMyTextPanel;
    _drawingSurface = display as IMyTextSurface;
    _drawingSurface.GetSprites(sprts);
    foreach(string spr in sprts) { debug(spr); }
    // Calculate the viewport offset by centering the surface size onto the texture size
    _viewport = new RectangleF(
        (_drawingSurface.TextureSize - _drawingSurface.SurfaceSize) / 2f,
        _drawingSurface.SurfaceSize
    );

  return "Тест спрайтов";
}

public void main(string argument, UpdateType updateSource)
{
    // Begin a new frame
    var frame = _drawingSurface.DrawFrame();

    // All sprites must be added to the frame here
    DrawSprites(ref frame);

    // We are done with the frame, send all the sprites to the text panel
    frame.Dispose();
}

// Drawing Sprites
public void DrawSprites(ref MySpriteDrawFrame frame)
{
    // Create background sprite
    var sprite = new MySprite()
    {
        Type = SpriteType.TEXTURE,
        Data = "Grid",
        Position = _viewport.Center,
        Size = _viewport.Size,
        Color = Color.White.Alpha(0.66f),
        Alignment = TextAlignment.CENTER
    };
    // Add the sprite to the frame
    frame.Add(sprite);

    // Set up the initial position - and remember to add our viewport offset
    var position = new Vector2(256, 20) + _viewport.Position;

    // Create our first line
    sprite = new MySprite()
    {
        Type = SpriteType.TEXT,
        Data = "Line 1",
        Position = position,
        RotationOrScale = 0.8f /* 80 % of the font's default size */,
        Color = Color.Red,
        Alignment = TextAlignment.CENTER /* Center the text on the position */,
        FontId = "White"
    };
    // Add the sprite to the frame
    frame.Add(sprite);

    // Move our position 20 pixels down in the viewport for the next line
    position += new Vector2(0, 20);

    // Create our second line, we'll just reuse our previous sprite variable - this is not necessary, just
    // a simplification in this case.
    sprite = new MySprite()
    {
        Type = SpriteType.TEXT,
        Data = "Line 2",
        Position = position,
        RotationOrScale = 0.8f,
        Color = Color.Blue,
        Alignment = TextAlignment.CENTER,
        FontId = "White"
    };
    // Add the sprite to the frame
    frame.Add(sprite);
}
