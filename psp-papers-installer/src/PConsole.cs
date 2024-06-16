using System;
using System.IO;

namespace psp_papers_installer;

public class PConsole {

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AttachConsole(int dwProcessId);

    private const int ATTACH_PARENT_PROCESS = -1;

    private readonly StreamWriter _stdOutWriter;

    public PConsole() {
        Stream stdout = Console.OpenStandardOutput();
        this._stdOutWriter = new StreamWriter(stdout);
        this._stdOutWriter.AutoFlush = true;

        AttachConsole(ATTACH_PARENT_PROCESS);
    }

    public void WriteLine(string line) {
        this._stdOutWriter.WriteLine(line);
        // Console.WriteLine(line);
    }

    public void Error(string error) {
        this._stdOutWriter.WriteLine($"ERR: {error}");
    }

}