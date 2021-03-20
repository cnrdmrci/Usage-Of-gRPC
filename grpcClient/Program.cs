using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace grpcClient
{
    class Program
    {
        private static string _url = "http://localhost:5000";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Unary Service");
            CallUnaryService();

            Console.WriteLine("Server Stream Service");
            await CallServerStreamService();

            Console.WriteLine("Client Stream Service");
            await CallClientStreamService();

            Console.WriteLine("Bi-Directional Stream Service");
            await CallBiDirectionalStreamService();
        }

        static void CallUnaryService()
        {
            using(var channel = GrpcChannel.ForAddress(_url))
            {
                var client = new Unary.UnaryClient(channel);

                int number1 = 5;
                int number2 = 8;
                Console.WriteLine($"Number1: {number1} , Number2: {number2}");

                var addReply = client.Add(new UnaryRequest(){Number1 = 5, Number2 = 8});
                Console.WriteLine($"Add Result: {addReply.Result}");

                var subReply = client.Sub(new UnaryRequest(){Number1 = 5, Number2 = 8});
                Console.WriteLine($"Sub Result: {subReply.Result}");
            }
        }

        static async Task CallServerStreamService()
        {
            using(var channel = GrpcChannel.ForAddress(_url))
            {
                var client = new ServerStream.ServerStreamClient(channel);
                var request = new ServerStremRequest();
                request.NumberList.Add(
                    new Numbers()
                    {
                        Number1 = 12,
                        Number2 = 6
                    }
                );
                request.NumberList.Add(new Numbers()
                    {
                        Number1 = 43,
                        Number2 = 23
                    }
                );
                request.NumberList.Add(new Numbers()
                    {
                        Number1 = 22,
                        Number2 = 45
                    }
                );
                request.NumberList.Add(new Numbers()
                    {
                        Number1 = 54,
                        Number2 = 33
                    }
                );
                request.NumberList.Add(new Numbers()
                    {
                        Number1 = 11,
                        Number2 = 1
                    }
                );

                var addReply = client.Add(request);

                await Task.Run(async () =>
                {
                    while (await addReply.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
                        Console.WriteLine($"Add result : {addReply.ResponseStream.Current.Result}");
                });

                var subReply = client.Sub(request);

                await Task.Run(async () =>
                {
                    while (await subReply.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
                        Console.WriteLine($"Sub result : {subReply.ResponseStream.Current.Result}");
                });
            }
        }
    
        static async Task CallClientStreamService()
        {
            using(var channel = GrpcChannel.ForAddress(_url))
            {
                var client = new ClientStream.ClientStreamClient(channel);

                var request = client.Add();

                await Task.Run(async () =>
                {
                    for (var i = 0; i < 5; i++)
                    {
                        await request.RequestStream.WriteAsync(new ClientStreamRequest { Number = i });
                    }
                });
        
                await request.RequestStream.CompleteAsync();
        
                var response = await request;
                Console.WriteLine($"Result : {response.Result}");
            }
        }

        static async Task CallBiDirectionalStreamService()
        {
            using(var channel = GrpcChannel.ForAddress(_url))
            {
                var client = new BiDirectionalStream.BiDirectionalStreamClient(channel);
                var reply = client.Add();

                await reply.RequestStream.WriteAsync(new BiDirectionalStreamRequest { Number1 = 3, Number2 = 44});
                await reply.ResponseStream.MoveNext(new CancellationToken());
                Console.WriteLine($"Result: {reply.ResponseStream.Current.Result}");

                await reply.RequestStream.WriteAsync(new BiDirectionalStreamRequest { Number1 = 11, Number2 = 32});
                await reply.ResponseStream.MoveNext(new CancellationToken());
                Console.WriteLine($"Result: {reply.ResponseStream.Current.Result}");

                await reply.RequestStream.WriteAsync(new BiDirectionalStreamRequest { Number1 = 23, Number2 = 12});
                await reply.ResponseStream.MoveNext(new CancellationToken());
                Console.WriteLine($"Result: {reply.ResponseStream.Current.Result}");

                await reply.RequestStream.WriteAsync(new BiDirectionalStreamRequest { Number1 = 21, Number2 = 9});
                await reply.ResponseStream.MoveNext(new CancellationToken());
                Console.WriteLine($"Result: {reply.ResponseStream.Current.Result}");

                await reply.RequestStream.CompleteAsync();
            }
        }
    }
}
