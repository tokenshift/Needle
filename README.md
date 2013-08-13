# Needle
_Simple, lightweight and portable dependecy injection for .NET._

**Needle** is a portable class library providing basic dependency injection
for .NET projects that want to target multiple platforms, including WinRT,
Xbox 360, and Windows Phone (7 or higher). It doesn't handle the more complex
use cases provided by something like [Ninject](http://www.ninject.org/), but
is extremely simple to use and has no platform-specific dependencies.

## Use

Create a `Kernel` instance (or use the singleton `Kernel.Current`). Call 
`kernel.For<TDependency>().Provide<TImplementation>()` to register `TImplementation`
as the concrete implementation of type `TDependency`; in general, `TDependency`
will be an interface, though the only requirement is that `TImplementation` inherit
from it. Call `kernel.Get<TDependency>()` to retrieve an instance of the registered
type.

To control how instances are provided, pass the `mode` parameter to `Provide`; e.g.
`kernel.For<IFoo>().Provide<Foo>(Mode.Singleton)`. Supported modes are:

* **Instance** - The default mode; a new instance is constructed each time the
dependency is requested.
* **Singleton** - A single shared instance is provided to all requestors.
* **Thread** - Instance-per-thread, or thread singleton; a new instance is created
for each thread that requests the dependency.
  
## Limitations

Code generation is not permitted on many of the supported platforms, so **Needle**
CANNOT generate constructor injection code. As a result, every concrete implementation
must have a public parameterless constructor that **Needle** can call, and needs to
call `kernel.Get...` for any of its own dependencies. (I guess that makes this service
location rather than dependency injection? Oh well.)

**Needle** provides very little in the way of lifecycle management. Singletons and
thread singletons are maintained as weak references to help avoid memory leaks, so if
a dependency is garbage collected, a new instance will need to be constructed. Future
updates may make this configurable at dependency registration; e.g.
`kernel.For<IFoo>().Provide<Foo>(Mode.PersistentSingleton)`. Unfortunately there isn't
any platform-independent way to determine whether a specific thread is still alive, so
there will not be any more advanced instance pooling for thread singletons (if I'm wrong
about this--pull request!).