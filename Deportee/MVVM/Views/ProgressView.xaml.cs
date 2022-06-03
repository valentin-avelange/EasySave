using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Diagnostics;

namespace Client.MVVM.Views
{
    /// <summary>
    /// Logique d'interaction pour ProgressView.xaml
    /// </summary>
    public partial class ProgressView : Page
    {
        private Window win;

        // The port number for the remote device.  
        private const int port = 11000;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        // The response from the remote device.  
        private static String response = String.Empty;
        public ProgressView(Window newWin)
        {
            this.win = newWin;
            InitializeComponent();
        }

        public void SearchSave(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(NameSave.Text);
            if (NameSave.Text != null || NameSave.Text != "")
            {
                AsynchroneClient a = new AsynchroneClient((progress) => { PB_Save.Value = progress; });
                a.StartClient(NameSave.Text); 
            }
             
        }

        public void setPourcent(int pourcent)
        {
            PB_Save.Value = pourcent;
        }

        public class StateObject
        {
            // Client socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 256;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }

        public class AsynchroneClient
        {

            private Action<int> callback;

            public AsynchroneClient(Action<int> newCallback)
            {
                callback = newCallback;
            }

            public void StartClient(string name)
            {
                
                // Connect to a remote device.  
                try
                {
                    // Establish the remote endpoint for the socket.  
                    // The name of the
                    // remote device is "host.contoso.com".  
                    //IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");  
                    //IPAddress ipAddress = ipHostInfo.AddressList[0];  
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Loopback, port);

                    // Create a TCP/IP socket.  
                    Socket client = new Socket(IPAddress.Loopback.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                    // Connect to the remote endpoint.  
                    client.BeginConnect(remoteEP,
                        new AsyncCallback(ConnectCallback), client);
                    connectDone.WaitOne();

                    // Send test data to the remote device.  
                    Send(client, name);
                    sendDone.WaitOne();

                    // Receive the response from the remote device.  
                    Receive(client);
                    receiveDone.WaitOne();

                    // Write the response to the console.  
                    Trace.WriteLine("c1:Response received : {0}");
                    try
                    {
                        int result = Int32.Parse(response);
                        callback(result);
                        Trace.WriteLine(result + "@@@@@@@@@@@@@@@@@@");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"c2:Unable to parse '{response}'");
                    }
                    // Release the socket.  
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();

                }
                catch (Exception e)
                {
                    Trace.WriteLine("c3:" + e.ToString());
                }
              
            }

            private static void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.  
                    Socket client = (Socket)ar.AsyncState;

                    // Complete the connection.  
                    client.EndConnect(ar);

                    Trace.WriteLine("c4:Socket connected to EasySave",
                            client.RemoteEndPoint.ToString());

                    // Signal that the connection has been made.  
                    connectDone.Set();
                }
                catch (Exception e)
                {
                    Trace.WriteLine("c5:" + e.ToString());
                }
            }

            private void Receive(Socket client)
            {
                try
                {
                    // Create the state object.  
                    StateObject state = new StateObject();
                    state.workSocket = client;

                    // Begin receiving the data from the remote device.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("c6:" + e.ToString());
                }
            }

            private void ReceiveCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the state object and the client socket
                    // from the asynchronous state object.  
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket client = state.workSocket;
                    string content = "";

                    // Read data from the remote device.  
                    int bytesRead = client.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        // There might be more data, so store the data received so far.  
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                        // Get the rest of the data.  
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                        // There  might be more data, so store the data received so far.  
                        content = state.sb.ToString();
                    }
                    else
                    {
                        // All the data has arrived; put it in response.  
                        if (state.sb.Length >= 1)
                        {
                            response = state.sb.ToString();
                        }
                        // Signal that all bytes have been received.  
                        receiveDone.Set();
                    }
                    client.Disconnect(true);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("c7:" + e.ToString());
                }
            }


            private void Send(Socket client, String data)
            {
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.  
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }

            private void SendCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.  
                    Socket client = (Socket)ar.AsyncState;

                    // Complete sending the data to the remote device.  
                    int bytesSent = client.EndSend(ar);
                    Trace.WriteLine("c8: Sent {0} bytes to server.");
                    // Signal that all bytes have been sent.  
                    sendDone.Set();
                }
                catch (Exception e)
                {
                    Trace.WriteLine("c9:" + e.ToString());
                }
            }
        }




    }
}
