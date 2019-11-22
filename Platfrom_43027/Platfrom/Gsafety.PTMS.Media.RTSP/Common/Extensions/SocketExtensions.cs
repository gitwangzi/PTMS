using Gsafety.PTMS.Media.RTSP.Common;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.Array;
using System;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Gsafety.PTMS.Media.RTSP.Extensions
{
    public static class SocketExtensions
    {
        /// <summary>
        /// Probes for an Open Port without needing a IPGlobalProperties implementation.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="even"></param>
        /// <param name="localIp"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static int ProbeForOpenPort(System.Net.Sockets.ProtocolType type, int start = 30000, bool even = true, System.Net.IPAddress localIp = null)
        {
            if (localIp == null) throw new Exception("localIp = GetFirstUnicastIPAddress(System.Net.Sockets.AddressFamily.InterNetwork)"); // System.Net.IPAddress.Any should give unused ports across all IP's?

            System.Net.Sockets.Socket working = null;

            //Switch on the type
            switch (type)
            {
                //Handle TCP
                case System.Net.Sockets.ProtocolType.Tcp:
                    {
                        working = new System.Net.Sockets.Socket(localIp.AddressFamily, System.Net.Sockets.SocketType.Stream, type);


                        break;
                    }
                //Handle UDP
                case System.Net.Sockets.ProtocolType.Udp:
                    {
                        working = new System.Net.Sockets.Socket(localIp.AddressFamily, System.Net.Sockets.SocketType.Dgram, type);


                        break;
                    }
                //Don't handle
                default: return -1;
            }

            //The port is in the valid range.
            using (working) while (start <= ushort.MaxValue)
                {
                    try
                    {
                        //Try to bind the end point.
                        //working.Bind(new System.Net.IPEndPoint(localIp, start));

                        //We are done if we can bind.
                        break;
                    }
                    catch (System.Exception ex)
                    {
                        //Check for the expected error.
                        if (ex is System.Net.Sockets.SocketException)
                        {
                            System.Net.Sockets.SocketException se = (System.Net.Sockets.SocketException)ex;

                            if (se.SocketErrorCode == System.Net.Sockets.SocketError.AddressAlreadyInUse)
                            {
                                //Try next port
                                if (++start > ushort.MaxValue)
                                {
                                    //No port found
                                    start = -1;

                                    break;
                                }

                                //Ensure even if possible
                                if (even && Binary.IsOdd(ref start) && start < ushort.MaxValue) ++start;

                                //Iterate again
                                continue;
                            }

                        }

                        //Something bad happened
                        start = -1;

                        break;
                    }
                }

            //Return the port.
            return start;
        }

        #region OffloadPreference

        //Todo should be enum..
        //public enum TcpOffloadPreference
        //{
        //    NoPreference = 0,
        //    NotPreferred = 1,
        //    Preferred = 2
        //}

        //
        // Offload preferences supported.
        //
        //#define TCP_OFFLOAD_NO_PREFERENCE	0
        public const int TcpOffloadNoPreference = 0;
        //#define	TCP_OFFLOAD_NOT_PREFERRED	1
        public const int TcpOffloadNotPreferred = 1;
        //#define TCP_OFFLOAD_PREFERRED		2
        public const int TcpOffloadPreferred = 2;

        const int TcpOffloadPreference = 11;

        #endregion

        /// <summary>
        /// Receives the given amount of bytes into the buffer given a offset and an amount.
        /// </summary>
        /// <param name="buffer">The array to receive into</param>
        /// <param name="offset">The location to receive into</param>
        /// <param name="amount">The 0 based amount of bytes to receive, 0 will have no result</param>
        /// <param name="socket">The socket to receive on</param>
        /// <returns>The amount of bytes recieved which will be equal to the amount paramter unless the data was unable to fit in the given buffer</returns>
        public static async Task<int> AlignedReceive(byte[] buffer, int offset, int amount, System.Net.Sockets.Socket socket)
        {
            //Store any socket errors here incase non-blocking sockets are being used.
            var error = System.Net.Sockets.SocketError.SocketError;

            //Return the amount if its negitive;
            if (amount <= 0) return amount;

            //To hold what was received and the maximum amount to receive
            int totalReceived = 0, max, attempt = 0, justReceived = 0;

            if (ArrayExtensions.IsNullOrEmpty(buffer, out max)) return 0;

            //Account for the offset
            max -= offset;

            //Ensure that only max is received
            if (amount > max) amount = max;

            //While there is something to receive
            while (amount > 0 && socket.Connected) //poll write 0
            {
                //Receive it into the buffer at the given offset taking into account what was already received

                CancellationToken cancelltionToken = new CancellationToken();
                var tcs = new TaskCompletionSource<int>();

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.BufferList = new[] { new ArraySegment<byte>(buffer, offset, amount) };

                var cancelRegistration = cancelltionToken.Register(() => Socket.CancelConnectAsync(args));

                EventHandler<SocketAsyncEventArgs> completeHandler = (sender, eventArgs) =>
                {
                    cancelRegistration.Dispose();

                    if (args.SocketError != SocketError.Success)
                    {
                        if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                            tcs.TrySetCanceled();
                        else
                            tcs.TrySetException(new WebException("Socket to " + " failed: " + args.SocketError));
                    }

                    tcs.TrySetResult(args.BytesTransferred);
                };

                args.Completed += completeHandler;

                if (!socket.ReceiveAsync(args))
                {
                    completeHandler(socket, args);
                }

                var receiveCount = await tcs.Task.ConfigureAwait(false);
                justReceived += receiveCount;
                error = args.SocketError;

                //justReceived = socket.ReceiveAsync(buffer, offset, amount);

                switch (error)
                {
                    case System.Net.Sockets.SocketError.ConnectionReset:
                    case System.Net.Sockets.SocketError.ConnectionAborted:
                    case System.Net.Sockets.SocketError.TimedOut:
                        goto Done;
                    default:
                        {
                            //If nothing was received
                            if (justReceived <= 0)
                            {
                                //Try again maybe
                                ++attempt;

                                //Only if the attempts in operations were greater then the amount of bytes requried
                                if (attempt > amount) goto Done;

                                continue;
                            }

                            //decrease the amount by what was received
                            amount -= justReceived;

                            //Increase the offset by what was received
                            offset += justReceived;

                            //Increase total received
                            totalReceived += justReceived;

                            continue;
                        }
                }
            }

        Done:
            return totalReceived;
        }

        /// <summary>
        /// Provides a way to Call SendTo specifying an <see cref="System.Net.Sockets.SocketError"/>.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="socket"></param>
        /// <param name="remote"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static async Task<int> SendTo(byte[] buffer, int offset, int size, Socket socket, EndPoint remote = null)
        {
            CancellationToken cancelltionToken = new CancellationToken();
            var tcs = new TaskCompletionSource<int>();

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.BufferList = new[] { new ArraySegment<byte>(buffer, offset, size) };
            if (remote != null)
            {
                args.RemoteEndPoint = remote;
            }

            var cancelRegistration = cancelltionToken.Register(() => Socket.CancelConnectAsync(args));

            EventHandler<SocketAsyncEventArgs> completeHandler = (sender, eventArgs) =>
            {
                cancelRegistration.Dispose();

                if (args.SocketError != SocketError.Success)
                {
                    if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetException(new WebException("Socket to " + " failed: " + args.SocketError));
                }

                tcs.TrySetResult(args.BytesTransferred);
            };

            args.Completed += completeHandler;

            if (!socket.SendAsync(args))
            {
                completeHandler(socket, args);
            }

            return await tcs.Task.ConfigureAwait(false);
        }

        public static async Task<int> SendTo(IList<ArraySegment<byte>> bufferList, Socket socket)
        {
            CancellationToken cancelltionToken = new CancellationToken();
            var tcs = new TaskCompletionSource<int>();

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.BufferList = bufferList;

            var cancelRegistration = cancelltionToken.Register(() => Socket.CancelConnectAsync(args));

            EventHandler<SocketAsyncEventArgs> completeHandler = (sender, eventArgs) =>
            {
                cancelRegistration.Dispose();

                if (args.SocketError != SocketError.Success)
                {
                    if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetException(new WebException("Socket to " + " failed: " + args.SocketError));
                }

                tcs.TrySetResult(args.BytesTransferred);
            };

            args.Completed += completeHandler;

            if (!socket.SendAsync(args))
            {
                completeHandler(socket, args);
            }

            return await tcs.Task.ConfigureAwait(false);
        }

        public static async Task Connect(this Socket socket, EndPoint endPoint)
        {
            var args = new SocketAsyncEventArgs()
            {
                RemoteEndPoint = endPoint
            };

            var tcs = new TaskCompletionSource<bool>();
            CancellationToken cancelltionToken = new CancellationToken();
            var cancelRegistration = cancelltionToken.Register(() => Socket.CancelConnectAsync(args));

            EventHandler<SocketAsyncEventArgs> completeHandler = (sender, eventArgs) =>
            {
                cancelRegistration.Dispose();

                if (args.SocketError != SocketError.Success)
                {
                    if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetException(new WebException("Socket to " + " failed: " + args.SocketError));
                }

                var isOk = false;
                if (args.BytesTransferred > 0)
                {
                    isOk = true;
                }
                tcs.TrySetResult(isOk);
            };

            args.Completed += completeHandler;

            if (!socket.ConnectAsync(args))
            {
                completeHandler(socket, args);
            }

            await tcs.Task.ConfigureAwait(false);
        }
    }
}
