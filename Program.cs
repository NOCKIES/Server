using System;
using Gtk;
using System.Reflection;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Ascra
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			//Application.Init ();
			//MainWindow win = new MainWindow ();
			//win.Show ();
			//Application.Run ();
		
			PortListener listener = new PortListener ();
			listener.Listen ();
		}
	}






	public class PortListener
	{
		private Byte[] bytes = new Byte[256];
		private String data = null;
		private Int32 port = 13000;
		private IPAddress localAddr = IPAddress.Parse("127.0.0.1");
		private NetworkStream stream;

		public String getData()
		{
			return data;
		}

		public Byte[] getBytes()
		{
			return bytes;
		}

		public void Listen()
		{

			TcpListener server=null;
			try
			{
				
				// TcpListener server = new TcpListener(port);
				server = new TcpListener(localAddr, port);
				// Start listening for client requests.
				server.Start();

				// Enter the listening loop.
				while(true) 
				{
					Console.Write("Waiting for a connection... ");

					// Perform a blocking call to accept requests.
					// You could also user server.AcceptSocket() here.
					TcpClient client = server.AcceptTcpClient();            
					Console.WriteLine("Connected!");

					data = null;

					// Get a stream object for reading and writing
					stream = client.GetStream();

					int i;

					// Loop to receive all the data sent by the client.
					while((i = stream.Read(bytes, 0, bytes.Length))!=0) 
					{   
						// Translate data bytes to a ASCII string.
						data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

						// Process the data sent by the client.
						byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
						            
					}

					Create();


					// Shutdown and end connection
					client.Close();
				}
			}
			catch(SocketException e)
			{
				Console.WriteLine("SocketException: {0}", e);
			}
			finally
			{
				// Stop listening for new clients.
				server.Stop();
			}

		}

		public void Create ()
		{
			String datas;
			System.Console.Write("Create...");
			string PathNameForTXT;
			PathNameForTXT = System.IO.Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);

			DateTime time = DateTime.Now;
			string fileName = time.ToString ("yyyy_MM_dd_HH_mmss");
			PathNameForTXT = PathNameForTXT + "/" + time.ToString ("yyyy_MM_dd_HH_mm_ss") + ".txt";



			datas = System.Text.Encoding.UTF8.GetString(bytes);
			Console.WriteLine (datas);


			if (!File.Exists(PathNameForTXT))
			{
					// Create a file to write to.
					File.WriteAllText(PathNameForTXT, data);
					System.Console.WriteLine("Done");
			}
			else
			{
				Console.WriteLine("File \"{0}\" already exists.", fileName);
				return;
			}
		}
	}

}
