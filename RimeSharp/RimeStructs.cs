using System.Runtime.InteropServices;

namespace RimeSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RimeTraits
    {
        private readonly int _dataSize;
        private IntPtr _sharedDataDir;
        private IntPtr _userDataDir;
        private IntPtr _distributionName;
        private IntPtr _distributionCodeName;
        private IntPtr _distributionVersion;
        private IntPtr _appName;
        private IntPtr _modules;
        public int MinLogLevel;
        private IntPtr _logDir;
        private IntPtr _prebuiltDataDir;
        private IntPtr _stagingDir;

        public string SharedDataDir
        {
            get => UTF8Marshal.PtrToStringUTF8(_sharedDataDir);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }

        public string UserDataDir
        {
            get => UTF8Marshal.PtrToStringUTF8(_userDataDir);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }
        
        public string DistributionName
        {
            get => UTF8Marshal.PtrToStringUTF8(_distributionName);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }
        
        public string DistributionCodeName
        {
            get => UTF8Marshal.PtrToStringUTF8(_distributionCodeName);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }
        
        public string DistributionVersion
        {
            get => UTF8Marshal.PtrToStringUTF8(_distributionVersion);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }
        
        public string AppName
        {
            get => UTF8Marshal.PtrToStringUTF8(_appName);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }
        
        public string Modules
        {
            get => UTF8Marshal.PtrToStringUTF8(_modules);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }
        
        public string LogDir
        {
            get => UTF8Marshal.PtrToStringUTF8(_logDir);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }
        
        public string PrebuiltDataDir
        {
            get => UTF8Marshal.PtrToStringUTF8(_prebuiltDataDir);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }
        
        public string StagingDir
        {
            get => UTF8Marshal.PtrToStringUTF8(_stagingDir);
            set => _sharedDataDir = UTF8Marshal.StringToHGlobalUTF8(value);
        }

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
        private readonly UIntPtr _reserved;
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

        public readonly RimeCandidate[] Candidates
        {
            get
            {
                var size = Marshal.SizeOf<RimeCandidate>();
                var result = new RimeCandidate[NumCandidates];
                for (var i = 0; i < NumCandidates; ++i)
                {
                    result[i] = Marshal.PtrToStructure<RimeCandidate>(_candidates + i * size);
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
        private readonly IntPtr _selectLabels;

        public string[] SelectLabels
        {
            get
            {
                var length = Menu.NumCandidates;
                var result = new string[length];
                for (var i = 0; i < length; ++i)
                {
                    var ptr = Marshal.ReadIntPtr(_selectLabels, i * IntPtr.Size);
                    result[i] = UTF8Marshal.PtrToStringUTF8(ptr);
                }
                return result;
            }
        }

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
        private readonly UIntPtr _reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RimeSchemaList
    {
        public UIntPtr Size;
        public IntPtr List;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct RimeStringSlice
    {
        private readonly IntPtr _str;
        private readonly UIntPtr _length;

        public string AsString() => UTF8Marshal.PtrToStringUTF8(_str, (int)_length.ToUInt32());
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
