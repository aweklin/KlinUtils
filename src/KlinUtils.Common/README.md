# KlinUtils.Common 🧩

Collection of common utilities enum, classes, etc

[![NuGet](https://img.shields.io/nuget/v/KlinUtils.Common.svg)](https://www.nuget.org/packages/KlinUtils.Common)
[![NuGet Downloads](https://img.shields.io/nuget/dt/KlinUtils.Common.svg)](https://www.nuget.org/packages/KlinUtils.Common)
[![GitHub Stars](https://img.shields.io/github/stars/aweklin/KlinUtils?style=social)](https://github.com/aweklin/KlinUtils)

---

## 🔧 Features

- ✅ `Result<TSuccess, TError>`: Functional-style result object with rich error handling support
- ✅ `Failure` and `ErrorInfo`: Domain-safe error modeling with first-class developer intent
- ✅ `PasswordHelper`: Utilities for secure password hashing and strength validation
- ✅ `GuidExtensions`: Base64-style GUID shortening and restoration
- ✅ `NavigationProvider`: Injectable abstraction around Blazor's `NavigationManager`
- ✅ `ExceptionExtensions`: Deep exception insight by peeling nested messages

---

## 📦 Installation

```bash
dotnet add package KlinUtils.Common
```

# 🚀 Enums

### `AlertType`

- Alert Type support: Info, Warning, Success, Error

### 🔁 Result Handling

```
Result<User, Failure> result = _userService.TryCreate(request);

string message = result.Match(
    success => $"Welcome, {success.Name}!",
    error => $"Error: {error.Errors.First().Message}");

```

### 🛡️ Password Utilities

```
bool strong = _passwordHelper.IsPasswordStrongEnough("MyP@ss123", 8);
string hash = _passwordHelper.HashPassword("MyP@ss123");
```

### 🔖 Guid Shortening

```
string shortened = Guid.NewGuid().Shorten();    // "m2dbxZp-UkCiFYwCHUk3rQ"
Guid roundTrip = shortened.FromString();        // Recovers original Guid
```

### ⚠️ Exception Insight

```
try
{
    ...
}
catch (Exception ex)
{
    string message = ex.GetDetails(); // Recurses to find root cause
}
```

### 📖 Inspired By

Some utilities (e.g., GuidExtensions) are inspired by [Nick Chapsas](https://www.youtube.com/@nickchapsas)’ work and modern functional C# idioms.

# 📄 License

This project is licensed under the [MIT License](https://github.com/aweklin/KlinUtils?tab=MIT-1-ov-file#readme).

# 💬 Support / Feedback

Made with ❤️ by [Akeem Aweda](https://github.com/aweklin) GitHub repo: [github.com/aweklin/KlinUtils](github.com/aweklin/KlinUtils). Feature ideas, issues, and contributions welcome.

If KlinUtils.Common helps you build better UIs, give it a ⭐ on GitHub and share the love!
