# KlinUtils.Blazor 🧩

A lightweight, flexible Blazor component library built with clean markup, extensibility, and developer ergonomics in mind.

[![NuGet](https://img.shields.io/nuget/v/KlinUtils.Blazor.svg)](https://www.nuget.org/packages/KlinUtils.Blazor)
[![NuGet Downloads](https://img.shields.io/nuget/dt/KlinUtils.Blazor.svg)](https://www.nuget.org/packages/KlinUtils.Blazor)
[![GitHub Stars](https://img.shields.io/github/stars/aweklin/KlinUtils?style=social)](https://github.com/aweklin/KlinUtils)

---

## ✨ Features

- ✅ Clean, minimal Blazor components for everyday apps
- 🎨 Styled using utility-first classes (Tailwind-friendly)
- ⚙️ Strong parameter APIs and service-injection support
- 🔄 Two-way binding, programmatic state toggling
- 🧪 Fully unit-tested with bUnit

---

## 📦 Installation

```bash
dotnet add package KlinUtils.Blazor
```

# 🚀 Components

### `AlertComponent`

```
<AlertComponent Type="AlertType.Warning" Message="Something went wrong!" />
```

- Optional Data="..." to render a bulleted list

- Alert Type support: Info, Warning, Success, Error

### `ButtonComponent`

```
<ButtonComponent Label="Submit" OnClick="HandleSubmit" UseImage="true" ImageUrl="icon.png" />
```

- Optionally renders custom Content

- Can show image/icon via parameters

### `CheckboxComponent`

```
<CheckboxComponent Label="Accept terms" @bind-IsChecked="isChecked" />
```

- Custom styles

- Programmatic Enable() / Disable()

### `SelectComponent<T>`

```
<SelectComponent T="int"
                 Items="Cities"
                 Placeholder="Choose a city"
                 @bind-Value="SelectedId" />
```

- Type-safe generics (int, string, enum, etc.)

- Supports placeholder and strongly-typed item lists

### `TextFieldComponent + NumberTextFieldComponent<T>`

```
<TextFieldComponent Placeholder="Email" @bind-Value="email" />
<NumberTextFieldComponent T="decimal" @bind-Value="price" />
```

### `FileComponent`

```
<FileComponent @bind-SelectedFile="uploadedFile" />
```

- Uses InputFile internally

- Triggers SelectedFileChanged callback

### `ModalDialogComponent`

```
<ModalDialogComponent IsVisible="@showModal">
    <ModalContent>
        <h3 class="text-lg">Confirm</h3>
        <p>Are you sure you want to proceed?</p>
    </ModalContent>
</ModalDialogComponent>
```

- Accessible modal dialog with internal visibility control

### `FragmentComponent`

```
<FragmentComponent CssClass="p-4 border" Style="background: #f0f0f0">
    <h4>Child Content Goes Here</h4>
</FragmentComponent>
```

### `ContainerComponent + ContainerStatesComponent`

```
<ContainerComponent State="@state">
    <Content>
        <p>Welcome home!</p>
    </Content>
</ContainerComponent>
```

### 🧭 Navigation Provider

```
_navigationProvider.NavigateTo("/dashboard", forceLoad: true);
string absolute = _navigationProvider.ToAbsoluteUrl("api/user");
```

# 🧪 Testing Philosophy

All components are validated using bUnit. Examples include:

- Parameter assertions ([Parameter])

- State transitions (Enable, Disable)

- DOM rendering tests (MarkupMatches, Find)

- Interaction tests (Click(), ChangeAsync())

# 📄 License

This project is licensed under the [MIT License](https://github.com/aweklin/KlinUtils?tab=MIT-1-ov-file#readme).

# 💬 Support / Feedback

Made with ❤️ by [Akeem Aweda](https://github.com/aweklin) GitHub repo: [github.com/aweklin/KlinUtils](github.com/aweklin/KlinUtils). Feature ideas, issues, and contributions welcome.

If KlinUtils.Blazor helps you build better UIs, give it a ⭐ on GitHub and share the love!
