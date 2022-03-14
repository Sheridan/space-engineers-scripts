// #include classes/main.cs
// #include classes/storage_info.cs

StorageInfo storage;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  storage   = new StorageInfo("МК", "Хранилище");
  return "Статус хранилищ";
}

public void main(string argument, UpdateType updateSource)
{
  storage  .update();
}
