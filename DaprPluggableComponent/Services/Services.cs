// ------------------------------------------------------------------------
// Copyright 2022 The Dapr Authors
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ------------------------------------------------------------------------

// Uncomment to import Dapr proto components namespace.
// using Dapr.Proto.Components.V1;

using Dapr.Client.Autogen.Grpc.v1;
using Dapr.Proto.Components.V1;
using Google.Protobuf.Collections;
using Grpc.Core;

namespace DaprComponents.Services;


// Uncomment the lines below to implement the StateStore methods defined in the following protofiles
// ./Protos/dapr/proto/components/v1/state.proto#L123
//public class StateStoreService : StateStore.StateStoreBase
//{
//    private readonly ILogger<StateStoreService> _logger;
//    public StateStoreService(ILogger<StateStoreService> logger)
//    {
//        _logger = logger;
//    }
//}

// Uncomment the lines below to implement the PubSub methods defined in the following protofiles
// ./Protos/dapr/proto/components/v1/pubsub.proto#L23
//public class PubSubService : PubSub.PubSubBase
//{
//    private readonly ILogger<PubSubService> _logger;
//    public PubSubService(ILogger<PubSubService> logger)
//    {
//        _logger = logger;
//    }
//}

// Uncomment the lines below to implement the InputBindings methods defined in the following protofiles
// ./Protos/dapr/proto/components/v1/bindings.proto#L23
// public class InputBindingService : InputBinding.InputBindingBase
// {
//     private readonly ILogger<InputBindingService> _logger;
//     public InputBindingService(ILogger<InputBindingService> logger)
//     {
//         _logger = logger;
//         logger.LogInformation("Input");
//     }

//     public Task Read(ReadRequest readRequest)
//     {
//         return Task.CompletedTask;
//     }

//     //public Task<PingResponse> Ping(PingRequest pingRequest)
//     //{
//     //    //PingResponse pingResponse = new PingResponse();
//     //    //return Task<PingResponse>.CompletedTask;
//     //}    
// }

// Uncomment the lines below to implement the OutputBindings methods defined in the following protofiles
// ./Protos/dapr/proto/components/v1/bindings.proto#L37
public class OutputBindingService : OutputBinding.OutputBindingBase
{
    private readonly ILogger<OutputBindingService> logger;
    public OutputBindingService(ILogger<OutputBindingService> logger)
    {
        this.logger = logger;
        logger.LogTrace("Outbound");
    }

    public override Task<OutputBindingInitResponse> Init(OutputBindingInitRequest request, ServerCallContext context)
    {
        logger.LogTrace("Init");

        LogMetadata(request.Metadata.Properties);

        var result = new OutputBindingInitResponse();

        return Task.FromResult(result);
    }

    private void LogMetadata(MapField<string,string> mapField)
    {
        foreach (var item in mapField)
        {
            logger.LogTrace($"{item.Key} {item.Value}");
        }
    }

    /// <summary>
    /// Invoke remote systems with optional payloads.
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<InvokeResponse> Invoke(InvokeRequest request, ServerCallContext context)
    {
        logger.LogTrace("Invoke");


        LogMetadata(request.Metadata);
        logger.LogTrace(request.Data.ToString());

        var result = new InvokeResponse()
        {
            ContentType = "application/json",
            Data = request.Data,
        };

        return Task.FromResult(result);
    }

    /// <summary>
    /// ListOperations list system supported operations.
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<ListOperationsResponse> ListOperations(ListOperationsRequest request, ServerCallContext context)
    {
        logger.LogTrace("List");

        var operations = new RepeatedField<string> { "read", "write" };
        var result = new ListOperationsResponse();
        result.Operations.AddRange(operations);

        return Task.FromResult(result);

    }

    /// <summary>
    /// Ping the OutputBinding. Used for liveness porpuses.
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        logger.LogTrace("Ping");
        return Task.FromResult(new PingResponse());
    }
}
