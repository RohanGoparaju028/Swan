using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace swan
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("swan command is not specified.");
                return 1;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "version":
                    Console.WriteLine("swan version 0.0.1");
                    return 0;

                case "install":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: swan install <package-name>");
                        return 1;
                    }

                    string pkg = args[1];
                    string? version = null;

                    if (args.Length >= 4 && args[2].ToLower() == "--version")
                    {
                        version = args[3];
                    }

                    return await InstallingCommand(pkg, version);

                case "uninstall":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: swan uninstall <package-name>");
                        return 1;
                    }

                    string uninstallpackage = args[1];
                    return await UninstallingCommand(uninstallpackage);

                case "list":
                    return await ListAsync();

                case "help":
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("  version - Show the version of swan.");
                    Console.WriteLine("  install <package-name> [--version x.y.z] - Install a package.");
                    Console.WriteLine("  uninstall <package-name> - Uninstall a package.");
                    Console.WriteLine("  list - List installed packages.");
                    Console.WriteLine("  new - Create a new project.");
                    Console.WriteLine("  help - Show this help message.");
                    return 0;
                case "new":
                    return await NewProject(args[1]);
                default:
                    Console.WriteLine("Unknown command: " + command);
                    return 1;
            }
        }

        private static async Task<int> InstallingCommand(string package, string? version)
        {
            Console.WriteLine("Installing package: " + package);
            if (version != null)
            {
                Console.WriteLine("Version: " + version);
            }
            else
            {
                Console.WriteLine("No specific version provided, installing the latest version.");
            }

            string args = $"add package {package}" + (version is null ? "" : $" --version {version}");

            var processInfo = new ProcessStartInfo("dotnet", args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processInfo };
            process.Start();

            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                Console.WriteLine("Package installed successfully. Happy Coding!");
                return 0;
            }
            else
            {
                string errorOutput = await process.StandardError.ReadToEndAsync();
                Console.WriteLine("Failed to install package: " + package);
                Console.WriteLine("Error: " + errorOutput);
                return 1;
            }
        }

        private static async Task<int> UninstallingCommand(string pkg)
        {
            Console.WriteLine("Uninstalling package: " + pkg);
            string args = $"remove package {pkg}";

            var processInfo = new ProcessStartInfo("dotnet", args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processInfo };
            process.Start();

            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                Console.WriteLine("Package uninstalled successfully.");

                // Run `dotnet restore` to avoid asset sync issues
                var restoreInfo = new ProcessStartInfo("dotnet", "restore")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var restoreProc = new Process { StartInfo = restoreInfo };
                restoreProc.Start();
                await restoreProc.WaitForExitAsync();

                if (restoreProc.ExitCode != 0)
                {
                    string restoreErr = await restoreProc.StandardError.ReadToEndAsync();
                    Console.WriteLine("Warning: restore failed.");
                    Console.WriteLine("Error: " + restoreErr);
                    return 1;
                }

                return 0;
            }
            else
            {
                string errorOutput = await process.StandardError.ReadToEndAsync();
                Console.WriteLine("Failed to uninstall package: " + pkg);
                Console.WriteLine("Error: " + errorOutput);
                return 1;
            }
        }

        private static async Task<int> ListAsync()
        {
            Console.WriteLine("Listing installed Packages");
            var processInfo = new ProcessStartInfo("dotnet", "list package")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = new Process { StartInfo = processInfo };
            process.Start();
            string stdoutput = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            if (process.ExitCode == 0)
            {
                Console.WriteLine("Installed Packages:");
                Console.WriteLine(stdoutput);
                return 0;
            }
            else
            {
                Console.WriteLine("Failed to list packages.");
                Console.WriteLine("Error: " + stderr);
                return 1;
            }

        }
        private static async Task<int> NewProject(string Projectname)
        {
            Console.WriteLine("Creating a new Project: " + Projectname);
            string args = $"new console -n {Projectname}";
            var processinfo = new ProcessStartInfo("dotnet", args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = new Process { StartInfo = processinfo };
            process.Start();
            await process.WaitForExitAsync();
            if (process.ExitCode == 0)
            {
                Console.WriteLine("Project Created Successfully");
                return 0;
            }
            else
            {
                string error = await process.StandardError.ReadToEndAsync();
                Console.WriteLine("Failed to create a project.");
                Console.WriteLine("Error: " + error);
                return 1;
            }
        }
    }
}
