using System.Runtime.InteropServices;

namespace RimeSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RimeTraits
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? SharedDataDir;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? UserDataDir;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? DistributionName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? DistributionCodeName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? DistributionVersion;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? AppName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? Modules;
        public int MinLogLevel;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? LogDir;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? PrebuiltDataDir;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? StagingDir;

        public RimeTraits()
        {
            _dataSize = Marshal.SizeOf<RimeTraits>() - Marshal.SizeOf<int>();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeComposition
    {
        public int Length;
        public int CursorPos;
        public int SelStart;
        public int SelEnd;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? Preedit;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeCandidate
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Text;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? Comment;
        public UIntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeMenu
    {
        public int PageSize;
        public int PageNo;
        public bool IsLastPage;
        public int HighlightedCandidateIndex;
        public int NumCandidates;
        public IntPtr Candidates;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string SelectKeys;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeCommit
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? Text;

        public RimeCommit()
        {
            _dataSize = Marshal.SizeOf<RimeCommit>() - Marshal.SizeOf<int>();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeContext
    {
        private readonly int _dataSize;

        [MarshalAs(UnmanagedType.Struct)]
        public RimeComposition Composition;

        [MarshalAs(UnmanagedType.Struct)]
        public RimeMenu Menu;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? CommitTextPreview;
        [MarshalAs(UnmanagedType.SafeArray)]
        public string[]? SelectLabels;

        public RimeContext()
        {
            _dataSize = Marshal.SizeOf<RimeContext>() - Marshal.SizeOf<int>();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeStatus
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? SchemaId;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? SchemaName;
        public bool IsDisabled;
        public bool IsComposing;
        public bool IsAsciiMode;
        public bool isFullShape;
        public bool IsSimplified;
        public bool IsTraditional;
        public bool IsAsciiPunct;

        public RimeStatus()
        {
            _dataSize = Marshal.SizeOf<RimeStatus>() - Marshal.SizeOf<int>();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeCandidateListIterator
    {
        public UIntPtr Ptr;
        public int Index;

        [MarshalAs(UnmanagedType.Struct)]
        public RimeCandidate Candidate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeSchemaListItem
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string SchemaId;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Name;
        public UIntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeSchemaList
    {
        public UIntPtr Size;
        public IntPtr List;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeStringSlice
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? Str;
        public ulong Length;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeModule
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string ModuleName;
        public IntPtr Initialize;
        public IntPtr Finalize;
        public IntPtr GetAPI;
    }
}
