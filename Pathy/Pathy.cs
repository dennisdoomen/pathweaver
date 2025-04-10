﻿// <autogenerated />

using System;
using System.IO;

#pragma warning disable

#nullable disable

namespace Pathy;

public class ChainablePath
{
    private readonly string path;

    private ChainablePath(string path)
    {
        this.path = path;
    }

    public static ChainablePath From(string path)
    {
        if (Path.IsPathRooted(path))
        {
            return new ChainablePath(Normalize(Path.GetFullPath(path)));
        }
        else
        {
            return new ChainablePath(Normalize(path));
        }
    }

    private static string Normalize(string path)
    {
        return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }

    public static ChainablePath operator /(ChainablePath leftPath, string rightPath)
    {
        return From(Path.Combine(leftPath.path, rightPath));
    }

    public static ChainablePath operator +(ChainablePath leftPath, string additionalPath)
    {
        return From(leftPath.ToString() + additionalPath);
    }


    public ChainablePath Directory => From(DirectoryName);

    public string DirectoryName => Path.GetDirectoryName(path);
    public bool IsRooted => Path.IsPathRooted(path);
    public string Name => Path.GetFileName(path);

    public bool Exists => FileExists || DirectoryExists;

    public bool FileExists => File.Exists(path);
    public bool DirectoryExists => System.IO.Directory.Exists(path);

    public ChainablePath Parent => From(DirectoryName);

    public string Extension
    {
        get
        {
            return Path.GetExtension(path);
        }
    }

    public ChainablePath Root => From(Path.GetPathRoot(path));
    public bool IsFile => File.Exists(ToString());
    public bool IsDirectory => System.IO.Directory.Exists(ToString());

    public static ChainablePath Current => From(Environment.CurrentDirectory);

    public static ChainablePath Temp => ChainablePath.From(Path.GetTempPath());

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public ChainablePath AsRelativeTo(ChainablePath basePath)
    {
        return Path.GetRelativePath(basePath.ToString()!, path).ToPath();
    }
#endif

    public override string ToString()
    {
        return path;
    }

    public static implicit operator string(ChainablePath chainablePath)
    {
        return chainablePath.ToString();
    }

    public void EnsureDirectoryExists()
    {
        System.IO.Directory.CreateDirectory(ToString());
    }

    public DirectoryInfo ToDirectoryInfo()
    {
        return new DirectoryInfo(ToString());
    }

    public FileInfo ToFileInfo()
    {
        return new FileInfo(ToString());
    }

    public override bool Equals(object obj) =>
        obj is ChainablePath other && string.Equals(ToString(), other.ToString(), StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => ToString().GetHashCode();

    public void Delete()
    {
        if (IsFile)
        {
            File.Delete(ToString());
        }
        else if (IsDirectory)
        {
            System.IO.Directory.Delete(ToString(), recursive: true);
        }
    }
}

public static class StringExtensions
{
    public static ChainablePath ToPath(this string path)
    {
        return ChainablePath.From(path);
    }
}
