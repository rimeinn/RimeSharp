namespace RimeSharp.Test
{
    internal class Program
    {
        static void PrintStatus(in RimeStatus status)
        {
            Console.WriteLine($"schema: {status.SchemaId} / {status.SchemaName}");
            Console.Write("status: ");
            if (status.IsDisabled)
                Console.Write("disabled ");
            if (status.IsComposing)
                Console.Write("composing ");
            if (status.IsAsciiMode)
                Console.Write("ascii ");
            if (status.IsFullShape)
                Console.Write("full_shape ");
            if (status.IsSimplified)
                Console.Write("simplified ");
            Console.WriteLine();
        }

        static void PrintComposition(in RimeComposition composition)
        {
            var preedit = composition.Preedit;
            if (preedit == null)
                return;
            var len = preedit.Length;
            var start = composition.SelStart;
            var end = composition.SelEnd;
            var cursor = composition.CursorPos;
            for (var i = 0; i <= len; ++i)
            {
                if (start < end)
                {
                    if (i == start)
                    {
                        Console.Write("[");
                    }
                    else if (i == end)
                    {
                        Console.Write("]");
                    }
                }
                if (i == cursor)
                {
                    Console.Write("|");
                }
                if (i < len)
                {
                    Console.Write($"{preedit[i]}");
                }
            }
            Console.WriteLine();
        }
        static void PrintMenu(in RimeMenu menu)
        {
            var candidates = menu.Candidates;
            if (candidates.Length == 0)
                return;
            Console.WriteLine($"page: {menu.PageNo + 1}" +
                (menu.IsLastPage ? "$" : " ") + $" (of size {menu.PageSize})");
            for (var i = 0; i < candidates.Length; ++i)
            {
                bool highlighted = i == menu.HighlightedCandidateIndex;
                Console.WriteLine($"{i + 1}. " + (highlighted ? "[" : " ") +
                    candidates[i].Text + (highlighted ? "]" : " ") +
                    candidates[i].Comment);
            }
        }
        static void PrintContext(in RimeContext context)
        {
            if (context.Composition.Length > 0 || context.Menu.NumCandidates > 0)
            {
                PrintComposition(context.Composition);
            }
            else
            {
                Console.WriteLine("(not composing)");
            }
            PrintMenu(context.Menu);
        }

        static void Print(UIntPtr sessionId)
        {
            var rime = Rime.Instance();

            using (var commit = rime.GetCommit(sessionId))
            {
                Console.WriteLine($"commit: {commit.Text}");
            }

            using (var status = rime.GetStatus(sessionId))
            {
                PrintStatus(status);
            }

            using var context = rime.GetContext(sessionId);
            PrintContext(context);
        }
        static bool ExecuteSpecialCommand(string line, UIntPtr sessionId)
        {
            var rime = Rime.Instance();
            if (line == "print schema list")
            {
                var list = rime.GetSchemaList();
                Console.WriteLine("schema list:");
                for (var i = 0; i < list.Length; ++i)
                {
                    Console.WriteLine($"{i + 1}. {list[i].SchemaId} [{list[i].Name}]");
                }
                var schema = rime.GetCurrentSchema(sessionId);
                Console.WriteLine($"current schema: [{schema}]");
                return true;
            }
            if (line == "print available schemas")
            {
                using var swicher = new RimeLevers.SwitcherSettings();
                swicher.LoadSettings();
                var list = swicher.GetAvailableSchemaList();
                Console.WriteLine("available schemas:");
                for (var i = 0; i < list.Length; ++i)
                {
                    Console.WriteLine($"{i + 1}. {list[i].SchemaId} [{list[i].Name}]");
                }
                return true;
            }
            if (line == "print selected schemas")
            {
                using var swicher = new RimeLevers.SwitcherSettings();
                swicher.LoadSettings();
                var list = swicher.GetSelectedSchemaList();
                Console.WriteLine("selected schemas:");
                for (var i = 0; i < list.Length; ++i)
                {
                    Console.WriteLine($"{i + 1}. {list[i].SchemaId} [{list[i].Name}]");
                }
                return true;
            }
            if (line.StartsWith("select schema "))
            {
                var schemaId = line["select schema ".Length..];
                if (rime.SelectSchema(sessionId, schemaId))
                {
                    Console.WriteLine($"selected schema: [{schemaId}]");
                }
                return true;
            }
            if (line.StartsWith("select candidate "))
            {
                var indexStr = line["select candidate ".Length..];
                if (int.TryParse(indexStr, out int index))
                {
                    if (index > 0 && rime.SelectCandidate(sessionId, index - 1))
                    {
                        Print(sessionId);
                    }
                    else
                    {
                        Console.WriteLine($"failed to select candidate at {index}");
                    }
                }
                else
                {
                    Console.WriteLine($"invalid candidate index: {indexStr}");
                }
                return true;
            }
            if (line == "print candidate list")
            {
                var candidates = rime.GetCandidates(sessionId);
                if (candidates.Length > 0)
                {
                    for (var i = 0; i < candidates.Length; ++i)
                    {
                        Console.Write($"{i + 1}. {candidates[i].Text}");
                        if (!string.IsNullOrEmpty(candidates[i].Comment))
                        {
                            Console.Write($" {candidates[i].Comment}");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("no candidates.");
                }
                return true;
            }
            if (line.StartsWith("set option "))
            {
                bool isOn = true;
                var option = line["set option ".Length..];
                if (option[0] == '!')
                {
                    isOn = false;
                    option = option[1..];
                }
                rime.SetOption(sessionId, option, isOn);
                Console.WriteLine($"{option} set " + (isOn ? "on." : "off."));
                return true;
            }
            if (line == "prev")
            {
                if (rime.ChangePage(sessionId, true))
                {
                    Print(sessionId);

                }
                return true;
            }

            if (line == "next")
            {
                if (rime.ChangePage(sessionId, false))
                {
                    Print(sessionId);

                }
                return true;
            }
            return false;
        }
        static void OnMessage(
            UIntPtr contextObject,
            UIntPtr sessionId,
            string messageType,
            string messageValue
        )
        {
            Console.WriteLine($"message: [{sessionId}] [{messageType}] [{messageValue}]");
            var rime = Rime.Instance();
            if (messageType == "option")
            {
                bool state = messageValue[0] != '!';
                var optionName = messageValue[(state ? 0 : 1)..];
                var stateLabel = rime.GetStateLabel(sessionId, optionName, state);
                if (!string.IsNullOrEmpty(stateLabel))
                {
                    Console.WriteLine($"update option: ${optionName} = {state} // {stateLabel}");
                }
            }
        }


        static void Main(string[] args)
        {

            var traits = new RimeTraits
            {
                AppName = "rime.console",
                SharedDataDir = "./shared",
                UserDataDir = "./user"
            };

            var rime = Rime.Instance();

            rime.Setup(ref traits);

            rime.SetNotificationHandler(OnMessage);

            Console.WriteLine("initializing...");
            rime.Initialize(ref traits);

            bool fullCheck = true;
            if (rime.StartMaintenance(fullCheck))
            {
                rime.JoinMaintenanceThread();
            }
            Console.WriteLine("ready.");

            var sessionId = rime.CreateSession();
            if (sessionId == 0)
            {
                Console.WriteLine("failed to create session.");
                return;
            }

            while (true)
            {
                var line = Console.ReadLine();
                if (line == null || line == "exit")
                    break;
                if (ExecuteSpecialCommand(line, sessionId))
                    continue;
                if (rime.SimulateKeySequence(sessionId, line))
                {
                    Print(sessionId);
                }
                else
                {
                    Console.WriteLine($"failed to simulate key sequence: {line}");
                }
            }

            rime.DestroySession(sessionId);
            rime.Finalize1();
        }
    }
}
