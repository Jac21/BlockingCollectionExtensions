![logo](https://raw.githubusercontent.com/Jac21/BlockingCollectionExtensions/master/media/logo.png)

[![NuGet Status](http://img.shields.io/nuget/v/BlockingCollectionExtensions.svg?style=flat)](https://www.nuget.org/packages/BlockingCollectionExtensions/)
[![MIT Licence](https://badges.frapsoft.com/os/mit/mit.svg?v=103)](https://opensource.org/licenses/mit-license.php)
[![CI](https://github.com/Jac21/BlockingCollectionExtensions/actions/workflows/ci.yml/badge.svg)](https://github.com/Jac21/BlockingCollectionExtensions/actions/workflows/ci.yml)
[![donate](https://img.shields.io/badge/%24-Buy%20me%20a%20coffee-ff69b4.svg?style=flat)](https://www.buymeacoffee.com/jac21) 

Helpers and adapters for working with `BlockingCollection<T>` in modern .NET applications.

## Installation

Find it on [nuget](https://www.nuget.org/packages/BlockingCollectionExtensions/)!

```powershell
PM> Install-Package BlockingCollectionExtensions -Version 7.0.0
```

## What's New

- Simpler `IProducerConsumerCollection<T>` adapters with better defaults
- `TimeSpan`-based overloads and optional cancellation tokens
- `IAsyncEnumerable<T>` support for async-first producer pipelines
- Modernized package metadata, symbols, readme packaging, and CI

## Usage

### Add from an enumerable

```csharp
using System.Collections.Concurrent;
using BlockingCollectionExtensions;

var collection = new BlockingCollection<int>();
collection.AddFromEnumerable(new[] { 1, 2, 3 }, completeAddingWhenDone: true);
```

### Add from an async enumerable

```csharp
using System.Collections.Concurrent;
using BlockingCollectionExtensions;

var collection = new BlockingCollection<int>();

await collection.AddFromAsyncEnumerable(GetItemsAsync(), completeAddingWhenDone: true);

static async IAsyncEnumerable<int> GetItemsAsync()
{
    yield return 1;
    await Task.Yield();
    yield return 2;
}
```

### Adapt to `IProducerConsumerCollection<T>`

```csharp
using System.Collections.Concurrent;
using BlockingCollectionExtensions;

var collection = new BlockingCollection<int>();
var adapter = collection.AsProducerConsumerCollection(TimeSpan.FromMilliseconds(250));

adapter.TryAdd(42);
adapter.TryTake(out var value);
```

## Target Frameworks

- `net8.0`
- `netstandard2.1`

## Notes

- `BlockingCollection<T>` is still useful for some producer/consumer scenarios, especially when interop with older code matters.
- For brand new async-heavy pipelines, `System.Threading.Channels` may still be the better default choice.
