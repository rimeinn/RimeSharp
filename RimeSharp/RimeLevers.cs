using System.Runtime.InteropServices;

namespace RimeSharp
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate nint CustomSettingsInit(
        [MarshalAs(UnmanagedType.LPStr)] string configId,
        [MarshalAs(UnmanagedType.LPStr)] string generatorId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate nint SwitcerSettingsInit();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CustomSettingsDestory(nint ptr);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool CustomSettingsAccess(nint ptr);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool SchemaListAccess(nint ptr, out RimeSchemaList list);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SchemaListDestroy(ref RimeSchemaList list);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool SelectSchemas(nint ptr,
        [In] string[] schemaIdList, int count);

    public sealed class RimeLevers
    {
        private static readonly Lazy<RimeLevers> s_instance = new(() => new RimeLevers());

        private readonly RimeLeversAPI _levers;

        private RimeLevers()
        {
            var rime = Rime.Instance();
            var module = rime.FindModule("levers");
            var apiPtr = module.GetAPI();
            _levers = Marshal.PtrToStructure<RimeLeversAPI>(apiPtr);
        }

        public class CustomSettings : SafeHandle
        {

            public CustomSettings(nint ptr) : base(ptr, true) { }
            public CustomSettings(string configId, string generatorId) :
                base(Instance()._levers.CustomSettingsInit(configId, generatorId), true)
            { }

            public override bool IsInvalid => handle == IntPtr.Zero;

            public bool LoadSettings() => Instance()._levers.LoadSettings(handle);

            public bool SaveSettings() => Instance()._levers.SaveSettings(handle);

            protected override bool ReleaseHandle()
            {
                Instance()._levers.CustomSettingsDestroy(handle);
                return true;
            }
        }

        public class SwitcherSettings : CustomSettings
        {
            public SwitcherSettings() : base(Instance()._levers.SwitcherSettingsInit()) { }

            public override bool IsInvalid => handle == IntPtr.Zero;

            private SchemaListItem[] GetSchemaList(SchemaListAccess access)
            {
                if (access(handle, out var list))
                {
                    var size = Marshal.SizeOf<RimeSchemaListItem>();
                    var items = new SchemaListItem[(int)list.Size];
                    for (var i = 0; i < (int)list.Size; ++i)
                    {
                        var ptr = IntPtr.Add(list.List, i * size);
                        var item = Marshal.PtrToStructure<RimeSchemaListItem>(ptr);
                        items[i] = new SchemaListItem(item.SchemaId, item.Name);
                    }
                    Instance()._levers.SchemaListDestroy(ref list);
                    return items;
                }
                return [];
            }

            public SchemaListItem[] GetAvailableSchemaList()
                => GetSchemaList(Instance()._levers.GetAvailableSchemaList);

            public SchemaListItem[] GetSelectedSchemaList()
                => GetSchemaList(Instance()._levers.GetSelectedSchemaList);

            public bool SelectSchemas(string[] schemaIdList)
                => Instance()._levers.SelectSchemas(handle, schemaIdList, schemaIdList.Length);

            protected override bool ReleaseHandle()
            {
                Instance()._levers.CustomSettingsDestroy(handle);
                return true;
            }
        }

        public static RimeLevers Instance() => s_instance.Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RimeLeversAPI
    {
        private readonly int _dataSize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CustomSettingsInit CustomSettingsInit;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CustomSettingsDestory CustomSettingsDestroy;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CustomSettingsAccess LoadSettings;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CustomSettingsAccess SaveSettings;
        public IntPtr CustomizeBool;
        public IntPtr CustomizeInt;
        public IntPtr CustomizeDouble;
        public IntPtr CustomizeString;
        public IntPtr IsFirstRun;
        public IntPtr SettingsIsModified;
        public IntPtr SettingsGetConfig;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SwitcerSettingsInit SwitcherSettingsInit;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SchemaListAccess GetAvailableSchemaList;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SchemaListAccess GetSelectedSchemaList;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SchemaListDestroy SchemaListDestroy;
        public IntPtr GetSchemaId;
        public IntPtr GetSchemaName;
        public IntPtr GetSchemaVersion;
        public IntPtr GetSchemaAuthor;
        public IntPtr GetSchemaDescription;
        public IntPtr GetSchemaFilePath;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SelectSchemas SelectSchemas;
        public IntPtr GetHotKeys;
        public IntPtr SetHotKeys;
        public IntPtr UserDictIteratorInit;
        public IntPtr UserDictIteratorDestroy;
        public IntPtr NextUserDict;
        public IntPtr BackupUserDict;
        public IntPtr RestoreUserDict;
        public IntPtr ExportUserDict;
        public IntPtr ImportUserDict;
        public IntPtr CustomizeItem;
    }
}
