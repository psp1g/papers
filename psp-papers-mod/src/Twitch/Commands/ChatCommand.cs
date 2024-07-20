using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace psp_papers_mod.Twitch.Commands;

[System.AttributeUsage(System.AttributeTargets.Method)]
public class ChatCommand(string identifier) : System.Attribute {
    
    private string identifier = identifier;
    private MethodInfo callbackMethodInfo;

    public static Dictionary<string, ChatCommand> commands = new();

    public static void FindAll() {
        MethodInfo[] methods = Assembly.GetExecutingAssembly()
            .GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(typeof(ChatCommand), false).Length > 0)
            .ToArray();

        foreach (MethodInfo methodInfo in methods) {
            ChatCommand cmd =
                (ChatCommand) methodInfo.GetCustomAttributes(typeof(ChatCommand), false)
                    .FirstOrDefault();

            if (cmd == null) continue;
            cmd.callbackMethodInfo = methodInfo;

            commands.Add(cmd.identifier, cmd);
        }
    }

    /// <summary>
    /// Processes chat inputs and runs commands
    /// </summary>
    /// <param name="chatter"></param>
    /// <param name="input">User chat input</param>
    /// <returns>true if a command was found in the message</returns>
    public static bool ProcessCommand(Chatter chatter, string input) {
        if (input.Length < 1 || !input.StartsWith(Cfg.CommandPrefix.Value.ToLower())) return false;
        
        // Split args by spaces except when within quotes
        // ie: input of `!ban littlebigbug "stupid idiot"
        // returns:
        // 1: !ban
        // 2: littlebigbug
        // 3: stupid idiot
        string[] args = Regex.Matches(input, @"[\""].+?[\""]|[^ ]+")
            .Select(m => m.Value)
            .ToArray();

        if (args.Length < 1) return false;

        commands.TryGetValue(args[0], out ChatCommand cmd);
        if (cmd == null) return false;

        cmd.Call(chatter, args.Skip(1).ToArray());
        return true;
    }

    private void Call(Chatter chatter, string[] args) {
        this.callbackMethodInfo.Invoke(null, [ chatter, ..args ]);
    }
    
}