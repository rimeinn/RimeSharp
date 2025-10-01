using System.Runtime.InteropServices;

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
    public delegate void DVoid();

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
        [MarshalAs(UnmanagedType.LPUTF8Str)] string option,
        bool value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetOption(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string option);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetSchemaList(out RimeSchemaList list);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool FreeSchemaList(ref RimeSchemaList list);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool GetCurrentSchema(RimeSessionId sessionId,
        [Out] char[] schemaId,
        int bufferSize);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool SelectSchema(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string schemaId);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ConfigOpen(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string configId,
        ref RimeConfig config);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ConfigClose(ref RimeConfig config);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ConfigGetBool(ref RimeConfig config,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string key,
        out bool value);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ConfigGetInt(ref RimeConfig config,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string key,
        out int value);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ConfigGetDouble(ref RimeConfig config,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string key,
        out double value);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr ConfigGetCString(ref RimeConfig config,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string key);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ConfigBegin(ref RimeConfigIterator iterator,
        ref RimeConfig config,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string key);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ConfigNext(ref RimeConfigIterator iterator);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool ConfigEnd(ref RimeConfigIterator iterator);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool SimulateKeySequence(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string keySequence);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr FindModule(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string moduleName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetAPI();
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate UIntPtr ConfigListSize(ref RimeConfig config,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string key);

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
    internal delegate RimeStringSlice GetStateLabelAbbr(RimeSessionId sessionId,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string optionName,
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

        public RimeCommit GetCommit(RimeSessionId sessionId)
        {
            var commit = new RimeCommit();
            _api.GetCommit(sessionId, ref commit);
            return commit;
        }

        internal bool FreeCommit(ref RimeCommit commit) => _api.FreeCommit(ref commit);

        public RimeContext GetContext(RimeSessionId sessionId)
        {
            var context = new RimeContext();
            _api.GetContext(sessionId, ref context);
            return context;
        }

        internal bool FreeContext(ref RimeContext context) => _api.FreeContext(ref context);

        public RimeStatus GetStatus(RimeSessionId sessionId)
        {
            var status = new RimeStatus();
            _api.GetStatus(sessionId, ref status);
            return status;
        }

        internal bool FreeStatus(ref RimeStatus status) => _api.FreeStatus(ref status);

        public void SetOption(RimeSessionId sessionId, string option, bool value)
            => _api.SetOption(sessionId, option, value);

        public bool GetOption(RimeSessionId sessionId, string option)
            => _api.GetOption(sessionId, option);

        public RimeSchemaListItem[] GetSchemaList()
        {
            if (!_api.GetSchemaList(out var list)) return [];
            var size = Marshal.SizeOf<RimeSchemaListItem>();
            var items = new RimeSchemaListItem[(int)list.Size];
            for (var i = 0; i < (int)list.Size; ++i)
            {
                var ptr = IntPtr.Add(list.List, i * size);
                items[i] = Marshal.PtrToStructure<RimeSchemaListItem>(ptr);
            }
            _api.FreeSchemaList(ref list);
            return items;
        }

        public string GetCurrentSchema(RimeSessionId sessionId)
        {
            var buffer = new char[256];
            return _api.GetCurrentSchema(sessionId, buffer, buffer.Length)
                ? new string(buffer) : string.Empty;
        }

        public bool SelectSchema(RimeSessionId sessionId, string schemaId)
            => _api.SelectSchema(sessionId, schemaId);

        public RimeConfig SchemaOpen(string schemaId)
        {
            var config = new RimeConfig();
            _api.SchemaOpen(schemaId, ref config);
            return config;
        }

        public RimeConfig ConfigOpen(string configId)
        {
            var config = new RimeConfig();
            _api.ConfigOpen(configId, ref config);
            return config;
        }
        
        internal bool ConfigClose(ref RimeConfig config)
            => _api.ConfigClose(ref config);
        
        internal bool? ConfigGetBool(ref RimeConfig config, string key)
        {
            return _api.ConfigGetBool(ref config, key, out var value)
                ? value
                : null;
        }
        
        internal int? ConfigGetInt(ref RimeConfig config, string key)
        {
            return _api.ConfigGetInt(ref config, key, out var value)
                ? value
                : null;
        }
        
        internal double? ConfigGetDouble(ref RimeConfig config, string key)
        {
            return _api.ConfigGetDouble(ref config, key, out var value)
                ? value
                : null;
        }

        internal string? ConfigGetString(ref RimeConfig config, string key)
        {
            var ptr = _api.ConfigGetCString(ref config, key);
            return ptr != IntPtr.Zero
                ? UTF8Marshal.PtrToStringUTF8(ptr)
                : null;
        }
        
        internal bool ConfigBeginMap(ref RimeConfigIterator iterator, ref RimeConfig config, string key)
            => _api.ConfigBeginMap(ref iterator, ref config, key);

        internal bool ConfigNext(ref RimeConfigIterator iterator)
            => _api.ConfigNext(ref iterator);

        internal bool ConfigEnd(ref RimeConfigIterator iterator)
            => _api.ConfigEnd(ref iterator);

        public bool SimulateKeySequence(RimeSessionId sessionId, string keySequence)
            => _api.SimulateKeySequence(sessionId, keySequence);

        public RimeModule FindModule(string moduleName)
        {
            var ptr = _api.FindModule(moduleName);
            return ptr != IntPtr.Zero ? Marshal.PtrToStructure<RimeModule>(ptr)
                : throw new InvalidOperationException($"Module '{moduleName}' not found.");
        }

        internal uint ConfigListSize(ref RimeConfig config, string key)
            => _api.ConfigListSize(ref config, key).ToUInt32();

        internal bool ConfigBeginList(ref RimeConfigIterator iterator, ref RimeConfig config, string key)
            => _api.ConfigBeginList(ref iterator, ref config, key);

        public bool SelectCandidate(RimeSessionId sessionId, int index, bool paged = false)
            => paged ? _api.SelectCandidateOnCurrentPage(sessionId, index) : _api.SelectCandidate(sessionId, index);

        public RimeCandidate[] GetCandidates(RimeSessionId sessionId, int start = 0, int count = int.MaxValue)
        {
            var iterator = new RimeCandidateListIterator();
            if (!_api.CandidateListFromIndex(sessionId, ref iterator, start)) return [];
            var candidates = new List<RimeCandidate>();
            var i = 0;
            while (i < count && _api.CandidateListNext(ref iterator))
            {
                candidates.Add(iterator.Candidate);
                ++i;
            }
            _api.CandidateListEnd(ref iterator);
            return [.. candidates];
        }

        public string GetStateLabel(RimeSessionId sessionId, string optionName, bool state, bool abbreviated = false)
            => _api.GetStateLabelAbbreviated(sessionId, optionName, state, abbreviated).AsString();

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
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigOpen SchemaOpen;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigOpen ConfigOpen;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigClose ConfigClose;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigGetBool ConfigGetBool;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigGetInt ConfigGetInt;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigGetDouble ConfigGetDouble;
        public IntPtr ConfigGetString; // unused
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigGetCString ConfigGetCString;
        public IntPtr ConfigUpdateSignature;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigBegin ConfigBeginMap;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigNext ConfigNext;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigEnd ConfigEnd;
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
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigListSize ConfigListSize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigBegin ConfigBeginList;
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
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ConfigOpen UserConfigOpen;
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

    internal static class Native
    {
        private const string RimeDll = "rime";

        [DllImport(RimeDll, EntryPoint = "rime_get_api", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetRimeAPI();
    }
}
