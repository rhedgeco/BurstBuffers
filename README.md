# Burst Buffers 🚀
Burst buffers is a collection of objects made for use within the Unity Engine. It's purpose is to serve as a ***relatively*** safe interface to create and manipulate native buffers.

### NativeBoxes
- `NativeBox<T>` - An abstract class for all other boxes to inherit from.
- `BufferBox<T>` - A safe wrapper for the `BurstArray<T>` type.
- `StructBox<T>` - A safe wrapper for any `struct` to be allocated in native space.

All native structures do not have exposed constructors and must be created using their respective `NativeBox`. It serves as a safe window into the underlying unsafe data, allowing it to be used in the project without enabling `unsafe` code site-wide.

### Unsafe usages with `[BurstCompile]`
To be passed into burst compiled methods we gotta get just a little **DANGEROUS**.

The `NativeBox<T>` base class exposes the data as an reference using `.GetUnsafeRef()`. This returns a `ref` struct of the underlying data. While this can be used to pass around the data without needing `unsafe`, it should be done sparingly and should usually only be for sending the data into a burst compiled methods.

The `ref` is literally just a pointer converted to a ref, and can still produce undefined behaviour if the source data is disposed of. So be careful.

## Installation
Go to the [latest release](https://github.com/rhedgeco/BurstBuffers/releases/latest).

Download and import the `.unitypackage` into your project.

## Usages
Buffers can be used like normal arrays outside of burst code

### Outside Burst Code
Create a Buffer:
```csharp
// creates a new native float array of length 20
BufferBox<float> buffer = new BufferBox(20);
```

Index a buffer:
```csharp
// buffers can be indexed like a normal array
float value = buffer[15];
```

Enumerate a buffer:
```csharp
// Buffers can be used in a foreach loop with no modification
foreach (float value in buffer)
{
    Debug.Log(value);
}
```

Copy buffers to and from managed arrays or other buffers:
```csharp
BufferBox<float> buffer1 = new BufferBox(20);
BufferBox<float> buffer2 = new BufferBox(20);
buffer1.CopyTo(buffer2);

// buffer types dont have to be the same. 
// This allows for mapping memory to other types like float to (float,float)
// or float to int if youre strange like that ;)
(float,float)[] floatTupleArray = new (float,float)[20];
buffer1.CopyTo(floatTupleArray);
buffer2.CopyFrom(floatTupleArray);
```

Clear the buffer:
```csharp
// zeroes all data in the buffer
// if buffer contains native resources, will dispose them as it clears as well
// so they arent left to leak
buffer.Clear();
```

### With Burst Code

Pass an unsafe reference into a burst function:
```csharp
BufferBox<float> buffer = new BufferBox(20);
BurstFunctionCall(ref buffer.GetUnsafeRef());
```

Indexing and enumeration:
```csharp
[BurstCompile]
public static void BurstFunctionProcess(ref BurstArray<float> array)
{
    // indexing the raw array returns references to the value
    ref float valueRef = ref array[15];
    
    // enumeration can happen as expected
    foreach (float value in array)
    {
        // do something with value
    }
    
    // can also enumerate values as reference
    foreach (ref float value in array.GetRefEnumerator())
    {
        // assigning to ref value overwrites it in the original buffer
        value = 5;
    }
}
```

Methods identical to their BufferBox counterparts (burst box just directly calls these):
`CopyTo`/`CopyFrom`/`Clear`

## [License](LICENSE.md)