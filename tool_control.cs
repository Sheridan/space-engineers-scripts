// #include classes/main.cs
// #include parts/tools_control/tools_control.cs

public string program()
{
  toolsInit(null);
  return "Контроль инструмента";
}

public void main(string argument, UpdateType updateSource)
{
  toolsArgumentsParse(argument);
}
