![logo](https://raw.githubusercontent.com/Jac21/BlockingCollectionExtensions/master/media/logo.png)

[![NuGet Status](http://img.shields.io/nuget/v/BlockingCollectionExtensions.svg?style=flat)](https://www.nuget.org/packages/BlockingCollectionExtensions/)
[![MIT Licence](https://badges.frapsoft.com/os/mit/mit.svg?v=103)](https://opensource.org/licenses/mit-license.php)
[![Build Status](https://app.travis-ci.com/Jac21/BlockingCollectionExtensions.svg?token=C2g4zraa9aMphS3q4ssZ&branch=master)](https://app.travis-ci.com/Jac21/BlockingCollectionExtensions)
[![donate](https://img.shields.io/badge/%24-Buy%20me%20a%20coffee-ff69b4.svg?style=flat)](https://www.buymeacoffee.com/jac21) 

📎 Utilities to aid in utilizing the ever-useful .NET structure BlockingCollection&lt;T>

## Installation

Find it on [nuget](https://www.nuget.org/packages/BlockingCollectionExtensions/)!

```
PM> Install-Package BlockingCollectionExtensions -Version 6.0.1
```

## API 

### Static Additive Utilities

```csharp
/// <summary>
/// Transfer contents of a generic enumerable into a target blocking collection,
/// determine whether blocking collection should complete adding when enumerable has finished being added
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="target"></param>
/// <param name="source"></param>
/// <param name="completeAddingWhenDone"></param>
public static void AddFromEnumerable<T>(this BlockingCollection<T> target, IEnumerable<T> source,
    bool completeAddingWhenDone);
    
```

### Static Transformative Utilities

```csharp
/// <summary>
/// Coalesce a target blocking collection to a structure that implements the IProducerConsumerCollection interface
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="collection"></param>
/// <param name="millisecondsTimeout"></param>
/// <param name="cancellationToken"></param>
/// <param name="count"></param>
/// <param name="isSynchronized"></param>
/// <param name="syncRoot"></param>
/// <returns></returns>
public static IProducerConsumerCollection<T> ToProducerConsumerCollection<T>(
    this BlockingCollection<T> collection, int millisecondsTimeout, CancellationToken cancellationToken,
    int count, bool isSynchronized, object syncRoot);
```
