// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Calculator.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Calculator {
  public static partial class Calculator
  {
    static readonly string __ServiceName = "Calculator.Calculator";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.Request> __Marshaller_Calculator_Request = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.Request.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.Response> __Marshaller_Calculator_Response = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.Response.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.PrimeNumberRequest> __Marshaller_Calculator_PrimeNumberRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.PrimeNumberRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.PrimeNumberResponse> __Marshaller_Calculator_PrimeNumberResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.PrimeNumberResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.AverageRequest> __Marshaller_Calculator_AverageRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.AverageRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.AverageResponse> __Marshaller_Calculator_AverageResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.AverageResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.MaximumRequest> __Marshaller_Calculator_MaximumRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.MaximumRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.MaximumResponse> __Marshaller_Calculator_MaximumResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.MaximumResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.RunDeadlineRequest> __Marshaller_Calculator_RunDeadlineRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.RunDeadlineRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Calculator.RunDeadlineResponse> __Marshaller_Calculator_RunDeadlineResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Calculator.RunDeadlineResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Calculator.Request, global::Calculator.Response> __Method_Add = new grpc::Method<global::Calculator.Request, global::Calculator.Response>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Add",
        __Marshaller_Calculator_Request,
        __Marshaller_Calculator_Response);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Calculator.PrimeNumberRequest, global::Calculator.PrimeNumberResponse> __Method_GetPrimes = new grpc::Method<global::Calculator.PrimeNumberRequest, global::Calculator.PrimeNumberResponse>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetPrimes",
        __Marshaller_Calculator_PrimeNumberRequest,
        __Marshaller_Calculator_PrimeNumberResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Calculator.AverageRequest, global::Calculator.AverageResponse> __Method_ComputeAverageStream = new grpc::Method<global::Calculator.AverageRequest, global::Calculator.AverageResponse>(
        grpc::MethodType.ClientStreaming,
        __ServiceName,
        "ComputeAverageStream",
        __Marshaller_Calculator_AverageRequest,
        __Marshaller_Calculator_AverageResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Calculator.MaximumRequest, global::Calculator.MaximumResponse> __Method_MaximumStream = new grpc::Method<global::Calculator.MaximumRequest, global::Calculator.MaximumResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "MaximumStream",
        __Marshaller_Calculator_MaximumRequest,
        __Marshaller_Calculator_MaximumResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Calculator.RunDeadlineRequest, global::Calculator.RunDeadlineResponse> __Method_RunDeadline = new grpc::Method<global::Calculator.RunDeadlineRequest, global::Calculator.RunDeadlineResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RunDeadline",
        __Marshaller_Calculator_RunDeadlineRequest,
        __Marshaller_Calculator_RunDeadlineResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Calculator.CalculatorReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Calculator</summary>
    [grpc::BindServiceMethod(typeof(Calculator), "BindService")]
    public abstract partial class CalculatorBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Calculator.Response> Add(global::Calculator.Request request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task GetPrimes(global::Calculator.PrimeNumberRequest request, grpc::IServerStreamWriter<global::Calculator.PrimeNumberResponse> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Calculator.AverageResponse> ComputeAverageStream(grpc::IAsyncStreamReader<global::Calculator.AverageRequest> requestStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task MaximumStream(grpc::IAsyncStreamReader<global::Calculator.MaximumRequest> requestStream, grpc::IServerStreamWriter<global::Calculator.MaximumResponse> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Calculator.RunDeadlineResponse> RunDeadline(global::Calculator.RunDeadlineRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Calculator</summary>
    public partial class CalculatorClient : grpc::ClientBase<CalculatorClient>
    {
      /// <summary>Creates a new client for Calculator</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public CalculatorClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Calculator that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public CalculatorClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected CalculatorClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected CalculatorClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Calculator.Response Add(global::Calculator.Request request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Add(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Calculator.Response Add(global::Calculator.Request request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Add, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Calculator.Response> AddAsync(global::Calculator.Request request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AddAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Calculator.Response> AddAsync(global::Calculator.Request request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Add, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Calculator.PrimeNumberResponse> GetPrimes(global::Calculator.PrimeNumberRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetPrimes(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Calculator.PrimeNumberResponse> GetPrimes(global::Calculator.PrimeNumberRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetPrimes, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncClientStreamingCall<global::Calculator.AverageRequest, global::Calculator.AverageResponse> ComputeAverageStream(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ComputeAverageStream(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncClientStreamingCall<global::Calculator.AverageRequest, global::Calculator.AverageResponse> ComputeAverageStream(grpc::CallOptions options)
      {
        return CallInvoker.AsyncClientStreamingCall(__Method_ComputeAverageStream, null, options);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Calculator.MaximumRequest, global::Calculator.MaximumResponse> MaximumStream(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return MaximumStream(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Calculator.MaximumRequest, global::Calculator.MaximumResponse> MaximumStream(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_MaximumStream, null, options);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Calculator.RunDeadlineResponse RunDeadline(global::Calculator.RunDeadlineRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RunDeadline(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Calculator.RunDeadlineResponse RunDeadline(global::Calculator.RunDeadlineRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_RunDeadline, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Calculator.RunDeadlineResponse> RunDeadlineAsync(global::Calculator.RunDeadlineRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RunDeadlineAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Calculator.RunDeadlineResponse> RunDeadlineAsync(global::Calculator.RunDeadlineRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_RunDeadline, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override CalculatorClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new CalculatorClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(CalculatorBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Add, serviceImpl.Add)
          .AddMethod(__Method_GetPrimes, serviceImpl.GetPrimes)
          .AddMethod(__Method_ComputeAverageStream, serviceImpl.ComputeAverageStream)
          .AddMethod(__Method_MaximumStream, serviceImpl.MaximumStream)
          .AddMethod(__Method_RunDeadline, serviceImpl.RunDeadline).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, CalculatorBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Add, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Calculator.Request, global::Calculator.Response>(serviceImpl.Add));
      serviceBinder.AddMethod(__Method_GetPrimes, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::Calculator.PrimeNumberRequest, global::Calculator.PrimeNumberResponse>(serviceImpl.GetPrimes));
      serviceBinder.AddMethod(__Method_ComputeAverageStream, serviceImpl == null ? null : new grpc::ClientStreamingServerMethod<global::Calculator.AverageRequest, global::Calculator.AverageResponse>(serviceImpl.ComputeAverageStream));
      serviceBinder.AddMethod(__Method_MaximumStream, serviceImpl == null ? null : new grpc::DuplexStreamingServerMethod<global::Calculator.MaximumRequest, global::Calculator.MaximumResponse>(serviceImpl.MaximumStream));
      serviceBinder.AddMethod(__Method_RunDeadline, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Calculator.RunDeadlineRequest, global::Calculator.RunDeadlineResponse>(serviceImpl.RunDeadline));
    }

  }
}
#endregion
