//using System;
//using System.IO.Pipes;
//using System.Threading.Tasks;
//using System.Threading;
//using System.IO;

//namespace ClientApp;

//internal static class ClientPipe
//{
//    private const string PipeName = "pipe72346";
//    private const string ServerName = ".";

//    private static async Task RunPipe()
//    {
//        WriteLine("Started.");
//        var cancellationToken = CancellationToken.None;

//        try
//        {
//            using (NamedPipeClientStream pipeClient =
//           new NamedPipeClientStream(".", "testpipe", PipeDirection.In))
//            {
//                Console.Write("Attempting to connect to pipe...");
//                pipeClient.Connect();
//                var timeout = TimeSpan.FromSeconds(30);

//                using (StreamReader sr = new StreamReader(pipeClient))
//                {
//                    // Display the read text to the console
//                    string temp;
//                    while ((temp = sr.ReadLine()) != null)
//                    {
//                        Console.WriteLine("Received from server: {0}", temp);
//                    }
//                }

//            }
//            do
//            {
//                WriteLine("Wait for sync...");
//                text = await stringPipe.ReadStringAsync();
//            }
//            while (!text.StartsWith("SYNC"));
//            WriteLine("Synced with server.");
//            do
//            {
//                WriteLine("Reading data.");
//                text = await stringPipe.ReadStringAsync();
//                WriteLine($"Received: {text}");
//            }
//            while (!text.Equals("EXIT", StringComparison.CurrentCultureIgnoreCase));

//            WriteLine("Write data to Server.");
//            await stringPipe.WriteStringAsync("Bye-bye.");
//            WriteLine("Writing data to Server complete.");
//        }
//        catch (Exception exception)
//        {
//            WriteLine(exception.Message);
//        }

//        WriteLine("Quit.");
//    }

//    private static void WriteLine(string text)
//    {
//        Console.WriteLine($"{DateTime.Now:mm.ss.ffffff} | [CLIENT] {text}");
//    }
//}