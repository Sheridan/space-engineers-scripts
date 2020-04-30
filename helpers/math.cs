public static double angleBetweenVectors(Vector3D a, Vector3D b)
{
  // Vector3D A = Vector3D.Normalize(a);
  // Vector3D B = Vector3D.Normalize(b);
  // return MathHelper.ToDegrees(Math.Acos(A.Dot(B) / (A.Length() * B.Length())));
  return MathHelper.ToDegrees(Math.Acos(a.Dot(b) / (a.Length() * b.Length())));
  // return (Math.Acos(a.Dot(b) / (a.Length() * b.Length()))) * (180 / (float)Math.PI);
}

// public static MatrixD gridTWorldTransformMatrix(IMyCubeGrid grid)
// {
//   // Vector3D origin = grid.GridIntegerToWorld(Vector3I.Zero);
//   // Vector3D up = grid.GridIntegerToWorld(Vector3I.Up) - origin;
//   // Vector3D backward = grid.GridIntegerToWorld(Vector3I.Backward) - origin;
//   // return MatrixD.CreateScale(grid.GridSize) * MatrixD.CreateWorld(origin, -backward, up);
//   return MatrixD.CreateScale(grid.GridSize) *
//          MatrixD.CreateWorld(grid.GridIntegerToWorld(Vector3I.Zero),
//                              grid.GridIntegerToWorld(Vector3I.Forward),
//                              grid.GridIntegerToWorld(Vector3I.Up));
// }

// public static MatrixD block2WorldTransformMatrix(IMyCubeBlock block)
// {
//   Matrix blockToGridMatrix;
//   block.Orientation.GetMatrix(out blockToGridMatrix);
//   return blockToGridMatrix *
//          MatrixD.CreateTranslation(((Vector3D)new Vector3D(blk.Min + blk.Max)) / 2.0) *
//          GetGrid2WorldTransform(blk.CubeGrid);
// }
