using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public sealed class SocketWrapper : ISocket
    {
        Socket _socket;
        Uri _url;

        public void Dispose()
        {
            Close();
        }

        public Task ConnectAsync(Uri url, CancellationToken cancellationToken)
        {
            _url = url;

            var args = new SocketAsyncEventArgs
            {
                RemoteEndPoint = new DnsEndPoint(url.Host, url.Port)
            };

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (null != Interlocked.CompareExchange(ref _socket, socket, null))
            {
                socket.Dispose();
                throw new InvalidOperationException("The socket is in use");
            }

            return DoAsync(_socket.ConnectAsync, args, cancellationToken);
        }

        public async Task<int> WriteAsync(byte[] buffer, int offset, int length, CancellationToken cancellationToken)
        {
            var args = new SocketAsyncEventArgs
            {
                BufferList = new[] { new ArraySegment<byte>(buffer, offset, length) }
            };

            await DoAsync(_socket.SendAsync, args, cancellationToken).ConfigureAwait(false);

            return args.BytesTransferred;
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int length, CancellationToken cancellationToken)
        {
            var args = new SocketAsyncEventArgs
            {
                BufferList = new[] { new ArraySegment<byte>(buffer, offset, length) }
            };

            await DoAsync(_socket.ReceiveAsync, args, cancellationToken).ConfigureAwait(false);

            return args.BytesTransferred;
        }

        public void Close()
        {
            var socket = Interlocked.Exchange(ref _socket, null);

            if (null != socket)
                socket.Dispose();
        }

        Task DoAsync(Func<SocketAsyncEventArgs, bool> op, SocketAsyncEventArgs args, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();

            var cancelRegistration = cancellationToken.Register(() => Socket.CancelConnectAsync(args));

            EventHandler<SocketAsyncEventArgs> completedHandler = (sender, eventArgs) =>
            {
                cancelRegistration.Dispose();

                if (SocketError.Success != args.SocketError)
                {
                    if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetException(new WebException("Socket to " + _url + " failed: " + args.SocketError));
                }
                else
                    tcs.TrySetResult(null);
            };

            args.Completed += completedHandler;

            if (!op(args))
                completedHandler(_socket, args);

            return tcs.Task;
        }
    }
}
