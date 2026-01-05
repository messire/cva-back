using System.Collections;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CVA.Tools.Common;

/// <summary>
/// Temporary config tool for registering configs.
/// </summary>
public static class ConfigTools
{
    private const string ConfigFile = "config/{0}.appsettings.json";
    private const string EnvFile = "config/{0}.env.{1}.json";

    /// <summary>
    /// Adds the JSON configuration provider at <paramref name="configName"/> to <paramref name="configuration"/>.
    /// </summary>
    /// <param name="configuration"><see cref="IConfigurationBuilder"/> instance.</param>
    /// <param name="configName">Config file name.</param>
    /// <param name="environment"><see cref="IHostEnvironment"/> instance.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="configName"/> is null or whitespace.</exception>
    /// <remarks>
    /// Supported docker container directory.
    /// </remarks>
    public static void AddConfigFiles(this IConfigurationBuilder configuration, string configName, IHostEnvironment environment)
    {
        var parentDir = GetRootPath(environment);

        var configFile = string.Format(ConfigFile, configName);
        var configPath = GetFilePath(parentDir, configFile);

        var envFile = string.Format(EnvFile, configName, environment.EnvironmentName);
        var configEnvPath = GetFilePath(parentDir, envFile);

        var config = File.ReadAllText(configPath);
        var envData = File.Exists(configEnvPath) ? DeserializeEnvs(configEnvPath) : ReadEnvs();
        var result = CombineConfigs(config, envData);

        configuration.AddJsonStream(new MemoryStream(Encoding.Default.GetBytes(result)));
    }

    /// <summary>
    ///     Setup environment from <paramref name="appEnv" /> from Kubernetes ConfigMap.
    /// </summary>
    /// <param name="environment">Host environment.</param>
    /// <param name="appEnv">APP_ENV.</param>
    public static void SetEnvironment(this IHostEnvironment environment, string appEnv = "APP_ENV")
    {
        var value = Environment.GetEnvironmentVariable(appEnv);
        if (string.IsNullOrEmpty(value)) return;

        environment.EnvironmentName = value;
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", value);
    }

    private static string GetFilePath(string rootPath, string fileName)
    {
        ArgumentNullException.ThrowIfNull(rootPath);
        ArgumentNullException.ThrowIfNull(fileName);

        return Path.Combine(rootPath, fileName);
    }

    private static string GetRootPath(IHostEnvironment environment)
    {
        if (environment.ContentRootPath == "/app/") return environment.ContentRootPath;

        var directoryInfo = new DirectoryInfo(environment.ContentRootPath);
        var configPath = GetParent(directoryInfo)?.FullName;
        ArgumentNullException.ThrowIfNull(configPath);

        return configPath;
    }

    private static DirectoryInfo? GetParent(DirectoryInfo? directoryInfo)
    {
        while (directoryInfo != null && CheckTargetDir(directoryInfo))
        {
            directoryInfo = directoryInfo.Parent;
        }

        return directoryInfo;
    }

    private static bool CheckTargetDir(DirectoryInfo directoryInfo)
        => directoryInfo.FullName.Contains("src") || directoryInfo.FullName.Contains("tests");

    private static IEnumerable<DictionaryEntry> DeserializeEnvs(string configEnvPath)
    {
        var envData = File.ReadAllText(configEnvPath);
        var data = JsonSerializer.Deserialize<IDictionary<string, object>>(envData);
        ArgumentNullException.ThrowIfNull(data);
        return data.Select(pair => new DictionaryEntry(pair.Key, pair.Value));
    }

    private static IEnumerable<DictionaryEntry> ReadEnvs()
        => Environment.GetEnvironmentVariables().Cast<DictionaryEntry>();

    private static string CombineConfigs(string config, IEnumerable<DictionaryEntry> envs)
        => envs.Aggregate(config, (current, envPair) => current.Replace($"${envPair.Key}", $"{envPair.Value}"));
}