using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace YoloDev.Sdk.RoslynAnalyzerLib.Tasks;

public class GenerateEmbeddedSourcesPropsTask
  : Task
{
  [Required]
  public string? PackageName { get; set; }

  [Required]
  public string? TargetFramework { get; set; }

  [Required]
  public ITaskItem[]? Sources { get; set; }

  [Required]
  public bool GenerateBuildScriptsInPackage { get; set; }

  [Required]
  public string? IntermediateOutputPath { get; set; }

  [Output]
  public ITaskItem[]? FilesToEmbed { get; set; }

  public override bool Execute()
  {
    if (string.IsNullOrWhiteSpace(PackageName))
    {
      Log.LogError("PackageName is required");
    }

    if (Sources is null || Sources.Length == 0)
    {
      Log.LogError("No embedded sources provided");
    }

    if (Log.HasLoggedErrors)
    {
      return false;
    }

    var groupsEnum = Sources!
      .GroupBy(static x => x.GetMetadata("GroupName") ?? string.Empty, CreateGroup);

    var output = new List<ITaskItem2>();
    var dict = new SortedDictionary<string, Group>();
    foreach (var group in groupsEnum)
    {
      dict.Add(group.Name!, group);

      foreach (var source in group.Sources!)
      {
        Log.LogMessage(MessageImportance.High, $"Embedding {source.File} as {source.LogicalPath}");
        var item = new TaskItem(source.Source!);
        // src/$(TargetFramework)/%(EmbeddedSource.Identity)
        item.SetMetadata("PackagePath", $"src/{TargetFramework}/{source.LogicalPath}");
        item.SetMetadata("BuildItem", "None");
        output.Add(item);
      }
    }

    var includePrefix = $"Include_RoslynAnalyzerLib_" + PackageName!.Replace(".", "_");

    var sb = new StringBuilder();
    sb.AppendLine("<Project>");
    sb.AppendLine("  <PropertyGroup>");
    sb.AppendLine($"""    <{includePrefix}>true</{includePrefix}>""");
    foreach (var group in dict.Values)
    {
      if (group.Name == "")
      {
        continue;
      }

      sb.AppendLine($"""    <{includePrefix}__{group.SanitizedName}>true</{includePrefix}__{group.SanitizedName}>""");
    }
    sb.AppendLine("  </PropertyGroup>");
    sb.AppendLine("</Project>");

    var intermediateOutputPath = $"{IntermediateOutputPath}/EmbeddedSources";
    EnsureDirExists(intermediateOutputPath!);
    var propsOutputPath = $"{intermediateOutputPath}/{PackageName}.EmbeddedSources.props";
    File.WriteAllText(propsOutputPath, sb.ToString());
    Log.LogMessage(MessageImportance.Low, $"Generated {propsOutputPath}");

    var propsItem = new TaskItem(propsOutputPath);
    propsItem.SetMetadata("BuildAction", "None");
    propsItem.SetMetadata("PackagePath", $"build/{TargetFramework}/{PackageName}.EmbeddedSources.props");
    output.Add(propsItem);

    sb.Clear();
    sb.AppendLine("<Project>");
    foreach (var group in dict.Values)
    {
      if (group.Name == "")
      {
        sb.AppendLine($"""  <ItemGroup Condition=" '$({includePrefix})' == 'true' ">""");
      }
      else
      {
        sb.AppendLine($"""  <ItemGroup Condition=" '$({includePrefix})' == 'true' And '$({includePrefix}__{group.SanitizedName})' == 'true' ">""");
      }

      foreach (var source in group.Sources!)
      {
        sb.AppendLine($"""    <Compile Include="$(MSBuildThisFileDirectory)..\..\src\{TargetFramework}\{source.LogicalPath}" Link="Lib\{PackageName}\{source.LinkPath}" ReadOnly="true" />""");
      }

      sb.AppendLine("  </ItemGroup>");
    }
    sb.AppendLine("</Project>");

    var targetsOutputPath = $"{intermediateOutputPath}/{PackageName}.EmbeddedSources.targets";
    File.WriteAllText(targetsOutputPath, sb.ToString());
    Log.LogMessage(MessageImportance.Low, $"Generated {targetsOutputPath}");

    var targetsItem = new TaskItem(targetsOutputPath);
    targetsItem.SetMetadata("BuildAction", "None");
    targetsItem.SetMetadata("PackagePath", $"build/{TargetFramework}/{PackageName}.EmbeddedSources.targets");
    output.Add(targetsItem);

    if (GenerateBuildScriptsInPackage)
    {
      var buildPropsOutputPath = $"{intermediateOutputPath}/{PackageName}.props";

      sb.Clear();
      sb.AppendLine("<Project>");
      sb.AppendLine($"""  <Import Project="$(MSBuildThisFileDirectory){PackageName}.EmbeddedSources.props" />""");
      sb.AppendLine("</Project>");

      File.WriteAllText(buildPropsOutputPath, sb.ToString());
      Log.LogMessage(MessageImportance.Low, $"Generated {buildPropsOutputPath}");

      var buildPropsItem = new TaskItem(buildPropsOutputPath);
      buildPropsItem.SetMetadata("BuildAction", "None");
      buildPropsItem.SetMetadata("PackagePath", $"build/{TargetFramework}/{PackageName}.props");
      output.Add(buildPropsItem);

      var buildTargetsOutputPath = $"{intermediateOutputPath}/{PackageName}.targets";

      sb.Clear();
      sb.AppendLine("<Project>");
      sb.AppendLine($"""  <Import Project="$(MSBuildThisFileDirectory){PackageName}.EmbeddedSources.targets" />""");
      sb.AppendLine("</Project>");

      File.WriteAllText(buildTargetsOutputPath, sb.ToString());
      Log.LogMessage(MessageImportance.Low, $"Generated {buildTargetsOutputPath}");

      var buildTargetsItem = new TaskItem(buildTargetsOutputPath);
      buildTargetsItem.SetMetadata("BuildAction", "None");
      buildTargetsItem.SetMetadata("PackagePath", $"build/{TargetFramework}/{PackageName}.targets");
      output.Add(buildTargetsItem);
    }

    FilesToEmbed = output.ToArray();
    return true;
  }

  private Group CreateGroup(string groupName, IEnumerable<ITaskItem> items)
  {
    var builder = ImmutableArray.CreateBuilder<SourceToEmbed>();
    foreach (var item in items)
    {
      var file = item.GetMetadata("FullPath");
      var relPath = item.GetMetadata("Identity");
      var linkPath = item.GetMetadata("LinkPath");

      if (string.IsNullOrEmpty(linkPath))
      {
        linkPath = relPath;
      }

      builder.Add(new SourceToEmbed
      {
        Source = item,
        File = file,
        LinkPath = linkPath,
        LogicalPath = relPath,
      });
    }

    return new Group
    {
      Name = groupName,
      Sources = builder.ToImmutable(),
    };
  }

  private static void EnsureDirExists(string dir)
  {
    if (!Directory.Exists(dir))
    {
      var parent = Path.GetDirectoryName(dir);
      EnsureDirExists(parent!);
      Directory.CreateDirectory(dir);
    }
  }

  private class SourceToEmbed
  {
    public ITaskItem? Source { get; set; }

    public string? File { get; set; }

    public string? LinkPath { get; set; }

    public string? LogicalPath { get; set; }
  }

  private class Group
  {
    public string? Name { get; set; }

    public string SanitizedName => Name!.Replace(".", "_");

    public ImmutableArray<SourceToEmbed>? Sources { get; set; }
  }
}
