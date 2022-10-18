using System;
using System.Text;
using SBE_Sample;
using SBE_Sample.sbe;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Validators;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Loggers;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class Program
{
    public static InternalMsgEnDecoder sbeEnDecoder = new InternalMsgEnDecoder();
    public static InternalMsg msgNVS = createSampleMsg();
    public static InternalMsg msgSBE = createSampleMsg();
    public static byte[] bytesNVS = InternalMsg.ToByteFromString(msgNVS);
    public static byte[] bytesSBE =  sbeEnDecoder.encode(msgSBE);
    public static void Main(String[] args)
    {

        var config = new ManualConfig()
         .WithOptions(ConfigOptions.DisableOptimizationsValidator)
         .AddValidator(JitOptimizationsValidator.DontFailOnError)
         .AddLogger(ConsoleLogger.Default)
         .AddColumnProvider(DefaultColumnProviders.Instance);
        
        
        //TestNVSEncodeDecode();
        //TestSBE();

        BenchmarkRunner.Run<Program>(config);
    }

    [Benchmark]
    public void benchNVSEncode()
    {
        byte[] bytes = InternalMsg.ToByteFromString(msgNVS);
    }

    [Benchmark]
    public void benchNVSDecode()
    {
        InternalMsg msg = InternalMsg.decode(bytesNVS);
    }

    [Benchmark]
    public void benchSBEEncode()
    {
        byte[] bytes = sbeEnDecoder.encode(msgSBE);
    }

    [Benchmark]
    public void benchSBEDecode()
    {
        InternalMsg msg = sbeEnDecoder.decode(bytesSBE);
    }

    public static void TestNVSEncodeDecode()
    {

        InternalMsg msg = createSampleMsg();
        String msgString = msg.ToString();
        byte[] bytes = InternalMsg.ToByteFromString(msg);
        Console.WriteLine("TestNVSEncodeDecode "+ bytes.Length);
        InternalMsg msg2 = InternalMsg.decode(bytes);
        String msgDecodedString = msg2.ToString();
        Console.WriteLine("NVS Decode " + msgDecodedString);
        Console.WriteLine("NVS Compare origin msg and decoded msg: " + msgString.Equals(msgDecodedString));
    }

    public static void TestSBE()
    {
        for(int i = 0; i < 100; i++)
        {
            InternalMsg msg = createSampleMsg();
            String msgString = msg.ToString();
            byte[] bytes = sbeEnDecoder.encode(msg);
            Console.WriteLine("TestSBE " + bytes.Length);
            InternalMsg msg2 = sbeEnDecoder.decode(bytes);
            String msgDecodedString = msg2.ToString();
            Console.WriteLine("InternalMsgEnDecoder Decode " + msgDecodedString);
            Console.WriteLine("InternalMsgEnDecoder Compare origin msg and decoded msg: " + msgString.Equals(msgDecodedString));
        }
        
    }

    public static  InternalMsg createSampleMsg()
    {
        InternalMsg msg = new InternalMsg();
        msg.id = 1000;
        msg.available = true;
        msg.type = 5;
        msg.quantity = 10.6;
        msg.price = 23200.5;
        msg.symbol = "ACB";
        msg.reqId = "109u19udfhjsadf090u";
        msg.custId = "9803428934";

        return msg;
    }
}
