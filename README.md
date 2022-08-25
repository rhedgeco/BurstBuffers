# Burst Buffers 🚀
Burst buffers is a collection of objects made for use within the Unity Engine. It's purpose is to serve as a ***relatively*** safe interface to create and manipulate native buffers.

### NativeBoxes
- `NativeBox<T>` - An abstract class for all other boxes to inherit from.
- `BufferBox<T>` - A safe wrapper for the `BurstArray<T>` type.
- `StructBox<T>` - A safe wrapper for any `struct` to be allocated in native space.

All native structures do not have exposed constructors and must be created using their respective `NativeBox`. It serves as a safe window into the underlying unsafe data, allowing it to be used in the project without enabling `unsafe` code site-wide.

### Using with `[BurstCompile]`
To be passed into burst compiled methods we gotta get just a little **DANGEROUS**.

The `NativeBox<T>` base class exposes the data as an reference using `.GetUnsafeRef()`. This returns a `ref` struct of the underlying data. While this can be used to pass around the data without needing `unsafe`, it should be done sparingly and should usually only be for sending the data into a burst compiled methods.

The `ref` is literally just a pointer converted to a ref, and can still produce undefined behaviour if the source data is disposed of. So be careful.

## Installation
Go to the [latest release](releases/latest).

Download and import the `.unitypackage` into your project.

## [License](LICENSE.md)