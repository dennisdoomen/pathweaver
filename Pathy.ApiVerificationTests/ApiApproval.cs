﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using PublicApiGenerator;
using VerifyTests;
using VerifyTests.DiffPlex;
using VerifyXunit;
using Xunit;

namespace Pathy.ApiVerificationTests;

public class ApiApproval
{
    static ApiApproval() => VerifyDiffPlex.Initialize(OutputType.Minimal);

    [Theory]
    [InlineData("netstandard2.0")]
    [InlineData("netstandard2.1")]
    [InlineData("net47")]
    [InlineData("net8.0")]
    public async Task ApprovePathyApi(string targetFramework)
    {
        var configuration = typeof(ApiApproval).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
        var assemblyFile = GetSolutionDirectory() / "Pathy" / "bin" / configuration / targetFramework / "Pathy.dll";
        var assembly = Assembly.LoadFile(assemblyFile);
        var publicApi = assembly.GeneratePublicApi(options: null);

        await Verifier
            .Verify(publicApi)
            .ScrubLinesContaining("FrameworkDisplayName")
            .UseDirectory("ApprovedApi")
            .UseFileName("pathy." + targetFramework)
            .DisableDiff();
    }

    [Theory]
    [InlineData("netstandard2.0")]
    [InlineData("netstandard2.1")]
    [InlineData("net47")]
    [InlineData("net8.0")]
    public async Task ApprovePathyGlobbingApi(string targetFramework)
    {
        var configuration = typeof(ApiApproval).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
        var assemblyFile = GetSolutionDirectory() / "Pathy.Globbing" / "bin" / configuration / targetFramework / "Pathy.Globbing.dll";
        var assembly = Assembly.LoadFile(assemblyFile);
        var publicApi = assembly.GeneratePublicApi(options: null);

        await Verifier
            .Verify(publicApi)
            .ScrubLinesContaining("FrameworkDisplayName")
            .UseDirectory("ApprovedApi")
            .UseFileName("pathy.globbing." + targetFramework)
            .DisableDiff();
    }

    private static ChainablePath GetSolutionDirectory([CallerFilePath] string path = "") =>
        ChainablePath.From(path).Directory / "..";
}
