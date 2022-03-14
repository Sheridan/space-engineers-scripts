// #include classes/main.cs
// #include classes/storage_info.cs

StorageInfo ore;
StorageInfo ice;
StorageInfo ignot;
StorageInfo comp;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  ore   = new StorageInfo("К Руда"      , "Хранилище руды");
  ice   = new StorageInfo("К Лёд"       , "Хранилище льда");
  ignot = new StorageInfo("К Слитки"    , "Хранилище слитков");
  comp  = new StorageInfo("К Компоненты", "Хранилище компонентов");
  return "Статус хранилищ";
}

public void main(string argument, UpdateType updateSource)
{
  ore  .update();
  ice  .update();
  ignot.update();
  comp .update();
}
