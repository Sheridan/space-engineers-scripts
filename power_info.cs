// #include classes/main.cs
// #include classes/power_info.cs
CPowerInfo pi;
public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  pi = new CPowerInfo("Генерация", "Потребление");
  return "Статус энергосети";
}

public void main(string argument, UpdateType updateSource)
{
  pi.update();
}
