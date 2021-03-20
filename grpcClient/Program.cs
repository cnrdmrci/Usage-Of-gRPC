using System;
using Grpc.Net.Client;

namespace grpcClient
{
    class Program
    {
        private static string _url = "http://localhost:5000";
        static void Main(string[] args)
        {
            Console.WriteLine("Unary Service");
            CallUnaryService();


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
    }
}
