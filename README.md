# 🦢 swan

`swan` is a lightweight CLI tool built using .NET that lets you manage NuGet packages and create new .NET projects with ease. It's designed to mimic basic functionality of package managers like `dotnet`, but with a simpler interface.

---

## 📦 Features

- Install NuGet packages (optionally with a specific version)
- Uninstall NuGet packages
- List installed packages in the current project
- Create a new .NET console project
- Show version info
- Print help instructions

---

## 🚀 Getting Started

### ✅ Requirements

- [.NET SDK 9.0+](https://dotnet.microsoft.com/en-us/download)
- macOS (currently `osx-x64` published binary — cross-platform support coming soon!)

---

## 🛠️ Build & Install

### 1. Publish the CLI:

```bash
dotnet publish -c Release -r osx-x64 --self-contained true -o ~/bin/swanpub
```

### 2. Link the binary globally:

```bash
ln -s ~/bin/swanpub/swan /usr/local/bin/swan
```

Now you can use `swan` from anywhere in your terminal.

---

## 📌 Usage

### Show swan version

```bash
swan version
```

### Install a NuGet package

```bash
swan install <package-name>
```

Install with a specific version:

```bash
swan install <package-name> --version x.y.z
```

### Uninstall a NuGet package

```bash
swan uninstall <package-name>
```

> 🔁 This automatically runs `dotnet restore` after uninstall to sync your project assets.

### List installed packages

```bash
swan list
```

> 📍 Must be run in the root of a valid .NET project.

### Create a new .NET project

```bash
swan new <project-name>
```

> This uses `dotnet new console -n <project-name>` under the hood.

### Show help

```bash
swan help
```

---

## 💡 Example

```bash
swan new HelloWorld
cd HelloWorld
swan install Newtonsoft.Json
swan list
swan uninstall Newtonsoft.Json
```

---

## 🤝 Contributing

Pull requests are welcome! If you’d like to contribute:

1. Fork the repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -m 'Add your feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Open a Pull Request

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).

---

## 🔗 Links

- [Official .NET SDK](https://dotnet.microsoft.com/)
- [NuGet Package Explorer](https://www.nuget.org/)
