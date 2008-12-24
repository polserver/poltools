/***************************************************************************
 *
 * $Author: MuadDib
 *
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace POLUtils.AuxSvc
{
    /// <summary>
    ///     Provides a stream that implements the AuxSvc protocol.
    /// </summary>
    public class AuxSvcConnection
    {

        /// <summary>
        ///     Privately holds the TCP socket for the AuxSvcConnection
        /// </summary>
        private TcpClient AuxSocket;

        /// <summary>
        ///     Privately holds the TCP Stream of the AuxSvcConnection
        /// </summary>
        private NetworkStream AuxStream;

        /// <summary>
        ///     Privately holds the StreamReader for the AuxSvcConnection
        /// </summary>
        private StreamReader AuxReader;

        /// <summary>
        ///     Privately holds the StreamWriter for the AuxSvcConnection
        /// </summary>
        private StreamWriter AuxWriter;

        /// <summary>
        ///     The default constructor is marked as private to
        ///     ensure the parameterized constructor is called.
        /// </summary>
        private AuxSvcConnection()
        {
        }

        /// <summary>
        ///     Creates an AuxSvc object against the specified socket to designated Server IP and Server Port.
        ///     It then initializes and connects the AuxSocket and uses the AuxSocket to initialize the AuxStream for our Read/Write work.
        /// </summary>
        /// <param name="server_ip">
        ///     The ip that the AuxSvcStream will try to connect to.
        /// </param>
        /// <param name="server_port">
        ///     The port that the AuxSvcStream will try to connect to.
        /// </param>
        public AuxSvcConnection(string server_ip, int server_port)
        {
            try
            {
                AuxSocket = new TcpClient(server_ip, server_port);
            }
            catch
            {
                // FIXME: We need to make an exception build here for checking in the 
                // end-user's code. 
                return;
            }
            try
            {
                if (AuxSocket.Connected)
                {
                    AuxStream = AuxSocket.GetStream();
                    AuxReader = new StreamReader(AuxStream);
                    AuxWriter = new StreamWriter(AuxStream);
                    AuxWriter.AutoFlush = true;
                }
                else
                    throw new IOException("Aux Svc Connection Is Not Active!");
            }
            catch
            {
                // FIXME:  We need to make an exception build here for checking in the 
                // end-user's code. 
                return;
            }
        }

        /// <summary>
        ///     Returns the current contents of the AuxStream in string format.
        /// </summary>
        public string Read()
        {
            try
            {
                if (AuxSocket.Connected)
                    return AuxReader.ReadLine();
                else
                    throw new IOException("Aux Svc Connection Is Not Active For Read!");
            }
            catch (System.OutOfMemoryException ex)
            {
                // FIXME:  We need to make an exception build here for checking in the 
                // end-user's code. 
                return ("System.OutOfMemoryException: " + ex);
            }
            catch (System.IO.IOException ex)
            {
                // FIXME:  We need to make an exception build here for checking in the 
                // end-user's code. 
                return ("System.IO.IOException: " + ex);
            }
        }

        /// <summary>
        ///     Attempts to send a string using the AuxWriter Stream. Returns a bool for success.
        /// </summary>
        /// <param name="input">
        ///     The POL Style Packed string to send to the POL Server.
        /// </param>

        public bool Write(string input)
        {
            try
            {
                if (AuxSocket.Connected)
                {
                    AuxWriter.WriteLine(input);
                    return true;
                }
                else
                    throw new IOException("Aux Svc Connection Is Not Active For Write!");
            }
            catch (System.ObjectDisposedException)
            {
                // FIXME:  We need to make an exception build here for checking in the 
                // end-user's code. 
                return false;
            }
            catch (System.NotSupportedException)
            {
                // FIXME:  We need to make an exception build here for checking in the 
                // end-user's code. 
                return false;
            }
            catch (System.IO.IOException)
            {
                // FIXME:  We need to make an exception build here for checking in the 
                // end-user's code. 
                return false;
            }
        }

        /// <summary>
        ///     Closes all AuxSvcConnection related Streams. Can be used to manually kill 
        ///     an AuxSvcConnection if an error is encountered before using Write(). Will 
        ///     result in the AuxSvc Script in POL to exit the While(connection) Loop and 
        ///     terminate.
        /// </summary>
        public void Close()
        {
            AuxReader.Close();
            AuxWriter.Close();
            AuxStream.Close();
            AuxSocket.Close();
        }

        /// <summary>
        ///     Returns whether or not the AuxSvcConnection is connected to the remote host
        ///     on the underlying socket.
        /// </summary>
        public bool Active
        {
            get
            {
                if (AuxSocket == null)
                    return false;
                else
                    return this.AuxSocket.Connected;
            }
        }

    }
}
