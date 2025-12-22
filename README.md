## About Me

I am a **Mobile Application Developer** with **15+ years of professional experience** building high-quality, scalable Native/Cross-platform iOS and Android mobile applications.
This repository reflects my approach to designing **robust, testable, and scalable** mobile solutions.

**Contact:**  
📧 Email: khasanrah@gmail.com  

**Available for hire:**  
💼 Upwork: https://www.upwork.com/freelancers/khasanr


---

## Project Overview – MoviesDemo (.NET Native – Fragment / UIKit)

This project demonstrates how to build **fully native iOS and Android applications** using **.NET Native**, while applying proven architectural patterns such as **MVVM**, **ViewModel-driven navigation**, **Domain-Driven Design (DDD)**, and **Dependency Injection (DI)**. Additionally, each application layer is instrumented with logging, providing clear traceability across UI, ViewModel, Service, and Domain layers, which improves maintainability, debugging, and long-term support.

The demo shows how:
- Native iOS and Android apps can share **most of the business logic**
- UI layers remain **100% native** and platform-optimized
- **MVVM** can be applied consistently across platforms
- Navigation can be driven entirely from **ViewModels**
- **Domain-Driven Design** structures the core business logic
- Services remain fully abstract and wired via **Dependency Injection**
- The architecture naturally supports **Unit Tests** and **Integration Tests**

The goal of this project is to demonstrate my experience in creating **beautiful**, **clear**, and **maintainable** mobile applications that can **scale to large, long-living products** without architectural bottlenecks.

### Other Implementations

- **MoviesDemo (KMP – Fragment / UIKit)**  
  https://github.com/xusan/MoviesKmpSimplified

- **MoviesDemo (KMP – Jetpack Compose / SwiftUI)**  
  https://github.com/xusan/movieskmpcompose
  
- **MoviesDemo (Swift – SwiftUI)**  
  https://github.com/devperson/MoviesSwift
  
> All *MoviesDemo* implementations have **identical domain models, architecture, and features**.
> The repositories differ only in **platform technology and UI framework**, demonstrating how the same
> core architecture can support multiple native UI approaches without changes to the business layer.
  
---

## Application Overview

This demo mobile application targets **iOS** and **Android** using **.NET Native iOS/Android**.

- ~**70% shared code**
- ~**15% native UI code per platform**
- Fully **native** UI on each platform
- Clear separation between UI, ViewModels, Services, and Domain logic

The project demonstrates how to combine **native performance** with **high code reuse** and **clean architecture**.

---

## Application Features

- Fetches movies list from server
- Caches data in local storage
- Loads cached data on app restart
- Pull-to-refresh reloads data from server and updates cache
- Add new movie:
  - Name
  - Description
  - Photo (camera or gallery)
- Update movie
- Delete movie

## Screenshots

| iOS | Android |
|-----|---------|
| ![iOS Demo](assets/iosDemoApp.gif) | ![Android Demo](assets/androidDemo.gif) |

---

## Architecture Overview

High-level layering:

```
UI Layer (Native iOS / Native Android)
        ↓
ViewModels (Shared)
        ↓
Service Layer (Shared)
        ↓
Domain Model (Shared)
        ↓
Infrastructure Services (Shared)
```

---

## UI Layer (Native)

The UI layer is implemented using **fully native frameworks** on each platform and follows the **MVVM pattern** with custom data binding.

### Android
- Native XML layouts
- MVVM pattern
- Navigation based on **FragmentManager**
- Lifecycle handling based on native Fragment / Activity callbacks

### iOS
- Native Swift
- UIKit + AsyncDisplayKit (Texture)
- MVVM pattern
- Navigation based on **UINavigationController**
- Lifecycle handling based on native UIViewController lifecycle

UI responsibility:
- Rendering
- User interaction
- Navigation
- Binding to ViewModels

No business logic is implemented in the UI layer.

---

## 🧠ViewModel Layer (Shared)

- Contains most application use-case logic
- Fully platform-agnostic
- Implements MVVM pattern
- Uses interfaces to communicate with services
- Designed using Object-Oriented principles
- Fully unit-tested

---

## 🔧Service Layer (Shared)

The service layer is designed using **Domain-Driven Design** and common enterprise patterns such as **Facade** and **Decorator**.

All services are:
- Fully abstract
- Platform-independent
- Injected via **Dependency Injection**

### Contains:
- Domains
- Domain Services
- Application Services
- Infrastructure Services

---

## 🧪Unit & Integration Testing

The project includes a comprehensive test suite:

1. **ViewModel Unit Tests**
   - Test use-case logic with mocked services

2. **Application Services Unit Tests**
   - Validate business logic

3. **Infrastructure Unit Tests**
   - Test concrete service implementations

4. **Integration Tests**
   - Similar to ViewModel tests
   - Use real services
   - Validate end-to-end behavior

---

## Dependencies

### Dependency Injection
- **DryIoc.dll** — Lightweight, fast dependency injection container

### Diagnostics & Logging
- **NLog** — Structured logging
- **Fody.MethodDecorator** — Compile-time AOP for logging, tracing, diagnostics
- **Sentry** — Error monitoring and crash reporting

### Local Storage
- **sqlite-net-pcl** — SQLite ORM and persistence

### Platform Utilities
- **Maui.Essentials** — Cross-platform APIs for permissions, device info, connectivity, file picker, camera access

*(Used only as an API layer, not for UI.)*

### Utility Libraries
- **Newtonsoft.Json** — JSON serialization/deserialization
- **Mapster** — Fast object mapping
- **Fody** — IL weaving and boilerplate reduction

### UI
- **DrasticTexture** — AsyncDisplayKit (Texture) binding for high-performance iOS UI

---

## Why This Architecture?

This demo demonstrates how to build:
- Fully **native** mobile applications
- With **maximum shared logic**
- Clean layering and separation of concerns
- High testability
- Long-term maintainability
- Scalability for enterprise-grade applications



## License

This project is provided for demonstration and educational purposes.
