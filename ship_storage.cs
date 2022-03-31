// #include classes/main.cs
// #include classes/storage_info.cs

StorageInfo sto;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  sto = new StorageInfo(structureName, "Хранилище");
  return "Статус контейнеров";
}

public void main(string argument, UpdateType updateSource)
{
  sto.update();
}
