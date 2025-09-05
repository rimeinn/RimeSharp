using System.Runtime.InteropServices;
using System.Text;

namespace RimeSharp
{
    using RimeSessionId = nuint;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RimeNotificationHandler(
        nuint contextObject,
        RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        string messageType,
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        string messageValue
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void DVoid();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool DBool();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void DTraitsVoid(ref RimeTraits traits);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetNotificationHandler(
        RimeNotificationHandler handler,
        RimeSessionId contextObject
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool StartMaintenance(bool fullCheck);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate RimeSessionId CreateSession();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool DestroySession(RimeSessionId sessionId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetCommit(RimeSessionId sessionId, ref RimeCommit commit);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool FreeCommit(ref RimeCommit commit);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetContext(RimeSessionId sessionId, ref RimeContext context);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool FreeContext(ref RimeContext context);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetStatus(RimeSessionId sessionId, ref RimeStatus status);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool FreeStatus(ref RimeStatus status);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetOption(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPStr)] string option,
        bool value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetOption(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPStr)] string option);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetSchemaList(out RimeSchemaList list);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool FreeSchemaList(ref RimeSchemaList list);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetCurrentSchema(RimeSessionId sessionId,
        StringBuilder schemaId,
        int bufferSize);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool SelectSchema(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPStr)] string schemaId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool SimulateKeySequence(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPStr)] string keySequence);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate nint FindModule(
        [MarshalAs(UnmanagedType.LPStr)] string moduleName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate nint GetAPI();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool SelectCandidate(RimeSessionId sessionId, int index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool CandidateListFromIndex(RimeSessionId sessionId,
        ref RimeCandidateListIterator iterator, int index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool CandidateListNext(ref RimeCandidateListIterator iterator);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CandidateListEnd(ref RimeCandidateListIterator iterator);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr GetStateLabelAbbr(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPStr)] string optionName,
        bool state,
        bool abbreviated);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ChangePage(RimeSessionId sessionId, bool backward);

    public sealed class Rime
    {
        private static readonly Lazy<Rime> s_instance = new(() => new Rime());

        private readonly RimeAPI _api;

        private Rime()
        {
            IntPtr apiPtr = Native.GetRimeAPI();
            if (apiPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to get Rime API.");
            }

            _api = Marshal.PtrToStructure<RimeAPI>(apiPtr);
        }

        public sealed class Commit : Disposable<RimeCommit>
        {
            internal Commit(ref RimeCommit commit) :
                base(ref commit, (ref RimeCommit v) => Instance()._api.FreeCommit(ref v))
            { }

            public string? Text { get => value.Text; }
        }

        public record class Composition
        {
            private RimeComposition _composition;
            internal Composition(ref RimeComposition composition)
            {
                _composition = composition;
            }
            public int Length { get => _composition.Length; }
            public int CursorPos { get => _composition.CursorPos; }
            public int SelStart { get => _composition.SelStart; }
            public int SelEnd { get => _composition.SelEnd; }
            public string? Preedit { get => _composition.Preedit; }
        }

        public record class Menu
        {
            private RimeMenu _menu;
            internal Menu(ref RimeMenu menu)
            {
                _menu = menu;
            }
            public int PageSize { get => _menu.PageSize; }
            public int PageNo { get => _menu.PageNo; }
            public bool IsLastPage { get => _menu.IsLastPage; }
            public int HighlightedCandidateIndex { get => _menu.HighlightedCandidateIndex; }
            public CandidateItem[] Candidates
            {
                get
                {
                    var size = Marshal.SizeOf<RimeCandidate>();
                    var candidates = new CandidateItem[_menu.NumCandidates];
                    var ptr = _menu.Candidates;
                    for (var i = 0; i < _menu.NumCandidates; ++i)
                    {
                        var candidate = Marshal.PtrToStructure<RimeCandidate>(ptr + i * size);
                        candidates[i] = new CandidateItem(candidate.Text, candidate.Comment);
                    }
                    return candidates;
                }
            }
            public string SelectKeys { get => _menu.SelectKeys; }
        }

        public sealed class Context : Disposable<RimeContext>
        {
            internal Context(ref RimeContext context) :
                base(ref context, (ref RimeContext v) => Instance()._api.FreeContext(ref v))
            { }

            public Composition Composition { get => new(ref value.Composition); }
            public Menu Menu { get => new(ref value.Menu); }
            public string? CommitTextPreview { get => value.CommitTextPreview; }
            public string[]? SelectLabels { get => value.SelectLabels; }
        }

        public sealed class Status : Disposable<RimeStatus>
        {
            internal Status(ref RimeStatus status) :
                base(ref status, (ref RimeStatus v) => Instance()._api.FreeStatus(ref v))
            { }

            public string? SchemaId { get => value.SchemaId; }
            public string? SchemaName { get => value.SchemaName; }
            public bool IsDisabled { get => value.IsDisabled; }
            public bool IsComposing { get => value.IsComposing; }
            public bool IsAsciiMode { get => value.IsAsciiMode; }
            public bool IsFullShape { get => value.isFullShape; }
            public bool IsSimplified { get => value.IsSimplified; }
            public bool IsTraditional { get => value.IsTraditional; }
            public bool IsAsciiPunct { get => value.IsAsciiPunct; }
        }

        public sealed class Module
        {
            private readonly RimeModule _module;
            private readonly DVoid _initialize;
            private readonly DVoid _finalize;
            private readonly GetAPI _getAPI;

            internal Module(ref RimeModule module)
            {
                _module = module;
                _initialize = Marshal.GetDelegateForFunctionPointer<DVoid>(_module.Initialize);
                _finalize = Marshal.GetDelegateForFunctionPointer<DVoid>(_module.Finalize);
                _getAPI = Marshal.GetDelegateForFunctionPointer<GetAPI>(_module.GetAPI);
            }
            public string ModuleName { get => _module.ModuleName; }

            public void Initialize() => _initialize();
            public void Finalize1() => _finalize();

            public nint GetAPI() => _getAPI();
        }

        public static Rime Instance() => s_instance.Value;

        public void Setup(ref RimeTraits traits) => _api.Setup(ref traits);

        public void SetNotificationHandler(RimeNotificationHandler handler)
            => _api.SetNotificationHandler(handler, UIntPtr.Zero);

        public void Initialize(ref RimeTraits traits) => _api.Initialize(ref traits);

        public void Finalize1() => _api.Finalize();

        public bool StartMaintenance(bool fullCheck) => _api.StartMaintenance(fullCheck);

        public bool IsMaintenanceMode() => _api.IsMaintenanceMode();

        public void JoinMaintenanceThread() => _api.JoinMaintenanceThread();

        public RimeSessionId CreateSession() => _api.CreateSession();

        public bool DestroySession(RimeSessionId sessionId) => _api.DestroySession(sessionId);

        public Commit GetCommit(RimeSessionId sessionId)
        {
            var commit = new RimeCommit();
            _api.GetCommit(sessionId, ref commit);
            return new Commit(ref commit);
        }

        public Context GetContext(RimeSessionId sessionId)
        {
            var context = new RimeContext();
            _api.GetContext(sessionId, ref context);
            return new Context(ref context);
        }

        public Status GetStatus(RimeSessionId sessionId)
        {
            var status = new RimeStatus();
            _api.GetStatus(sessionId, ref status);
            return new Status(ref status);
        }

        public void SetOption(RimeSessionId sessionId, string option, bool value)
            => _api.SetOption(sessionId, option, value);

        public bool GetOption(RimeSessionId sessionId, string option)
            => _api.GetOption(sessionId, option);

        public SchemaListItem[] GetSchemaList()
        {

            if (_api.GetSchemaList(out var list))
            {
                var size = Marshal.SizeOf<RimeSchemaListItem>();
                var items = new SchemaListItem[(int)list.Size];
                for (var i = 0; i < (int)list.Size; ++i)
                {
                    var ptr = IntPtr.Add(list.List, i * size);
                    var item = Marshal.PtrToStructure<RimeSchemaListItem>(ptr);
                    items[i] = new SchemaListItem(item.SchemaId, item.Name);
                }
                _api.FreeSchemaList(ref list);
                return items;
            }
            return [];
        }

        public string GetCurrentSchema(RimeSessionId sessionId)
        {
            var bufferSize = 256;
            var buffer = new StringBuilder(bufferSize);
            if (_api.GetCurrentSchema(sessionId, buffer, bufferSize))
            {
                return buffer.ToString();
            }
            return "";
        }

        public bool SelectSchema(RimeSessionId sessionId, string schemaId)
            => _api.SelectSchema(sessionId, schemaId);

        public bool SimulateKeySequence(RimeSessionId sessionId, string keySequence)
            => _api.SimulateKeySequence(sessionId, keySequence);

        public Module FindModule(string moduleName)
        {
            var ptr = _api.FindModule(moduleName);
            if (ptr != IntPtr.Zero)
            {
                var module = Marshal.PtrToStructure<RimeModule>(ptr);
                return new Module(ref module);
            }
            throw new InvalidOperationException($"Module '{moduleName}' not found.");
        }

        public bool SelectCandidate(RimeSessionId sessionId, int index, bool paged = false)
            => paged ? _api.SelectCandidateOnCurrentPage(sessionId, index) : _api.SelectCandidate(sessionId, index);

        public CandidateItem[] GetCandidates(RimeSessionId sessionId, int start = 0, int count = int.MaxValue)
        {
            var iterator = new RimeCandidateListIterator();
            var candidates = new List<CandidateItem>();
            if (_api.CandidateListFromIndex(sessionId, ref iterator, start))
            {
                int i = 0;
                while (i < count && _api.CandidateListNext(ref iterator))
                {
                    var candidate = iterator.Candidate;
                    candidates.Add(new CandidateItem(candidate.Text, candidate.Comment));
                    ++i;
                };
                _api.CandidateListEnd(ref iterator);
            }
            return [.. candidates];
        }

        public string? GetStateLabel(RimeSessionId sessionId, string optionName, bool state, bool abbreviated = false)
        {
            var ptr = _api.GetStateLabelAbbreviated(sessionId, optionName, state, abbreviated);
            var slice = Marshal.PtrToStructure<RimeStringSlice>(ptr);
            return slice.Str;
        }

        public bool ChangePage(RimeSessionId sessionId, bool backward)
            => _api.ChangePage(sessionId, backward);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RimeAPI
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public DTraitsVoid Setup;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SetNotificationHandler SetNotificationHandler;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public DTraitsVoid Initialize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public DVoid Finalize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public StartMaintenance StartMaintenance;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public DBool IsMaintenanceMode;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public DVoid JoinMaintenanceThread;
        public IntPtr DeployerInitialize; // unused
        public IntPtr Prebuild; // unused
        public IntPtr Deploy; // unused
        public IntPtr DeploySchema; // unused
        public IntPtr DeployConfigFile; // unused
        public IntPtr SyncUserData; // unused
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CreateSession CreateSession;
        public IntPtr FindSession; // unused
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public DestroySession DestroySession;
        public IntPtr CleanupStaleSessions; // unused
        public IntPtr CleanupAllSessions; // unused
        public IntPtr ProcessKey;
        public IntPtr CommitComposition;
        public IntPtr ClearComposition;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetCommit GetCommit;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FreeCommit FreeCommit;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetContext GetContext;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FreeContext FreeContext;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetStatus GetStatus;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FreeStatus FreeStatus;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SetOption SetOption;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetOption GetOption;
        public IntPtr SetProperty;
        public IntPtr GetProperty;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetSchemaList GetSchemaList;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FreeSchemaList FreeSchemaList;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetCurrentSchema GetCurrentSchema;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SelectSchema SelectSchema;
        public IntPtr SchemaOpen;
        public IntPtr ConfigOpen;
        public IntPtr ConfigClose;
        public IntPtr ConfigGetBool;
        public IntPtr ConfigGetInt;
        public IntPtr ConfigGetDouble;
        public IntPtr ConfigGetString;
        public IntPtr ConfigGetCString;
        public IntPtr ConfigUpdateSignature;
        public IntPtr ConfigBeginMap;
        public IntPtr ConfigNext;
        public IntPtr ConfigEnd;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SimulateKeySequence SimulateKeySequence;
        public IntPtr RegisterModule;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FindModule FindModule;
        public IntPtr RunTask;
        private readonly IntPtr _getSharedDataDir; // deprecated
        private readonly IntPtr _getUserDataDir; // deprecated
        private readonly IntPtr _getSyncDir; // deprecated
        public IntPtr GetUserId;
        public IntPtr GetUserDataSyncDir;
        public IntPtr ConfigInit;
        public IntPtr ConfigLoadString;
        public IntPtr ConfigSetBool;
        public IntPtr ConfigSetInt;
        public IntPtr ConfigSetDouble;
        public IntPtr ConfigSetString;
        public IntPtr ConfigGetItem;
        public IntPtr ConfigSetItem;
        public IntPtr ConfigClear;
        public IntPtr ConfigCreateList;
        public IntPtr ConfigCreateMap;
        public IntPtr ConfigListSize;
        public IntPtr ConfigBeginList;
        public IntPtr GetInput;
        public IntPtr getCaretPos;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SelectCandidate SelectCandidate;
        public IntPtr GetVersion;
        public IntPtr SetCaretPos;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SelectCandidate SelectCandidateOnCurrentPage;
        public IntPtr CandidateListBegin; // unused
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CandidateListNext CandidateListNext;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CandidateListEnd CandidateListEnd;
        public IntPtr UserConfigOpen;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CandidateListFromIndex CandidateListFromIndex;
        private readonly IntPtr _getPrebuiltDataDir; // deprecated
        private readonly IntPtr _getStagingDir; // deprecated
        private readonly IntPtr _commitProto; // deprecated
        private readonly IntPtr _contextProto; // deprecated
        private readonly IntPtr _statusProto; // deprecated
        public IntPtr GetStateLabel; // unused
        public IntPtr DeleteCandidate;
        public IntPtr DeleteCandidateOnCurrentPage;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetStateLabelAbbr GetStateLabelAbbreviated;
        public IntPtr SetInput;
        public IntPtr GetSharedDataDirS;
        public IntPtr GetUserDataDirS;
        public IntPtr GetPrebuiltDataDirS;
        public IntPtr GetStagingDirS;
        public IntPtr GetSyncDirS;
        public IntPtr HighlightCandidate;
        public IntPtr HighlightCandidateOnCurrentPage;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ChangePage ChangePage;
    }

    internal partial class Native
    {
        private const string RimeDll = "rime.dll";

        [DllImport(RimeDll, EntryPoint = "rime_get_api", CallingConvention = CallingConvention.Cdecl)]
        internal static extern nint GetRimeAPI();
    }
}
