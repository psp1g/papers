using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace psp_papers_installer;

public static class Util {

    public static async Task DownloadFileAsync(this HttpClient client, Uri uri, string filePath, bool replaceOld = false) {
        if (replaceOld && File.Exists(filePath)) File.Delete(filePath);

        await using Stream stream = await client.GetStreamAsync(uri);
        await using FileStream fileStream = new(filePath, FileMode.CreateNew);

        await stream.CopyToAsync(fileStream);
    }

    public static async Task DownloadFileAsync(this HttpClient client, string url, string filePath, bool replaceOld = false) {
        await client.DownloadFileAsync(new Uri(url), filePath, replaceOld);
    }

}