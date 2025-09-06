using System.Runtime.InteropServices;

namespace RimeSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RimeTraits
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? SharedDataDir;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? UserDataDir;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? DistributionName;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? DistributionCodeName;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? DistributionVersion;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? AppName;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? Modules;
        public int MinLogLevel;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? LogDir;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? PrebuiltDataDir;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? StagingDir;

        public RimeTraits()
        {
            _dataSize = Marshal.SizeOf<RimeTraits>() - Marshal.SizeOf<int>();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct RimeComposition
    {
        public int Length;
        public int CursorPos;
        public int SelStart;
        public int SelEnd;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? Preedit;
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct RimeCandidate
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Text;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? Comment;
        public UIntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct RimeMenu
    {
        public int PageSize;
        public int PageNo;
        public bool IsLastPage;
        public int HighlightedCandidateIndex;
        public int NumCandidates;
        private readonly IntPtr _candidates;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string SelectKeys;

        public readonly CandidateItem[] Candidates
        {
            get
            {
                var size = Marshal.SizeOf<RimeCandidate>();
                var result = new CandidateItem[NumCandidates];
                for (var i = 0; i < NumCandidates; ++i)
                {
                    var candidate = Marshal.PtrToStructure<RimeCandidate>(_candidates + i * size);
                    result[i] = new CandidateItem(candidate.Text, candidate.Comment);
                }
                return result;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct RimeCommit : IDisposable
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? Text;

        public RimeCommit()
        {
            _dataSize = Marshal.SizeOf<RimeCommit>() - Marshal.SizeOf<int>();
        }

        public void Dispose() => Rime.Instance().FreeCommit(ref this);
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct RimeContext : IDisposable
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

        public void Dispose() => Rime.Instance().FreeContext(ref this);
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct RimeStatus : IDisposable
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? SchemaId;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string? SchemaName;
        public bool IsDisabled;
        public bool IsComposing;
        public bool IsAsciiMode;
        public bool IsFullShape;
        public bool IsSimplified;
        public bool IsTraditional;
        public bool IsAsciiPunct;

        public RimeStatus()
        {
            _dataSize = Marshal.SizeOf<RimeStatus>() - Marshal.SizeOf<int>();
        }

        public void Dispose() => Rime.Instance().FreeStatus(ref this);
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
    public record struct RimeSchemaListItem
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
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public DVoid Initialize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public DVoid Finalize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetAPI GetAPI;
    }
}
